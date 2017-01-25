(function () {
    angular.module('explorerApp').directive('artFileCard', FileCardDirective);

    function FileCardDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'partials/directives/art-file-card.html',
            controller: FileCardDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attr, ctrl) {
                ctrl.setFile(element, JSON.parse(attr.file), attr.enableLink);
            }
        }
    }

    angular.module('explorerApp').controller('FileCardDirectiveController', FileCardDirectiveController);

    function FileCardDirectiveController(api, $scope, fileService) {
        var t = this;

        t.setFile = function (element, file, enableLink) {
            t.file = file;
            t.fileName = fileService.getFileNameWithoutExtension(file.label);
            t.fileExtension = fileService.getFileExtension(file.label);
            t.thumbnail = api.hasThumbnail(file.uri);
            t.thumbnailUrl = api.getThumbnailUrl(file.uri);
            t.link = enableLink === undefined || enableLink === true;

            $(element).find('.file-thumbnail').css('background-image', 'url(' + t.thumbnailUrl + ')');
        }
    }
})();