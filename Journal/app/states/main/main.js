(function () {
    angular.module('app').controller("MainStateController", MainStateController);

    MainStateController.$inject = ['$rootScope', '$scope', '$state', '$stateParams', '$timeout', 'projectService', 'windowService', 'cookieService', 'syncService'];

    function MainStateController($rootScope, $scope, $state, $stateParams, $timeout, projectService, windowService, cookieService, syncService) {
        var t = this;

        t.unregisterStateChangeSuccess = null;

        t.onStateChangeSuccess = function (e, state, stateParams) {
            if (stateParams) {
                var n = parseInt(stateParams.index);

                if (t.selectedTab === n && state.name.startsWith('main')) {
                    var context = tabService.getTabContext(n);
                    context.state = state;
                    context.stateParams = stateParams;

                    tabService.setSelectedTab(n);
                    tabService.setTabContext(n, context);
                }
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

                    if (scope && scope.$index) {
                        var context = tabService.getTabContext(scope.$index);

                        if (context && context.stateParams && context.stateParams.project) {
                            var project = context.stateParams.project;
                            var file = t.droppedFile;

                            // Project is mapped automatically, file manually. this is why the caps of the uri property are different
                            projectService.addFile(project.Uri, file.uri);
                        }
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
        }

        t.onInit();
    }
})();