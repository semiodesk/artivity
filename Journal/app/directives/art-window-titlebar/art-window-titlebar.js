(function () {
    angular.module('app').directive('artWindowTitlebar', WindowTitlebarDirective);

    function WindowTitlebarDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-window-titlebar/art-window-titlebar.html',
            controller: WindowTitlebarDirectiveController
        }
    }

    angular.module('app').controller('WindowTitlebarDirectiveController', WindowTitlebarDirectiveController);

    WindowTitlebarDirectiveController.$inject = ['$scope', 'windowService', 'navigationService'];

    function WindowTitlebarDirectiveController($scope, windowService, navigationService) {
        $scope.canNavigateBack = function() {
            return navigationService.canNavigateBack();
        }
        
        $scope.navigateBack = function() {
            navigationService.navigateBack();
        };
    }
})();