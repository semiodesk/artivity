(function () {
	angular.module('app').directive('artProjectHeader', function () {
		return {
			restrict: 'E',
			templateUrl: 'app/directives/art-project-header/art-project-header.html',
			controller: ProjectHeaderDirectiveController,
			controllerAs: 't',
			bindToController: true,
			scope: {
				project: "=project",
				collapsed: "=?"
			}
		}
	});

	ProjectHeaderDirectiveController.$inject = ['$rootScope', '$scope', '$sce', '$uibModal', 'agentService', 'projectService'];

	function ProjectHeaderDirectiveController($rootScope, $scope, $sce, $uibModal, agentService, projectService) {
		var t = this;

		if (t.project === null) {
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
		t.createUser = createUser;
		t.editUser = editUser;
		t.removeUser = removeUser;
		t.setFolder = setFolder;

		function setFolder(folderUrl) {
			if (t.folder) {
				projectService.removeFolder(t.project.Uri, t.folder);
			}

			t.folder = folderUrl;

			projectService.addFolder(t.project.Uri, t.folder);
		}

		function createUser() {
			agentService.newPerson().then(function (person) {
				if (person) {
					var modalInstance = $uibModal.open({
						animation: true,
						templateUrl: 'app/dialogs/edit-person-dialog/edit-person-dialog.html',
						controller: 'EditPersonDialogController',
						controllerAs: 't',
						resolve: {
							person: function () {
								return person;
							}
						}
					});

					modalInstance.result.then(function () {
						projectService.addMember(t.project.Uri, person.Uri);
					});
				}
			});
		}

		function editUser(user) {
			var modalInstance = $uibModal.open({
				animation: true,
				templateUrl: 'app/dialogs/edit-person-dialog/edit-person-dialog.html',
				controller: 'EditPersonDialogController',
				controllerAs: 't',
				resolve: {
					person: function () {
						return user;
					}
				}
			});
		}

		function removeUser(user) {
			var i = t.users.indexOf(user);

			if (i > -1) {
				t.users.splice(i, 1);
			}
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