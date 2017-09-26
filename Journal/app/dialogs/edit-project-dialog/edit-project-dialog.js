(function () {
    angular.module('app').controller('EditProjectDialogController', EditProjectDialogController);

    EditProjectDialogController.$inject = ['$scope', '$mdDialog', 'projectService', 'agentService'];

    function EditProjectDialogController($scope, $mdDialog, projectService, agentService) {
        var t = this;

        // PROPERTIES
		t.setFolder = function (folderUrl) {
			if (t.folder) {
				projectService.removeFolder(t.project.Uri, t.project.folder);
			}

			t.project.folder = folderUrl;

			projectService.addFolder(t.project.Uri, t.project.folder);
		}

        t.commit = function () {
            if ($scope.projectForm && $scope.projectForm.$valid) {
                $mdDialog.hide();
            }
        };

        t.cancel = function () {
            $mdDialog.cancel();
        };
    };
})();