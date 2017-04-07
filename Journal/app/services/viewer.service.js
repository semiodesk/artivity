(function () {
    angular.module('app').factory('viewerService', viewerService);

    viewerService.$inject = ['markService'];

    function viewerService(markService) {
        var currentViewer = null;

        return {
            viewer: viewer,
            hasCommand: hasCommand,
            executeCommand: executeCommand,
            createMark: createMark,
            showMarks: showMarks,
            hideMarks: hideMarks
        };

        // CONTEXT
        function viewer(viewer) {
            if (viewer) {
                currentViewer = viewer;
            }

            return currentViewer;
        }

        // COMMANDS
        function hasCommand(command) {
            if (currentViewer) {
                var c = currentViewer.getCommand(command);

                if (c) {
                    return true;
                }
            }

            return false;
        }

        function executeCommand(command, param) {
            return new Promise(function (resolve, reject) {
                if (currentViewer) {
                    var result = currentViewer.executeCommand(command, param);

                    resolve(result);
                } else {
                    reject();
                }
            });
        }

        // MARKS
        var marks = [];

        function createMark(entity) {
            if (entity && entity.uri) {
                executeCommand('createMark', entity.uri);
            }
        }

        function showMarks(entity) {
            if (entity && entity.uri) {
                markService.getMarksForEntity(entity.uri).then(function (data) {
                    if (data && data.length > 0) {
                        marks = data;
                        
                        executeCommand('showMarks', marks);
                    } else {
                        marks = [];
                    }
                });
            }
        }

        function hideMarks(entity) {
            if (entity && entity.uri && marks && marks.length > 0) {
                executeCommand('hideMarks', marks);

                marks = [];
            }
        }
    }
})();