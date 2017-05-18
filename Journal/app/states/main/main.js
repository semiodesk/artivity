(function () {
    angular.module('app').controller("MainStateController", MainStateController);

    MainStateController.$inject = ['$rootScope', '$scope', '$state', '$stateParams', '$timeout', 'tabService', 'projectService', 'windowService', 'cookieService', 'syncService'];

    function MainStateController($rootScope, $scope, $state, $stateParams, $timeout, tabService, projectService, windowService, cookieService, syncService) {
        var t = this;

        // TABS
        t.activateTab = function (e, index) {
            if (index === t.tabs.length - 1) {
                t.addTab(e);
            } else {
                t.selectTab(e, index);
            }
        }

        t.selectTab = function (e, index) {
            var n = parseInt(index);

            var context = tabService.getTabContext(n);

            if (context && context.state) {
                t.selectedTab = n;

                $state.go(context.state.name, context.stateParams);
            }
        }

        t.addTab = function (e) {
            if (e) {
                e.preventDefault();
            }

            // If there is already a new project, select the already opened tab.
            for (var i = 0; i < t.tabs.length; i++) {
                var tab = t.tabs[i];

                var stateParams = tab.context.stateParams;

                if (stateParams && stateParams.project) {
                    var project = stateParams.project;

                    if (project.new) {
                        t.selectTab(e, i);

                        return;
                    }
                }
            }

            // Otherwise create a new project.
            projectService.create().then(function (result) {
                var project = result;
                project.new = true;
                project.folder = null;
                project.members = [];

                var t = tabService.addTab(project);

                // $scope.$apply(function () {
                //     t.selectTab(e, t.index);
                // });
            });
        }

        t.removeTab = function (e, index) {
            if (e) {
                e.preventDefault();
            }

            $timeout(function () {
                tabService.closeTab(index);
                
                t.tabs = tabService.getTabs();

                if (t.selectedTab > 0) {
                    t.selectedTab = t.selectedTab - 1;
                }
            }, 0);
        }

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

            // Initialize the tabs when all tabs and states are loaded.
            tabService.openTabs().then(function () {
                t.tabs = tabService.getTabs();

                // Reset the last selected tab.
                var n = 0;

                if ($stateParams && $stateParams.index < t.tabs.length) {
                    n = parseInt($stateParams.index);
                } else {
                    n = cookieService.get('tabs.selectedTab', 0);

                    if (n >= t.tabs.length - 1) {
                        n = 0;

                        cookieService.set('tabs.selectedTab', 0);
                    }
                }

                t.selectTab(null, n);
            });

            // Listen to state changes.
            t.unregisterStateChangeSuccess = $rootScope.$on('$stateChangeSuccess', t.onStateChangeSuccess);
            t.unregisterStateChangeError = $rootScope.$on('$stateChangeError', t.onStateChangeError);

            $rootScope.$on('$stateNotFound', function (e, state, $stateParams) {
                console.error(state, $stateParams);
            });

            // Update the tab title when the project title changes.
            $scope.$on('commit', function (e, project) {
                var tab = tabService.getTab(t.selectedTab);

                if (tab) {
                    tab.title = project.Title;
                }
            });

            // Unregister the state change listeners when the controller is being destroyed.
            $scope.$on('$destroy', function () {
                t.unregisterStateChangeSuccess();
                t.unregisterStateChangeError();
            });

            // Save the last selected tab so that we can restore it after an app restart.
            $scope.$watch('t.selectedTab', function () {
                // Do not save the position of the '+' tab.
                if (t.selectedTab < t.tabs.length - 1) {
                    cookieService.set('tabs.selectedTab', t.selectedTab);
                }
            });
        }

        t.onInit();
    }
})();