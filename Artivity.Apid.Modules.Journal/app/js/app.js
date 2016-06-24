var explorerApp = angular.module('explorerApp', [
    'ngRoute',
    'explorerControllers',
    'ui.bootstrap'
]).config(function ($httpProvider) {
    $httpProvider.interceptors.push(function ($q) {
        return {
            responseError: function (rejection) {
                if (rejection.status <= 0) {
                    window.location = "#/error-no-apid-connection";
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
            templateUrl: 'partials/file-list.html'
        }).
        when('/files/view', {
            templateUrl: 'partials/file-view.html'
        }).
        when('/settings', {
            templateUrl: 'partials/settings.html'
        }).
		when('/query', {
            templateUrl: 'partials/query.html'
        }).
        when('/error-no-apid-connection', {
            templateUrl: 'partials/error-no-apid-connection.html'
        }).
        otherwise({
            redirectTo: '/files'
        });
  }]);

explorerApp.factory('api', function ($http) {
    var endpoint = 'http://localhost:'.concat(port, '/artivity/api/1.0/');

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
        getAgent: function (entityUri) {
            return $http.get(endpoint + '/agents?uri=' + entityUri).then(
                function (response) {
                    return response.data;
                })
        },
        setAgent: function (data) {
            return $http.post(endpoint + '/agents', data);
        },
        getUser: function () {
            return $http.get(endpoint + '/agents/user').then(
                function (response) {
                    return response.data;
                })
        },
        setUser: function (data) {
            return $http.post(endpoint + '/agents/user', data);
        },
        setUserPhoto: function (data) {
            return $http.post(endpoint + '/agents/user/photo', data);
        },
        getUserPhotoUrl: function () {
            return endpoint + '/agents/user/photo';
        },
        getFile: function (entityUri) {
            return $http.get(endpoint + '/files?uri=' + entityUri).then(
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
        getCanvas: function (entityUri) {
            return $http.get(endpoint + '/files/canvas?uri=' + entityUri).then(
                function (response) {
                    return response.data;
                })
        },
        getActivities: function (entityUri) {
            return $http.get(endpoint + '/activities?uri=' + entityUri).then(
                function (response) {
                    return response.data;
                })
        },
        getInfluences: function (entityUri) {
            return $http.get(endpoint + '/influences?uri=' + entityUri).then(
                function (response) {
                    return response.data;
                })
        },
        getThumbnails: function (entityUri, time) {
            if (time !== undefined) {
                return $http.get(endpoint + '/renderings?uri=' + entityUri + '&timestamp=' + time).then(
                    function (response) {
                        return response.data;
                    })
            } else {
                return $http.get(endpoint + '/renderings?uri=' + entityUri).then(
                    function (response) {
                        return response.data;
                    })
            }
        },
        getThumbnailUrl: function (entityUri, fileName) {
            return endpoint + '/renderings?uri=' + entityUri + '&file=' + fileName;
        },
        getStats: function (entityUri, time) {
            if (time !== undefined) {
                return $http.get(endpoint + '/stats/influences?uri=' + entityUri + '&timestamp=' + time).then(
                    function (response) {
                        return response.data;
                    })

            } else {
                return $http.get(endpoint + '/stats/influences?uri=' + entityUri).then(
                    function (response) {
                        return response.data;
                    })
            }
        },
		getQueryResults(query) {
			return $http.post(endpoint + '/query', query).then(
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

// Warn if overriding existing method
if(Array.prototype.equals) {
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

    for (var i = 0, l=this.length; i < l; i++) {
        // Check if we have nested arrays
        if (this[i] instanceof Array && array[i] instanceof Array) {
            // recurse into the nested arrays
            if (!this[i].equals(array[i]))
                return false;       
        }           
        else if (this[i] != array[i]) { 
            // Warning - two different object instances will never be equal: {x:20} != {x:20}
            return false;   
        }           
    }       
    return true;
}

// Hide method from for-in loops
Object.defineProperty(Array.prototype, "equals", {enumerable: false});