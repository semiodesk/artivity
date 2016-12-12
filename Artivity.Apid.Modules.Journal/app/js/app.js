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
		when('/start', {
			templateUrl: 'partials/start.html',
			controller: 'StartController'
		}).
		when('/files', {
			templateUrl: 'partials/file-list.html',
			controller: 'FileListController'
		}).
		when('/files/view', {
			templateUrl: 'partials/file-view.html',
			controller: 'FileViewController'
		}).
		when('/settings', {
			templateUrl: 'partials/settings.html',
			controller: 'SettingsController'
		}).
		when('/query', {
			templateUrl: 'query.html'
		}).
		otherwise({
			redirectTo: '/start'
		});
}]);