(function () {
    angular.module('app').directive('artDocumentViewer', DocumentViewerDirective);

    function DocumentViewerDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/directives/art-document-viewer/art-document-viewer.html',
            controller: DocumentViewerDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attr, ctrl) {
                ctrl.setFile(attr.file);
            }
        }
    }

    angular.module('app').controller('DocumentViewerDirectiveController', DocumentViewerDirectiveController);

    DocumentViewerDirectiveController.$inject = ['$rootScope', '$scope', 'api', 'viewerService', 'agentService', 'entityService', 'selectionService', 'commentService', 'markService'];

    function DocumentViewerDirectiveController($rootScope, $scope, api, viewerService, agentService, entityService, selectionService, commentService, markService) {
        var t = this;

        t.user = null;
        t.entity = null;
        t.canvas = null;
        t.viewer = null;
        t.fileUri = null;
        t.setFile = setFile;
        t.update = update;

        $(document).ready(function () {
            // Initialize the viewer and load the renderings when the document is ready.
            t.canvas = document.getElementById('canvas');

            if (t.user != null && t.entity != null) {
                initializeViewer();
            }
        });

        function setFile(fileUri) {
            t.fileUri = fileUri;

            // TODO: this only loads the most recent entity by file
            entityService.getLatestDerivationFromFile(fileUri).then(function (entity) {
                t.entity = entity;

                agentService.getUser().then(function (agent) {
                    t.user = agent;

                    if (t.canvas != null) {
                        initializeViewer();
                    }
                });
            });
        }

        function initializeViewer() {

            t.viewer = new DocumentViewer(t.user, t.canvas, "", selectionService);
            t.viewer.addCommand(new SelectCommand(t.viewer, selectionService), true);
            t.viewer.addCommand(new PanCommand(t.viewer));
            t.viewer.addCommand(new ZoomInCommand(t.viewer));
            t.viewer.addCommand(new ZoomOutCommand(t.viewer));
            t.viewer.addCommand(new CreateMarkCommand(t.viewer, markService));
            t.viewer.addCommand(new UpdateMarkCommand(t.viewer, markService));
            t.viewer.addCommand(new DeleteMarkCommand(t.viewer, markService));
            t.viewer.addCommand(new ShowMarksCommand(t.viewer, markService));
            t.viewer.addCommand(new HideMarksCommand(t.viewer, markService));
            t.viewer.addRenderer(new MarkRenderer(t.viewer, markService));

            $scope.$broadcast('viewerInitialized', t.viewer);

            viewerService.setViewer(t.viewer);

            // Handle the resize of UI panes.
            $scope.$on('resize', function () {
                t.viewer.onResize();
            });

            api.getCanvasRenderingsFromEntity(t.entity.Uri).then(function (data) {
                t.viewer.pageCache.load(data, function () {
                    console.log('Loaded pages:', data);

                    var derivation = t.entity.Uri;

                    t.viewer.setEntity(derivation);
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