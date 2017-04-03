(function () {
    angular.module('app').directive('artDocumentHistoryViewer', DocumentHistoryViewerDirective);

    function DocumentHistoryViewerDirective() {
        return {
            restrict: 'E',
            scope: {
                'file': '=file'
            },
            templateUrl: 'app/directives/art-document-history-viewer/art-document-history-viewer.html',
            controller: DocumentHistoryViewerDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attr, t) {
                t.element = element;
            }
        }
    }

    angular.module('app').controller('DocumentHistoryViewerDirectiveController', DocumentHistoryViewerDirectiveController);

    DocumentHistoryViewerDirectiveController.$inject = ['$rootScope', '$scope', 'api', 'viewerService', 'agentService', 'entityService'];

    function DocumentHistoryViewerDirectiveController($rootScope, $scope, api, viewerService, agentService, entityService) {
        var t = this;

        t.viewer = null;
        t.update = update;
        t.initialize = initialize;

        function initialize() {
            if (t.file) {
                var fileUri = t.file;

                console.log('Loading file:', fileUri);

                entityService.getLatestRevisionFromFileUri(fileUri).then(function (data) {
                    if (data.revision) {
                        var revisionUri = data.revision;

                        console.log('Loading revision:', revisionUri);

                        var canvas = $(t.element).find('canvas');

                        if (canvas) {
                            initializeViewer(canvas, revisionUri);
                        } else {
                            console.warn('Unable to find canvas for viewer element:', canvas);
                        }
                    }
                });
            } else {
                console.warn('Invalid file:', t.file);
            }
        }

        function initializeViewer(canvas, revisionUri) {
            t.viewer = new DocumentHistoryViewer(agentService.currentUser, canvas, api.getRenderingUrl(revisionUri));
            t.viewer.addCommand(new PanCommand(t.viewer));

            $scope.$broadcast('viewerInitialized', t.viewer);

            viewerService.setViewer(t.viewer);

            // Handle the resize of UI panes.
            $scope.$on('resize', function () {
                t.viewer.onResize();
            });

            api.getCanvasRenderingsFromEntity(revisionUri).then(function (data) {
                t.viewer.pageCache.load(data, function () {
                    console.log('Loaded pages:', data);

                    t.viewer.setEntity(revisionUri);
                    t.viewer.zoomToFit();
                });
            });
        }

        function update() {
            if (t.viewer) {
                t.viewer.stage.update();
            }
        }
    }
})();