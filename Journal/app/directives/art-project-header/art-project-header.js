(function () {
	angular.module('app').directive('artProjectHeader', function () {
		return {
			restrict: 'E',
			templateUrl: 'app/directives/art-project-header/art-project-header.html',
			controller: ProjectHeaderDirectiveController,
			controllerAs: 't',
			bindToController: true,
			scope: {
				project: "@project",
				create: "@create"
			}
		}
	});

	ProjectHeaderDirectiveController.$inject = ['$rootScope', '$scope', 'projectService'];

	function ProjectHeaderDirectiveController($rootScope, $scope, projectService) {
		var t = this;

		if (t.create) {
			projectService.create().then(function (result) {
				t.project = result;

				console.log(t.project);
			});
		}

		t.rootScope = $rootScope;

		t.commit = commit;
		t.cancel = cancel;

		function commit() {
			console.log(t.project);

			if ($scope.projectForm.$valid) {
				projectService.update(t.project);

				$rootScope.$broadcast('projectAdded');
			}
		}

		function cancel() {
			projectService.selectedProject = null;
		}
	}
})();