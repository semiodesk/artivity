(function () {
    angular.module('app').directive('artProjectTabs', ProjectTabsDirective);

    function ProjectTabsDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-project-tabs/art-project-tabs.html',
            controller: ProjectTabsDirectiveController,
            controllerAs: 't'
        }
    }

    angular.module('app').controller('ProjectTabsDirectiveController', ProjectTabsDirectiveController);

    ProjectTabsDirectiveController.$inject = ['$rootScope', '$scope', '$uibModal', 'api', 'agentService', 'cookieService', 'selectionService', 'projectService'];

    function ProjectTabsDirectiveController($rootScope, $scope, $uibModal, api, agentService, cookieService, selectionService, projectService) {
        var t = this;

        t.projects = [];
        t.activeTab = cookieService.get('tabs.activeTab', 0);

        $scope.$watch('t.activeTab', function() {
            cookieService.set('tabs.activeTab', t.activeTab);
        });

        t.selectProject = function (project) {
            if (project !== null) {
                projectService.getFolders(project.Uri).then(function (result) {
                    if (result.length > 0) {
                        project.folder = result[0].Url.Uri;
                    }
                });

                projectService.getMembers(project.Uri).then(function (result) {
                    if (result.length > 0) {
                        project.members = result;
                    }
                });
            }
        }

        t.closeProject = function (project) {
            projectService.currentProject = project;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'app/dialogs/close-project-dialog/close-project-dialog.html',
                controller: 'CloseProjectDialogController',
                controllerAs: 't'
            }).closed.then(function (account) {
                t.refresh();
            });
        }

		t.getRecentlyUsedFiles = function (callback) {
			return api.getRecentFiles().then(callback);
		}

        t.droppedFile = null;

        t.onFileDropped = function (event) {
            if (event.target && t.droppedFile) {
                var target = $(event.target);

                if (target.length > 0) {
                    var scope = angular.element(target[0]).scope();

                    if (scope && scope.project) {
                        var project = scope.project;
                        var file = t.droppedFile;

                        // Project is mapped automatically, file manually. this is why the caps of the uri property are different
                        projectService.addFile(project.Uri, file.uri);
                    }
                }
            }
        }

        t.refresh = function () {
            api.getRecentFiles().then(function (result) {
                t.files = result;
            });

            projectService.getAll().then(function (data) {
                var list = data;

                if (list && list.length > 0) {
                    list.sort(function compare(a, b) {
                        if (a.Title < b.Title) return -1;
                        if (a.Title > b.Title) return 1;
                        return 0;
                    });

                    t.projects = list;

                    // Set the currently active tab.
                    var i = t.projects.indexOf(projectService.currentProject);

                    if (i !== -1) {
                        t.activeTabIndex = i + 1;
                    }
                }

                $scope.$apply();
            });
        }

        function init() {
            // Make the left and right panes resizable.
            $rootScope.$on('dragStarted', function () {
                $('.project-item').addClass('drop-target');
            });

            $rootScope.$on('dragStopped', function () {
                $('.project-item').removeClass('drop-target');
            });

            $rootScope.$on('projectAdded', function (project) {
                t.refresh();

                t.selectProject(project);
            });

            t.refresh();
        }

        init();
    }
})();