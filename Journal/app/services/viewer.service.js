(function () {
    angular.module('app').factory('viewerService', viewerService);

    viewerService.$inject = [];

    function viewerService() {
        var t = {
            viewer: null
        };

        return {
            setViewer: setViewer,
            executeCommand: executeCommand
        };

        function setViewer(viewer) {
            t.viewer = viewer;
        }

        function executeCommand(command, param) {
            return new Promise(function (resolve, reject) {
                if (t.viewer) {
                    var result = t.viewer.executeCommand(command, param);

                    resolve(result);
                } else {
                    reject();
                }
            });
        }
    }
})();