(function () {
    angular.module('app').controller("SettingsStateController", SettingsStateController);

    SettingsStateController.$inject = ['$scope', '$state', 'settingsService', 'tabService'];

    function SettingsStateController($scope, $state, settingsService, tabService) {
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
        };

        t.reset = function () {
            settingsService.resetAll();
        };
    }
})();