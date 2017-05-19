(function () {
    angular.module('app').controller("DocumentViewController", DocumentViewController);

    DocumentViewController.$inject = ['$scope', '$state', '$stateParams', 'entityService', 'viewerService', 'syncService'];

    function DocumentViewController($scope, $state, $stateParams, entityService, viewerService, syncService) {
        var t = this;

        t.latestRevisionUri = null;

        t.loadLatestRevision = function (file) {
            if (file && file.uri) {
                entityService.getLatestRevisionFromFileUri(file.uri).then(function (data) {
                    if (data.revision) {
                        t.latestRevisionUri = data.revision;
                    }
                });
            }
        }

        t.publishLatestRevision = function (file) {
            if (file && file.uri) {
                entityService.publishLatestRevisionFromFileUri(file.uri).then(function (data) {
                    syncService.synchronize();
                });
            }
        }

        t.navigateBack = function () {
            $state.go('main.view.project-dashboard', $stateParams);
        }

        t.$onInit = function () {
            if ($stateParams.fileUri) {
                entityService.get($stateParams.fileUri).then(function (data) {
                    t.file = data;

                    t.loadLatestRevision(t.file);
                });
            }
        }
    }
})();