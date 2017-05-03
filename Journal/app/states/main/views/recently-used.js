(function () {
    angular.module('app').controller("RecentlyUsedViewController", RecentlyUsedViewController);

    RecentlyUsedViewController.$inject = ['$scope', '$state', '$stateParams', 'api'];

    function RecentlyUsedViewController($scope, $state, $stateParams, api) {
        var t = this;

        t.getRecentlyUsedFiles = function (callback) {
            return api.getRecentFiles().then(callback);
        }
    }
})();