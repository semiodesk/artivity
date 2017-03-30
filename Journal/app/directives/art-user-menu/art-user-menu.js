(function () {
    angular.module('app').directive('artUserMenu', UserMenuDirective);

    function UserMenuDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/directives/art-user-menu/art-user-menu.html',
            controller: UserMenuDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('UserMenuDirectiveController', UserMenuDirectiveController);

    UserMenuDirectiveController.$inject = ['$scope', 'agentService'];

    function UserMenuDirectiveController($scope, agentService) {
        var t = this;

        agentService.initialized.then(function () {
            $scope.$apply(function () {
                t.name = agentService.currentUser.Name;
                t.photoUrl = agentService.currentUser.PhotoUrl;
            });
        });
    }
})();