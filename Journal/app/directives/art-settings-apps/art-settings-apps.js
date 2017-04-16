(function () {
    angular.module('app').directive('artSettingsApps', AppsSettingsDirective);

    function AppsSettingsDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-settings-apps/art-settings-apps.html',
            controller: AppsSettingsDirectiveFormController,
            controllerAs: 't',
            bindToController: true,
            scope: {
                'setup': '=?setup'
            }
        }
    };

    angular.module('app').controller('AppsSettingsDirectiveFormController', AppsSettingsDirectiveFormController);

    AppsSettingsDirectiveFormController.$inject = ['$scope', '$element', '$timeout', 'api', 'settingsService'];

    function AppsSettingsDirectiveFormController($scope, $element, $timeout, api, settingsService) {
        var t = this;

        t.initalized = false;
        t.ready = false;
        t.loading = false;

        // APPS
        t.agents = [];

        t.toggleInstall = function (agent, e) {
            if (e) {
                // Do not trigger collapsing the accordion panel body.
                e.preventDefault();
                e.stopImmediatePropagation();
            }

            if (agent.pluginInstalled) {
                t.installAgent(agent);
            } else {
                t.uninstallAgent(agent);
            }
        };

        t.loadAgents = function () {
            t.loading = true;

            api.getAgents().then(function (data) {
                var agents = [];

                for (var i = 0; i < data.length; i++) {
                    var plugin = data[i];

                    var agent = {
                        uri: plugin.Manifest.AgentUri,
                        name: plugin.Manifest.DisplayName,
                        color: plugin.Manifest.DefaultColor,
                        associationUri: plugin.AssociationUri,
                        iconSrc: api.getAgentIconUrl(plugin.Manifest.AgentUri),
                        softwareInstalled: plugin.IsSoftwareInstalled,
                        softwareVersion: plugin.DetectedVersions.join(),
                        pluginInstalled: plugin.IsPluginInstalled,
                        pluginVersion: plugin.Manifest.PluginVersion,
                        pluginEnabled: plugin.IsPluginEnabled,
                        autoInstall: plugin.Manifest.AutoInstall,
                        hasError: false
                    };

                    if (t.setup && agent.softwareInstalled && !agent.pluginInstalled && agent.autoInstall) {
                        t.installAgent(agent);
                    }

                    if (agent.softwareInstalled) {
                        agents.push(agent);
                    }
                }

                agents.sort(function (a, b) {
                    return a.name.localeCompare(b.name);
                });

                t.agents = agents;

                // This somehow started throwing errors after i added the project list ~ Mo
                if ($scope.appsForm.form != undefined) {
                    $scope.appsForm.$setPristine();
                }

                t.ready = t.hasInstalledAgents();
                t.loading = false;
            });

            if (!t.initalized) {
                t.initalized = true;
            }
        };

        t.hasInstalledAgents = function () {
            for (i = 0; i < t.agents.length; i++) {
                var agent = t.agents[i];

                // Editing software is installed automatically.
                if (agent.autoInstall && agent.pluginInstalled) {
                    return true;
                }
            }

            return false;
        }

        t.installAgent = function (agent) {
            api.installAgent(agent.uri).then(function (response) {
                agent.pluginInstalled = response.success;

                agent.hasError = !response.success;

                if (agent.hasError) {
                    agent.errorType = response.error.data.type;
                    agent.errorMessage = response.error.data.message;
                }

                t.ready = t.hasInstalledAgents();
            });
        }

        t.uninstallAgent = function (agent) {
            api.uninstallAgent(agent.uri).then(function (response) {
                agent.pluginInstalled = !response.success;

                agent.hasError = !response.success;

                if (agent.hasError) {
                    agent.errorType = response.error.data.type;
                    agent.errorMessage = response.error.data.message;
                }

                t.ready = t.hasInstalledAgents();
            });
        }

        t.reload = function () {
            t.loadAgents();
        }

        t.submit = function () {
            if (t.agents.length > 0) {
                console.log("Submitting Apps");

                api.setAgents(t.agents);
            }
        };

        t.reset = function () {
            $scope.appsForm.reset();
        };

        t.$onInit = function () {
            // Register the controller with its parent for global apply/cancel.
            settingsService.registerController(t);
        }

        t.$postLink = function () {
            $element.on('appear', function () {
                if (!t.initalized) {
                    t.loadAgents();
                }
            });
        }
    };
})();