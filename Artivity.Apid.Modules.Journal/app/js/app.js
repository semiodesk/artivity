var explorerApp = angular.module('explorerApp', [
    'ngRoute',
    'explorerControllers',
    'ui.bootstrap',
    'pascalprecht.translate',
	'angularMoment',
	'cfp.hotkeys'
]);

explorerApp.config(['$locationProvider', '$routeProvider',
  function ($locationProvider, $routeProvider) {
	  //$locationProvider.hashPrefix('#!');

	$routeProvider.
		when('/load', {
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
			redirectTo: '/load'
		});
}]);

explorerApp.factory('fileService', function () {
	return {
		getFileName: function (fileUrl) {
			var url = fileUrl;

			// Remove the anchor at the end, if there is one
			url = url.substring(0, (url.indexOf("#") == -1) ? url.length : url.indexOf("#"));

			// Remove the query after the file name, if there is one
			url = url.substring(0, (url.indexOf("?") == -1) ? url.length : url.indexOf("?"));

			// Remove everything before the last slash in the path
			url = url.substring(url.lastIndexOf("/") + 1, url.length);

			return decodeURIComponent(url);
		},
		getFileNameWithoutExtension: function (fileName) {
			var components = fileName.split('.');

			// Return the file name which does not contain any dot.
			return components[0];
		},
		getFileExtension: function (fileName) {
			var components = fileName.split('.');

			if (components.length == 1) {
				// Return the file name which does not contain any dot.
				return components[0];
			} else {
				// Return all components which are seperated by a dot. i.e. 'tar.gz'
				return components.slice(1).join('.');
			}
		}
	};
});

explorerApp.factory('clientService', function() {
	return {
		pollServiceState : function (api, sessionId, handleClientStateChange) {
			// The polling interval hanler.
			var interval = undefined;

			// The polling interval in milliseconds.
			var intervalMs = 500;

			// The maximum number of queries.
			var maxQueries = 500;

			// The number of status queries.
			var n = 0;

			// Stop polling if errors occur.
			var onError = function (data) {
				if (interval) {
					clearInterval(interval);
				}
			};

			// Query the current client status in a regular interval.
			var interval = setInterval(function () {
				api.getAccountClientStatus(sessionId, onError).then(function (data) {
					var client = data;

					if (!client) {
						// The sessionId was not found, no need to query again.
						clearInterval(interval);
					}

					n++;

					if (handleClientStateChange) {
						handleClientStateChange(interval, client);
					}

					if (n === maxQueries) {
						clearInterval(interval);
					}
				});
			}, intervalMs);
		}
	}
});

function serialize(obj, prefix) {
	var str = [];

	for (var p in obj) {
		if (obj.hasOwnProperty(p)) {
			var k = prefix ? prefix + "[" + p + "]" : p,
				v = obj[p];
			str.push(typeof v == "object" ?
				serialize(v, k) :
				encodeURIComponent(k) + "=" + encodeURIComponent(v));
		}
	}

	return str.join("&");
}

function getValue(obj, path) {
	for (var i = 0, path = path.split('.'), len = path.length; i < len; i++) {
		if (obj === undefined) {
			break;
		}

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

// Warn if overriding existing method
if (Array.prototype.equals) {
	console.warn("Overriding existing Array.prototype.equals. Possible causes: New API defines the method, there's a framework conflict or you've got double inclusions in your code.");
}

// attach the .equals method to Array's prototype to call it on any array
Array.prototype.equals = function (array) {
	// if the other array is a falsy value, return
	if (!array)
		return false;

	// compare lengths - can save a lot of time 
	if (this.length != array.length)
		return false;

	for (var i = 0, l = this.length; i < l; i++) {
		// Check if we have nested arrays
		if (this[i] instanceof Array && array[i] instanceof Array) {
			// recurse into the nested arrays
			if (!this[i].equals(array[i]))
				return false;
		} else if (this[i] != array[i]) {
			// Warning - two different object instances will never be equal: {x:20} != {x:20}
			return false;
		}
	}
	return true;
}

// Hide method from for-in loops
Object.defineProperty(Array.prototype, "equals", {
	enumerable: false
});