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
        t.canvas = null;
        t.viewer = null;
        t.fileUri = null;
        t.revisionUri = null;
        t.setFile = setFile;
        t.update = update;

        $(document).ready(function () {
            // Initialize the viewer and load the renderings when the document is ready.
            t.canvas = document.getElementById('canvas');

            if (t.user && t.canvas && t.revisionUri) {
                initializeViewer();
            }
        });

        function setFile(fileUri) {
            t.fileUri = fileUri;

            entityService.getLatestRevision(fileUri).then(function (data) {
                t.revisionUri = data.revision;

                agentService.getAccountOwner().then(function (agent) {
                    t.user = agent;

                    if (t.user && t.canvas && t.revisionUri) {
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

            if (t.revisionUri) {
                api.getCanvasRenderingsFromEntity(t.revisionUri).then(function (data) {
                    t.viewer.pageCache.load(data, function () {
                        console.log('Loaded pages:', data);

                        var revision = t.revisionUri;

                        t.viewer.setEntity(revision);
                        t.viewer.zoomToFit();
                    });
                });
            }
        }

        function update() {
            if (t.viewer) {
                t.viewer.stage.update();
            }
        }
    }
})();