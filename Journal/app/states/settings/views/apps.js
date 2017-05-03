angular.module('app').controller("AppsSettingsViewController", AppsSettingsViewController);

function AppsSettingsViewController($scope, $stateParams) {
    var t = this;

    t.index = $stateParams.index;
    t.timestamp = new Date()
}