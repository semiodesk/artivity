(function () {
    angular.module('app').directive('artSettingsUser', UserSettingsDirective);

    function UserSettingsDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-settings-user/art-settings-user.html',
            controller: UserSettingsDirectiveFormController,
            controllerAs: 't',
            bindToController: true,
            scope: {
                setup: '=?setup'
            }
        }
    };

    angular.module('app').controller('UserSettingsDirectiveFormController', UserSettingsDirectiveFormController);

    UserSettingsDirectiveFormController.$inject = ['$rootScope', '$scope', 'api', 'agentService', 'settingsService'];

    function UserSettingsDirectiveFormController($rootScope, $scope, api, agentService, settingsService) {
        var t = this;

        // Load the user data.
        t.user = null;
        t.userPhotoUrl = null;

        t.setUser = function (user) {
            t.user = user;
            t.userPhoto = null;
            t.userPhotoUrl = user.PhotoUrl;
        };

        t.onPhotoChanged = function (file) {
            t.userPhoto = file;
            $scope.userForm.$pristine = false;
        };

        // BACKUP
        t.backupStatus = null;

        t.createBackup = function () {
            var fileName = 'Unknown';

            if (t.user.Name) {
                fileName = t.user.Name;
            }

            fileName += '-' + moment().format('DDMMYYYY') + '.artb';

            console.log("Creating backup to file:", fileName);

            api.backupUserProfile(fileName).then(function (data) {
                t.backupStatus = data;

                if (t.backupStatus.Id && t.backupStatus.Error === null) {
                    var interval = setInterval(function () {
                        api.getUserProfileBackupStatus(t.backupStatus.Id).then(function (data) {
                            t.backupStatus.PercentComplete = data.PercentComplete;
                            t.backupStatus.Error = data.Error;

                            if (t.backupStatus.PercentComplete === 100 || t.backupStatus.Error) {
                                clearInterval(interval);

                                if (t.backupStatus.Error) {
                                    console.log(t.backupStatus.Error);
                                }
                            }
                        });
                    }, 500);
                }
            });
        };

        t.submit = function () {
            if ($scope.userForm.$valid) {
                console.log("Submitting Profile");

                if (t.user) {
                    api.putUser(t.user).then(function () {
                        agentService.off('currentUserChanged', t.setUser);
                        agentService.initialize();
                        agentService.on('currentUserChanged', t.setUser);
                    });
                }

                if (t.userPhoto) {
                    api.putUserPhoto(t.user.Uri, t.userPhoto).then(function () {
                        t.userPhotoUrl = '';
                        t.userPhotoUrl = api.getUserPhotoUrl(t.user.Uri);
                    });
                }

                if (t.setup) {
                    $rootScope.$emit('navigateNext');
                }
            }
        };

        t.reset = function () {
            $scope.userForm.reset();
        };

        t.$onInit = function () {
            // Register the controller with its parent for global apply/cancel.
            settingsService.registerController(t);

            if (agentService.currentUser) {
                t.setUser(agentService.currentUser);
            }

            agentService.on('currentUserChanged', t.setUser);
        }

        t.$onDestroy = function () {
            agentService.off('currentUserChanged', t.setUser);
        }
    };
})();