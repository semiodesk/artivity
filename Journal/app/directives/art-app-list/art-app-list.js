(function () {
    angular.module('app').directive('artAppList', artAppListDirective);

    function artAppListDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-app-list/art-app-list.html',
            scope: {},
            controller: AppListDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('AppListDirectiveController', AppListDirectiveController);

    AppListDirectiveController.$inject = ['$scope', '$element', 'agentService'];

    function AppListDirectiveController($scope, $element, agentService) {
        var t = this;

        t.apps = [];

        t.$onInit = function () {
            t.loading = true;

            t.apps = agentService.getActiveSoftwareAgents();

            t.loading = false;
        }
    }
})();