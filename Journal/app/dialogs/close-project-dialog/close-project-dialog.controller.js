(function () {
    angular.module('app').controller('CloseProjectDialogController', CloseProjectDialogController);

    CloseProjectDialogController.$inject = ['$scope', '$filter', '$uibModalInstance', '$sce', 'projectService'];

    function CloseProjectDialogController($scope, $filter, $uibModalInstance, $sce, projectService) {
        var t = this;

        t.project = null;
        
        t.commit = commit;
        t.cancel = cancel;

        initialize();

        function initialize() {
            t.project = projectService.selectedProject;
        }

        function commit() {
            projectService.remove(t.project.Uri);

            $uibModalInstance.close();
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }
    };
})();