var app = angular.module('explorerApp');

app.directive('artSettingsAccounts', AccountsSettingsDirective);

function AccountsSettingsDirective() {
    return {
        scope: {},
        templateUrl: 'partials/directives/art-settings-accounts.html',
        controller: AccountsSettingsDirectiveFormController
    }
};

app.controller('AccountsSettingsDirectiveFormController', AccountsSettingsDirectiveFormController);

function AccountsSettingsDirectiveFormController (api, $scope, settingsService, $uibModal) {
    var t = this;
    var s = $scope;

    if(settingsService) {
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
            templateUrl: 'partials/dialogs/add-account-dialog.html',
            controller: 'AddAccountDialogController'
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