(function () {
    angular.module('app').controller("DashboardViewController", DashboardViewController);

    DashboardViewController.$inject = ['$scope', '$state', '$stateParams', 'api', 'agentService', 'tabService', 'projectService'];

    function DashboardViewController($scope, $state, $stateParams, api, agentService, tabService, projectService) {
        var t = this;

        t.openProject = function(project) {
            tabService.addTab(project);
            tabService.saveTabs();
        }

        t.$onInit = function () {
            agentService.getCurrentUser().then(function(user) {
                t.user = user;
                t.userPhotoUrl = t.user ? t.user.PhotoUrl : '';
            }, function() {
                t.user = null;
                t.userPhotoUrl = '';
            });

            t.projects = [];

            projectService.getAll().then(function (data) {
                t.projects = data;
            })

            t.files = [];

            api.getRecentFiles().then(function (data) {
                t.files = data;
            });
        }
    }
})();