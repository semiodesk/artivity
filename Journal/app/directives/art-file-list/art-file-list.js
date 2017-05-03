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
        t.query = '';

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

        t.onMoreClicked = function (e) {
            $scope.$emit('showMore');
        }

        t.onDragStart = function () {
            $scope.$emit('dragStarted');
        }

        t.onDragStop = function () {
            $scope.$emit('dragStopped');
        }

        t.viewFile = function (e) {
            if (e) {
                e.preventDefault();
            }

            var context = $element.find('.dropdown-menu').data('context');

            if (context) {
                $scope.$emit('viewFile', context);
            }
        }

        t.viewFileHistory = function (e) {
            if (e) {
                e.preventDefault();
            }

            var context = $element.find('.dropdown-menu').data('context');

            if (context) {
                $scope.$emit('viewFileHistory', context);
            }
        }

        t.editFile = function (e) {
            if (e) {
                e.preventDefault();
            }
            
            var context = $element.find('.dropdown-menu').data('context');

            if (context) {
                $scope.$emit('editFile', context);
            }
        }

        t.showContextMenu = function (e, data) {
            // Only show the context menu for file cards which have their own scope.
            var menu = $element.find('.dropdown-menu');

            // Substract the menu container offset from the event position.
            var p = $element.find('.dropdown').offset();

            // Show the menu at the relative mouse cursor position.
            menu.css({
                display: "block",
                left: e.clientX - p.left,
                top: e.clientY - p.top
            });

            // Remember the original event target when handling clicks in the menu.
            menu.data('context', data);
        }

        t.hideContextMenu = function (e) {
            var menu = $element.find('.dropdown-menu');

            menu.css({
                display: "none"
            });

            menu.data('context', null);
        }

        t.$onInit = function () {
            hotkeys.add({
                combo: 'f5',
                description: 'Reload the current view.',
                callback: function () {
                    t.loadFiles();
                }
            });

            $scope.$watch('t.query', function () {
                if (t.files) {
                    if (t.query) {
                        var q = t.query.toLowerCase();

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
            });

            $scope.$on('refresh', function () {
                t.loadFiles();
            });

            $element.click(function (e) {
                // Hide the context menu when clicking into the control area.
                t.hideContextMenu(e);
            });

            $scope.$on('leftClick', function (e, args) {
                e.preventDefault();

                $scope.$emit('viewFile', args.sourceScope);
            });

            $scope.$on('rightClick', function (e, args) {
                e.preventDefault();

                // Show the context menu when clicking onto a list item.
                t.showContextMenu(args.sourceEvent, args.sourceScope);
            });
        }

        t.$postLink = function() {
            t.loadFiles();
        }
    }
})();