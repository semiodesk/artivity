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

                t.viewer.on('startExecute', function (command, param) {
                    console.log(command);
                });

                t.viewer.on('stopExecute', function (command, param) {
                    console.log(command);
                });

                $('.btn[data-command]').each(function (i, element) {
                    $(element).click(t.executeCommand);
                });
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