(function () {
	angular.module('app').config(['$stateProvider', '$urlRouterProvider',
		function ($stateProvider, $urlRouterProvider) {
			$stateProvider
				.state('main', {
					url: '/main',
					templateUrl: 'app/states/main/main.html',
					controller: 'MainStateController as t'
				})
				.state('main.view', {
					url: '/{index:int}',
					template: "<ui-view></ui-view>",
					abstract: true
				})
				.state('main.view.recently-used', {
					url: '/recently-used',
					templateUrl: 'app/states/main/views/recently-used.html',
					controller: 'RecentlyUsedViewController as t'
				})
				.state('main.view.project-dashboard', {
					url: '/project-dashboard',
					templateUrl: 'app/states/main/views/project-dashboard.html',
					controller: 'ProjectDashboardViewController as t',
					params: {
						project: null
					}
				})
				.state('main.view.document', {
					url: '/document/{fileUri:string}',
					templateUrl: 'app/states/main/views/document.html',
					controller: 'DocumentViewController as t',
					params: {
						project: null,
						fileUri: null
					}
				})
				.state('main.view.document-history', {
					url: '/document-history/{fileUri:string}',
					templateUrl: 'app/states/main/views/document-history.html',
					controller: 'DocumentHistoryViewController as t',
					params: {
						project: null,
						fileUri: null
					}
				})
				.state('settings', {
					url: '/settings',
					templateUrl: 'app/states/settings/settings.html',
					controller: 'SettingsStateController as t',
					abstract: true
				})
				.state('settings.profile', {
					url: '/profile',
					views: {
						'tab-content': {
							templateUrl: 'app/states/settings/views/profile.html',
							controller: 'ProfileSettingsViewController as t'
						}
					}
				})
				.state('settings.apps', {
					url: '/apps',
					views: {
						'tab-content': {
							templateUrl: 'app/states/settings/views/apps.html',
							controller: 'AppsSettingsViewController as t'
						}
					}
				})
				.state('settings.accounts', {
					url: '/accounts',
					views: {
						'tab-content': {
							templateUrl: 'app/states/settings/views/accounts.html',
							controller: 'AccountsSettingsViewController as t'
						}
					}
				})
				.state('query', {
					url: '/query',
					templateUrl: 'app/states/query/query.html',
					controller: 'QueryStateController as t'
				})
				.state('setup', {
					url: '/setup',
					templateUrl: 'app/states/setup/setup.html',
					controller: 'SetupStateController as t'
				})
				.state('start', {
					url: '/',
					templateUrl: 'app/states/start/start.html',
					controller: 'StartStateController as t'
				});

			$urlRouterProvider.otherwise('/');
		}
	]);
})();