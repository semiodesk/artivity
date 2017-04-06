(function () {
    angular.module('app').directive('artUserMenu', UserMenuDirective);

    function UserMenuDirective() {
        return {
            restrict: 'E',
            scope: {
                user: '='
            },
            templateUrl: 'app/directives/art-user-menu/art-user-menu.html',
            controller: UserMenuDirectiveController,
            controllerAs: 't',
            bindToController: true,
            link: function (scope, element, attr, t) {
                scope.$watch('user', t.update);
            }
        }
    }

    angular.module('app').controller('UserMenuDirectiveController', UserMenuDirectiveController);

    UserMenuDirectiveController.$inject = ['$scope', 'agentService'];

    function UserMenuDirectiveController($scope, agentService) {
        var t = this;

        t.update = function () {
            if (t.user) {
                t.name = t.user.Name;
                t.photoUrl = t.user.PhotoUrl;
            } else {
                t.name = '';
                t.photoUrl = '';
            }
        }
    }
})();