(function (ko, datacontext) {
    datacontext.product = product;
    datacontext.area = area;
    datacontext.blurb = blurb;
    datacontext.translation = translation;
    //product definition
    function product(data) {
        var self = this;
        var data = data || {};

        self.Code = $.trim(data.Code);
        self.Name = $.trim(data.Name);
    };

    //Area definition
    function area(data) {
        var self = this;
        var data = data || {};

        // Persisted properties
        self.AreaId = $.trim(data.AreaId);
        self.Name = ko.observable($.trim(data.Name));
        self.NewName = ko.observable($.trim(data.Name));
        self.productId = data.productId;
        self.ParentCategoryId = $.trim(data.ParentCategoryId);
        self.ChildAreaList = ko.observableArray([]);
        self.BlurbList = ko.observableArray(data.BlurbList || []);
        self.NewChidAreaName = ko.observable(self.Name);
        self.IsEditingNew = ko.observable(false);
        self.errorObservable = ko.observable();
        self.errorMessage = ko.observable();
        self.ProcessStatus = ko.observable(datacontext.ProcessStatus.Idle);
        self.createChildArea = function () {
            self.IsEditingNew(true);

        };

        self.BlurbList.subscribe(function (data) {
            //inject blurbs to blurbViewModel
            var blurbListViewModel = window.blurbsApp.blurbListViewModel;
            blurbListViewModel.productId(self.productId);
            blurbListViewModel.AreaId(self.AreaId);
            blurbListViewModel.Area({ AreaId: self.AreaId, Name: self.Name() });
            blurbListViewModel.blurbArray(data);
        });


        self.toJson = function () {
            return ko.toJSON(
                {
                    AreaId: self.AreaId,
                    Name: self.NewName,
                    ParentCategoryId: self.ParentCategoryId,
                    productId: self.productId
                });

        };
    };

    function blurb(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.productId = $.trim(data.productId);
        self.AreaId = $.trim(data.AreaId);
        self.BlurbId = ko.observable($.trim(data.BlurbId));
        self.Description = ko.observable(data.Description);
        self.NewDescription = ko.observable(data.Description);
        self.Languages = ko.observableArray(data.Languages);
        //translation  format:
        // [
        //    { "BlurbId": 300001, "CultureCode": "zh-CN     ", "Content": "全球卓著英语培训专家", "TranslationId": 3000004 },
        //    { "BlurbId": 300001, "CultureCode": "id-ID     ", "Content": "Lembaga Pendidikan Bahasa Inggris Terdepan di Dunia", "TranslationId": 3000002 },
        //    { "BlurbId": 300001, "CultureCode": "ru-RU     ", "Content": "Мировой лидер в обучении английскому", "TranslationId": 3000003 }
        //]
        self.Translations = ko.observableArray();

        self.errorObservable = ko.observable();

        self.getTranslations = function (blurb, event) {
            if (!blurb.Translations().length) {
                datacontext.getTranslations(blurb.Translations, blurb.BlurbId(), self.errorObservable, self.ProcessStatus);
            }
            $($(event.target).attr('data-target')).collapse('toggle');
        };

        self.BlurbId.subscribe(function () {
            self.Translations.push(new datacontext.createTranslation({ BlurbId: self.BlurbId(), productId: self.productId }));
        });
        self.NewDescription.subscribe(function () {
            self.ProcessStatus(datacontext.ProcessStatus.Idle);
        });
        self.NewTranslation = ko.observable();
        // Non-persisted properties
        self.errorMessage = ko.observable();
        self.IsEditing = ko.observable(false);
        self.IsAddTranslation = ko.observable(false);
        self.switchEditMode = function (data, event) {
            if (self.IsEditing()) {
                self.IsEditing(false);
                self.NewDescription(self.Description());
            } else {
                self.IsEditing(true);
                $("textarea", $(event.target).next()).first().focus();
            }
        };


        self.ProcessStatus = ko.observable(datacontext.ProcessStatus.Idle); // 0- no processing; 1- processing; 2-succeed; 3-failed.

        self.clickAddTranslation = function () {
            self.IsAddTranslation(true);
            self.NewTranslation(new datacontext.createTranslation({ BlurbId: self.BlurbId(), productId: self.productId, IsEditing: true }));
        };

        self.addTranslation = function (translation) {
            if (!!translation.TranslationId()) {
                //update translation content
                return datacontext.updateTranslation(translation);
            } else {
                //add translation
                //var newCultureCode = translation.NewCulture().Code;

                var newCultureCode = 'zh-CN';

                var newContent = translation.NewContent();
                var existingCultures = self.Languages();
                if (newCultureCode && newContent) {
                    return datacontext.addTranslation(translation)
                        .done(function (result) {
                            translation.TranslationId(result.TranslationId);
                            translation.Content(newContent);
                            translation.CultureCode(newCultureCode);
                            translation.IsEditing(false);
                            self.Translations.push(translation);
                            self.Languages.push(newCultureCode);
                            self.IsAddTranslation(false);
                            self.ProcessStatus(datacontext.ProcessStatus.Idle);
                            self.IsEditing(false);
                        });

                }
            }
        };
        self.deleteTranslation = function (translation) {
            return datacontext.deleteTranslation(translation)
                       .done(function () {
                           translation.ProcessStatus(datacontext.ProcessStatus.Idle);
                           self.Translations.remove(translation);
                       });
        };

        self.scrollnewBlurb = function () {
            $('#blurbpop').modal('hide');
            $("#" + self.BlurbId()).animatescroll();
        };
        self.toJson = function () {
            return ko.toJSON(
                {
                    BlurbId: self.BlurbId,
                    Description: self.NewDescription,
                    AreaId: self.AreaId,
                    productId: self.productId
                });

        };

    };

    function translation(data) {
        var self = this;
        data = data || {};

        // Persisted properties
        self.BlurbId = data.BlurbId;
        self.productId = data.productId;
        self.CultureCode = ko.observable($.trim(data.CultureCode));
        self.NewCulture = ko.observable();
        self.Content = ko.observable($.trim(data.Content));
        self.NewContent = ko.observable(self.Content());
        self.TranslationId = ko.observable(data.TranslationId);
        self.IsEditing = ko.observable(data.IsEditing || false);
        self.ProcessStatus = ko.observable(datacontext.ProcessStatus.Idle); // 0- no processing; 1- processing; 2-succeed; 3-failed.
        
        self.switchEditMode = function (data, event) {
            if (self.IsEditing()) {
                self.IsEditing(false);
                self.NewContent(self.Content());
            } else {
                self.IsEditing(true);
                $("textarea", $(event.currentTarget).next()).first().focus();
            }
        };

        // Non-persisted properties
        self.errorMessage = ko.observable();

        self.changeCulture = function(data, existingLang) {
            if ($.inArray(data.NewCulture().Code, existingLang) > -1) {
                self.errorMessage('This language is already set, please select another language or eidt the existing translation.');
            } else {
                self.errorMessage('');
            }
        };
        //self.NewCulture.subscribe(function (data, event) {
            
        //});

        self.toJson = function () {
            return ko.toJSON(
                {
                    TranslationId: self.TranslationId || '',
                    Content: self.NewContent,
                    CultureCode: self.NewCulture() ? self.NewCulture().Code : self.CultureCode,
                    BlurbId: self.BlurbId,
                    productId: self.productId
                });

        };


    }

})(ko, blurbsApp.datacontext);