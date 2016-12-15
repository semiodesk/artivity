var explorerApp = angular.module('explorerApp', [
	'ngAnimate',
	'ngInputModified',
    'ngRoute',
	'angularMoment',
	'ui.bootstrap',
	'ui.bootstrap.modal',
	'ui.bootstrap.progressbar',
    'pascalprecht.translate',
	'cfp.hotkeys'
]);

explorerApp.config(['$locationProvider', '$routeProvider',
  function ($locationProvider, $routeProvider) {
	  //$locationProvider.hashPrefix('#!');

	$routeProvider.
		when('/files', {
			templateUrl: 'partials/file-list.html',
			controller: 'FileListController',
			controllerAs: 't'
		}).
		when('/files/view', {
			templateUrl: 'partials/file-view.html',
			controller: 'FileViewController',
			controllerAs: 't'
		}).
		when('/settings', {
			templateUrl: 'partials/settings.html',
			controller: 'SettingsController',
			controllerAs: 't'
		}).
		when('/setup', {
			templateUrl: 'partials/setup.html',
			controller: 'SetupController',
			controllerAs: 't'
		}).
		when('/start', {
			templateUrl: 'partials/start.html',
			controller: 'StartController',
			controllerAs: 't'
		}).
		when('/query', {
			templateUrl: 'partials/query.html',
			controller: 'QueryController',
			controllerAs: 't'
		}).
		otherwise({
			redirectTo: '/start'
		});
}]);