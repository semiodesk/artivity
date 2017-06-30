(function () {
    angular.module('app').controller("RecentlyUsedViewController", RecentlyUsedViewController);

    RecentlyUsedViewController.$inject = ['$rootScope', '$scope', '$state', '$stateParams', 'api'];

    function RecentlyUsedViewController($rootScope, $scope, $state, $stateParams, api) {
        var t = this;

        t.findFiles = function (query) {
            $rootScope.$broadcast('search', query);
        }

        t.getRecentlyUsedFiles = function (callback) {
            return api.getRecentFiles().then(callback);
        }

        t.getFiles = function() {
            return api.getRecentFiles().then(function(data) {
                return data;
            });
        }
    }
})();