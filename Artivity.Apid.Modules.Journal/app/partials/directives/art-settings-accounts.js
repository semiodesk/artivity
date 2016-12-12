var app = angular.module('explorerApp');

app.directive('artAccountsSettings', AccountsSettingsDirective);

function AccountsSettingsDirective() {
    return {
        scope: true,
        templateUrl: 'partials/directives/art-settings-accounts.html',
    }
};

app.controller('AccountsSettingsDirectiveFormController', AccountsSettingsDirectiveFormController);

function AccountsSettingsDirectiveFormController (api, $scope, $log, $uibModal) {
    var t = this;
    var s = $scope;

    // Register the controller with its parent for global apply/cancel.
    s.$parent.children.push(this);

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
            console.log("Reloading accounts..");

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