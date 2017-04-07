(function () {
	angular.module('app').directive('artProjectView', function () {
		return {
			restrict: 'E',
			templateUrl: 'app/directives/art-project-view/art-project-view.html',
			controller: ProjectViewDirectiveController,
			controllerAs: 't',
			bindToController: true,
			scope: {
				project: "=?",
				collapsed: "=?",
				new: "=?"
			},
			link: function (scope, element, attr, t) {

			}
		}
	});

	ProjectViewDirectiveController.$inject = ['$scope', '$element', '$sce', '$uibModal', 'agentService', 'navigationService', 'projectService', 'hotkeys'];

	function ProjectViewDirectiveController($scope, $element, $sce, $uibModal, agentService, navigationService, projectService, hotkeys) {
		var t = this;

		if (t.new) {
			projectService.create().then(function (result) {
				t.project = result;
				t.project.folder = null;
				t.project.members = [];

				t.togglePropertyPane();
			});
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
			console.log("Loading files:", callback);

			if (t.project) {
				return projectService.getFiles(t.project.Uri).then(callback);
			} else {
				return callback();
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
			var panel = $($element).find('.view-secondary-content');

			if (!t.new || t.new && !panel.hasClass('panel-visible')) {
				// Note: This also works when invoked from keyboard shortcuts.
				panel.toggleClass('panel-visible');

				t.collapsed = !t.collapsed;

				if (!t.collapsed && t.project) {
					projectService.getFolders(t.project.Uri).then(function (result) {
						if (result.length > 0) {
							t.project.folder = result[0].Url.Uri;
						}
					});

					projectService.getMembers(t.project.Uri).then(function (result) {
						if (result.length > 0) {
							t.project.members = result;
						}
					});
				}
			}
		}

		t.commit = function () {
			if ($scope.projectForm.$valid) {
				projectService.update(t.project).then(function () {
					$scope.$emit('projectAdded', t.project);
				});

				t.togglePropertyPane();
			}
		}

		t.cancel = function () {
			projectService.currentProject = null;

			t.togglePropertyPane();
		}

		t.$onInit = function () {
			hotkeys.add({
				combo: 'alt+enter',
				description: 'Toggle the project properties panel.',
				callback: function () {
					t.togglePropertyPane();
				}
			});
		}

		t.$postLink = function () {
			$scope.$watch('t.project', function (newValue, oldValue) {
				$scope.$broadcast('refresh');
			});

			$element.on('appear', function () {
				if (t.project && t.project != projectService.currentProject) {
					projectService.currentProject = t.project;

					console.log('Selected project: ', t.project);
				}

				$scope.$broadcast('refresh');
			});
		}
	}
})();