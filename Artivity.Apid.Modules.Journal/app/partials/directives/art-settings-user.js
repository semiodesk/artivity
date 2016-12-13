var app = angular.module('explorerApp');

app.directive('artSettingsUser', UserSettingsDirective);

function UserSettingsDirective() {
    return {
        scope: {
            backupEnabled: '@backupEnabled'
        },
        templateUrl: 'partials/directives/art-settings-user.html',
        controller: UserSettingsDirectiveFormController
    }
};

app.controller('UserSettingsDirectiveFormController', UserSettingsDirectiveFormController);

function UserSettingsDirectiveFormController (api, $scope, settingsService) {
    var t = this;
    var s = $scope;

    if(settingsService) {
        // Register the controller with its parent for global apply/cancel.
        settingsService.registerController(t);
    }

    // Load the user data.
    api.getUser().then(function (data) {
        s.user = data;
    });

    // Set the user photo URL.
    s.userPhotoUrl = api.getUserPhotoUrl();

    s.onPhotoChanged = function (e) {
        // Update the preview image..
        var files = window.event.srcElement.files;

        if (FileReader && files.length) {
            var reader = new FileReader();

            reader.onload = function () {
                document.getElementById('photo-img').src = reader.result;
            }

            reader.readAsDataURL(files[0]);

            s.form.$pristine = false;
        }
    };

    // Set attribute default values.
    if(s.backupEnabled === undefined) {
        s.backupEnabled = true;
    }
    
    s.backupStatus = null;
    
    s.createBackup = function () {
        var fileName = 'Unknown';
        
        if(s.user.Name) {
            fileName = s.user.Name;
        }
        
        fileName += '-' + moment().format('DDMMYYYY') + '.artb';
        
        console.log("Creating backup to file:", fileName);
        
        api.backupUserProfile(fileName).then(function (data) {
            s.backupStatus = data;
            
            if(s.backupStatus.Id && s.backupStatus.Error === null) {
                var interval = setInterval(function() {					
                    api.getUserProfileBackupStatus(s.backupStatus.Id).then(function(data) {
                        s.backupStatus.PercentComplete = data.PercentComplete;
                        s.backupStatus.Error = data.Error;
                        
                        if(s.backupStatus.PercentComplete === 100 || s.backupStatus.Error) {
                            clearInterval(interval);
                            
                            if(s.backupStatus.Error) {
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
            api.setUser(s.user);
        }

        if (s.userPhoto) {
            api.setUserPhoto(s.userPhoto).then(function () {
                s.userPhotoUrl = '';
                s.userPhotoUrl = api.getUserPhotoUrl();
            });
        }
    };

    t.reset = function () {
        s.form.reset();
    };
};