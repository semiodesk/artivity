var explorerApp = angular.module('explorerApp', [
    'ngRoute',
    'explorerControllers',
    'ui.bootstrap'
]).config(function ($httpProvider) {
	$httpProvider.interceptors.push(function ($q) {
		return {
			responseError: function (rejection) {
				if (rejection.status <= 0) {
					window.location = "/app/index.html#/error-no-apid-connection";
					return;
				}
				return $q.reject(rejection);
			}
		};
	});
});

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
		when('/error-no-apid-connection', {
			templateUrl: 'partials/error-no-apid-connection.html',
			controller: 'ErrorController'
		}).
		otherwise({
			redirectTo: '/files'
		});
  }]);

explorerApp.factory('api', function ($http) {
	var endpoint = 'http://localhost:8262/artivity/api/1.0/';

	return {
		getAccounts: function () {
			return $http.get(endpoint + '/accounts/').then(
				function (response) {
					return response.data;
				})
		},
		getAccountProviders: function () {
			return $http.get(endpoint + '/accounts/providers').then(
				function (response) {
					return response.data;
				})
		},
		getAccountProvider: function (providerId) {
			return $http.get(endpoint + '/accounts/providers?providerId=' + providerId).then(
				function (response) {
					return response.data;
				})
		},
		installAccount: function (providerId) {
			return $http.get(endpoint + '/accounts/oauth2/redirect?providerId=' + providerId).then(
				function (response) {
					return response.data;
				})
		},
		uninstallAccount: function (accountId) {
			return $http.get(endpoint + '/accounts/uninstall?accountId=' + accountId);
		},
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
		setAgent: function (data) {
			return $http.post(endpoint + '/agents', data);
		},
		getUser: function () {
			return $http.get(endpoint + '/user').then(
				function (response) {
					return response.data;
				})
		},
		setUser: function (data) {
			return $http.post(endpoint + '/user', data);
		},
		setUserPhoto: function (data) {
			return $http.post(endpoint + '/user/photo', data);
		},
		getUserPhotoUrl: function () {
			return endpoint + '/user/photo';
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
		},
		getThumbnails: function (fileUrl, time) {
			return $http.get(endpoint + '/thumbnails?fileUrl=' + fileUrl + '&timestamp=' + time).then(
				function (response) {
					return response.data;
				})
		},
		getThumbnailUrl: function (thumbnailUrl) {
			return endpoint + '/thumbnails?thumbnailUrl=' + thumbnailUrl;
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

function getValue(obj, path) {
	for (var i = 0, path = path.split('.'), len = path.length; i < len; i++) {
		obj = obj[path[i]];
	}
	return obj;
}

function setValue(obj, path, value) {
	var p;

	for (var i = 0, path = path.split('.'), len = path.length - 1; i < len; i++) {
		obj = obj[path[i]];

		p = path[i + 1];
	}

	if (p) {
		obj[p] = value;
	}
}

function loadItems(items, action, done) {
	if (!items) {
		return;
	}

	// convert single item to array.
	if ("undefined" === items.length) {
		items = [items];
	}

	var count = items.length;

	// this callback counts down the things to do.
	var completed = function (items, i) {
		count--;

		if (0 == count) {
			done(items);
		}
	};

	// invoke each action, and await callback.
	for (var i = 0; i < items.length; i++) {
		action(items, i, completed);
	}
}