(function () {
    angular.module('explorerApp').directive('artProjectList', ProjectListDirective);

    function ProjectListDirective() {
        return {
            restrict: 'E',
            templateUrl: 'partials/directives/art-project-list.html',
            controller: ProjectListDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attr, ctrl) {
            }
        }
    }

    angular.module('explorerApp').controller('ProjectListDirectiveController', ProjectListDirectiveController);

    function ProjectListDirectiveController($rootScope, $scope, $uibModal, projectService, artFileService) {
        var t = this;

        t.projectList = [];

        t.droppedFile = null;
        t.addProject = addProject;
        t.updateProject = updateProject;
        t.refreshProjectList = refreshProjectList;
        t.addFileToProject = addFileToProject;
        t.filterBy = filterBy;
        t.removeFilter = removeFilter;

    

        initialize();
        
        function initialize()
        {
            t.refreshProjectList();
        }

        function refreshProjectList(){
            projectService.getAll().then(function(list) {t.projectList = list;});
        }

       function updateProject (project) {
           projectService.selectedProject = project;
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'partials/dialogs/update-project-dialog.html',
                controller: 'UpdateProjectDialogController',
                controllerAs: 't'
            });

            modalInstance.result.then(function (account) {
                console.log("TODO: Reloading Projects");

            });
        };

        function addProject() {
            projectService.create().then(function(result){
                updateProject(result);
            })
        }

        function addFileToProject(event){
            var file = t.droppedFile;
            var project = $(event.target).scope().project;
            // Project is mapped automatically, file manually. this is why the caps of the uri property are different
            projectService.addFileToProject(project.Uri, file.uri); 
        }

        function filterBy(project) {
            artFileService.filterFilesByProject(project.Uri);
        }

        function removeFilter() {
            artFileService.removeFilter();
        }

    }
})();