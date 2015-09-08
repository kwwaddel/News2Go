(function () {
    'use strict';

    angular.module('NewsApp')
        .controller('NewsSearchController', ['GuardianNewsService', NewsSearchController])
        .controller('LoginController', LoginController)
        .controller('RegisterController', RegisterController);

    function LoginController() {

    }

    function RegisterController() {

    }

    function NewsSearchController(GuardianNewsService) {
        var vm = this;
        vm.search = 'Breaking News'
        vm.order = 'newest'
        vm.doSearch = doSearch
        vm.result = []
        getNews()
       

        function getNews() {
            GuardianNewsService.get({ q: vm.search, 'order-by': vm.order, 'show-blocks': 'all', 'show-fields': 'all', 'show-tags': 'all'},
                function (articles) {
                    if (articles.response.results) {
                        vm.result = articles.response.results;
                    }
                    else {
                        vm.result = articles.push.apply(vm.result, articles.response);
                    }
                })

        }

        function doSearch() {
            getNews()

        }
    }
})();