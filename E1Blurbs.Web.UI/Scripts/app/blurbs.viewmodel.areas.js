window.blurbsApp.areasViewModel = (function (ko, datacontext) {
    function Areas(data) {
        var self = this;

        self.Product = ko.observable(data.Product);
        self.ParentCategoryId = data.ParentCategoryId || 0; //root area id =0

        self.AreaList = ko.observableArray(data.AreaList);


        self.errorObservable = ko.observable('');
        self.ProcessStatus = ko.observable(datacontext.ProcessStatus.Idle);
        self.errorMessage = ko.observable('');
        // new area 
        self.NewParentArea = ko.observable(data.ParentArea);
        self.NewAreaName = ko.observable();

        //active area
        self.ActiveArea = ko.observable();
        self.ActiveParentArea = ko.observable();

        //for search
        self.SearchMode = ko.observable();
        self.SearchResult = ko.observableArray([]);

        self.exitSearchMode = function () {
            self.SearchMode(false);
        };
        self.SearchMode.subscribe(function (isSearchMode) {
            if (!isSearchMode)
                datacontext.getAreas(self.AreaList, null, self.Product().Code, self.ParentCategoryId, self.errorObservable, self.ProcessStatus);
        });
        self.Product.subscribe(function() {
            datacontext.getAreas(self.AreaList, null, self.Product().Code, self.ParentCategoryId, self.errorObservable, self.ProcessStatus);
        });

        self.clickNewArea = function (area) {
            //this area is root

            $('#createarea').modal('show');
            self.NewParentArea(area);
            datacontext.getAreas(area.ChildAreaList, null, area.productId, area.AreaId, area.errorObservable, area.ProcessStatus);
        };
        self.clickEdit = function (rootContext, parentContext, area) {
            self.setActiveArea(rootContext, area);
            self.ActiveParentArea(parentContext);
            $('#editarea').modal('show');

        };

        self.updateArea = function (area) {
            if (area.NewName()) {
                return datacontext.updateArea(area)
                    .done(function () {
                        area.Name(area.NewName());
                        area.ProcessStatus(datacontext.ProcessStatus.Idle);
                        $('#editarea').modal('hide');
                    });
            }
        };

        self.clickDeleteArea = function () {
            $('#editarea').find('.modal-footer>.btn-group').hide().end().find('.alert').show();
        };
        self.closeAlert = function () {
            $('#editarea').find('.alert').hide().end().find('.modal-footer>.btn-group').show();
        };
        self.deleteArea = function (area) {
            return datacontext.deleteArea(area)
                        .done(function () {
                            area.ProcessStatus(datacontext.ProcessStatus.Idle);
                            $('#editarea').modal('hide');
                            //remove area from the child area list in parent area
                            self.ActiveParentArea().ChildAreaList.remove(area);
                        });
        };

        self.addArea = function () {
            var newArea = {
                productId: self.Product().Code,
                ParentCategoryId: self.NewParentArea().AreaId,
                Name: self.NewAreaName()
            };
            var newAreaObservable = new datacontext.area(newArea);
            return datacontext.saveNewArea(newAreaObservable, self.errorMessage)
                            .done(function (result) {
                                newAreaObservable.AreaId = result.AreaId;

                                var childAreaListKo = self.NewParentArea().ChildAreaList;
                                if (childAreaListKo().length) {
                                    self.NewParentArea().ChildAreaList.push(newAreaObservable);
                                }
                                //clear dialog
                                self.NewParentArea(self.AreaList()[0]);
                                self.NewAreaName('');
                                $('#createarea').modal('hide');
                            });
        };

        self.setActiveArea = function (rootContext, area) {
            rootContext.ActiveArea(area);
        };

        self.setNewParentArea = function (area) {
            self.NewParentArea(area);
        };

        self.SearchResult.subscribe(function (data) {
            if (self.AreaList().length) {
                datacontext.getAreaListBySearch(self.AreaList()[0].ChildAreaList, data, self.Product().Code);
                self.ActiveArea(undefined);
            }
        });

        self.ActiveArea.subscribe(function (area) {
            var blurbListViewModel = window.blurbsApp.blurbListViewModel;
            if (self.SearchMode()) {
                //set areaId to blurblistviewmodel
                blurbListViewModel.productId(area.productId);
                blurbListViewModel.AreaId(area.AreaId);
                blurbListViewModel.Area({ AreaId: area.AreaId, Name: area.Name() });
                blurbListViewModel.blurbArray(area.BlurbList());
            } else if (!!area) {
                //try to get child arealist if there is no child area then try to load blurbs
                datacontext.getAreas(area.ChildAreaList, area.BlurbList, area.productId, area.AreaId, self.errorObservable, area.ProcessStatus);
            }
            blurbListViewModel.LangFilters([]);

            //if (area) {
            //    if (area.AreaId) {
            //        if (self.SearchMode()) {
            //            var blurbListViewModel = window.blurbsApp.blurbListViewModel;
            //            blurbListViewModel.productId(area.productId);
            //            blurbListViewModel.AreaId(area.AreaId);
            //            blurbListViewModel.Area({ AreaId: area.AreaId, Name: area.Name() });
            //            blurbListViewModel.blurbArray(area.BlurbList());
            //        } else {
            //            datacontext.getAreas(area.ChildAreaList, area.productId, area.AreaId, self.errorObservable);
            //            //try to load blurbs for each area level
            //            datacontext.getBlurbs(area.BlurbList, area.productId, area.AreaId, self.errorObservable, self.ProcessStatus);
            //        }
            //    } else {
            //        //if there is no active area then the sub area and blurb list is empty
            //        area.ChildAreaList([]);
            //        area.BlurbList([]);
            //    }
            //} else {
            //    //todo: clear blurblistview
            //}
        });
    };



    var areas = new Areas({});
    return areas;

})(ko, blurbsApp.datacontext);


// Initiate the Knockout bindings
var $areasEle = $("#areas");
if ($areasEle.length) {
    ko.applyBindings(window.blurbsApp.areasViewModel, $areasEle[0]);
}
