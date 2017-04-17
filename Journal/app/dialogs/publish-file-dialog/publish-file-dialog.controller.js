(function () {
    angular.module('app').controller('PublishFileDialogController', PublishFileDialogController);

    PublishFileDialogController.$inject = ['$scope', '$filter', '$uibModalInstance', '$sce', 'api', 'agentService', 'selectionService', 'filesystemService', 'clientService'];

    function PublishFileDialogController($scope, $filter, $uibModalInstance, $sce, api, agentService, selectionService, filesystemService, clientService) {
        var t = this;

        t.dialog = {
            step: 'publishing-options',
            title: 'Publish Editing History',
            subtitle: 'Create a dataset for your file and upload it into a digital repository.'
        };

        t.file = selectionService.selectedItem();

        t.getFileName = filesystemService.getFileName;
        t.getFileNameWithoutExtension = filesystemService.getFileNameWithoutExtension;
        t.getFileExtension = filesystemService.getFileExtension;
        t.hasFileThumbnail = api.hasThumbnail;
        t.getFileThumbnailUrl = api.getThumbnailUrl;

        // Accounts
        t.accounts = [];
        t.selectedAccount = null;

        // At first, we need to determine if there are any accounts which can be used for publishing.
        api.getAccountsWithFeature('http://w3id.org/art/terms/1.0/features/PublishArchive').then(function (data) {
            console.log("Accounts:", data);

            t.accounts = data;

            if (0 < t.accounts.length) {
                var account = t.accounts[0];

                t.selectedAccount = account;

                t.authentication = {
                    protocol: account.AuthenticationProtocol.Uri,
                    parameter: {}
                };

                for (var i = 0; i < account.AuthenticationParameters.length; i++) {
                    var p = account.AuthenticationParameters[i];

                    t.authentication.parameter[p.Name] = p.Value;
                }
            } else {
                t.dialog.step = 'no-accounts';
                t.dialog.title = 'No Accounts';
                t.dialog.subtitle = 'You have not yet added any accounts which can be used for publication.';
            }
        });

        t.selectAccount = function () {
            t.dialog.step = 'upload-select-account';
            t.dialog.title = 'Select Account';
            t.dialog.subtitle = 'Choose the account used for publication and authorize the upload by logging in.';
        };

        // Publishing
        t.archive = {
            title: 'Artivity data for ' + t.file.label,
            description: '',
            creators: [],
            license: null,
            licenseOptions: [{
                uri: 'https://creativecommons.org/licenses/by-nc-nd/4.0/',
                label: 'Creative Commons BY-NC-ND',
                description: 'Attribution, Non-Commercial, No Derivatives'
            }, {
                uri: 'https://creativecommons.org/licenses/by-nc-sa/4.0/',
                label: 'Creative Commons BY-NC-SA',
                description: 'Attribution, Non-Commercial, Share Alike'
            }, {
                uri: 'https://creativecommons.org/licenses/by-nc/4.0/',
                label: 'Creative Commons BY-NC',
                description: 'Attribution, Non-Commercial'
            }, {
                uri: 'https://creativecommons.org/licenses/by-nd/4.0/',
                label: 'Creative Commons BY-ND',
                description: 'Attribution, No Derivatives'
            }, {
                uri: 'https://creativecommons.org/licenses/by-sa/4.0/',
                label: 'Creative Commons BY-SA',
                description: 'Attribution, Share Alike'
            }, {
                uri: 'https://creativecommons.org/licenses/by/4.0/',
                label: 'Creative Commons BY',
                description: 'Attribution'
            }],
            contentOptions: {
                includeFile: true,
                includeEditingHistory: true,
                includeBrowsingHistory: false,
                includeComments: false
            }
        };

        t.archive.license = t.archive.licenseOptions[0].uri;

        // Load author information.
        t.userPhotoUrl = api.getUserPhotoUrl();

        agentService.getAccountOwner().then(function (data) {
            t.user = data;

            t.archive.creators = [{
                name: data.Name,
                email: data.EmailAddress
            }];
        });

        // Upload
        var interval = undefined;

        t.beginUpload = function () {
            t.dialog.step = 'upload-progress';
            t.dialog.title = "Publishing File";

            t.progress = {
                Tasks: [],
                CurrentTask: '',
                PercentComplete: 0
            };

            api.uploadArchive(t.selectedAccount.Uri, t.file.uri, t.authentication.parameter, t.archive).then(function (data) {
                var sessionId = data.Id;

                console.log("Session: ", sessionId);

                clientService.pollServiceState(api, sessionId, function (intervalHandle, state) {
                    interval = intervalHandle;

                    t.dialog.subtitle = $filter('translate')('FILEPUBLISH.' + state.Progress.CurrentTask.Id + "#description");

                    t.progress = state.Progress;

                    if (parseInt(state.Progress.PercentComplete) === 100) {
                        clearInterval(interval);

                        // Delay the closing of the window so that the UI can update the progress.
                        setTimeout(t.endUpload, 2000);
                    }
                });
            });
        };

        t.endUpload = function () {
            if (interval) {
                clearInterval(interval);
            }

            if (t.percentComplete < 100) {
                t.percentComplete = 0;
            } else {
                $uibModalInstance.close();
            }
        };

        t.cancel = function () {
            t.endUpload();

            $uibModalInstance.dismiss('cancel');
        };
    };
})();