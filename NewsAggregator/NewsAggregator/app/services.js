(function () {
    'use strict';

    angular.module('NewsApp')
        .factory('GuardianNewsService', ['$resource', GuardianNewsService])
        .factory('LoginService', ['$http', '$q', '$window', LoginService]);

    function LoginService() {

    }

    function GuardianNewsService($resource) {
        var requestUri = 'http://content.guardianapis.com/:action';

        return $resource(requestUri,
            {
                action: 'search',
                'api-key': '6njmcb8y9nbgxgmrjyaukyhz',
                callback: 'JSON_CALLBACK'
            },
            {
                get: { method: 'JSONP' }
            });
    }
    
})();