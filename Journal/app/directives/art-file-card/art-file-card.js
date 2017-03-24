(function () {
    angular.module('app').directive('artFileCard', FileCardDirective);

    function FileCardDirective() {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: 'app/directives/art-file-card/art-file-card.html',
            controller: FileCardDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attr, ctrl) {
                ctrl.setFile(element, JSON.parse(attr.file), attr.enableLink);
            }
        }
    }

    angular.module('app').controller('FileCardDirectiveController', FileCardDirectiveController);

    FileCardDirectiveController.$inject = ['$scope', 'api', 'filesystemService', '$location'];

    function FileCardDirectiveController($scope, api, filesystemService, $location) {
        var t = this;

        t.setFile = setFile;
        t.onClick = onClick;

        function setFile(element, file, enableLink) {
            t.file = file;
            t.fileName = filesystemService.getFileNameWithoutExtension(file.label);
            t.fileExtension = filesystemService.getFileExtension(file.label);

            if (file.thumbnail !== undefined) {
                t.thumbnail = true;
                t.thumbnailUrl = file.thumbnail;
            }
            
            t.link = enableLink === undefined || enableLink === true;

            $(element).find('.file-thumbnail').css('background-image', 'url(' + t.thumbnailUrl + ')');
        }

        function onClick(e) {
            e.preventDefault();

            if (event.ctrlKey) {
                $location.path("/files/view").search("uri", t.file.uri).search("entityUri", t.file.entityUri);
            } else {
                $location.path("/files/preview").search("uri", t.file.uri);
            }
        }
    }
})();