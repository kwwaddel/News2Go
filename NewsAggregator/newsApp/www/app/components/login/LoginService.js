(function () {
	
	angular.module('NewsApp')
		.factory('LoginService', ['$http', '$q', '$window','$ionicPopup', LoginService]);
	
	function LoginService($http, $q, $window, $ionicPopup){
		var service = {};
		service.login = login;
		service.logout = logout;
		service.isLoggedIn = isLoggedIn;

		function isLoggedIn(){
			return $window.sessionStorage.getItem('token')
		}

		function logout(){
			$window.sessionStorage.removeItem('token');
			$window.localStorage.removeItem('username');
		}

		function login(username, password){
			var deferred = $q.defer()

			$http({
				url: 'http://mobilenewsapp.azurewebsites.net/Token',
				method: 'POST',
				data: 'username=' + username + '&password=' + password + '&grant_type=password',
				headers: { 'Content-Type': 'application/x-www-form-urlencoded'}
			}).success(function (data){
				$window.sessionStorage.setItem('token', data.access_token);
				deferred.resolve();
			}).error(function (data){
				$ionicPopup.alert({
					title: '<h5>Invalid Login</h5>',
					template:'<h5>' +'<center>' +data.error_description +'</center>'+ '</h5>'})
				deferred.reject(data);
			})

			return deferred.promise;
		}

		return service;
	}
})();