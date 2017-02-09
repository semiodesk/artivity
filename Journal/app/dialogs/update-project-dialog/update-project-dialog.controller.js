(function () {
    angular.module('app').controller('UpdateProjectDialogController', UpdateProjectDialogController);

    function UpdateProjectDialogController(api, $scope, $filter, $uibModalInstance, $sce, projectService) {
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