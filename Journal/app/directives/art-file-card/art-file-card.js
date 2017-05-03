(function () {
    angular.module('app').directive('artFileCard', FileCardDirective);

    function FileCardDirective() {
        return {
            restrict: 'E',
            scope: {
                'file': '='
            },
            templateUrl: 'app/directives/art-file-card/art-file-card.html',
            controller: FileCardDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('FileCardDirectiveController', FileCardDirectiveController);

    FileCardDirectiveController.$inject = ['$scope', '$element', '$location', 'selectionService', 'filesystemService'];

    function FileCardDirectiveController($scope, $element, $location, selectionService, filesystemService) {
        var t = this;

        t.thumbnailStyle = {};

        t.onLeftClick = function (e) {
            e.preventDefault();

            $scope.$emit('leftClick', { 
                sourceEvent: e,
                sourceScope: t.file
            });
        }

        t.onRightClick = function (e) {
            e.preventDefault();

            $scope.$emit('rightClick', { 
                sourceEvent: e,
                sourceScope: t.file
            });
        }

        t.$postLink = function () {
            $scope.$watch('t.file', function () {
                if (t.file) {
                    var label = t.file.label;

                    t.fileName = filesystemService.getFileNameWithoutExtension(label);
                    t.fileExtension = filesystemService.getFileExtension(label);

                    if (t.file.thumbnail) {
                        t.thumbnailStyle = {
                            'background-image': 'url(' + t.file.thumbnail + ')'
                        }
                    }
                }
            });

            $element.click(t.onLeftClick);
            $element.contextmenu(t.onRightClick);
        }
    }
})();