(function (){

	angular.module('NewsApp')
		.factory('RegisterService', ['$http', '$q', '$window', '$ionicPopup', RegisterService]);

	function RegisterService($http, $q, $window, $ionicPopup){
		var service = {};
		service.register = register;

		function register(email, password, confirmPassword){
			var deferred = $q.defer()

			$http({
				url: 'http://mobilenewsapp.azurewebsites.net/api/Account/Register',
				method: 'POST',
				data: {
					'email': email,
					'password': password,
					'confirmPassword': confirmPassword
				}
			}).success(function (data){
				deferred.resolve();
			}).error(function (data){
				var errorMessage = [];
					
					for(var dat in data.ModelState){
						errorMessage.push(data.ModelState[dat][0]);
					}
				$ionicPopup.alert({
					title: '<h5>Registration Failed</h5>',
					template:'<h5 class="text-center">' + errorMessage + '</h5>'})
				deferred.reject(data);
			})

			return deferred.promise;
		}

		return service;
	}
})();