(function () {
    angular.module('app').directive('artFilePreview', artFilePreviewDirective);

    function artFilePreviewDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/directives/art-file-preview/art-file-preview.html',
            controller: FilePreviewDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attr, ctrl) {
                ctrl.setFile(attr.file);
            }
        }
    }

    angular.module('app').controller('FilePreviewDirectiveController', FilePreviewDirectiveController);

    function FilePreviewDirectiveController(api, $scope, entityService, derivationService, commentService) {
        var t = this;

        t.entity = null;
        t.setFile = setFile;

        function setFile(entityUri) {
            entityService.getById(entityUri).then(function (response) {
                t.entity = response;

                if (t.entity.Revisions.length > 0) {
                    // Initialize the viewer and load the renderings when the document is ready.
                    $(document).ready(function () {
                        t.canvas = document.getElementById('canvas');

                        if (t.canvas) {
                            t.viewer = new DocumentViewer(t.canvas, api.getRenderingUrl(entityUri));

                            api.getCanvasRenderingsFromEntity(t.entity.Uri).then(function (data) {
                                t.viewer.pageCache.load(data, function () {
                                    console.log('Loaded pages:', data);

                                    t.viewer.render(t.entity.Revisions[0]);
                                    t.viewer.zoomToFit();
                                });
                            });
                        }
                    });
                }
            });
        }
    }
})();