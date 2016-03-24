var explorerControllers = angular.module('explorerControllers', []);

explorerControllers.controller('FileListController', function (api, $scope, $rootScope) {
	api.getUser().then(function (data) {
		$scope.user = data;
	});

	api.getRecentFiles().then(function (data) {
		$scope.files = data;
	});
});

explorerControllers.controller('FileDetailController', function (api, $scope, $rootScope) {
	$scope.skipPrev = function (value) {
		$rootScope.$broadcast('skipPrev', value);
		
		alert('skipPrev');
	}

	$scope.togglePlay = function (value) {
		$rootScope.$broadcast('togglePlay', value);
		
		alert('togglePlay');
	}

	$scope.skipNext = function (value) {
		$rootScope.$broadcast('skipNext', value);
		
		alert('skipNext');
	}

	$scope.$on('someEvent', function (event, args) {
	});
});