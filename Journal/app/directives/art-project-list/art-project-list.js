(function () {
    angular.module('app').directive('artProjectList', ProjectListDirective);

    function ProjectListDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-project-list/art-project-list.html',
            controller: ProjectListDirectiveController,
            controllerAs: 't'
        }
    }

    angular.module('app').controller('ProjectListDirectiveController', ProjectListDirectiveController);

    function ProjectListDirectiveController($rootScope, $scope, $uibModal, selectionService, projectService, fileService) {
        var t = this;

        t.droppedFile = null;
        t.projectList = [];
        t.addProject = addProject;
        t.addFileToProject = addFileToProject;
        t.updateProject = updateProject;
        t.deleteProject = deleteProject;
        t.refreshProjectList = refreshProjectList;
        t.selectedProject = null;
        t.selectProject = selectProject;
        t.isDeleting = false;
        t.toggleDelete = toggleDelete;

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

            t.refreshProjectList();
        }

        function refreshProjectList() {
            projectService.getAll().then(function (response) {
                var list = response.data;

                if(list && list.length > 0) {
                    list.sort(function compare(a, b) {
                        if (a.Name < b.Name) return -1;
                        if (a.Name > b.Name) return 1;
                        return 0;
                    });

                    t.projectList = list;
                }

                $scope.$apply();
            });
        }

        function updateProject(project) {
            projectService.selectedProject = project;

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'app/dialogs/update-project-dialog/update-project-dialog.html',
                controller: 'UpdateProjectDialogController',
                controllerAs: 't'
            }).closed.then(function (account) {
                t.refreshProjectList();
            });
        };

        function addProject() {
            projectService.create().then(function (result) {
                t.updateProject(result);
            });
        }

        function addFileToProject(event) {
            var target = $(event.target);

            var file = t.droppedFile;
            var project = angular.element(target[0]).scope().project;

            // Project is mapped automatically, file manually. this is why the caps of the uri property are different
            projectService.addFileToProject(project.Uri, file.uri);
        }

        function deleteProject(project) {
            projectService.remove(project.Uri).then(function (result) {
                t.refreshProjectList();
            });
        }

        function selectProject(project) {
            selectionService.selectedItem(project);

            t.selectedProject = project;

            if (project === null) {
                fileService.removeFilter();
            } else {
                fileService.filterFilesByProject(project.Uri);
            }
        }

        function toggleDelete() {
            t.isDeleting = !t.isDeleting;
        }
    }
})();