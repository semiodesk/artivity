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
                'emptySlot': '?empty'
            },
            scope: {
                'onload': '='
            }
        }
    }

    angular.module('app').controller('FileListDirectiveController', FileListDirectiveController);

    FileListDirectiveController.$inject = ['$scope', '$element', 'hotkeys'];

    function FileListDirectiveController($scope, $element, hotkeys) {
        var t = this;

        t.files = undefined;
        t.loading = false;
        t.query = '';

        t.loadFiles = function () {
            if (typeof (t.onload) === 'function') {
                t.loading = true;

                t.onload(function (data) {
                    t.loading = false;
                    t.files = data;

                    console.log("Loaded files:", data);
                });
            } else {
                t.loading = false;
                t.files = undefined;
            }
        }

        t.getFiles = function (query) {
            return [];
        }

        t.filterFiles = function (query) {
            if (t.files) {
                if (query) {
                    var q = query.toLowerCase();

                    for (i = 0; i < t.files.length; i++) {
                        var f = t.files[i];

                        f.visible = f.label.toLowerCase().includes(q);
                    }
                } else {
                    for (i = 0; i < t.files.length; i++) {
                        t.files[i].visible = true;
                    }
                }
            }
        }

        t.onMoreClicked = function (e) {
            $scope.$emit('showMore');
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

            $scope.$on('refresh', function () {
                t.loadFiles();
            });
        }

        t.$postLink = function () {
            t.loadFiles();
        }
    }
})();