(function () {
	
	angular.module('NewsApp')
		.controller('LoginController', [ '$scope', 'LoginService', '$location', '$ionicPopup', '$ionicModal','$window', 'RegisterService', LoginController]);
	
	function LoginController( $scope, LoginService, $location, $ionicPopup, $ionicModal, $window, RegisterService){
		$scope.login = login;
		$scope.logoutUser = logoutUser
		$scope.registerUser = registerUser;
		$scope.isLoggedIn = isLoggedIn;

		function isLoggedIn(){
			return LoginService.isLoggedIn();
		}

		 $ionicModal.fromTemplateUrl('app/components/login/Login.html', {
		 	id: '1',
    		scope: $scope
  		}).then(function(modal) {
    		$scope.oModal1 = modal;
  		});

  		function logoutUser(){
  			LoginService.logout();
  		}

		function login(){
			LoginService.login($scope.user.username, $scope.user.password).then(success1, fail1);
		}

		function success1(){
			$scope.oModal1.hide();
			//$location.path('/news');
			
            $window.localStorage.setItem('username', $scope.user.username);
			$location.path('/selectCategories');
		}

		function fail1(data){
			console.log('Failed to login', data)
		}

		function registerUser(){
			RegisterService.register(
					$scope.regUser.regEmail,
					$scope.regUser.regPassword,
					$scope.regUser.regConfirmPassword
				).then(success2, fail2)
		}

		function success2(){
			$ionicPopup.alert({
				title: '<h3>Thank you for registering, you now have access to new features.</h3>'
			})
			$scope.oModal2.hide();
			//$scope.oModal1.show();
			//$location.path('#/app/login')
			$scope.user = {username:$scope.regUser.regEmail, password:$scope.regUser.regPassword};
			login();
		}

		function fail2(data){
			console.log('Failed to register', data)
		}

		$ionicModal.fromTemplateUrl('app/components/login/RegisterView.html', {
			id: '2',
    		scope: $scope
  		}).then(function(modal) {
    		$scope.oModal2 = modal;
  		});

  		$scope.closeModal1 = function() {
    		$scope.oModal1.hide();
  		};

  		$scope.closeModal2 = function() {
    		$scope.oModal2.hide();
  		};

  
  		$scope.openModal1 = function() {
  			$scope.user = {username:"", password:""};
  			$scope.oModal1.show();
  		};

  		$scope.openModal2 = function() {
  			$scope.regUser = {username:"", password:""}
  			$scope.oModal2.show();
  		};
	}
})();