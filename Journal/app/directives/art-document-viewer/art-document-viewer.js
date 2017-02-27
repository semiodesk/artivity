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

    DocumentViewerDirectiveController.$inject = ['$scope', 'api', 'agentService', 'entityService', 'selectionService', 'commentService', 'markService'];

    function DocumentViewerDirectiveController($scope, api, agentService, entityService, selectionService, commentService, markService) {
        var t = this;

        t.user = null;
        t.entity = null;
        t.canvas = null;
        t.viewer = null;
        t.setFile = setFile;

        $(document).ready(function () {
            // Initialize the viewer and load the renderings when the document is ready.
            t.canvas = document.getElementById('canvas');

            if (t.user != null && t.entity != null) {
                initializeViewer();
            }
        });

        function setFile(entityUri) {
            entityService.getById(entityUri).then(function (entity) {
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
            var url = api.getRenderingUrl(t.entity.Uri);

            t.viewer = new DocumentViewer(t.user, t.canvas, url, selectionService);
            t.viewer.addCommand(new PanCommand(t.viewer));
            t.viewer.addCommand(new SelectCommand(t.viewer, selectionService));
            t.viewer.addCommand(new CreateMarkCommand(t.viewer, markService));
            t.viewer.addCommand(new UpdateMarkCommand(t.viewer, markService));
            t.viewer.addCommand(new DeleteMarkCommand(t.viewer, markService));
            t.viewer.addRenderer(new MarkRenderer(t.viewer, markService));
            
            $scope.$broadcast('viewerInitialized', t.viewer);

            api.getCanvasRenderingsFromEntity(t.entity.Uri).then(function (data) {
                t.viewer.pageCache.load(data, function () {
                    console.log('Loaded pages:', data);

                    var derivation = t.entity.RevisionUris[0];

                    t.viewer.setEntity(derivation);
                    t.viewer.zoomToFit();
                });
            });
        }
    }
})();