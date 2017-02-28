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

    DocumentViewerToolbarDirectiveController.$inject = ['$scope', 'api', 'selectionService'];

    function DocumentViewerToolbarDirectiveController($scope, api, selectionService) {
        var t = this;

        t.viewer = null;
        t.commands = {};

        t.canExecuteCommand = canExecuteCommand;
        t.executeCommand = executeCommand;

        initialize();

        function initialize() {
            $scope.$on('viewerInitialized', function (e, viewer) {
                t.viewer = viewer;

                $('.btn[data-command]').each(function (i, btn) {
                    $(btn).click(t.executeCommand);
                });

                t.viewer.on('commandSelected', function (e) {
                    updateButtonStates(e);
                });

                updateButtonStates(e);
            });
        }

        function updateButtonStates(e) {
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

        function canExecuteCommand(e) {
            if (t.viewer) {
                var command = $(e.currentTarget).data('command');
                var param = undefined;

                if (command) {
                    t.viewer.canExecuteCommand(command, param);
                }
            }
        }

        function executeCommand(e) {
            if (t.viewer) {
                var command = $(e.currentTarget).data('command');
                var param = undefined;

                if (command) {
                    t.viewer.executeCommand(command, param);
                }
            }
        }
    }
})();