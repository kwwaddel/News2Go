(function () {
    angular.module('NewsApp')
        .controller('NewsSearchController', ['GuardianNewsService', '$scope', '$ionicModal', '$ionicPopup', '$http', '$window', '$cordovaVibration', '$rootScope', '$ionicPush', '$ionicUser','$q','$ionicLoading', NewsSearchController]);

    function NewsSearchController(GuardianNewsService, $scope, $ionicModal, $ionicPopup, $http, $window, $cordovaVibration, $rootScope, $ionicPush, $ionicUser, $q, $ionicLoading) {
        $scope.search = '';
        $scope.order = 'relevance';
        $scope.doSearch = doSearch;
        //$scope.results = [];
        $scope.productionOffice = 'us';
        $scope.pageSize = '50';
        $scope.comments = [];
        $scope.loaded = false;
        $scope.likeDisLike = likeDisLike
        $scope.friends = [];
        $scope.shared = false;
        $scope.canNotify = true;
        $scope.showButton = true;
        $scope.unreadShared = 0;

        $window.setInterval(function () {notify()}, 30000);
        $window.setInterval(function () {hasFriendRequests()}, 20000);

        $rootScope.catList = [
            {
                icon: '<i class="icon ion-paintbrush"></i> ',
                name: 'Art',
                checked:true
            },
            {
                icon: '<i class="icon ion-ios-briefcase"></i> ',
                name:'Business', 
                checked:true
            },
            {
                icon: '<i class="icon ion-film-marker"></i> ',
                name:'Entertainment', 
                checked:true
            },
            {
                icon: '<i class="icon ion-ios-flag"></i> ',
                name:'Politics', 
                checked:true
            },
            {
                icon: '<i class="icon ion-ios-flask"></i> ',
                name:'Science', 
                checked:true
            }, 
            {
                icon: '<i class="icon ion-ios-americanfootball"></i> ',
                name:'Sport', 
                checked:true
            },
            {
                icon: '<i class="icon ion-monitor"></i> ',
                name:'Technology', 
                checked:true
            }, 
            {
                icon: '<i class="icon ion-star"></i> ',
                name:'Top Stories',
                checked:true
            },
            {
                icon: '<i class="icon ion-android-globe"></i> ',
                name:'World News',
                checked:true
            }];
        
        $scope.printScope = function() {
            console.log($scope);
        };

        function getNews() {

            $scope.canNotify = true;
            $scope.shared = false;
            GuardianNewsService.get({ q: $scope.search, 'page-size': $scope.pageSize, 'production-office': $scope.productionOffice, 'order-by': $scope.order, 'show-blocks': 'all', 'show-fields': 'all', 'show-tags': 'all' },
                function (articles) {
                    if (articles.response.results) {
                        if (!$scope.results)
                            $scope.results = articles.response.results;
                        else
                            $scope.results = articles.response.results;
                        $ionicLoading.hide();
                        likeDisLike(); 
                        }
                        
                    else {
                        $scope.results = "Server is being slow today please try again in a minute."
                    }
                })

        }


        function getNewsById(articleId) {
            if (articleId)
            {
                GuardianNewsService.get({ ids: articleId, 'show-blocks': 'all', 'show-fields': 'all', 'show-tags': 'all' },
                function (articles) {
                    if (articles.response.results) {
                        $scope.results = articles.response.results;
                        $ionicLoading.hide();
                        likeDisLike(); 
                        console.log("results: ");
                        console.log($scope.results);
                        }
                        
                    else {
                        $scope.results = "Server is being slow today please try again in a minute."
                    }
                })
            }
            else
            {
                $scope.search = "";
                getNews();
            }
            
        }

        function likeDisLike(){

                var ids = [];
                            for (var i = 0; i < $scope.results.length; i++){
                            ids[i] = $scope.results[i].id;
                        }

                        $http({
                            url: "http://mobilenewsapp.azurewebsites.net/api/Articles/GetPopularities",
                            method: "POST",
                            data: ids
                        }).success(function (data) {
                            
                            for (var i = 0; i < $scope.results.length; i++){
                                $scope.results[i].ratings = {like:data[i][0], dislike:data[i][1], popularity:data[i][2]};
                            }
                        }).error(function (data) {
                            
                        });
                            //if ($scope.canNotify)
                                //notify();                           
                        }

        function notify () {
            $scope.canNotify = false;
            console.log("notifying");
            //alert("TEST");
            //$cordovaVibration.vibrate(100);
            $http({
                    url: "http://mobilenewsapp.azurewebsites.net/api/Articles/GetNewPushedArticles",
                    method: "POST",
                    params: {username:$window.localStorage.getItem("username")}
                }).success(function (data) {
                    console.log("success get new shared articles: ");
                    console.log(data);
                    if ((data != null) && (data[0] != null)){
                    //$scope.shared = true;

                    alert("You have a new article recommendation from " + data[0][0] + "!");

                    $scope.unreadShared = data.length;

                    $scope.temp = "";
                    $scope.sharers = [];

                    for (var i = 0; i < data.length; i++){
                        if (i != 0)
                            $scope.temp += "," + data[i][1];
                        else
                            $scope.temp += data[i][1];

                        $scope.sharers[i] = data[i][0];
                    }
                    console.log("sharer ids: " + $scope.sharers);
                    //getNewsById($scope.temp);
                    
                    //$scope.showShared();
                }
                    }).error(function (data) {
                    console.log("failed get new shared articles");
                });

                $scope.canNotify = true;
        }

        $scope.printScope = function() {
            console.log($scope);
        }

        $scope.pushRegister = function() {
            console.log('Ionic Push: Registering user');

            // Register with the Ionic Push service.  All parameters are optional.
            $ionicPush.register({
              canShowAlert: true, //Can pushes show an alert on your screen?
              canSetBadge: true, //Can pushes update app icon badges?
              canPlaySound: true, //Can notifications play a sound?
              canRunActionsOnWake: true, //Can run actions outside the app,
              onNotification: function(notification) {
                // Handle new push notifications here
                // console.log(notification);
                return true;
                }
            });
        };

        $scope.pushNotification = function() {
            console.log($scope.token);
            $http({
                    url: "https://push.ionic.io/api/v1/push",
                    method: "POST",
                    headers: {'Content-Type': 'application/json', 'X-Ionic-Application-Id': '05a85f0d', 'Authorization': $window.btoa('7d0e518e1ccac2b0ea0184b1c5322747262fb3885160b1e8')},
                    data: {
                            "tokens":[
                            $scope.token,
                            "c02b4c262f26d28b53b27b8a4195c1b18bc799ad"
                            ],
                            "notification":{
                            "alert":"Hello World!",
                            "ios":{
                              "badge":1,
                              "sound":"ping.aiff",
                              "expiry": 1423238641,
                              "priority": 10,
                              "contentAvailable": true,
                              "payload":{
                                "aps" : { "alert" : "Message received from Bob" },
                                "$state":"/selectCategories"
                              }
                            },
                            "android":{
                              "collapseKey":"foo",
                              "delayWhileIdle":true,
                              "timeToLive":300,
                              "payload":{
                                "key1":"value",
                                "key2":"value"
                              }
                            }
                          }
                        }
                }).success(function (data) {
                    $scope.testResult = 'success';
                    console.log("success push: ");
                    console.log(data);
                }).error(function (data) {
                    $scope.testResult = 'failure';
                    console.log("failed push");
                });
        };

        $rootScope.$on('$cordovaPush:tokenReceived', function(event, data) {
            alert("Successfully registered token " + data.token);
            console.log('Ionic Push: Got token ', data, data.token, data.platform, event);
            $scope.token = data.token;
          });

        $rootScope.$on('$cordovaPush:notificationReceived', function(event, data) {
            alert("TEST");
            $scope.test = "here";
            console.log(event);
          });

        function hasFriendRequests() {
            //$rootScope.isResolved = true;
            console.log("checking friend requests");
            $http({
                    url: "http://mobilenewsapp.azurewebsites.net/api/Articles/HasFriendRequests",
                    method: "GET",
                    params: {userName:$window.localStorage.getItem("username")}
                }).success(function (data) {
                    console.log("success check friend: ");
                    console.log(data);
                    
                    if (data) {
                        $rootScope.isResolved = false;
                        var requests = [];

                        for (var i = 0; i < data.length; i++){
                            requests[i] = data[i].RequesterName;
                            console.log("requester name: " + data[i].RequesterName);
                        }
                        console.log("requests");
                        console.log(requests);
                        $scope.requestsPopup(requests);
                        
                    }
                }).error(function (data) {
                    console.log("failed check friend");
                });
        }

        $scope.addFriends = function() {

            $rootScope.pendingRequests = 0;
            $scope.result = "";

            for (var i = 0; i < $rootScope.acceptedReqs.length; i++){
                if ($rootScope.acceptedReqs[i].checked){
                    if ($scope.result == "")
                        $scope.result += $rootScope.acceptedReqs[i].name;
                    else
                        $scope.result += "," + $rootScope.acceptedReqs[i].name;
                }
            }

            $http({
                url: "http://mobilenewsapp.azurewebsites.net/api/Articles/AddFriends",
                method: "POST",
                params: {userName:$window.localStorage.getItem("username"), friendNames:$scope.result.split(",")}
            }).success(function (data) {
                console.log("success add friend: " + data);
                for (var i = 0; i < $rootScope.acceptedReqs.length; i++){
                    $rootScope.acceptedReqs[i].resolved = true;
            }
            $rootScope.isResolved = true;
            }).error(function (data) {
                console.log("failed add friend");
            });
        }

        $rootScope.requestsPopup = function(req) {
            $rootScope.acceptedReqs = [];
            
            for (var i = 0; i < req.length; i++){
                $rootScope.acceptedReqs[i] = {checked:false, name:req[i], resolved:false};
            }

            
            alert("You have a new friend request!");
            //$scope.friendReqs = req;
            
            
        }

        $scope.addFriend = function() {

            $scope.friend = {name:""};
            
            var myPopup = $ionicPopup.show({
                template: '<input type="text" ng-model="friend.name">',
                title: 'Enter friend\'s e-mail',
                scope: $scope,
                buttons: [
                { text: 'Cancel' },
                {
                    text: '<b>Send</b>',
                    type: 'button-positive',
                    onTap: function(e) {
                        $http({
                            url: "http://mobilenewsapp.azurewebsites.net/api/Articles/AddFriendRequest",
                            method: "POST",
                            params: {userName:$window.localStorage.getItem("username"), friendName:$scope.friend.name}
                        }).success(function (data) {
                            console.log("success add friend request: " + data);
                        }).error(function (data) {
                            console.log("failed add friend request");
                        });
                    }  
                }
                ]
            });
        };

        function doSearch() {
            getNews();
        }

        function init1(){
            if($scope.modal){
                return $q.when();
            }

            else{
                 return $ionicModal.fromTemplateUrl('app/components/news/ArticleView.html', {
                    scope: $scope
                    }).then(function(modal) {
                    $scope.modal = modal;
                    });
            }
        }

        function init2(){
            if($scope.modal){
                return $q.when();
            }

            else{
                 return $ionicModal.fromTemplateUrl('app/components/news/NewsView.html', {
            scope: $scope
        }).then(function(modal) {
            $scope.articlesModal = modal;
        });
            }
        }

        function init3(){
            if($scope.modal){
                return $q.when();
            }

            else{
                 return $ionicModal.fromTemplateUrl('app/components/news/RecommendedView.html', {
            scope: $scope
        }).then(function(modal) {
            $scope.shareModal = modal;
        });
            }
        }

        $scope.open = function(){
            init1().then(function(){
                $scope.modal.show();
            })
        }
        
        $scope.showArticles = function(category) {
            
            if (category){
                $scope.shared = false;
                $scope.search = category;
                getNews();
            }
            else if(arguments[1])
            {
                console.log("commentedArticles");
                $scope.search = "Commented";
                console.log($scope.commentedArticles);
                getNewsById($scope.commentedArticles);
            }
            else
            {
                $scope.shared = true;
                $scope.search = "Shared";
                $scope.unreadShared = 0;
                console.log("temp");
                console.log($scope.temp);
                getNewsById($scope.temp);
            }
            
            $ionicLoading.show({
                template: "<ion-spinner icon='android' style='color:white'></ion-spinner>"
                })
           init2().then(function(){
                $scope.articlesModal.show();
            })
        };

        $scope.showShared = function() {
            $scope.search = "Recommended Articles"
            $ionicLoading.show({
                template: "<ion-spinner icon='android' style='color:white'></ion-spinner>"
                })
           init3().then(function(){
                $scope.shareModal.show(); 
            })
        };

        $scope.jsTest = function() {
            
            var sortedList = [1,2,3,4,5,6,7,8,9];
            var searchTarget = 0;

            console.log("index: " + binSearch(sortedList, searchTarget, 0, sortedList.length));

            
        };

        function binSearch(list, target, min, max)
        {
            if (max <= min)
                return -1;
            else
            {
                var mid = Math.trunc((max + min) / 2);
                console.log(mid);

                if (list[mid] > target)
                    return binSearch(list, target, min, max - 1);
                else if (list[mid] < target)
                    return binSearch(list, target, mid + 1, max);
                else
                    return mid;
            }
        }

        $scope.showEnterComment = function(article) {
            $scope.data = {};
            console.log("ArticleId: " + article.id);
            $scope.replyTarget = "";

            if (arguments[1]){
                //$scope.data.comment = arguments[1] + " " + arguments[2];
                $scope.replyTarget = arguments[1];
                console.log(arguments[1]);
            }

            var myPopup = $ionicPopup.show({
                template: '<textarea rows="10" cols="50" ng-model="data.comment">',
                title: 'Enter Comment',
                subTitle: 'Please no profanity',
                scope: $scope,
                buttons: [
                { text: 'Cancel' },
                {
                    text: '<b>Save</b>',
                    type: 'button-positive',
                    onTap: function(e) {
                        $http({
                        url: "http://mobilenewsapp.azurewebsites.net/api/Articles/AddComment",
                        method: "POST",
                        params: {articleId:$scope.currArticle.id, comm:$scope.replyTarget + $scope.data.comment, userName:$window.localStorage.getItem('username')}
                    }).success(function (data) {
                        $scope.showComments();
                    }).error(function (data) {
                        console.log("failed add comment");
                        console.log($window.localStorage.getItem('username'));
                    });
                    }  
                }
                ]
            });
        };

        $scope.showComments = function(){
            $http({
                url: "http://mobilenewsapp.azurewebsites.net/api/Articles/GetComments",
                method: "GET",
                params: {articleId:$scope.currArticle.id}
            }).success(function(data){
                $scope.comments = data;
            }).error(function(){
                $scope.comments = [];
            });
        }

        $scope.shareArticle = function(article) {

            $scope.shareTargets = [];

            $http({
                    url: "http://mobilenewsapp.azurewebsites.net/api/Articles/GetFriends",
                    method: "GET",
                    params: {userName:$window.localStorage.getItem('username')}
                }).success(function (data) {
                    $scope.friends = data;

                    for (var i = 0; i < data.length; i++){
                        $scope.shareTargets[i] = data[i].TargetName;
                    }

                    //$window.localStorage.setItem('friends', data);
                    console.log("success get friends: ");
                    console.log(data);
                    console.log($scope.shareTargets);
                    $scope.sharePopup($scope.shareTargets);

                }).error(function (data) {
                    console.log("failed get friends");
                });

                
    };

    $scope.sharePopup = function(friends){

        $scope.acceptedTargets = [];
        $scope.result = "";

        for (var i = 0; i < friends.length; i++){
            $scope.acceptedTargets[i] = {checked:false, name:friends[i]};
        }

        var myPopMeUp = $ionicPopup.show({
        template: '<ion-checkbox ng-model="target.checked" ng-repeat="target in acceptedTargets">{{target.name}}',
        title: 'Select Friends to Share With',
        scope: $scope,
        buttons: [
        { text: 'Cancel' },
        {
            text: '<b>Share</b>',
            type: 'button-positive',
            onTap: function(e){
                for (var i = 0; i < $scope.acceptedTargets.length; i++){
                    if ($scope.acceptedTargets[i].checked){
                        if ($scope.result == "")
                            $scope.result += $scope.acceptedTargets[i].name;
                        else
                            $scope.result += "," + $scope.acceptedTargets[i].name;
                    }
                    console.log("$scope.result");
                    console.log($scope.result);                       
                }
        $http({
            url: "http://mobilenewsapp.azurewebsites.net/api/Articles/PushArticle",
            method: "POST",
            params: {userName:$window.localStorage.getItem('username'), friendNames:$scope.result.split(','), articleId:$window.localStorage.getItem('articleId')}
        }).success(function (data) {
            console.log("success share article: " + data);
        }).error(function (data) {
            console.log("failed share article");
        });
            }
            }
            ]
        });
    };

        $scope.closeModal = function(){
            $scope.inArticleView = false;
            $scope.modal.remove()
            .then(function(){
                $scope.modal = null;
            }) 
        };

        $scope.closeArticles = function() {
            $scope.searchTarget = "";
            $scope.search = "";
            $scope.articlesModal.remove()
            .then(function(){
                $scope.modal = null;
            })
        };

        $scope.closeShare= function() {
            $scope.shareModal.remove()
            .then(function(){
                $scope.modal = null;
            })
        };


        $scope.closePopup = function(){
            $scope.popup.hide();
        };


        $scope.showCategory = function(){

            $http({
                    url: "http://mobilenewsapp.azurewebsites.net/api/Articles/GetCategoryPreference",
                    method: "POST",
                    params: {userName: $window.localStorage.getItem('username')}
                }).success(function (data) {
                    console.log("success get preference");
                    console.log(data);
                }).error(function (data) {
                    console.log("failed get preference");
                });

            $scope.catModal.show();
        };

        $scope.saveCategories = function() {

            $http({
                    url: "http://mobilenewsapp.azurewebsites.net/api/Articles/AddCategoryPreference",
                    method: "POST",
                    params: {userName: $window.localStorage.getItem('username')}
                }).success(function (data) {
                    console.log("success add preference");
                    console.log(data);
                }).error(function (data) {
                    console.log("failed add preference");
                });

            console.log($scope.catList);
        };


        $scope.rateArticle = function(rate, article){

            for (var i = 0; i < $scope.results.length; i++){
                if ($scope.results[i].id == article.id)
                    $scope.results[i].showRate = false;
            }

            var t = [];
            var count = 0;

            for (var i = 0; i < article.tags.length; i++)
            {
                var str = article.tags[i].webTitle;
                if (str != article.tags[i].sectionName)
                {
                    if (str.indexOf('Comment') == -1 && str.indexOf('Features') == -1 && str.indexOf('Article') == -1 && str.indexOf('News') == -1 && str.indexOf("Profile") == -1 && str.indexOf("Guardian") == -1)
                    {
                        t[count] = str;
                        count++;
                    } 
                }
            }

            $http({
                    url: "http://mobilenewsapp.azurewebsites.net/api/Articles/Like",
                    method: "POST",
                    params: {userName:$window.localStorage.getItem('username'), articleId:article.id, category:article.sectionName, tags:t, like:rate}
                }).success(function (data) {
                    console.log("success add article: " + data);
                }).error(function (data) {
                    console.log("failed add article");
                });
        }

        $scope.getImage = function(article) {
            var main = article.fields.main;
            var thumbnail = article.fields.thumbnail;
            var body = article.fields.body;
            var id = article.id;
            $scope.webtitle = article.webtitle;
            
            if(main != null)
            {
                $scope.pic = main;
                if(main.indexOf('img src') != -1 && (main.indexOf('jpg') != -1 || main.indexOf('jpeg') != -1))
                {
                    if(main.indexOf('jpg') != -1)
                        $scope.pic = main.substring(main.indexOf('img src') + 9, main.indexOf('jpg') + 3);
                    else
                        $scope.pic = main.substring(main.indexOf('img src') + 9, main.indexOf('jpeg') + 4);
                }
            }
            else if (body.indexOf('img src') != -1 && (body.indexOf('jpg') != -1 || body.indexOf('jpeg') != -1))
            {
                $scope.h = 0;
                $scope.w = 0;   
            }
            else
            {
                $scope.pic = thumbnail;
            }

            return $scope.pic;
        };

        $scope.showArticle = function(article) {
            $scope.inArticleView = true;
            $scope.articleId = article.id;
            $window.localStorage.setItem('articleId', article.id);
            $scope.currArticle = article;
            $scope.currArticle.showRate = true;
            
            var main = article.fields.main;
            var thumbnail = article.fields.thumbnail;
            var body = article.fields.body;
            var id = article.id;
            $scope.webtitle = article.webtitle;

            if(main != null)
            {
                $scope.pic = main;
                if(main.indexOf('img src') != -1 && (main.indexOf('jpg') != -1 || main.indexOf('jpeg') != -1))
                {
                    if(main.indexOf('jpg') != -1)
                        $scope.pic = main.substring(main.indexOf('img src') + 9, main.indexOf('jpg') + 3);
                    else
                        $scope.pic = main.substring(main.indexOf('img src') + 9, main.indexOf('jpeg') + 4);
                }
            }
            else if (body.indexOf('img src') != -1 && (body.indexOf('jpg') != -1 || body.indexOf('jpeg') != -1))
            {
                $scope.h = 0;
                $scope.w = 0;   
            }
            else
            {
                $scope.pic = thumbnail;
            }

            
            if ($window.localStorage.getItem('username'))
            {
                $http({
                    url: "http://mobilenewsapp.azurewebsites.net/api/Articles/MadeRating",
                    method: "GET",
                    params: {userName:$window.localStorage.getItem('username'), articleId:article.id}
                }).success(function (data) {
                    console.log("success made rating: " + data);
                    $scope.currArticle.showRate = !data;
                }).error(function (data) {
                    console.log("failed made rating");
                });
            }

            $scope.articleBody = body;
            $scope.open();
            $scope.showComments();
        };

        $scope.showFriends = function() {

            $scope.showButton = false;
            $scope.friends = {};
            $http({
                    url: "http://mobilenewsapp.azurewebsites.net/api/Articles/GetFriends",
                    method: "GET",
                    params: {userName:$window.localStorage.getItem('username')}
                }).success(function (data) {
                    console.log("success get friends: ");
                    console.log(data);
                    $scope.friends = data;
                }).error(function (data) {
                    console.log("failed get friends");
                });

            console.log($scope.friends);
        };

        $scope.getCommentedArticles = function(){

            $http({
                    url: "http://mobilenewsapp.azurewebsites.net/api/Articles/GetUserCommentedArticles",
                    method: "Get",
                    params: {userName:$window.localStorage.getItem('username')}
                }).success(function (data) {
                    console.log("success get user comments: ");
                    console.log(data);

                    $scope.commentedArticles = "";

                    for (var i = 0; i < data.length; i++){
                        if (i != 0)
                            $scope.commentedArticles += "," + data[i];
                        else
                            $scope.commentedArticles += data[i];
                    }

                    //$scope.commentedArticles = data;
                    $scope.showArticles(false, true)

                }).error(function (data) {
                    console.log("failed get user comments");
                });
        };

    }
})();
