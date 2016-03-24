var explorerApp = angular.module('explorerApp', [
    'ngRoute',
    'explorerControllers'
]);

explorerApp.config(['$routeProvider',
  function($routeProvider) {
    $routeProvider.
      when('/files/recent', {
        templateUrl: 'partials/file-list.html',
        controller: 'FileListController'
      }).
	  when('/files/view', {
        templateUrl: 'partials/file-detail.html',
        controller: 'FileDetailController'
      }).
      otherwise({
        redirectTo: '/files/recent'
      });
  }]);

explorerApp.factory('api', function ($http) {
    var endpoint = 'http://localhost:8262/artivity/1.0/api';
    
    return {
		getUser: function () {
            return $http.get(endpoint + '/agents/user').then(
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
        getFileHistory: function (fileUrl) {
            return $http.get(endpoint + '/activities?fileUrl=' + fileUrl).then(
                function (response) {
                    return response.data;
                })
        }
    };
});