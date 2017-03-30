(function () {
    angular.module('app').directive('artFileList', FileListDirective);

    function FileListDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/directives/art-file-list/art-file-list.html',
            controller: FileListDirectiveController,
            controllerAs: 't',
            bindToController: true,
            transclude: {
                'emptySlot': 'empty'
            },
            scope: {
                'onload': '=',
                'clicked': '=?'
            },
            link: function (scope, element, attr, t) {
                $(element).on('appear', function (event) {
                    t.loadFiles();
                });
            }
        }
    }

    angular.module('app').controller('FileListDirectiveController', FileListDirectiveController);

    FileListDirectiveController.$inject = ['$rootScope', '$scope', 'hotkeys'];

    function FileListDirectiveController($rootScope, $scope, hotkeys) {
        var t = this;

        t.files = [];
        t.loading = false;

        t.loadFiles = function () {
            if (typeof (t.onload) === 'function') {
                t.loading = true;

                t.onload(function (data) {
                    t.loading = false;
                    t.files = data;
                });
            }
        }

        t.onClick = function (e, data) {
            if (t.clicked) {
                t.clicked(e, data);
            }
        }

        t.onDragStart = function () {
            $rootScope.$broadcast('dragStarted');
        }

        t.onDragStop = function () {
            $rootScope.$broadcast('dragStopped');
        }

        hotkeys.add({
            combo: 'f5',
            description: 'Reload the current view.',
            callback: function () {
                t.loadFiles();
            }
        });
    }
})();