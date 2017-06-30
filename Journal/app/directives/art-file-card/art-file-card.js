(function () {
    angular.module('app').directive('artFileCard', FileCardDirective);

    function FileCardDirective() {
        return {
            restrict: 'E',
            scope: {
                'file': '=',
                'highlightText': '=?'
            },
            templateUrl: 'app/directives/art-file-card/art-file-card.html',
            controller: FileCardDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('FileCardDirectiveController', FileCardDirectiveController);

    FileCardDirectiveController.$inject = ['$rootScope', '$scope', '$element', '$location', '$mdBottomSheet', 'selectionService', 'filesystemService'];

    function FileCardDirectiveController($rootScope, $scope, $element, $location, $mdBottomSheet, selectionService, filesystemService) {
        var t = this;

        t.onDragStart = function () {
            $scope.$emit('dragStarted');
        }

        t.onDragStop = function () {
            $scope.$emit('dragStopped');
        }
        
        t.showSheet = function ($event) {
            t.alert = '';

            // The container needs to be relative positioned and with hidden overflow.
            var card = angular.element($element.find('.card'));

            $mdBottomSheet.show({
                parent: card,
                templateUrl: 'app/directives/art-file-card/art-file-card.sheet.html',
                controller: 'FileCardDirectiveSheetController',
                controllerAs: "t",
                bindToController: true,
                disableParentScroll: true,
                targetEvent: $event,
                locals: {
                    file: t.file
                }
            }).then(function (clickedItem) {}).catch(function (error) {
                // User clicked outside or hit escape
            });
        };

        t.$postLink = function () {
            $scope.$watch('t.file', function () {
                if (t.file) {
                    var label = t.file.label;

                    t.fileName = filesystemService.getFileNameWithoutExtension(label);
                    t.fileExtension = filesystemService.getFileExtension(label);

                    $element.find('.file-extension').addClass(t.fileExtension);
                }
            });
        }
    }
})();