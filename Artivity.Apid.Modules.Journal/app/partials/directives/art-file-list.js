(function () {
    angular.module('explorerApp').directive('artFileList', FileListDirective);

    function FileListDirective() {
        return {
            scope: {},
            templateUrl: 'partials/directives/art-file-list.html',
            controller: FileListDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attr, ctrl) {
                ctrl.setFiles();
            }
        }
    }

    angular.module('explorerApp').controller('FileListDirectiveController', FileListDirectiveController);

    function FileListDirectiveController(api, $scope, artFileService) {
        var t = this;

        initialize();

        function initialize()
        {
            artFileService.loadRecentFiles();
        }

        t.setFiles = function () {
            t.files = artFileService.files; 
        }
    }
})();