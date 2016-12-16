angular.module('explorerApp').controller('AddAccountDialogController', AddAccountDialogController);

function AddAccountDialogController(api, $scope, $filter, $uibModalInstance, $sce, clientService) {
    var t = this;
    var interval = undefined;

    t.title = $filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE');
    t.clients = [];

    api.getAccountClients().then(function (data) {
        t.clients = data;

        console.log("Available clients:", t.clients);
    });

    t.selectedClient = null;

    t.selectClient = function (client) {
        t.title = ($filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE_X')).replace('{0}', client.Title);
        t.selectedClient = client;
        t.parameter = {
            clientUri: client.Uri,
            authType: client.SupportedAuthenticationClients[0].Uri
        };

        // TODO: Remove hard-wiring. Receive presets and target sites from client.
        if (client.Uri === 'http://orcid.org') {
            t.parameter.presetId = 'orcid.org';

            t.connectAccount(t.selectedClient);
        } else if (client.Uri === 'http://eprintt.org') {
            t.parameter.url = 'https://ualresearchonline.artt.ac.uk';
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
        $uibModalInstance.dismiss('cancel');

        if (interval) {
            window.clearInterval(interval);
        }
    };
};