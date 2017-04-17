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

        t.viewer = null;
        t.commands = {};
        t.canExecuteCommand = canExecuteCommand;
        t.executeCommand = executeCommand;

        function updateButtonStates(e) {
            if (t.viewer) {
                var selected = t.viewer.selectedCommand;

                $element.find('.btn[data-command]').each(function (i, btn) {
                    var b = $(btn);

                    if (b.data('command') === selected.id) {
                        b.addClass('btn-active');
                    } else {
                        b.removeClass('btn-active');
                    }
                });
            }
        }

        function canExecuteCommand(e) {
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

        function executeCommand(e) {
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
                }
            }
        }

        t.$postLink = function () {
            $element.find('.btn[data-command]').each(function (i, btn) {
                $(btn).click(t.executeCommand);
            });

            viewerService.on('viewerChanged', function(e) {
                if(e.oldViewer) {
                    e.oldViewer.off('commandSelected', updateButtonStates);
                }

                if(e.newViewer) {
                    e.newViewer.on('commandSelected', updateButtonStates);
                }

                t.viewer = e.newViewer;

                updateButtonStates();
            });

            t.viewer = viewerService.viewer();

            if(t.viewer) {
                updateButtonStates();

                t.viewer.on('commandSelected', updateButtonStates);
            }
        }
    }
})();