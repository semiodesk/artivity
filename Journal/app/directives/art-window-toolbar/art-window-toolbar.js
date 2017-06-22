(function () {
    angular.module('app').directive('artWindowToolbar', WindowToolbarDirective);

    function WindowToolbarDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-window-toolbar/art-window-toolbar.html',
            controller: WindowToolbarDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('WindowToolbarDirectiveController', WindowToolbarDirectiveController);

    WindowToolbarDirectiveController.$inject = ['$scope', 'windowService', 'navigationService'];

    function WindowToolbarDirectiveController($scope, windowService, navigationService) {
        var t = this;

        t.navigateBack = function() {
            navigationService.navigateBack();
        };
    }
})();