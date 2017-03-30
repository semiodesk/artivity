(function () {
    angular.module('app').directive('artFileCard', FileCardDirective);

    function FileCardDirective() {
        return {
            restrict: 'E',
            scope: {
                'file': '=',
                'clicked': '=?'
            },
            templateUrl: 'app/directives/art-file-card/art-file-card.html',
            controller: FileCardDirectiveController,
            controllerAs: 't',
            bindToController: true,
            link: function (scope, element, attr, t) {
                t.setFile(element);
            }
        }
    }

    angular.module('app').controller('FileCardDirectiveController', FileCardDirectiveController);

    FileCardDirectiveController.$inject = ['$scope', '$location', 'api', 'selectionService', 'filesystemService'];

    function FileCardDirectiveController($scope, $location, api, selectionService, filesystemService) {
        var t = this;

        t.thumbnailStyle = {};

        t.setFile = function (element) {
            t.fileName = filesystemService.getFileNameWithoutExtension(t.file.label);
            t.fileExtension = filesystemService.getFileExtension(t.file.label);

            if(t.file.thumbnail) {
                t.thumbnailStyle = {
                    'background-image': 'url(' + t.file.thumbnail + ')'
                }
            }
        }

        t.onClicked = function (e) {
            e.preventDefault();

            if (t.clicked) {
                t.clicked(e, t.file);
            }
        }
    }
})();