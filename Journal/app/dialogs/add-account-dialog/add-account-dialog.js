(function () {
    angular.module('app').controller('AddAccountDialogController', AddAccountDialogController);

    AddAccountDialogController.$inject = ['$scope', '$mdDialog', '$filter', '$sce', 'api', 'clientService'];

    function AddAccountDialogController($scope, $mdDialog, $filter, $sce, api, clientService) {
        var t = this;
        var interval = undefined;

        t.title = $filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE');
        t.clients = [];

        api.getAccountClients().then(function (data) {
            var clients = [];

            for (var i = 0; i < data.length; i++) {
                var client = data[i];

                // Do not allow to add Artivity Online accounts via the accounts tab.
                if (!client.Uri.startsWith('http://artivity.online')) {
                    clients.push(client);
                }
            }

            t.clients = clients;

            console.log("Available clients:", t.clients);
        });

        t.selectedClient = null;

        t.selectClient = function (client) {
            // On macOS the Mono serializes the URIs differently - of course - and appends a trailing slash.. :/
            if (client.Uri.endsWith('/')) {
                client.Uri = client.Uri.slice(0, -1);
            }

            t.title = ($filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE_X')).replace('{0}', client.Title);
            t.selectedClient = client;
            t.parameter = {
                clientUri: client.Uri,
                authType: client.SupportedAuthenticationClients[0].Uri
            };

            // TODO: Remove hard-wiring. Receive presets and target sites from client.
            if (client.Uri.startsWith('http://orcid.org')) {
                t.parameter.presetId = 'orcid.org';

                // Open the ORCiD website in the systems default browser.
                var shell = require('electron').shell;
                shell.openExternal(client.SupportedAuthenticationClients[0].AuthorizeUrl);

                t.connectAccount(t.selectedClient);
            } else if (client.Uri === 'http://eprints.org') {
                t.parameter.url = 'https://ualresearchonline.arts.ac.uk';
            }

            console.log("Client selected: ", client);
        }

        // Prevent an account from being installed twice.
        t.isInstalling = false;

        // The client state '0' refers to 'None'.	
        t.clientState = 0;

        t.connectAccount = function (client) {
            // The client state '1' refers to 'InProgress'.
            t.clientState = 1;

            api.authorizeAccount(t.parameter).then(function (data) {
                var sessionId = data.Id;

                clientService.pollServiceState(api, sessionId, function (intervalHandle, state) {
                    interval = intervalHandle;

                    console.log(state);

                    for (var i = 0; i < state.Client.SupportedAuthenticationClients.length; i++) {
                        var c = state.Client.SupportedAuthenticationClients[i];

                        // Allow iframes to connect to the URL.
                        t.clientUrl = $sce.trustAsResourceUrl(c.AuthorizeUrl);

                        if (c.ClientState > 1) {
                            clearInterval(interval);

                            t.clientState = c.ClientState;

                            // The client state '2' refers to 'Authorized'.
                            if (!t.isInstalling && c.ClientState == 2) {
                                t.isInstalling = true;

                                api.installAccount(sessionId).then(function (r) {
                                    console.log("Account installed:", sessionId);

                                    // Close the dialog after the account was successfully connected.
                                    setTimeout(function () {
                                        $uibModalInstance.close();
                                    }, 1000);
                                });
                            }

                            break;
                        }
                    }
                });
            });
        };

        t.cancel = function () {
            if (interval) {
                // Stop polling for changes in the installation progress..
                window.clearInterval(interval);
            }

            $mdDialog.cancel();
        };
    };
})();