(function () {
	angular.module('app').controller("ProjectDashboardViewController", ProjectDashboardViewController);

	ProjectDashboardViewController.$inject = ['$scope', '$state', '$stateParams', '$element', '$mdSidenav', 'hotkeys', 'tabService', 'projectService'];

	function ProjectDashboardViewController($scope, $state, $stateParams, $element, $mdSidenav, hotkeys, tabService, projectService) {
		var t = this;

		t.project = null;

		t.getFiles = function (callback) {
			t.getProject.then(function () {
				if (t.project) {
					return projectService.getFiles(t.project.Uri).then(callback);
				} else {
					return callback();
				}
			});
		}

		t.toggleSettings = function () {
			$mdSidenav('right').toggle().then(function () {
				if (!t.project.new && t.isSettingsOpen()) {
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

		t.isSettingsOpen = function () {
			return $mdSidenav('right').isOpen();
		}

		t.onInit = function () {
			hotkeys.add({
				combo: 'alt+enter',
				description: 'Toggle the project properties panel.',
				callback: function () {
					t.toggleSettings();
				}
			});

			t.getProject = projectService.getAll().then(function (projects) {
				// We load the project from the given tab index with project starting from index 1.
				t.project = projects[$stateParams.index - 1];

				if (t.project && t.project.new) {
					t.toggleSettings();
				}
			});

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

		t.onInit();
	}
})();