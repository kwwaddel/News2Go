(function () {
    angular.module('NewsApp')
        .factory('GuardianNewsService', ['$resource',GuardianNewsService])

    function GuardianNewsService($resource, $ionicLoading) {
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