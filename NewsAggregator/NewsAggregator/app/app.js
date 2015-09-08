(function () {
    'use strict';

    angular.module('NewsApp', ['ngRoute', 'ngResource'])
    .config(['$routeProvider', '$httpProvider', Config]);


    function Config($routeProvider, $httpProvider) {
        $routeProvider
            .when('/', {
                templateUrl: '/app/views/home.html',
                controller: 'NewsSearchController',
                controllerAs: 'vm'
            })
            .when('/login', {
                templateUrl: '/app/views/login.html',
                controller: 'LoginController',
                controllerAs: 'vm'
            })
            .when('/register', {
                templateUrl: '/app/views/register.html',
                controller: 'RegisterController',
                controllerAs: 'vm'
            })
            .otherwise({
                redirectTo: '/'
            })
    }
})();