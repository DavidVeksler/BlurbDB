window.blurbsApp.productsViewModel = (function (ko, datacontext) {
    /// <field name="products" value="[new datacontext.todoList()]"></field>
    /// 
    var areasViewModel = window.blurbsApp.areasViewModel;
    var blurbListViewModel = window.blurbsApp.blurbListViewModel;
    //products definition
    function Products(data) {
        var self = this;
        self.productList = ko.observableArray(data.productList || null);
        self.selectedProduct = ko.observable(data.selectedProduct || null);
        self.SearchTerm = ko.observable();
        self.errorObservable = ko.observable('');
        self.changeProduct = function (product) {
            self.selectedProduct(product);
            self.SearchTerm('');
            areasViewModel.Product(product);
            areasViewModel.SearchMode(false);
            areasViewModel.SearchResult([]);

            blurbListViewModel.productId(product.Code);
            blurbListViewModel.blurbArray([]);
        };
        self.search = function (data) {
            if (data.SearchTerm()) {
                return datacontext.search(data)
                    .done(function (result) {
                        areasViewModel.SearchResult(result);
                        areasViewModel.SearchMode(true);
                    });
            } else {
                areasViewModel.SearchMode(false);
            }

        };
    };

    var products = new Products({});
    return products;
})(ko, blurbsApp.datacontext);

// Initiate the Knockout bindings
var $productsEle = $("#efprods");
var productsViewModel = window.blurbsApp.productsViewModel;
if ($productsEle.length) {
    ko.applyBindings(productsViewModel, $productsEle[0]);
    blurbsApp.datacontext.getProductList(productsViewModel.productList, productsViewModel.errorObservable);

}
