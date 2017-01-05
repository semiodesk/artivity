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

function LayerControlDirectiveController($scope, selectionService) {
	var t = this;
    
    selectionService.on('selectionChanged', function(influence) {
        $scope.$apply();
    });

    t.toggleVisibility = function(layer) {
        console.log("Toggle visibility: ", layer);
        
        layer.visible = !layer.visible;

        selectionService.selectedItem(influence);
    }
};