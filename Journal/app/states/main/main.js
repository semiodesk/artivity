(function () {
    angular.module('app').controller("MainStateController", MainStateController);

    MainStateController.$inject = ['$rootScope', '$scope', '$state', '$stateParams', '$element', '$timeout', '$mdDialog', 'projectService', 'windowService', 'cookieService', 'syncService'];

    function MainStateController($rootScope, $scope, $state, $stateParams, $element, $timeout, $mdDialog, projectService, windowService, cookieService, syncService) {
        var t = this;

        // PROJECTS
        t.projects = [];

        t.loadProjects = function () {
            return new Promise(function (resolve, reject) {
                t.projects = [];

                projectService.getAll().then(function (data) {
                    $scope.$apply(function () {
                        t.projects = data;
                    });
                });

                resolve(t.projects);
            });
        };

        t.addProject = function (e) {
            projectService.create().then(function (project) {
                $mdDialog.show({
                    attachTo: angular.element(document.body),
                    templateUrl: 'app/dialogs/edit-project/edit-project.html',
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
                    focusOnOpen: true,
                    locals: {
                        project: project,
                        projects: t.projects
                    }
                }).then(function () {
                    projectService.update(project).then(function () {
                        project.IsNew = false;

                        t.projects.push(project);

                        syncService.synchronize();
                    }, function () {});
                }, function () {});
            });
        }

        t.selectProject = function (project, i) {
            // Note: We invoke $state.go manually because we need to pass the project 
            // variable as a reference. When using ui-sref a copy of the variable is passed
            // as parameter, hence the project title and properties would not be updated 
            // when they are being changed in dialogs.
            $state.go('main.view.project', {
                index: i,
                project: project
            });
        }

        // STATE
        t.unregisterStateChangeSuccess = null;

        t.onStateChangeSuccess = function (e, state, stateParams) {
            if (stateParams) {
                var n = parseInt(stateParams.index);

                t.selectedIndex = n;
            }
        }

        t.unregisterStateChangeError = null;

        t.onStateChangeError = function (e, state, stateParams) {
            console.error(state, stateParams);
        }

        // DRAG & DROP
        t.droppedFile = null;

        t.onFileDropped = function (event) {
            if (event.target && t.droppedFile) {
                var target = $(event.target);

                if (target.length > 0) {
                    var scope = angular.element(target[0]).scope();

                    if (scope.project) {
                        var project = scope.project;

                        // Project is mapped automatically, file manually. this is why the caps of the uri property are different
                        projectService.addFile(project.Uri, t.droppedFile.uri);
                    }
                }
            }
        }

        // INIT
        t.onInit = function () {
            windowService.setMinimizable();
            windowService.setMaximizable();

            // Listen to state changes.
            t.unregisterStateChangeSuccess = $rootScope.$on('$stateChangeSuccess', t.onStateChangeSuccess);
            t.unregisterStateChangeError = $rootScope.$on('$stateChangeError', t.onStateChangeError);

            $rootScope.$on('$stateNotFound', function (e, state, $stateParams) {
                console.error(state, $stateParams);
            });

            // Unregister the state change listeners when the controller is being destroyed.
            $scope.$on('$destroy', function () {
                t.unregisterStateChangeSuccess();
                t.unregisterStateChangeError();
            });

            t.loadProjects();
        }

        t.onInit();
    }
})();