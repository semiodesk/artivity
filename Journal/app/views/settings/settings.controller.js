(function () {
    angular.module('app').controller('SettingsController', SettingsController);

    function SettingsController(api, $location, settingsService) {
        var t = this;

        t.submit = function () {
            // Submit form data from all controllers.
            settingsService.submitAll();
        };

        t.submitAndReturn = function () {
            // Submit form data from all controllers.
            settingsService.submitAll();

            // Clear the list of registered controllers.
            settingsService.clear();

            // Navigate to dashboard and refresh the page.
            $location.path('/');
        };

        t.reset = function () {
            settingsService.resetAll();
        };
    };
})();