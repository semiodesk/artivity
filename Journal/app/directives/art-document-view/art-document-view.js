(function () {
    angular.module('app').directive('artDocumentView', DocumentViewDirective);

    function DocumentViewDirective() {
        return {
            restrict: 'E',
            scope: {
                'file': '='
            },
            templateUrl: 'app/directives/art-document-view/art-document-view.html',
            controller: DocumentViewDirectiveController,
            controllerAs: 't',
            bindToController: true,
            link: function (scope, element, attr, t) {
                scope.$watch('t.file', function () {
                    if (t.file) {
                        t.loadLatestRevision(t.file);
                    }
                });

                $(".ui-sidebar-right").resizable({
                    handles: "w",
                    resize: function (event, ui) {
                        // Only resize the width of the element.
                        if (ui.position.right < 0) {
                            ui.size.width = ui.originalSize.width + Math.abs(ui.position.left);
                        }

                        // Do not use the left CSS property at all because it breaks the table layout.
                        ui.position.right = 0;
                    }
                });
            }
        }
    }

    angular.module('app').controller('DocumentViewDirectiveController', DocumentViewDirectiveController);

    DocumentViewDirectiveController.$inject = ['$rootScope', '$scope', 'entityService', 'navigationService'];

    function DocumentViewDirectiveController($rootScope, $scope, entityService, navigationService) {
        var t = this;

        t.latestRevisionUri = null;

        t.loadLatestRevision = function (file) {
            if (file && file.uri) {
                entityService.getLatestRevisionFromFileUri(file.uri).then(function (data) {
                    if (data.revision) {
                        t.latestRevisionUri = data.revision;
                    }
                });
            }
        }

        t.publishLatestRevision = function (file) {
            if (file && file.uri) {
                entityService.publishLatestRevisionFromFileUri(file.uri).then(function (data) {
                    console.log(data);
                });
            }
        }

        t.navigateBack = function () {
            navigationService.navigateBack($scope);
        }
    }
})();