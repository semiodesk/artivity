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
			},
			link: function(scope, element, attributes) {
				$('.btn-select-folder').click(function() {
					$('.form-input-folder').click();
				});
			}
		}
	});

	ProjectHeaderDirectiveController.$inject = ['$rootScope', '$scope', '$sce', 'projectService'];

	function ProjectHeaderDirectiveController($rootScope, $scope, $sce, projectService) {
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

		t.users = [];
		t.createUser = createUser;
		t.removeUser = removeUser;

		function createUser() {
			t.users.push({
				name: 'Max Mustermann',
				email: 'max@mustermann.de',
				photoUrl: $sce.trustAsResourceUrl('http://127.0.0.1:8262/artivity/api/1.0//agents/user/photo')
			});
		}

		function removeUser(user) {
			var i = t.users.indexOf(user);

			if (i > -1) {
				t.users.splice(i, 1);
			}
		}

		function commit() {
			console.log(t.project);

			if ($scope.projectForm.$valid) {
				projectService.update(t.project).then(function () {
					$rootScope.$broadcast('projectAdded', t.project);
				});
			}
		}

		function cancel() {
			projectService.selectedProject = null;
		}
	}
})();