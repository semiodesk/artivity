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

        // MEMBERS
		t.getPhotoUrl = agentService.getPhotoUrl;

		t.addMember = function () {
			agentService.newPerson().then(function (member) {
				if (member) {
					var modalInstance = $uibModal.open({
						animation: true,
						templateUrl: 'app/dialogs/edit-person-dialog/edit-person-dialog.html',
						controller: 'EditPersonDialogController',
						controllerAs: 't',
						resolve: {
							person: function () {
								return member;
							}
						}
					});

					modalInstance.result.then(function (m) {
						projectService.addMember(t.project.Uri, m.Uri).then(function () {
							projectService.getMembers(t.project.Uri).then(function (result) {
								if (result.length > 0) {
									t.project.members = result;
								}
							});
						});
					});
				}
			});
		}

		t.editMember = function (member) {
			var modalInstance = $uibModal.open({
				animation: true,
				templateUrl: 'app/dialogs/edit-person-dialog/edit-person-dialog.html',
				controller: 'EditPersonDialogController',
				controllerAs: 't',
				resolve: {
					person: function () {
						return member.Agent;
					}
				}
			});

			modalInstance.result.then(function () {
				projectService.getMembers(t.project.Uri).then(function (result) {
					if (result.length > 0) {
						t.project.members = result;

						syncService.synchronize();
					}
				});
			});
		}

		t.removeMember = function (member) {
			projectService.removeMember(t.project.Uri, member.Agent.Uri).then(function () {
				var i = t.project.members.indexOf(member);

				if (i > -1) {
					t.project.members.splice(i, 1);
				}
			});
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