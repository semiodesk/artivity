angular.module('app').controller("ProfileSettingsViewController", ProfileSettingsViewController);

function ProfileSettingsViewController($scope, $stateParams) {
    var t = this;

    t.index = $stateParams.index;
    t.timestamp = new Date();
}