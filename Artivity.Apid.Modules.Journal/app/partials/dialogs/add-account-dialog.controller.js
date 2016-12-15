angular.module('explorerApp').controller('AddAccountDialogController', AddAccountDialogController);

function AddAccountDialogController(api, $scope, $filter, $uibModalInstance, $sce, clientService) {
    var t = this;
    var s = $scope;
    var interval = undefined;

    s.title = $filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE');
    s.clients = [];

    api.getAccountClients().then(function (data) {
        s.clients = data;

        console.log("Available clients:", s.clients);
    });

    s.selectedClient = null;

    s.selectClient = function (client) {
        s.title = ($filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE_X')).replace('{0}', client.Title);
        s.selectedClient = client;
        s.parameter = {
            clientUri: client.Uri,
            authType: client.SupportedAuthenticationClients[0].Uri
        };

        // TODO: Remove hard-wiring. Receive presets and target sites from client.
        if (client.Uri === 'http://orcid.org') {
            s.parameter.presetId = 'orcid.org';

            s.connectAccount(s.selectedClient);
        } else if (client.Uri === 'http://eprints.org') {
            s.parameter.url = 'https://ualresearchonline.arts.ac.uk';
        }

        console.log("Client selected: ", client);
    }

    // Prevent an account from being installed twice.
    s.isInstalling = false;

    // The client state '0' refers to 'None'.	
    s.clientState = 0;

    s.connectAccount = function (client) {
        // The client state '1' refers to 'InProgress'.
        s.clientState = 1;

        api.authorizeAccount(s.parameter).then(function (data) {
            var sessionId = data.Id;

            clientService.pollServiceState(api, sessionId, function (intervalHandle, state) {
                interval = intervalHandle;

                console.log(state);

                for (var i = 0; i < state.Client.SupportedAuthenticationClients.length; i++) {
                    var c = state.Client.SupportedAuthenticationClients[i];

                    // Allow iframes to connect to the URL.
                    s.clientUrl = $sce.trustAsResourceUrl(c.AuthorizeUrl);

                    if (c.ClientState > 1) {
                        clearInterval(interval);

                        s.clientState = c.ClientState;

                        // The client state '2' refers to 'Authorized'.
                        if (!s.isInstalling && c.ClientState == 2) {
                            s.isInstalling = true;

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

    s.cancel = function () {
        $uibModalInstance.dismiss('cancel');

        if (interval) {
            window.clearInterval(interval);
        }
    };
};