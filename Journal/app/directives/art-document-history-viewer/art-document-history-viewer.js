(function () {
    angular.module('app').directive('artDocumentHistoryViewer', DocumentHistoryViewerDirective);

    function DocumentHistoryViewerDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/directives/art-document-history-viewer/art-document-history-viewer.html',
            controller: DocumentHistoryViewerDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('DocumentHistoryViewerDirectiveController', DocumentHistoryViewerDirectiveController);

    DocumentHistoryViewerDirectiveController.$inject = ['$rootScope', '$scope', '$element', 'api', 'hotkeys', 'viewerService', 'agentService'];

    function DocumentHistoryViewerDirectiveController($rootScope, $scope, $element, api, hotkeys, viewerService, agentService) {
        var t = this;

        t.viewer = null;

        t.setViewerVisibleRegion = function () {
            if (t.viewer) {
                var sidebar = $(document).find('.ui-sidebar-right');
                var canvas = $(document).find('.viewer-canvas');

                if (sidebar.length && canvas.length) {
                    // TODO: This is ignoring the margin and possible offset of the sidebar.
                    var x = 0;
                    var y = 0;
                    var w = canvas.width() - sidebar.width();
                    var h = canvas.height();

                    if (w > 0 && h > 0) {
                        t.viewer.setViewport(x, y, w, h);
                    }
                }
            }
        }

        t.$postLink = function () {
            var canvas = $element.find('canvas')[0];

            if (canvas) {
                // EaselJS addresses the canvas by its id.
                canvas.id = 'canvas-' + $scope.$id;

                t.viewer = new DocumentHistoryViewer(agentService.currentUser, canvas);
                t.viewer.addCommand(new PanCommand(t.viewer));

                t.setViewerVisibleRegion();

                viewerService.viewer(t.viewer);

                $element.on('appear', function (event) {
                    viewerService.viewer(t.viewer);
                });

                // Handle the resize of UI panes.
                $scope.$on('resize', function () {
                    t.setViewerVisibleRegion();

                    t.viewer.onResize();
                });

                $scope.$on('fileLoaded', function (e, data) {
                    t.onFileLoaded(data.file, data.influences);
                });

                $scope.$on('influenceSelected', function (e, args) {
                    var influence = args.data;

                    if (influence && args.sourceScope !== t) {
                        t.viewer.render(influence);
                    }
                });
            } else {
                console.warn('Unable to find canvas for viewer element:', canvas);
            }
        }

        t.onFileLoaded = function (file, influences) {
            var fileUri = file.uri;

            console.log('Loading file:', fileUri);

            t.viewer.influences = influences;

            // Canvases in the file.
            api.getCanvases(fileUri).then(function (data) {
                t.viewer.canvasCache.load(data, function () {
                    console.log("Loaded canvases: ", t.viewer.canvasCache);

                    api.getLayers(fileUri).then(function (data) {
                        t.viewer.layerCache.load(data, function (layers) {
                            console.log("Loaded layers: ", t.viewer.layerCache);

                            $rootScope.$broadcast('layersLoaded', {
                                sourceScope: $scope,
                                data: layers
                            });

                            values(layers, function (uri, layer) {
                                // TODO: The layer state should be recorded and returned by the API.
                                layer.visible = true;

                                console.log(layer);
                            });

                            t.viewer.renderCache.endpointUrl = api.getRenderingUrl(fileUri);

                            // Trigger loading the bitmaps.
                            api.getRenderings(fileUri).then(function (data) {
                                t.viewer.renderCache.load(data, function () {
                                    console.log("Loaded renderings: ", t.viewer.renderCache);

                                    t.viewer.setFile(fileUri);
                                    t.viewer.render(influences[0]);
                                    t.viewer.zoomToFit();
                                });
                            });
                        });
                    });
                });
            });
        }

        hotkeys.add({
            combo: 'f5',
            description: 'Reload the document view.',
            callback: function () {
                if (t.viewer) {
                    t.viewer.stage.update();
                }
            }
        });
    }
})();