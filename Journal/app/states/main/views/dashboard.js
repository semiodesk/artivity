(function () {
    angular.module('app').controller("DashboardViewController", DashboardViewController);

    DashboardViewController.$inject = ['$rootScope', '$scope', '$state', '$stateParams', '$templateCache', '$mdMenu', '$mdDialog', 'api', 'agentService', 'projectService'];

    function DashboardViewController($rootScope, $scope, $state, $stateParams, $templateCache, $mdMenu, $mdDialog, api, agentService, projectService) {
        var t = this;

        t.openProject = function (project) {
            $rootScope.$broadcast('openProject', {
                data: project
            });
        };

        t.openMenu = function (e, $mdMenu) {
            $mdMenu.open(e);
        };

        t.viewFile = function (e, file) {
            if (file) {
                $stateParams.fileUri = file.uri;
				$state.go('main.view.document-history', $stateParams);
            }
        }

        t.deleteProject = function (e, project) {
            var template = $templateCache.get('delete-project-dialog.html');

            $mdDialog.show({
                parent: angular.element(document.body),
                template: template,
                targetEvent: e,
                controller: function($scope) {
                    $scope.project = project;

                    $scope.ok = function () {
                        projectService.remove(project.Uri).then(function() {
                            t.projects.splice(t.projects.indexOf(project), 1);
                        });

                        $mdDialog.hide();
                    }

                    $scope.cancel = $mdDialog.cancel;
                }
            });
        };

        t.loadUserAgent = function () {
            return agentService.getCurrentUser().then(function (user) {
                t.user = user;
                t.userPhotoUrl = t.user ? t.user.PhotoUrl : '';
            }, function () {
                t.user = null;
                t.userPhotoUrl = '';
            });
        };

        t.loadUserAccount = function() {
            return agentService.getArtivityAccount().then(function(account) {
                t.userAccountId = null;
                
                for (var i = 0; i < account.AuthenticationParameters.length; i++) {
                    var p = account.AuthenticationParameters[i];

                    if (p.Name === 'username') {
                        t.userAccountId = p.Value;

                        break;
                    }
                }
            });
        }

        t.loadProjects = function () {
            return new Promise(function (resolve, reject) {
                t.projects = [];

                projectService.getAll().then(function (data) {
                    t.projects = data;
                });

                resolve(t.projects);
            });
        };

        t.loadRecentFiles = function () {
            return new Promise(function (resolve, reject) {
                t.files = [];

                api.getRecentFiles().then(function (data) {
                    t.files = data;
                });

                resolve(t.files);
            });
        };

        t.$onInit = function () {
            t.loadUserAgent()
                .then(t.loadUserAccount)
                .then(t.loadProjects)
                .then(t.loadRecentFiles);
        };
    }
})();