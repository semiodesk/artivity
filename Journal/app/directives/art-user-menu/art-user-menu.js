(function () {
    angular.module('app').directive('artUserMenu', UserMenuDirective);

    function UserMenuDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/directives/art-user-menu/art-user-menu.html',
            controller: UserMenuDirectiveController,
            controllerAs: 't'
        }
    }

    angular.module('app').controller('UserMenuDirectiveController', UserMenuDirectiveController);

    UserMenuDirectiveController.$inject = ['$scope', 'agentService'];

    function UserMenuDirectiveController($scope, agentService) {
        var t = this;

        agentService.on('currentUserChanged', function () {
            var user = agentService.currentUser;

            if (user) {
                t.name = user.Name;
                t.photoUrl = user.PhotoUrl;
            } else {
                t.name = '';
                t.photoUrl = '';
            }
        });
    }
})();