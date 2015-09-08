(function () {

    angular.module('NewsApp')
        .config(function($stateProvider, $urlRouterProvider, $httpProvider) {
            
            
            $stateProvider
              
              .state('app', {
                  url: '/app',
                  abstract: true,
                  templateUrl: 'app/components/news/Menu.html',
                  controller: 'LoginController'
              })
              .state('app.news', {
                  url: '/news',
                  views: {
                    'menuContent':{
                  templateUrl: 'app/components/news/CategoryView.html',
                  controller: 'NewsSearchController'
                }
              }
              })
              .state('app.selectCategories', {
                  url: '/selectCategories',
                  views: {
                    'menuContent':{
                  templateUrl: 'app/components/news/CategorySelectView.html',
                  controller: 'NewsSearchController'
                }
              }
              })
              .state('app.sharedArticles', {
                  url: '/sharedArticles',
                  views: {
                    'menuContent':{
                  templateUrl: 'app/components/news/SharedView.html',
                  controller: 'NewsSearchController'
                }
              }
              })
              .state('app.settings', {
                  url: '/settings',
                  views: {
                    'menuContent':{
                  templateUrl: 'app/components/news/SettingsView.html',
                  controller: 'NewsSearchController'
                }
              }
              })
              .state('app.requests', {
                  url: '/requests',
                  views: {
                    'menuContent':{
                  templateUrl: 'app/components/news/FriendRequestsView.html',
                  controller: 'NewsSearchController'
                }
              }
              })
              
              $urlRouterProvider.otherwise('/app/news')
              $httpProvider.interceptors.push('AuthService')
        })
})();