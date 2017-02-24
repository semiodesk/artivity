(function () {
    angular.module('app').directive('artSettingsAccounts', AccountsSettingsDirective);

    function AccountsSettingsDirective() {
        return {
            scope: {},
            templateUrl: 'app/directives/art-settings-accounts/art-settings-accounts.html',
            controller: AccountsSettingsDirectiveFormController
        }
    };

    angular.module('app').controller('AccountsSettingsDirectiveFormController', AccountsSettingsDirectiveFormController);

    AccountsSettingsDirectiveFormController.$inject = ['$scope', '$uibModal', 'api', 'settingsService'];

    function AccountsSettingsDirectiveFormController($scope, $uibModal, api, settingsService) {
        var t = this;
        var s = $scope;

        if (settingsService) {
            // Register the controller with its parent for global apply/cancel.
            settingsService.registerController(t);
        }

        s.selectedItem = null;

        // Load the user accounts.
        s.accounts = [];

        api.getAccounts().then(function (data) {
            s.accounts = data;
        });

        s.addAccount = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'app/dialogs/add-account-dialog/add-account-dialog.html',
                controller: 'AddAccountDialogController',
                controllerAs: 't'
            });

            modalInstance.result.then(function (account) {
                console.log("Reloading Accounts");

                // Reload the user accounts.
                api.getAccounts().then(function (data) {
                    s.accounts = data;
                });
            });
        };

        s.selectAccount = function (account) {
            s.selectedItem = account;
        };

        s.uninstallAccount = function (a) {
            api.uninstallAccount(a.Uri).then(function (data) {
                console.log("Account uninstalled:", a.Uri);

                var i = s.accounts.indexOf(a);

                s.accounts.splice(i, 1);
            });
        };

        t.submit = function () {
            // Changes are handled by the addAccount and uninstallAccount functions.
            // Nothing to sumbmit here, yet.
        };

        t.reset = function () {
            s.form.reset();
        };
    };
})();