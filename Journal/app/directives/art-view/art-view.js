(function () {
    angular.module('app').directive('artView', ViewDirective);

    function ViewDirective() {
        return {
            restrict: 'E',
            template: '<ng-transclude></ng-transclude>',
            transclude: true,
            controller: ViewDirectiveController,
            controllerAs: 't',
            bindToController: true,
            scope: {
                title: '@'
            }
        }
    }

    angular.module('app').controller('ViewDirectiveController', ViewDirectiveController);

    ViewDirectiveController.$inject = ['$scope', 'windowService'];

    function ViewDirectiveController($scope, windowService) {
        var t = this;

        t.$postLink = function () {
            if (!t.title) {
                windowService.setTitle('');
            }

            $scope.$watch('t.title', function () {
                if (t.title && t.title.length) {
                    windowService.setTitle(t.title);
                }
            });
        }
    }
})();