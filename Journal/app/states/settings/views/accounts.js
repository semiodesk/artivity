angular.module('app').controller("AccountsSettingsViewController", AccountsSettingsViewController);

function AccountsSettingsViewController($scope, $stateParams) {
    var t = this;

    t.index = $stateParams.index;
    t.timestamp = new Date()
}