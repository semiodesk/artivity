(function () {
	angular.module('app').controller("ProjectDashboardViewController", ProjectDashboardViewController);

	ProjectDashboardViewController.$inject = ['$scope', '$state', '$stateParams', '$element', 'hotkeys', 'projectService'];

	function ProjectDashboardViewController($scope, $state, $stateParams, $element, hotkeys, projectService) {
		var t = this;

		t.project = null;

		t.getFiles = function (callback) {
			console.log("Loading files:", callback);

			if (t.project) {
				return projectService.getFiles(t.project.Uri).then(callback);
			} else {
				return callback();
			}
		}

		t.toggleProjectSettings = function () {
			if (t.project) {
				var panel = $($element).find('.view-secondary-content');

				panel.toggleClass('panel-visible');

				if (panel.hasClass('panel-visible') && !t.project.new) {
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
			}
		}

		t.onInit = function () {
			hotkeys.add({
				combo: 'alt+enter',
				description: 'Toggle the project properties panel.',
				callback: function () {
					t.togglePropertyPane();
				}
			});

			t.project = $stateParams.project;

			if (t.project && t.project.new) {
				t.toggleProjectSettings();
			}

			$scope.$on('viewFile', function (e, file) {
				$stateParams.file = file;
				$state.go('main.view.document', $stateParams);
			});

			$scope.$on('viewFileHistory', function (e, file) {
				$stateParams.file = file;
				$state.go('main.view.document-history', $stateParams);
			});

			$scope.$on('showMore', function () {
				t.toggleProjectSettings();
			});

			$scope.$on('commit', function () {
				t.toggleProjectSettings();
			});

			$scope.$on('cancel', function () {
				t.toggleProjectSettings();
			});
		}

		t.onInit();
	}
})();