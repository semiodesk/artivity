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
                'clicked': '=?',
                'moreClicked': '=?'
            }
        }
    }

    angular.module('app').controller('FileListDirectiveController', FileListDirectiveController);

    FileListDirectiveController.$inject = ['$scope', '$element', 'hotkeys'];

    function FileListDirectiveController($scope, $element, hotkeys) {
        var t = this;

        t.loading = false;
        t.files = undefined;

        t.loadFiles = function () {
            if (typeof (t.onload) === 'function') {
                t.loading = true;

                t.onload(function (data) {
                    t.loading = false;
                    t.files = data;
                });
            } else {
                t.loading = false;
                t.files = undefined;
            }
        }

        t.onClick = function (e, data) {
            if (t.clicked) {
                t.clicked(e, data);
            }
        }

        t.onMoreClicked = function (e) {
            if (t.moreClicked && typeof (t.moreClicked) === 'function') {
                t.moreClicked();
            }
        }

        t.onDragStart = function () {
            $scope.$emit('dragStarted');
        }

        t.onDragStop = function () {
            $scope.$emit('dragStopped');
        }

        t.$onInit = function () {
            hotkeys.add({
                combo: 'f5',
                description: 'Reload the current view.',
                callback: function () {
                    t.loadFiles();
                }
            });

            t.loadFiles();

            $scope.$on('refresh', function () {
                t.loadFiles();
            });
        }
    }
})();