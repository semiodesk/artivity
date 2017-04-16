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

    AppListDirectiveController.$inject = ['$scope', '$element', 'api'];

    function AppListDirectiveController($scope, $element, api) {
        var t = this;

        t.apps = [];

        t.$onInit = function () {
            t.loading = true;

            api.getAgents().then(function (data) {
                var agents = [];

                for (var i = 0; i < data.length; i++) {
                    var plugin = data[i];

                    var agent = {
                        uri: plugin.Manifest.AgentUri,
                        name: plugin.Manifest.DisplayName,
                        iconSrc: api.getAgentIconUrl(plugin.Manifest.AgentUri),
                        softwareInstalled: plugin.IsSoftwareInstalled,
                        pluginInstalled: plugin.IsPluginInstalled,
                        pluginEnabled: plugin.IsPluginEnabled,
                        autoInstall: plugin.Manifest.AutoInstall,
                        hasError: false
                    };

                    if (agent.softwareInstalled && agent.autoInstall) {
                        agents.push(agent);
                    }
                }

                agents.sort(function (a, b) {
                    return a.name.localeCompare(b.name);
                });

                t.apps = agents;
                t.loading = false;
            });
        }
    }
})();