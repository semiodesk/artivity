(function () {
    angular.module('app').directive('artSettingsUser', UserSettingsDirective);

    function UserSettingsDirective() {
        return {
            scope: {
                backupEnabled: '@backupEnabled'
            },
            templateUrl: 'app/directives/art-settings-user/art-settings-user.html',
            controller: UserSettingsDirectiveFormController
        }
    };

    angular.module('app').controller('UserSettingsDirectiveFormController', UserSettingsDirectiveFormController);

    UserSettingsDirectiveFormController.$inject = ['$scope', 'api', 'agentService', 'settingsService'];

    function UserSettingsDirectiveFormController($scope, api, agentService, settingsService) {
        var t = this;
        var s = $scope;

        if (settingsService) {
            // Register the controller with its parent for global apply/cancel.
            settingsService.registerController(t);
        }

        // Load the user data.
        s.user = agentService.currentUser;
        
        agentService.getAccountOwner().then(function (data) {
            s.user = data;
            s.userPhoto = null;
            s.userPhotoUrl = agentService.getPhotoUrl(s.user.Uri);
        });

        s.onPhotoChanged = function (file) {
            s.userPhoto = file;
            s.form.$pristine = false;
        };

        // Set attribute default values.
        if (s.backupEnabled === undefined) {
            s.backupEnabled = true;
        }

        s.backupStatus = null;

        s.createBackup = function () {
            var fileName = 'Unknown';

            if (s.user.Name) {
                fileName = s.user.Name;
            }

            fileName += '-' + moment().format('DDMMYYYY') + '.artb';

            console.log("Creating backup to file:", fileName);

            api.backupUserProfile(fileName).then(function (data) {
                s.backupStatus = data;

                if (s.backupStatus.Id && s.backupStatus.Error === null) {
                    var interval = setInterval(function () {
                        api.getUserProfileBackupStatus(s.backupStatus.Id).then(function (data) {
                            s.backupStatus.PercentComplete = data.PercentComplete;
                            s.backupStatus.Error = data.Error;

                            if (s.backupStatus.PercentComplete === 100 || s.backupStatus.Error) {
                                clearInterval(interval);

                                if (s.backupStatus.Error) {
                                    console.log(s.backupStatus.Error);
                                }
                            }
                        });
                    }, 500);
                }
            });
        };

        t.submit = function () {
            console.log("Submitting Profile");

            if (s.user) {
                api.putUser(s.user);
            }

            if (s.userPhoto) {
                api.putUserPhoto(s.user.Uri, s.userPhoto).then(function () {
                    s.userPhotoUrl = '';
                    s.userPhotoUrl = api.getUserPhotoUrl(s.user.Uri);
                });
            }
        };

        t.reset = function () {
            s.form.reset();
        };
    };
})();