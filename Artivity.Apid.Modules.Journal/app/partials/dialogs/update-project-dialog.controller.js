(function () {
    angular.module('explorerApp').controller('UpdateProjectDialogController', UpdateProjectDialogController);

    function UpdateProjectDialogController(api, $scope, $filter, $uibModalInstance, $sce, projectService) {
        var t = this;

        // Public members
        t.project = null;

        // Public methods
        t.updateProject = updateProject;
        t.cancel = cancel;


        // Initaliziation
        initialize();

        function initialize() {
            t.project = projectService.selectedProject;
        }

        // Methods
        function updateProject() {
            projectService.update(t.project);
        }

        function cancel() {
            projectService.selectedProject = null;
            $uibModalInstance.dismiss('cancel');

        };
    };
})();