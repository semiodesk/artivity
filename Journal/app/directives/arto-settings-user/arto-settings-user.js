(function () {
    angular.module('app').directive('artoSettingsUser', ArtivityOnlineUserSettingsDirective);

    function ArtivityOnlineUserSettingsDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/arto-settings-user/arto-settings-user.html',
            controller: ArtivityOnlineUserSettingsDirectiveFormController,
            controllerAs: 't',
            bindToController: true
        }
    };

    angular.module('app').controller('ArtivityOnlineUserSettingsDirectiveFormController', ArtivityOnlineUserSettingsDirectiveFormController);

    ArtivityOnlineUserSettingsDirectiveFormController.$inject = ['$rootScope', '$scope', 'api', 'agentService', 'settingsService', 'syncService', 'clientService'];

    function ArtivityOnlineUserSettingsDirectiveFormController($rootScope, $scope, api, agentService, settingsService, syncService, clientService) {
        var t = this;

        // The Artivity Online account client.
        t.client = null;

        // The client state '0' refers to 'None'.	
        t.clientState = 0;

        // The authentication parameters.
        t.parameter = {};

        // Indicates if the client is connecting.
        t.connecting = false;

        // Indicates if the client is synchronizing.
        t.synchronizing = false;

        // Indicates if the account has been successfully connected and set up.
        t.installed = false;

        t.connect = function () {
            t.connecting = true;

            t.clientState = 1;

            var interval = undefined;

            api.authorizeAccount(t.parameter).then(function (data) {
                var sessionId = data.Id;

                clientService.pollServiceState(api, sessionId, function (intervalHandle, state) {
                    interval = intervalHandle;

                    for (var i = 0; i < state.Client.SupportedAuthenticationClients.length; i++) {
                        var c = state.Client.SupportedAuthenticationClients[i];

                        if (c.ClientState > 1) {
                            clearInterval(interval);

                            t.clientState = c.ClientState;

                            // The client state '2' refers to 'Authorized'.
                            if (c.ClientState == 2) {
                                t.clientState = 0;

                                api.installAccount(sessionId).then(function (r) {
                                    console.log("Account installed:", sessionId);

                                    t.installed = true;

                                    syncService.synchronize();

                                    $rootScope.$emit('navigateNext');
                                });
                            }

                            break;
                        }
                    }
                });
            });
        }

        t.submit = function () {};

        t.reset = function () {
            $scope.loginForm.reset();
        };

        t.$onInit = function () {
            // Register the controller with its parent for global apply/cancel.
            settingsService.registerController(t);

            api.getAccounts().then(function (accounts) {
                if (accounts) {
                    for (i = 0; i < accounts.length; i++) {
                        var account = accounts[i];

                        // There is already an installed Artivity Online account.
                        if (account.ServiceClient.Uri === 'http://artivity.online/') {
                            t.installed = true;

                            $rootScope.$emit('navigateNext');
                        }
                    }
                }

                if (!t.installed) {
                    api.getAccountClient('http://artivity.online').then(function (client) {
                        if (client) {
                            t.client = client;
                            t.parameter.url = 'http://localhost:8080';
                            t.parameter.clientUri = client.Uri;
                            t.parameter.authType = client.SupportedAuthenticationClients[0].Uri;
                            t.parameter.username = '';
                            t.parameter.password = '';
                        } else {
                            console.warn('Unable to load Artivity Online service client.');
                        }
                    });
                }
            });
        }
    };
})();