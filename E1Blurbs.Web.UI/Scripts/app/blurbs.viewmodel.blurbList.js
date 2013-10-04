window.blurbsApp.blurbListViewModel = (function (ko, datacontext) {

    function BlurbList(data) {
        var self = this;
        self.CultureList = ko.observableArray(data.CultureList);

        //for language filter
        self.LangFilters = ko.observableArray();
        self.filterByLang = function () {
            var filterArray = self.LangFilters();
            //var $blurb = $('.blurb');
            if (filterArray.length) {
                $('.blurb').hide();
                for (var index = 0; index < filterArray.length; index++) {
                    console.log('.blurb:not([rel*="' + filterArray[index] + '"])');
                    $('.blurb:not([rel*="' + filterArray[index] + '"])').show();
                }
            } else {
                $('.blurb').show();
            }

            //if (filterArray.length) {
            //    $blurb.hide();
            //    for (var blurbIndex = 0; blurbIndex < $blurb.length; blurbIndex++) {
            //        var $currentBlurb = $($blurb[blurbIndex]);
            //        var currentBlurbRel = $currentBlurb.attr('rel');
            //        for (var index = 0; index < filterArray.length; index++) {
            //            if (!currentBlurbRel.contains(filterArray[index])) {
            //                $currentBlurb.show();
            //                break;
            //            }
            //        }
            //    }
            //} else {
            //    $blurb.show();
            //}
        };

        self.clearLangFilter = function () {
            self.LangFilters([]);
            $('.blurb').show();
        };
        self.AreaId = ko.observable(data.AreaId);
        self.Area = ko.observable(data.Area);
        self.productId = ko.observable(data.productId);

        self.blurbArray = ko.observableArray(data.blurbArray);
        self.errorObservable = ko.observable('');

        self.ProcessStatus = ko.observable(datacontext.ProcessStatus.Idle);
        self.NewBlurb = ko.observable();

        self.productId.subscribe(function (data) {
            datacontext.getCultures(self.CultureList, data, self.errorObservable);
        });

        self.createBlurb = function () {
            $("#blurbpop").modal('show');
            var newBlurb = {
                productId: self.productId(),
                AreaId: self.AreaId(),
            };

            self.NewBlurb(new datacontext.blurb(newBlurb));
        };
        self.addBlurb = function (blurb) {
            //here the blurb is linked to self.NewBlurb
            if (blurb.NewDescription() && !blurb.BlurbId()) {
                return datacontext.addBlurb(blurb)
       .done(function (result) {
           blurb.BlurbId(result.BlurbId);
           blurb.Description(result.Description);
           blurb.ProcessStatus(datacontext.ProcessStatus.Succeed);

           //add it to blurb list on main page
           self.blurbArray.push(blurb);
       });
            }
            else if (blurb.BlurbId()) {
                self.updateBlurb(blurb);
            }
        };
        self.updateBlurb = function (blurb) {
            if (blurb.NewDescription()) {
                return datacontext.updateBlurb(blurb)
                    .done(function (blurb) {
                        blurb.IsEditing(false);
                    });
            }
        };
        self.deleteBlurb = function (blurb) {
            return datacontext.deleteBlurb(blurb)
                        .done(function (result) {
                            blurb.ProcessStatus(datacontext.ProcessStatus.Idle);
                            self.blurbArray.remove(blurb);
                        });
        };
    };

    var blurbList = new BlurbList({});
    return blurbList;

})(ko, blurbsApp.datacontext);


//Initiate the Knockout bindings
var $blurbsEle = $("#blurblist");
if ($blurbsEle.length) {
    ko.applyBindings(window.blurbsApp.blurbListViewModel, $blurbsEle[0]);
}