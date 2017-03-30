(function () {
	angular.module('app').directive('artProjectView', function () {
		return {
			restrict: 'E',
			templateUrl: 'app/directives/art-project-view/art-project-view.html',
			controller: ProjectViewDirectiveController,
			controllerAs: 't',
			bindToController: true,
			scope: {
				project: "=project",
				collapsed: "=?"
			}
		}
	});

	ProjectViewDirectiveController.$inject = ['$rootScope', '$scope', '$sce', '$uibModal', 'agentService', 'navigationService', 'projectService', 'selectionService'];

	function ProjectViewDirectiveController($rootScope, $scope, $sce, $uibModal, agentService, navigationService, projectService, selectionService) {
		var t = this;

		t.collapsed = true;

		if (!t.project) {
			projectService.create().then(function (result) {
				t.project = result;
				t.project.new = true;
				t.project.folder = null;
				t.project.members = [];

				t.collapsed = false;
			});
		} else {
			t.project.new = false;
		}

		t.file = null;

		t.viewFile = function (e, file) {
			t.file = file;
		}

		navigationService.registerScope($scope);

		$scope.onNavigateBack = function (e) {
			if (t.file) {
				t.file = null;
				e.handled = true;
			}
		}

		t.getFiles = function (callback) {
			if (t.project) {
				return projectService.getFiles(t.project.Uri).then(callback);
			} else {
				return callback([]);
			}
		}

		t.setFolder = function (folderUrl) {
			if (t.folder) {
				projectService.removeFolder(t.project.Uri, t.project.folder);
			}

			t.project.folder = folderUrl;

			projectService.addFolder(t.project.Uri, t.project.folder);
		}

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
						projectService.addMember(t.project.Uri, m.Uri);

						projectService.getMembers(t.project.Uri).then(function (result) {
							if (result.length > 0) {
								t.project.members = result;
							}
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

		t.getPhotoUrl = agentService.getPhotoUrl;

		t.togglePropertyPane = function () {
			t.collapsed = !t.collapsed;
		}

		t.commit = function () {
			if ($scope.projectForm.$valid) {
				projectService.update(t.project).then(function () {
					$rootScope.$broadcast('projectAdded', t.project);
				});

				if (!t.new) {
					t.collapsed = true;
				}
			}
		}

		t.cancel = function () {
			projectService.selectedProject = null;

			if (!t.new) {
				t.collapsed = true;
			}
		}
	}
})();