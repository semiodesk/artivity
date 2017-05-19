(function () {
	angular.module('app').controller("ProjectDashboardViewController", ProjectDashboardViewController);

	ProjectDashboardViewController.$inject = ['$scope', '$state', '$stateParams', '$element', '$timeout', '$mdSidenav', 'hotkeys', 'projectService'];

	function ProjectDashboardViewController($scope, $state, $stateParams, $element, $timeout, $mdSidenav, hotkeys, projectService) {
		var t = this;

		t.project = null;

		t.getFiles = function (callback) {
			if (t.project) {
				return projectService.getFiles(t.project.Uri).then(callback);
			} else {
				return callback();
			}
		}

		t.toggleSettings = function () {
			$mdSidenav('right').toggle().then(function () {
				var sidenav = $mdSidenav('right');

				if(t.project.IsNew) {
					sidenav.isLockedOpen(true);
				}

				if (!t.project.IsNew && sidenav.isOpen()) {
					// Load the folders and members when the panel is being made visible.
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
			});
		}

		t.$onInit = function () {
			hotkeys.add({
				combo: 'alt+enter',
				description: 'Toggle the project properties panel.',
				callback: function () {
					t.toggleSettings();
				}
			});

			if ($stateParams && $stateParams.project) {
				t.project = $stateParams.project;
			}

			$timeout(function () {
				if (t.project && t.project.IsNew) {
					t.toggleSettings();
				}
			}, 0);

			$scope.$on('viewFile', function (e, file) {
				$stateParams.fileUri = file.uri;
				$state.go('main.view.document', $stateParams);
			});

			$scope.$on('viewFileHistory', function (e, file) {
				$stateParams.fileUri = file.uri;
				$state.go('main.view.document-history', $stateParams);
			});

			$scope.$on('showMore', function () {
				t.toggleSettings();
			});

			$scope.$on('commit', function () {
				t.toggleSettings();
			});

			$scope.$on('cancel', function () {
				t.toggleSettings();
			});
		}
	}
})();