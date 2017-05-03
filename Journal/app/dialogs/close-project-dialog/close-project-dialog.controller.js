(function () {
    angular.module('app').controller('CloseProjectDialogController', CloseProjectDialogController);

    CloseProjectDialogController.$inject = ['$scope', '$uibModalInstance', 'project', 'projectService'];

    function CloseProjectDialogController($scope, $uibModalInstance, project, projectService) {
        var t = this;

        t.project = project;

        t.commit = function () {
            if (t.project) {
                projectService.remove(t.project.Uri).then(function () {
                    $scope.$close();
                });
            }
            else {
                $scope.$dismiss();
            }
        }

        t.cancel = function () {
            $scope.$dismiss();
        }
    };
})();