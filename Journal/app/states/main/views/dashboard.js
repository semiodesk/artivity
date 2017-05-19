(function () {
    angular.module('app').controller("DashboardViewController", DashboardViewController);

    DashboardViewController.$inject = ['$rootScope', '$scope', '$state', '$stateParams', 'api', 'agentService', 'projectService'];

    function DashboardViewController($rootScope, $scope, $state, $stateParams, api, agentService, projectService) {
        var t = this;

        t.openProject = function (project) {
            $rootScope.$broadcast('openProject', {
                data: project
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
        }

        t.loadRecentFiles = function () {
            return new Promise(function (resolve, reject) {
                t.files = [];

                api.getRecentFiles().then(function (data) {
                    t.files = data;
                });

                resolve(t.files);
            });
        }

        t.$onInit = function () {
            agentService.getCurrentUser().then(function (user) {
                    t.user = user;
                    t.userPhotoUrl = t.user ? t.user.PhotoUrl : '';
                }, function () {
                    t.user = null;
                    t.userPhotoUrl = '';
                })
                .then(t.loadProjects)
                .then(t.loadRecentFiles);
        }
    }
})();