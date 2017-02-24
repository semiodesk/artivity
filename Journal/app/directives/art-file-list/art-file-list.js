(function () {
    angular.module('app').directive('artFileList', FileListDirective);

    function FileListDirective() {
        return {
            scope: {},
            templateUrl: 'app/directives/art-file-list/art-file-list.html',
            controller: FileListDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attr, ctrl) {
                ctrl.setFiles();
            }
        }
    }

    angular.module('app').controller('FileListDirectiveController', FileListDirectiveController);

    FileListDirectiveController.$inject = ['$rootScope', '$scope', 'api', 'fileService'];

    function FileListDirectiveController($rootScope, $scope, api, fileService) {
        var t = this;

        initialize();

        function initialize() {
            fileService.loadRecentFiles();
            fileService.on('dataChanged', function () {
                t.setFiles();
            });
        }

        t.setFiles = function () {
            t.files = fileService.files;
        }

        t.onDragStart = function () {
            $rootScope.$broadcast('dragStarted');
        }

        t.onDragStop = function () {
            $rootScope.$broadcast('dragStopped');
        }
    }
})();