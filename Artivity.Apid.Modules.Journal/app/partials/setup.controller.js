angular.module('explorerApp').controller('SetupController', SetupController);

function SetupController(api, $scope, $rootScope, $location, $routeParams, $controller, settingsService) {
    var t = this;
    var s = $scope;

    // Inherit from SettingsController.
    $controller('SettingsController', {
        api: api,
        $scope: $scope,
        $rootScope: $rootScope,
        $location: location,
        $routeParams: $routeParams,
        settingsService: settingsService
    });

    s.steps = ['art-settings-user', 'art-settings-apps', 'art-settings-complete'];
    s.currentStep = s.steps[0];
    s.canGoNext = true;
    s.canGoBack = false;

    s.goNext = function() {
        var n = s.steps.indexOf(s.currentStep);

        if(n < s.steps.length - 1) {
            s.currentStep = s.steps[n + 1];
            s.canGoNext = n + 1 < s.steps.length - 1;

            onStepChanged();
        }
    }

    s.goBack = function() {
        var n = s.steps.indexOf(s.currentStep);

        if(n > 0) {
            s.currentStep = s.steps[n - 1];
            s.canGoBack = n - 1 > 0;

            onStepChanged();
        }
    }

    function onStepChanged() {
        var n = s.steps.indexOf(s.currentStep);

        if(n === s.steps.length - 1) {
            // Submit all setup pages.
            s.submit();

            api.setRunSetup(false).then(function() {
                // After the setup has been disabled, go to the homepage.
                $location.path("/");
            });
        }
    }
}