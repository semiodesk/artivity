(function () {
    'use strict';

    angular.module('explorerApp').controller('PublishFileDialogController', PublishFileDialogController);

    function PublishFileDialogController(api, $scope, $filter, $uibModalInstance, $sce, fileService, clientService) {
        $scope.dialog = {
            step: 'publishing-options',
            title: 'Publish File',
            subtitle: 'Create a dataset for your file and upload it into a digital repository.'
        };

        $scope.getFileName = fileService.getFileName;
        $scope.getFileNameWithoutExtension = fileService.getFileNameWithoutExtension;
        $scope.getFileExtension = fileService.getFileExtension;
        $scope.hasFileThumbnail = api.hasThumbnail;
        $scope.getFileThumbnailUrl = api.getThumbnailUrl;

        console.log($scope.entity);

        // Accounts
        $scope.accounts = [];
        $scope.selectedAccount = null;

        // At first, we need to determine if there are any accounts which can be used for publishing.
        api.getAccountsWithFeature('http://w3id.org/art/terms/1.0/features/PublishArchive').then(function (data) {
            console.log("Accounts:", data);

            $scope.accounts = data;

            if (0 < $scope.accounts.length) {
                var account = $scope.accounts[0];

                $scope.selectedAccount = account;

                $scope.authentication = {
                    protocol: account.AuthenticationProtocol.Uri,
                    parameter: {}
                };

                for (var i = 0; i < account.AuthenticationParameters.length; i++) {
                    var p = account.AuthenticationParameters[i];

                    $scope.authentication.parameter[p.Name] = p.Value;
                }
            } else {
                $scope.dialog.step = 'no-accounts';
                $scope.dialog.title = 'No Accounts';
                $scope.dialog.subtitle = 'You have not yet added any accounts which can be used for publication.';
            }
        });

        $scope.selectAccount = function () {
            $scope.dialog.step = 'upload-select-account';
            $scope.dialog.title = 'Select Account';
            $scope.dialog.subtitle = 'Choose the account used for publication and authorize the upload by logging in.';
        };

        // Publishing
        $scope.archive = {
            title: 'Artivity data for ' + $scope.file.label,
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
                }
            ],
            contentOptions: {
                includeFile: true,
                includeEditingHistory: true,
                includeBrowsingHistory: false,
                includeComments: false
            }
        };

        $scope.archive.license = $scope.archive.licenseOptions[0].uri;

        // Load author information.
        $scope.userPhotoUrl = api.getUserPhotoUrl();

        api.getUser().then(function (data) {
            $scope.user = data;

            $scope.archive.creators = [{
                name: data.Name,
                email: data.EmailAddress
            }];
        });

        // Upload
        var interval = undefined;

        $scope.beginUpload = function () {
            $scope.dialog.step = 'upload-progress';
            $scope.dialog.title = "Publishing File";

            $scope.progress = {
                Tasks: [],
                CurrentTask: '',
                PercentComplete: 0
            };

            api.uploadArchive($scope.selectedAccount.Uri, $scope.entity.uri, $scope.authentication.parameter, $scope.archive).then(function (data) {
                var sessionId = data.Id;

                console.log("Session: ", sessionId);

                clientService.pollServiceState(api, sessionId, function (intervalHandle, state) {
                    interval = intervalHandle;

                    $scope.dialog.subtitle = $filter('translate')(state.Progress.CurrentTask.Id + "#description");

                    $scope.progress = state.Progress;

                    if (parseInt(state.Progress.PercentComplete) === 100) {
                        clearInterval(interval);

                        // Delay the closing of the window so that the UI can update the progress.
                        setTimeout($scope.endUpload, 2000);
                    }
                });
            });
        };

        $scope.endUpload = function () {
            if (interval) {
                clearInterval(interval);
            }

            if ($scope.percentComplete < 100) {
                $scope.percentComplete = 0;
            } else {
                $uibModalInstance.close();
            }
        };

        $scope.cancel = function () {
            $scope.endUpload();

            $uibModalInstance.dismiss('cancel');
        };
    };
})();