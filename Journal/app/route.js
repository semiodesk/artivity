(function () {
	angular.module('app').config(['$locationProvider', '$routeProvider',
		function ($locationProvider, $routeProvider) {
			$routeProvider.
			when('/files', {
				templateUrl: 'app/views/file-list/file-list.html',
				controller: 'FileListController',
				controllerAs: 't'
			}).
			when('/files/view', {
				templateUrl: 'app/views/file-view/file-view.html',
				controller: 'FileViewController',
				controllerAs: 't'
			}).
			when('/files/preview', {
				templateUrl: 'app/views/file-preview/file-preview.html',
				controller: 'FilePreviewController',
				controllerAs: 't'
			}).
			when('/settings', {
				templateUrl: 'app/views/settings/settings.html',
				controller: 'SettingsController',
				controllerAs: 't'
			}).
			when('/setup', {
				templateUrl: 'app/views/setup/setup.html',
				controller: 'SetupController',
				controllerAs: 't'
			}).
			when('/start', {
				templateUrl: 'app/views/start/start.html',
				controller: 'StartController',
				controllerAs: 't'
			}).
			when('/query', {
				templateUrl: 'app/views/query/query.html',
				controller: 'QueryController',
				controllerAs: 't'
			}).
			otherwise({
				redirectTo: '/start'
			});
		}
	]);
    
})();