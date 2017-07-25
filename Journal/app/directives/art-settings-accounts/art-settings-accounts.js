(function () {
    angular.module('app').directive('artSettingsAccounts', AccountsSettingsDirective);

    function AccountsSettingsDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/directives/art-settings-accounts/art-settings-accounts.html',
            controller: AccountsSettingsDirectiveFormController
        }
    };

    angular.module('app').controller('AccountsSettingsDirectiveFormController', AccountsSettingsDirectiveFormController);

    AccountsSettingsDirectiveFormController.$inject = ['$scope', '$mdDialog', 'api', 'settingsService'];

    function AccountsSettingsDirectiveFormController($scope, $mdDialog, api, settingsService) {
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

        s.addAccount = function (e) {
            $mdDialog.show({
                attachTo: angular.element(document.body),
                templateUrl: 'app/dialogs/add-account-dialog/add-account-dialog.html',
                controller: 'AddAccountDialogController',
                controllerAs: 't',
                bindToController: true,
                hasBackdrop: true,
                trapFocus: true,
                zIndex: 150,
                targetEvent: e,
                disableParentScroll: true,
                clickOutsideToClose: false,
                escapeToClose: true,
                focusOnOpen: true,
                locals: {
                    accounts: s.accounts
                }
            }).then(function () {
                console.log("Reloading Accounts..");

                // Reload the user accounts.
                api.getAccounts().then(function (data) {
                    s.accounts = data;
                });
            }, function () {});
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