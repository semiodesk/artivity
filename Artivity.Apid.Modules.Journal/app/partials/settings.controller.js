angular.module('explorerApp').controller('SettingsController', SettingsController);

function SettingsController(api, $scope, $rootScope, $location, $routeParams, settingsService) {
    var t = this;
    var s = $scope;

    s.submit = function () {
        settingsService.controllers(function(c) {
            if(c.submit) {
                c.submit();
            }
        });
    };

    s.$watch('agent.iconUrl', function () {
        if (s.agent && s.agent.iconUrl !== "") {
            timeline.setUserPhotoUrl(scope.user.photoUrl);
        }
    });

    s.submitAndReturn = function () {
        s.submit();

        // Navigate to dashboard and refresh the page.
        $location.path('/');
    };

    s.reset = function () {
        settingsService.controllers(function(c) {
            if(c.reset) {
                c.reset();
            }
        });
    };
};