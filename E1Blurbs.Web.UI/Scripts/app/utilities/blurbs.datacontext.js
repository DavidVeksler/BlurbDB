window.blurbsApp = window.blurbsApp || {};
window.blurbsApp.datacontext = (function () {

    var datacontext = {
        getProductList: getProductList,
        getAreas: getAreas,
        getBlurbs: getBlurbs,
        getTranslations: getTranslations,
        getCultures: getCultures,
        addArea: addArea,
        saveNewArea: saveNewArea,
        updateArea: updateArea,
        deleteArea: deleteArea,
        createTranslation: createTranslation, // create translation model
        //createArea: createArea,
        //createBlurbItem: createBlurbItem,
        addBlurb: addBlurb,
        updateBlurb: updateBlurb,
        addTranslation: addTranslation, // add new translation item
        updateTranslation: updateTranslation,
        deleteTranslation: deleteTranslation,
        search: search,
        getAreaListBySearch: getAreaListBySearch,
        deleteBlurb: deleteBlurb,
        exportTemplate: exportTemplate,
        importTemplate: importTemplate,
        ProcessStatus: {
            Idle: 0,
            Processing: 1,
            Succeed: 2,
            Failed: 3
        }
    };

    return datacontext;

    function search(product) {
        return ajaxRequest("get", searchUrl(product.SearchTerm(), product.selectedProduct().Code))
            .fail(getFailed);

        function getFailed(result) {
            //errorObservable("Error retrieving todo lists.");
        }
    }

    function getAreaListBySearch(areaListOberservable, searchResult, productId) {
        //clear blurblistview
        var blurbListViewModel = window.blurbsApp.blurbListViewModel;
        blurbListViewModel.AreaId(undefined);
        blurbListViewModel.Area({ AreaId: undefined, Name: undefined });
        blurbListViewModel.blurbArray([]);

        // 1. set area data
        // 2. set blurbList in this area
        var areaList = $.map(searchResult, function (item) {
            var area = {};
            area.productId = productId;
            area.AreaId = item.Blurbs[0].AreaId;
            area.Name = item.BlurbName;

            var blurbs = item.Blurbs;
            var mappedBlurbList = $.map(blurbs, function (blurb) {
                blurb.productId = productId;
                return new createBlurb(blurb);
            });
            area.BlurbList = mappedBlurbList;
            return new createArea(area);
        });
        areaListOberservable(areaList);
    }

    function getProductList(productListObservable, errorObservable) {
        return ajaxRequest("get", porductListUrl())
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedProductList = $.map(data, function (product) { return new createProduct(product); });
            productListObservable(mappedProductList);
        }

        function getFailed() {
            errorObservable("Error retrieving todo lists.");
        }
    }

    //helper method: 
    function createProduct(data) {
        return new datacontext.product(data);
    }

    function getCultures(cultureListObservable, productId, errorObservable) {
        return ajaxRequest("get", cultureListUrl(productId))
            .done(getSucceeded)
            .fail(getFailed);
        function getSucceeded(data) {
            cultureListObservable(data);
        }

        function getFailed() {
            errorObservable("Error retrieving language cultures.");
        }
    }


    function getAreas(areaListObservable, blurbsObservable, productId, selectedAreaId, errorObservable, processStatusObservable) {
        var prodCode = $.trim(productId);
        var areaId = !!selectedAreaId ? selectedAreaId : 0;

        //clear blurblistview
        blurbsApp.blurbListViewModel.blurbArray([]);

        processStatusObservable(datacontext.ProcessStatus.Processing);
        return ajaxRequest("get", areaListUrl({ productId: prodCode, ParentCategoryId: areaId }))
            .done(getSucceeded)
            .fail(getFailed);
        function getSucceeded(data) {

            var mappedAreas = $.map(data, function (area) {
                area.productId = productId;
                area.ParentCategoryId = selectedAreaId;
                return new createArea(area);
            });
            areaListObservable(mappedAreas);
            processStatusObservable(datacontext.ProcessStatus.Idle);

            if (blurbsObservable) {
                //if no area returned then try to get blurbs
                getBlurbs(blurbsObservable, prodCode, areaId, errorObservable, processStatusObservable);
            }
        }

        function getFailed() {
            errorObservable("Error retrieving todo lists.");
            processStatusObservable(datacontext.ProcessStatus.Idle);
        }
    }

    function createArea(data) {
        return new datacontext.area(data);
    }

    function updateArea(area) {
        clearErrorMessage(area);

        area.ProcessStatus(datacontext.ProcessStatus.Processing);
        return ajaxRequest("put", areaListUrl({ id: area.AreaId }), area)
            .fail(function (result) {
                area.errorMessage(formatErrorMessage(result));
                area.ProcessStatus(datacontext.ProcessStatus.Failed);
            });
    }

    function deleteArea(area) {
        area.ProcessStatus(datacontext.ProcessStatus.Processing);
        return ajaxRequest("delete", areaListUrl({ id: area.AreaId }))
            .fail(function (result) {
                area.errorMessage(formatErrorMessage(result));
                area.ProcessStatus(datacontext.ProcessStatus.Failed);
            });
    }

    function getTranslations(translationsObservable, blurbId, errorObservable, statusObservalble) {
        statusObservalble(datacontext.ProcessStatus.Processing);
        return ajaxRequest("get", translationsUrl({ blurbId: blurbId }))
            .done(getSucceeded)
            .fail(getFailed);
        function getSucceeded(data) {
            //translation  format:
            // [
            //    { "BlurbId": 300001, "CultureCode": "zh-CN     ", "Content": "全球卓著英语培训专家", "TranslationId": 3000004 },
            //    { "BlurbId": 300001, "CultureCode": "id-ID     ", "Content": "Lembaga Pendidikan Bahasa Inggris Terdepan di Dunia", "TranslationId": 3000002 },
            //    { "BlurbId": 300001, "CultureCode": "ru-RU     ", "Content": "Мировой лидер в обучении английскому", "TranslationId": 3000003 }
            //]

            var mappedTranslationList = $.map(data, function (translation) {
                return new createTranslation(translation);
            });
            translationsObservable(mappedTranslationList);
            statusObservalble(datacontext.ProcessStatus.Idle);
        }

        function getFailed() {
            errorObservable("Error retrieving translation.");
            statusObservalble(datacontext.ProcessStatus.Failed);
        }
    }

    function addTranslation(newTranslation) {
        clearErrorMessage(newTranslation);
        newTranslation.ProcessStatus(datacontext.ProcessStatus.Processing);
        return ajaxRequest("post", translationsUrl(), newTranslation)
            .fail(function (result) {
                newTranslation.errorMessage(formatErrorMessage(result));
                newTranslation.ProcessStatus(datacontext.ProcessStatus.Idle);
            });
    }


    function updateTranslation(translation) {
        clearErrorMessage(translation);
        translation.ProcessStatus(datacontext.ProcessStatus.Processing);

        return ajaxRequest("put", translationsUrl({ id: translation.TranslationId() }), translation)
            .done(function () {
                translation.Content(translation.NewContent());
                translation.IsEditing(false);
                translation.ProcessStatus(datacontext.ProcessStatus.Idle);
            })
            .fail(function (result) {
                translation.errorMessage(formatErrorMessage(result));
                translation.ProcessStatus(datacontext.ProcessStatus.Idle);

            });

    }

    function deleteTranslation(translation) {
        clearErrorMessage(translation);
        translation.ProcessStatus(datacontext.ProcessStatus.Processing);
        return ajaxRequest("delete", translationsUrl({ id: translation.TranslationId() }))
            .fail(function (result) {
                translation.errorMessage(formatErrorMessage(result));
                translation.ProcessStatus(datacontext.ProcessStatus.Failed);
            });
    }

    function createTranslation(data) {
        return new datacontext.translation(data);
    }
    function addArea(data) {
        return new datacontext.area(data);
    }
    function saveNewArea(newArea, errorMessageObservable) {
        clearErrorMessage(newArea);
        return ajaxRequest("post", areaListUrl(), newArea)
            .fail(function (result) {
                errorMessageObservable(formatErrorMessage(result));
            });
    }

    function getBlurbs(blurbListObservable, productId, areaId, errorObservable, processStatusObservable) {
        console.log('get blurbs:' + datacontext.ProcessStatus.Processing);
        processStatusObservable(datacontext.ProcessStatus.Processing);
        return ajaxRequest("get", blurbListUrl({ productId: productId, areaId: areaId }))
            .done(getSucceeded)
            .fail(getFailed);
        function getSucceeded(data) {
            processStatusObservable(datacontext.ProcessStatus.Idle);
            var mappedBlurbList = $.map(data, function (blurb) {
                blurb.productId = productId;
                blurb.AreaId = areaId;
                var langArray = blurb.Languages;
                blurb.Languages = $.map(langArray, function (lang) {
                    return $.trim(lang);
                });
                return new createBlurb(blurb);
            });
            blurbListObservable(mappedBlurbList);

            //var blurbListViewModel = window.blurbsApp.blurbListViewModel;
            //blurbListViewModel.productId(self.productId);
            //blurbListViewModel.AreaId(self.AreaId);
            //blurbListViewModel.Area({ AreaId: self.AreaId, Name: self.Name() });
            //blurbListViewModel.blurbArray(data);
        }

        function getFailed() {
            processStatusObservable(datacontext.ProcessStatus.Idle);
            errorObservable("Error retrieving todo lists.");
        }
    }

    function createBlurb(data) {
        return new datacontext.blurb(data);
    }

    function addBlurb(newBlurb) {
        clearErrorMessage(newBlurb);

        newBlurb.ProcessStatus(datacontext.ProcessStatus.Processing);
        return ajaxRequest("post", blurbListUrl(), newBlurb)
            .fail(function (result) {
                newBlurb.errorMessage(formatErrorMessage(result));
                newBlurb.ProcessStatus(datacontext.ProcessStatus.Failed);

            });
    }

    function updateBlurb(blurb) {
        clearErrorMessage(blurb);

        blurb.ProcessStatus(datacontext.ProcessStatus.Processing);
        return ajaxRequest("put", blurbListUrl({ id: blurb.BlurbId() }), blurb)
            .done(function () {
                blurb.Description(blurb.NewDescription());
                blurb.ProcessStatus(datacontext.ProcessStatus.Idle);
                blurb.IsEditing(false);
            })
            .fail(function (result) {
                blurb.errorMessage(formatErrorMessage(result));
                blurb.ProcessStatus(datacontext.ProcessStatus.Failed);
            });
    }

    function deleteBlurb(blurb) {
        blurb.ProcessStatus(datacontext.ProcessStatus.Processing);
        return ajaxRequest("delete", blurbListUrl({ id: blurb.BlurbId() }))
            .fail(function (result) {
                blurb.errorMessage(formatErrorMessage(result));
                blurb.ProcessStatus(datacontext.ProcessStatus.Failed);
            });
    }

    function exportTemplate(area) {
        return ExportUrl(area.AreaId);
    }

    function importTemplate(area) {
    }

    //function deleteTodoList(todoList) {
    //    return ajaxRequest("delete", todoListUrl(todoList.todoListId))
    //        .fail(function () {
    //            todoList.errorMessage("Error removing todo list.");
    //        });
    //}
    //function saveChangedTodoItem(todoItem) {
    //    clearErrorMessage(todoItem);
    //    return ajaxRequest("put", todoItemUrl(todoItem.todoItemId), todoItem, "text")
    //        .fail(function () {
    //            todoItem.errorMessage("Error updating todo item.");
    //        });
    //}
    //function saveChangedTodoList(todoList) {
    //    clearErrorMessage(todoList);
    //    return ajaxRequest("put", todoListUrl(todoList.todoListId), todoList, "text")
    //        .fail(function () {
    //            todoList.errorMessage("Error updating the todo list title. Please make sure it is non-empty.");
    //        });
    //}

    // Private

    function formatErrorMessage(result) {
        var responseText = $.parseJSON(result.responseText);
        return responseText.Message + ' : ' + responseText.ExceptionMessage;
    }

    function clearErrorMessage(entity) { entity.errorMessage(null); }
    function ajaxRequest(type, url, data, dataType) { // Ajax helper
        var options = {
            dataType: dataType || "json",
            contentType: "application/json",
            cache: false,
            type: type,
            data: data ? data.toJson() : null
        };
        var antiForgeryToken = $("#antiForgeryToken").val();
        if (antiForgeryToken) {
            options.headers = {
                'RequestVerificationToken': antiForgeryToken
            };
        }
        return $.ajax(url, options);
    }
    // routes
    function searchUrl(searchTerm, productId) {
        return "/search/searchblurbs/?searchTerm=" + searchTerm + '&productId=' + productId;
    }
    function cultureListUrl(productId) {
        return "/lookup/Cultures/?productId=" + productId;
    }

    function porductListUrl() {
        return "/api/Product/";
    }
    function areaListUrl(params) {
        var queryStr = '';
        if (params) {
            queryStr += 'productId=' + (params.productId ? params.productId : '');
            queryStr += '&ParentCategoryId=' + (params.ParentCategoryId ? params.ParentCategoryId : 0);
            queryStr += '&id=' + (params.id ? params.id : '');

        }
        return "/api/Area/?" + queryStr;
    }

    function blurbListUrl(params) {
        var queryStr = '';
        if (params) {
            queryStr += 'productId=' + (params.productId ? params.productId : '');
            queryStr += '&areaId=' + (params.areaId ? params.areaId : '');
            queryStr += '&id=' + (params.id ? params.id : '');
        }
        return "/api/Blurbs/?" + queryStr;
    }

    function translationsUrl(params) {
        var queryStr = '';
        if (params) {
            queryStr += 'blurbId=' + (params.blurbId ? params.blurbId : '');
            queryStr += '&id=' + (params.id ? params.id : '');
        }
        return "/api/Translation/?" + queryStr;
    }

    function ExportUrl(ParentCategoryId) {
        return "import/exportBlubs?ParentCategoryId=" + ParentCategoryId;
    }

})();