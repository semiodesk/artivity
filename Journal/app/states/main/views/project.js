(function () {
	angular.module('app').controller("ProjectDashboardViewController", ProjectDashboardViewController);

	ProjectDashboardViewController.$inject = ['$rootScope', '$scope', '$state', '$stateParams', '$element', '$timeout', '$mdDialog', 'hotkeys', 'projectService', 'syncService'];

	function ProjectDashboardViewController($rootScope, $scope, $state, $stateParams, $element, $timeout, $mdDialog, hotkeys, projectService, syncService) {
		var t = this;

		t.project = null;

		t.findFiles = function (query) {
			if (t.files) {
				if (query) {
					var q = query.toLowerCase();

					for (i = 0; i < t.files.length; i++) {
						var f = t.files[i];

						f.visible = f.label.toLowerCase().includes(q);
					}
				} else {
					for (i = 0; i < t.files.length; i++) {
						t.files[i].visible = true;
					}
				}
			}
		}

		t.editProject = function (e) {
			$mdDialog.show({
				attachTo: angular.element(document.body),
				templateUrl: 'app/dialogs/edit-project-dialog/edit-project-dialog.html',
				controller: 'EditProjectDialogController',
				controllerAs: 't',
				bindToController: true,
				hasBackdrop: true,
				trapFocus: true,
				zIndex: 150,
				targetEvent: e,
				disableParentScroll: true,
				clickOutsideToClose: false,
				escapeToClose: true,
				preserveScope: true,
				focusOnOpen: true,
				locals: {
					project: t.project
				}
			}).then(function () {
				projectService.update(t.project).then(function () {
					t.project.IsNew = false;

					syncService.synchronize();
				}, function () {});
			});
		}

		t.deleteProject = function (e) {
			$mdDialog.show({
					templateUrl: 'delete-project-dialog.html',
					parent: angular.element(document.body),
					targetEvent: e,
					controller: function ($scope) {
						$scope.project = t.project;

						$scope.ok = function () {
							$mdDialog.hide();
						}

						$scope.cancel = function () {
							$mdDialog.cancel();
						}
					}
				})
				.then(function (answer) {
					projectService.remove(t.project.Uri).then(function () {
						t.project.IsDeleted = true;

						syncService.synchronize();
					}, function () {});
				}, function () {});
		}

		t.publishProject = function (e) {
			$mdDialog.show({
					templateUrl: 'share-project-dialog.html',
					parent: angular.element(document.body),
					targetEvent: e,
					controller: function ($scope) {
						$scope.project = t.project;

						$scope.ok = function () {
							$mdDialog.hide();
						}

						$scope.cancel = function () {
							$mdDialog.cancel();
						}
					}
				})
				.then(function (answer) {
					projectService.publish(t.project.Uri).then(function () {
						t.project.IsSynchronizationEnabled = true;

						syncService.synchronize();
					}, function () {});
				}, function () {});
		}

		t.$onInit = function () {
			if ($stateParams && $stateParams.project) {
				t.project = $stateParams.project;
			}

			$scope.$on('refresh', function () {
				console.log(t.project);
			});

			if (t.project) {
				projectService.getFiles(t.project.Uri).then(function (data) {
					t.files = data;
				})
			} else {
				t.files = [];
			}
		}
	}
})();