(function () {
    angular.module('app').directive('artWindowToolbar', WindowToolbarDirective);

    function WindowToolbarDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-window-toolbar/art-window-toolbar.html',
            controller: WindowToolbarDirectiveController
        }
    }

    angular.module('app').controller('WindowToolbarDirectiveController', WindowToolbarDirectiveController);

    WindowToolbarDirectiveController.$inject = ['$scope', 'windowService', 'navigationService'];

    function WindowToolbarDirectiveController($scope, windowService, navigationService) {
        $scope.canNavigateBack = function() {
            return navigationService.canNavigateBack();
        }
        
        $scope.navigateBack = function() {
            navigationService.navigateBack();
        };
    }
})();