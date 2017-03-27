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
			},
			link: function (scope, element, attributes, ctrl) {}
		}
	});

	ProjectViewDirectiveController.$inject = ['$rootScope', '$scope', '$sce', '$uibModal', 'agentService', 'projectService'];

	function ProjectViewDirectiveController($rootScope, $scope, $sce, $uibModal, agentService, projectService) {
		var t = this;

		if (!t.project) {
			t.new = true;
			t.collapsed = false;
			t.folder = null;

			projectService.create().then(function (result) {
				t.project = result;
			});
		} else {
			t.new = false;
			t.collapsed = true;
			t.folder = null;

			projectService.getFolders(t.project.Uri).then(function (result) {
				if (result.length > 0) {
					t.folder = result[0].Url.Uri;
				}
			});

			projectService.getMembers(t.project.Uri).then(function (result) {
				if (result.length > 0) {
					t.members = result;
				}
			});
		}

		t.rootScope = $rootScope;

		t.commit = commit;
		t.cancel = cancel;

		t.members = [];
		t.addMember = addMember;
		t.editMember = editMember;
		t.removeMember = removeMember;
		t.getPhotoUrl = agentService.getPhotoUrl;

		t.setFolder = setFolder;

		function setFolder(folderUrl) {
			if (t.folder) {
				projectService.removeFolder(t.project.Uri, t.folder);
			}

			t.folder = folderUrl;

			projectService.addFolder(t.project.Uri, t.folder);
		}

		function addMember() {
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
								t.members = result;
							}
						});
					});
				}
			});
		}

		function editMember(member) {
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
						t.members = result;
					}
				});
			});
		}

		function removeMember(member) {
			projectService.removeMember(t.project.Uri, member.Agent.Uri).then(function () {
				var i = t.members.indexOf(member);

				if (i > -1) {
					t.members.splice(i, 1);
				}
			});
		}

		function commit() {
			if ($scope.projectForm.$valid) {
				projectService.update(t.project).then(function () {
					$rootScope.$broadcast('projectAdded', t.project);
				});

				if (!t.new) {
					t.collapsed = true;
				}
			}
		}

		function cancel() {
			projectService.selectedProject = null;

			if (!t.new) {
				t.collapsed = true;
			}
		}
	}
})();