window.blurbsApp.importViewModel = (function (ko, datacontext) {
    function ImportViewModel() {
        var self = this;
        self.productList = ko.observableArray();
        self.errorObservable = ko.observable('');
        self.ProcessStatus = ko.observable();

        //for export
        self.ExAreaList = ko.observableArray();
        self.SelectedExProduct = ko.observable();
        self.SelectedExArea = ko.observable();
        self.DownloadUrl = ko.observable();
        self.SelectedExProduct.subscribe(function (product) {
            datacontext.getAreas(self.ExAreaList, null, product.Code, 0, self.errorObservable, self.ProcessStatus);
        });
        self.ExAreaList.subscribe(function (arealist) {
            if (arealist.length) {
                self.SelectedExArea(arealist[0]);
            }
        });
        self.SelectedExArea.subscribe(function (area) {
            datacontext.getAreas(area.ChildAreaList, null, self.SelectedExProduct().Code, area.AreaId, self.errorObservable, self.ProcessStatus);
            self.DownloadUrl(datacontext.exportTemplate(area));
        });

        self.setSelectedExArea = function (area, e) {
            self.SelectedExArea(area);
            //var $target = $(e.target);
            //$target.siblings('.nav-pills').slideToggle();
        };

        //for import
        self.ImAreaList = ko.observableArray();
        self.SelectedImProduct = ko.observable();
        self.SelectedImArea = ko.observable();
        self.ImportFile = ko.observable();
        
        self.SelectedImProduct.subscribe(function (product) {
            datacontext.getAreas(self.ImAreaList, null, product.Code, 0, self.errorObservable, self.ProcessStatus);
            self.errorObservable('');
        });
        self.ImAreaList.subscribe(function (arealist) {
            if (arealist.length) {
                self.SelectedImArea(arealist[0]);
            }
        });
        self.SelectedImArea.subscribe(function (area) {
            datacontext.getAreas(area.ChildAreaList, null, self.SelectedImProduct().Code, area.AreaId, self.errorObservable, self.ProcessStatus);
            self.errorObservable('');
        });

        self.setSelectedImArea = function (area, e) {
            self.SelectedImArea(area);
        };
        
        var $bar = $('.progress-bar');
        var $submitbtn = $('#importbtn');
        var $status = $('#status');
        $('#importfrm').ajaxForm({
            beforeSend: function () {
                var percentVal = '0%';
                $bar.width(percentVal);
                self.errorObservable('');
                self.ProcessStatus(datacontext.ProcessStatus.Processing);
            },
            uploadProgress: function (event, position, total, percentComplete) {
                var percentVal = percentComplete + '%';
                $bar.width(percentVal);
            },
            success: function () {
                var percentVal = '100%';
                $bar.width(percentVal);
                self.ProcessStatus(datacontext.ProcessStatus.Succeed);
            },
            complete: function (xhr, statusText) {
                $bar.width('0%');
                if (statusText == 'error') {
                    self.ProcessStatus(datacontext.ProcessStatus.Failed);
                    self.errorObservable(xhr.responseText);
                }
            }
        }
        );
    };

    var importViewModel = new ImportViewModel({});

    return importViewModel;

})(ko, blurbsApp.datacontext);

// Initiate the Knockout bindings
var $importEle = $("#importmod");
var importViewModel = window.blurbsApp.importViewModel;
if ($importEle.length) {
    // init product list
    blurbsApp.datacontext.getProductList(importViewModel.productList, importViewModel.errorObservable);
    ko.applyBindings(importViewModel, $importEle[0]);
}
