(function () {
    angular.module('app').directive('artDocumentViewerToolbar', artDocumentViewerToolbar);

    function artDocumentViewerToolbar() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-document-viewer-toolbar/art-document-viewer-toolbar.html',
            scope: {},
            controller: DocumentViewerToolbarDirectiveController,
            controllerAs: 't'
        }
    }

    angular.module('app').controller('DocumentViewerToolbarDirectiveController', DocumentViewerToolbarDirectiveController);

    DocumentViewerToolbarDirectiveController.$inject = ['$scope', '$element', '$parse', 'viewerService', 'selectionService'];

    function DocumentViewerToolbarDirectiveController($scope, $element, $parse, viewerService, selectionService) {
        var t = this;

        t.layout = 'single';
        t.viewer = null;
        t.commands = {};

        t.canExecuteCommand = function (e) {
            if (t.viewer) {
                var command = $(e.currentTarget).data('command');

                if (command) {
                    var exp = $(e.currentTarget).data('param');

                    if (exp) {
                        var param = $parse(exp)($scope);

                        t.viewer.canExecuteCommand(command, param);
                    } else {
                        t.viewer.canExecuteCommand(command);
                    }
                }
            }
        }

        t.executeCommand = function (e) {
            if (t.viewer) {
                var command = $(e.currentTarget).data('command');

                if (command) {
                    var exp = $(e.currentTarget).data('param');

                    if (exp) {
                        var param = $parse(exp)($scope);

                        t.viewer.executeCommand(command, param);
                    } else {
                        t.viewer.executeCommand(command);
                    }

                    $scope.$apply(function () {
                        updatePageInfo();
                    });
                }
            }
        }

        t.onPageLayoutChanged = function () {
            t.viewer.setPageLayout(t.layout);
        }

        function updateButtonStates(e) {
            if (t.viewer) {
                var selected = t.viewer.selectedCommand;

                $('.btn[data-command]').each(function (i, btn) {
                    var b = $(btn);

                    if (b.data('command') === selected.id) {
                        b.addClass('btn-active');
                    } else {
                        b.removeClass('btn-active');
                    }
                });
            }
        }

        function updatePageInfo() {
            t.currentPage = t.viewer.pageIndex + 1;
            t.pageCount = t.viewer.pages ? t.viewer.pages.length : 1;
        }

        t.$postLink = function () {
            $element.find('[data-command]').each(function (i, btn) {
                $(btn).click(t.executeCommand);
            });

            viewerService.on('viewerChanged', function (e) {
                if (e.oldViewer) {
                    e.oldViewer.off('commandSelected', updateButtonStates);
                }

                if (e.newViewer) {
                    e.newViewer.on('commandSelected', updateButtonStates);
                }

                t.viewer = e.newViewer;

                updatePageInfo();
                updateButtonStates();
            });

            t.viewer = viewerService.viewer();

            if (t.viewer) {
                updatePageInfo();
                updateButtonStates();

                t.viewer.on('commandSelected', updateButtonStates);
            }

            $scope.$on('fileLoaded', function (e) {
                $scope.$apply(function () {
                    updatePageInfo();
                    updateButtonStates();
                });
            });
        }
    }
})();