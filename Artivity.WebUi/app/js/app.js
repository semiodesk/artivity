var explorerApp = angular.module('explorerApp', [
    'ngRoute',
    'explorerControllers'
]);

explorerApp.config(['$routeProvider',
  function ($routeProvider) {
		$routeProvider.
		when('/files', {
			templateUrl: 'partials/file-list.html',
			controller: 'FileListController'
		}).
		when('/files/:fileUrl', {
			templateUrl: 'partials/file-detail.html',
			controller: 'FileDetailController'
		}).
		when('/settings', {
			templateUrl: 'partials/settings.html',
			controller: 'SettingsController'
		}).
		otherwise({
			redirectTo: '/files'
		});
  }]);

explorerApp.factory('api', function ($http) {
	var endpoint = 'http://localhost:8262/artivity/1.0/api';

	return {
		getAgents: function () {
			return $http.get(endpoint + '/agents').then(
				function (response) {
					return response.data;
				})
		},
		getAgent: function (fileUrl) {
			return $http.get(endpoint + '/agents?fileUrl=' + fileUrl).then(
				function (response) {
					return response.data;
				})
		},
		getUser: function () {
			return $http.get(endpoint + '/agents/user').then(
				function (response) {
					response.data['PhotoUrl'] = endpoint + '/agents/user/photo';

					return response.data;
				})
		},
		setUser: function (data) {
			return $http.post(endpoint + '/agents/user', data);
		},
		getFile: function (fileUrl) {
			return $http.get(endpoint + '/files?fileUrl=' + fileUrl).then(
				function (response) {
					return response.data;
				})
		},
		getRecentFiles: function () {
			return $http.get(endpoint + '/files/recent').then(
				function (response) {
					return response.data;
				})
		},
		getActivities: function (fileUrl) {
			return $http.get(endpoint + '/activities?fileUrl=' + fileUrl).then(
				function (response) {
					return response.data;
				})
		},
		getInfluences: function (fileUrl) {
			return $http.get(endpoint + '/influences?fileUrl=' + fileUrl).then(
				function (response) {
					return response.data;
				})
		}
	};
});

function getFileName(fileUrl) {
	var url = fileUrl;

	// Remove the anchor at the end, if there is one
	url = url.substring(0, (url.indexOf("#") == -1) ? url.length : url.indexOf("#"));

	// Remove the query after the file name, if there is one
	url = url.substring(0, (url.indexOf("?") == -1) ? url.length : url.indexOf("?"));

	// Remove everything before the last slash in the path
	url = url.substring(url.lastIndexOf("/") + 1, url.length);

	return decodeURIComponent(url);
}

function getValue(obj, path){
	for (var i=0, path=path.split('.'), len=path.length; i<len; i++){
		obj = obj[path[i]];
	}
	return obj;
}