(function () {
    angular.module('app').controller('UpdateProjectDialogController', UpdateProjectDialogController);

    UpdateProjectDialogController.$inject = ['$scope', '$filter', '$uibModalInstance', '$sce', 'api', 'projectService'];

    function UpdateProjectDialogController($scope, $filter, $uibModalInstance, $sce, api, projectService) {
        var t = this;

        // Public members
        t.project = null;

        // Public methods
        t.save = save;
        t.cancel = cancel;

        // Initaliziation
        initialize();

        function initialize() {
            t.project = projectService.selectedProject;
        }

        // Methods
        function save() {
            if ($scope.projectForm.$valid) {
                projectService.update(t.project);
                
                $uibModalInstance.close();
            }
        }

        function cancel() {
            projectService.selectedProject = null;

            $uibModalInstance.dismiss('cancel');
        }
    };
})();