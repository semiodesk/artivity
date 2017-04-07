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

        t.setUser = function (user) {
            if (user) {
                t.name = user.Name;
                t.photoUrl = user.PhotoUrl;
            } else {
                t.name = '';
                t.photoUrl = '';
            }
        }

        t.$onInit = function () {
            if (agentService.currentUser) {
                t.setUser(agentService.currentUser);
            }

            agentService.on('currentUserChanged', t.setUser);
        }

        t.$onDestroy = function () {
            agentService.off('currentUserChanged', t.setUser);
        }
    }
})();