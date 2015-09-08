(function (){
	angular.module('NewsApp')
		.factory('AuthService', ['$window', '$q', AuthService]);

	function AuthService($window, $q) {
        var service = {};

        service.request = request;

        function request(config) {
            config.headers = config.headers || {};
            if ($window.sessionStorage.getItem('token')) {
                config.headers.Authorization = 'Bearer ' + $window.sessionStorage.getItem('token');
            }

            return config || $q.when(config);
        }

        return service;
    }
})();

