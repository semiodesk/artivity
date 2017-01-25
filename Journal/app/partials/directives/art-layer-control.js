(function () {
    angular.module('explorerApp').directive('artLayerControl', function () {
        return {
            restrict: 'E',
            templateUrl: 'partials/directives/art-layer-control.html',
            controller: LayerControlDirectiveController,
            controllerAs: 't',
            scope: {
                layers: "=layers"
            }
        }
    });

    function LayerControlDirectiveController($rootScope, $scope, selectionService) {
        var t = this;

        selectionService.on('selectedItemChanged', function(influence) {
            if (!$rootScope.$$phase) {
                $rootScope.$digest();
            }
        });

        t.toggleVisibility = function (layer) {
            console.log("Toggle visibility: ", layer);

            layer.visible = !layer.visible;

            $rootScope.$broadcast('redraw');
        }
    };
})();