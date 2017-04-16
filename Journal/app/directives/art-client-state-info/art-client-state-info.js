(function () {
    angular.module('app').directive('artClientStateInfo', ClientStateInfoDirective);

    function ClientStateInfoDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-client-state-info/art-client-state-info.html',
            controller: ClientStateInfoDirectiveController,
            controllerAs: 't',
            bindToController: true,
            scope: {
                'clientState': '=state'
            }
        }
    }

    angular.module('app').controller('ClientStateInfoDirectiveController', ClientStateInfoDirectiveController);

    ClientStateInfoDirectiveController.$inject = ['$scope'];

    function ClientStateInfoDirectiveController($scope) {
        var t = this;
    }
})();