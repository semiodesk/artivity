(function () {
    angular.module('app').directive('artLayerControl', function () {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-layer-control/art-layer-control.html',
            controller: LayerControlDirectiveController,
            controllerAs: 't',
            scope: {
                layers: "=layers"
            }
        }
    });

    LayerControlDirectiveController.$inject = ['$scope', '$timeout', 'viewerService'];

    function LayerControlDirectiveController($scope, $timeout, viewerService) {
        var t = this;

        t.toggleLayerVisibility = function (layer) {
            console.log("Toggle visibility: ", layer);

            layer.visible = !layer.visible;

            $scope.$emit('redraw');
        }

        t.getLayersFromViewer = function (viewer, influence) {
            if (viewer && viewer.layerCache && influence) {
                var time = new Date(influence.time);
                var layers = [];

                viewer.layerCache.getAll(time, function (layer, depth) {
                    layer.label = layer.getValue(time, 'http://www.w3.org/2000/01/rdf-schema#label');

                    layers.push(layer);
                });

                return layers;
            } else {
                return [];
            }
        }

        t.$onInit = function () {
            t.viewer = viewerService.viewer();

            viewerService.on('viewerChanged', function (e) {
                t.viewer = e.newViewer;

                t.layers = t.getLayersFromViewer(t.viewer, t.influence);
            });

            $scope.$on('layersLoaded', function (e, args) {
                t.layers = t.getLayersFromViewer(t.viewer, t.influence);
            });

            $scope.$on('influenceSelected', function (e, args) {
                t.influence = args.data;

                t.layers = t.getLayersFromViewer(t.viewer, t.influence);
            });
        }
    };
})();