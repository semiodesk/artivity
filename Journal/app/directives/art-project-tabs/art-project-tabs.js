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

    ProjectTabsDirectiveController.$inject = ['$rootScope', '$scope', '$uibModal', 'selectionService', 'projectService', 'fileService'];

    function ProjectTabsDirectiveController($rootScope, $scope, $uibModal, selectionService, projectService, fileService) {
        var t = this;

        t.projects = [];
        t.refresh = refresh;
        t.selectProject = selectProject;
        t.closeProject = closeProject;

        // Drag & Drop
        t.droppedFile = null;
        t.onFileDropped = onFileDropped;

        initialize();

        function initialize() {
            // Make the left and right panes resizable.
            $(".ui-pane-left").resizable({
                handles: "e"
            });

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

        function refresh() {
            projectService.getAll().then(function (data) {
                var list = data;

                if (list && list.length > 0) {
                    list.sort(function compare(a, b) {
                        if (a.Title < b.Title) return -1;
                        if (a.Title > b.Title) return 1;
                        return 0;
                    });

                    t.projects = list;
                }

                $scope.$apply();
            });
        }

        function onFileDropped(event) {
            if (event.target) {
                var target = $(event.target);

                if (target.length > 0) {
                    var scope = angular.element(target[0]).scope();

                    if (scope) {
                        var project = scope.project;
                        var file = t.droppedFile;

                        // Project is mapped automatically, file manually. this is why the caps of the uri property are different
                        projectService.addFile(project.Uri, file.uri);
                    }
                }
            }
        }

        function closeProject(project) {
            projectService.selectedProject = project;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'app/dialogs/close-project-dialog/close-project-dialog.html',
                controller: 'CloseProjectDialogController',
                controllerAs: 't'
            }).closed.then(function (account) {
                t.refresh();
            });
        }

        function selectProject(project) {
            selectionService.selectedItem(project);

            if (project === null) {
                fileService.removeFilter();
            } else {
                fileService.filterFilesByProject(project.Uri);
            }
        }
    }
})();