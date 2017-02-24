(function () {
    angular.module('app').directive('artSettingsApps', AppsSettingsDirective);

    function AppsSettingsDirective() {
        return {
            scope: {},
            templateUrl: 'app/directives/art-settings-apps/art-settings-apps.html',
            controller: AppsSettingsDirectiveFormController
        }
    };

    angular.module('app').controller('AppsSettingsDirectiveFormController', AppsSettingsDirectiveFormController);

    AppsSettingsDirectiveFormController.$inject = ['$scope', 'api', 'settingsService'];

    function AppsSettingsDirectiveFormController($scope, api, settingsService) {
        var t = this;
        var s = $scope;

        if (settingsService) {
            // Register the controller with its parent for global apply/cancel.
            settingsService.registerController(t);
        }

        s.agents = [];

        s.hasError = false;
        s.errorType = '';
        s.errorMessage = '';

        s.toggleInstall = function (agent) {
            if (agent.pluginInstalled) {
                api.installAgent(agent.uri).then(function (response) {
                    agent.pluginInstalled = response.success;

                    s.hasError = !response.success;
                    s.errorType = response.error.data.type;
                    s.errorMessage = response.error.data.message;
                });
            } else {
                api.uninstallAgent(agent.uri).then(function (response) {
                    agent.pluginInstalled = !response.success;

                    s.hasError = !response.success;
                    s.errorType = response.error.data.type;
                    s.errorMessage = response.error.data.message;
                });
            }
        };

        s.reload = function () {
            s.hasError = false;

            api.getAgents().then(function (data) {
                s.agents = [];

                for (var i = 0; i < data.length; i++) {
                    var agent = data[i];

                    if (agent.IsSoftwareInstalled) {
                        s.agents.push({
                            uri: agent.Manifest.AgentUri,
                            name: agent.Manifest.DisplayName,
                            color: agent.Manifest.DefaultColor,
                            associationUri: agent.AssociationUri,
                            iconSrc: api.getAgentIconUrl(agent.Manifest.AgentUri),
                            softwareInstalled: agent.IsSoftwareInstalled,
                            softwareVersion: agent.DetectedVersions.join(),
                            pluginInstalled: agent.IsPluginInstalled,
                            pluginVersion: agent.Manifest.PluginVersion,
                            pluginEnabled: agent.IsPluginEnabled
                        });
                    }
                }

                s.agents.sort(function (a, b) {
                    return a.name.localeCompare(b.name);
                });

                // This somehow started throwing errors after i added the project list ~ Mo
                if (s.form != undefined)
                    s.form.$setPristine();
            });
        }

        s.reload();

        t.submit = function () {
            if (s.agents.length > 0) {
                console.log("Submitting Apps");

                api.setAgents(s.agents);
            }
        };

        t.reset = function () {
            s.form.reset();
        };
    };
})();