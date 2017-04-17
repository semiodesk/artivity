(function () {
    angular.module('app').controller('CloseProjectDialogController', CloseProjectDialogController);

    CloseProjectDialogController.$inject = ['$scope', '$filter', '$uibModalInstance', '$sce', 'projectService'];

    function CloseProjectDialogController($scope, $filter, $uibModalInstance, $sce, projectService) {
        var t = this;

        t.project = null;

        t.commit = function () {
            if (t.project) {
                projectService.remove(t.project.Uri).then(function () {
                    $uibModalInstance.close();
                });
            }
            else {
                $uibModalInstance.close();
            }
        }

        t.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        }

        function init() {
            t.project = projectService.currentProject;
        }

        init();
    };
})();