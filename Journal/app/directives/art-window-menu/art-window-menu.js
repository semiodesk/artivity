(function () {
    angular.module('app').directive('artWindowMenu', WindowMenuDirective);

    function WindowMenuDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-window-menu/art-window-menu.html',
            controller: WindowMenuDirectiveController,
            controllerAs: 't'
        }
    }

    angular.module('app').controller('WindowMenuDirectiveController', WindowMenuDirectiveController);

    WindowMenuDirectiveController.$inject = ['$scope', '$mdMenu', 'agentService'];

    function WindowMenuDirectiveController($scope, $mdMenu, agentService) {
        var t = this;

        t.$onInit = function () {
            agentService.getCurrentUser().then(function (user) {
                if (user.Name) {
                    $scope.userName = user.Name.split(' ')[0];
                } else {
                    // TODO: Set a translated string here.
                }
            });
        }
    }
})();