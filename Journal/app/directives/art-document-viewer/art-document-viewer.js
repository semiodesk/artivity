(function () {
    angular.module('app').directive('artDocumentViewer', DocumentViewerDirective);

    function DocumentViewerDirective() {
        return {
            restrict: 'E',
            scope: {
                'file': '='
            },
            templateUrl: 'app/directives/art-document-viewer/art-document-viewer.html',
            controller: DocumentViewerDirectiveController,
            controllerAs: 't',
            bindToController: true,
            link: function (scope, element, attr, t) {
                scope.$watch('t.file', function () {
                    if (t.file) {
                        t.load(t.file);
                    }
                });
            }
        }
    }

    angular.module('app').controller('DocumentViewerDirectiveController', DocumentViewerDirectiveController);

    DocumentViewerDirectiveController.$inject = ['$rootScope', '$scope', '$element', 'api', 'hotkeys', 'viewerService', 'agentService', 'entityService', 'selectionService', 'markService'];

    function DocumentViewerDirectiveController($rootScope, $scope, $element, api, hotkeys, viewerService, agentService, entityService, selectionService, markService) {
        var t = this;

        t.viewer = null;

        t.setViewerVisibleRegion = function () {
            if (t.viewer) {
                var sidebar = $(document).find('.ui-sidebar-right');
                var canvas = $(document).find('.viewer-canvas');

                if (sidebar.length && canvas.length) {
                    var padding = 10;

                    // TODO: This is ignoring the margin and possible offset of the sidebar.
                    var x = padding;
                    var y = padding;
                    var w = canvas.width() - sidebar.width() - 2 * padding;
                    var h = canvas.height() - 2 * padding;

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

                t.viewer = new DocumentViewer(agentService.currentUser, canvas, "", selectionService);
                t.viewer.addCommand(new SelectCommand(t.viewer, selectionService), true);
                t.viewer.addCommand(new NextPageCommand(t.viewer));
                t.viewer.addCommand(new PreviousPageCommand(t.viewer));
                t.viewer.addCommand(new ToggleTwoPageLayoutCommand(t.viewer));
                t.viewer.addCommand(new PanCommand(t.viewer));
                t.viewer.addCommand(new ZoomInCommand(t.viewer));
                t.viewer.addCommand(new ZoomOutCommand(t.viewer));
                t.viewer.addCommand(new CreateMarkCommand(t.viewer, markService));
                t.viewer.addCommand(new UpdateMarkCommand(t.viewer, markService));
                t.viewer.addCommand(new DeleteMarkCommand(t.viewer, markService));
                t.viewer.addCommand(new ShowMarksCommand(t.viewer, markService));
                t.viewer.addCommand(new HideMarksCommand(t.viewer, markService));
                t.viewer.addRenderer(new MarkRenderer(t.viewer, markService));

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
            } else {
                console.warn('Unable to find canvas for viewer element:', canvas);
            }
        }

        t.load = function (file) {
            var fileUri;

            if (file.uri) {
                fileUri = file.uri;
            } else if (file.Uri) {
                fileUri = file.Uri;
            }

            if (fileUri) {
                console.log('Loading file:', fileUri);

                entityService.getLatestRevisionFromFileUri(fileUri).then(function (data) {
                    if (data.revision) {
                        var revisionUri = data.revision;

                        console.log('Loading revision:', revisionUri);

                        if (revisionUri) {
                            api.getCanvasRenderingsFromEntity(revisionUri).then(function (data) {
                                t.viewer.pageCache.load(data, function () {
                                    console.log('Loaded pages:', data);

                                    t.viewer.setFile(fileUri);
                                    t.viewer.setRevision(revisionUri);
                                    t.viewer.zoomToFit();

                                    $rootScope.$broadcast('fileLoaded');
                                });
                            });
                        }
                    }
                });
            } else {
                console.warn("Unable to determine URI from object:", file);
            }
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

        hotkeys.add({
            combo: 'shift+f9',
            description: 'Toggle debug view',
            callback: function () {
                if (t.viewer) {
                    t.viewer.enableDebug = !t.viewer.enableDebug;
                    t.viewer.stage.update();
                }
            }
        });
    }
})();