(function () {
    'use strict';

    angular.module('explorerApp').controller('SettingsController', SettingsController);

    function SettingsController(api, $scope, $rootScope, $location, $routeParams) {
        var t = this;
        var s = $scope;

        s.children = [];

        s.submit = function () {
            s.children.forEach(function (child) {
                if (child.submit) {
                    child.submit();
                }
            });
        };

        s.$watch('agent.iconUrl', function () {
            if (s.agent && s.agent.iconUrl !== "") {
                timeline.setUserPhotoUrl(scope.user.photoUrl);
            }
        });

        s.submitAndReturn = function () {
            s.submit();

            // Navigate to dasboard and refresh the page.
            $location.path('/');
        };

        s.reset = function () {
            s.children.forEach(function (child) {
                if (child.reset) {
                    child.reset();
                }
            });
        };
    };
})();