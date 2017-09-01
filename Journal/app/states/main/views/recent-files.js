(function () {
    angular.module('app').controller("RecentFilesViewController", RecentFilesViewController);

    RecentFilesViewController.$inject = ['$rootScope', '$scope', '$state', '$stateParams', 'api'];

    function RecentFilesViewController($rootScope, $scope, $state, $stateParams, api) {
        var t = this;

        t.findFiles = function (query) {
            if (t.files) {
                if (query) {
                    var q = query.toLowerCase();

                    for (i = 0; i < t.files.length; i++) {
                        var f = t.files[i];

                        f.visible = f.label.toLowerCase().includes(q);
                    }
                } else {
                    for (i = 0; i < t.files.length; i++) {
                        t.files[i].visible = true;
                    }
                }
            }
        }

        t.files = [];

        t.$onInit = function () {
            api.getRecentFiles().then(function (data) {
                t.files = data;
            });
        }
    }
})();