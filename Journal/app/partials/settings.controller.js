(function () {
    angular.module('explorerApp').controller('SettingsController', SettingsController);

    function SettingsController(api, $location, settingsService) {
        var t = this;

        t.submit = function () {
            settingsService.submitAll();
        };

        t.submitAndReturn = function () {
            settingsService.submitAll();

            // Navigate to dashboard and refresh the page.
            $location.path('/');
        };

        t.reset = function () {
            settingsService.resetAll();
        };
    };
})();