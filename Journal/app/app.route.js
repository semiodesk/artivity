var explorerApp = angular.module('explorerApp', [
	'ngAnimate',
	'ngInputModified',
	'ngRoute',
	'angularMoment',
	'ui.bootstrap',
	'ui.bootstrap.modal',
	'ui.bootstrap.progressbar',
	'pascalprecht.translate',
	'cfp.hotkeys',
	'ngDragDrop'
]);

explorerApp.config(['$locationProvider', '$routeProvider',
	function ($locationProvider, $routeProvider) {
		$routeProvider.
		when('/files', {
			templateUrl: 'app/partials/file-list.html',
			controller: 'FileListController',
			controllerAs: 't'
		}).
		when('/files/view', {
			templateUrl: 'app/partials/file-view.html',
			controller: 'FileViewController',
			controllerAs: 't'
		}).
		when('/files/preview', {
			templateUrl: 'app/partials/file-view-simple.html',
			controller: 'FileViewSimpleController',
			controllerAs: 't'
		}).
		when('/settings', {
			templateUrl: 'app/partials/settings.html',
			controller: 'SettingsController',
			controllerAs: 't'
		}).
		when('/setup', {
			templateUrl: 'app/partials/setup.html',
			controller: 'SetupController',
			controllerAs: 't'
		}).
		when('/start', {
			templateUrl: 'app/partials/start.html',
			controller: 'StartController',
			controllerAs: 't'
		}).
		when('/query', {
			templateUrl: 'app/partials/query.html',
			controller: 'QueryController',
			controllerAs: 't'
		}).
		otherwise({
			redirectTo: '/start'
		});
	}
]);