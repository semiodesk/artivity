(function () {
    angular.module('app').controller("SettingsStateController", SettingsStateController);

    SettingsStateController.$inject = ['$scope', '$state', 'settingsService', 'tabService'];

    function SettingsStateController($scope, $state, settingsService, tabService) {
        var t = this;

        t.navigateBack = function (e) {
            var context = tabService.getSelectedTabContext();

            if (context) {
                $state.go(context.state.name, context.stateParams);
            } else {
                context = tabService.getTabContext(0);

                $state.go(context.state.name, context.stateParams);
            }
        }

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
            t.navigateBack();
        };

        t.reset = function () {
            settingsService.resetAll();

            // Navigate to dashboard and refresh the page.
            t.navigateBack();
        };
    }
})();