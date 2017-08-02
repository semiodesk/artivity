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

    FileCardDirectiveController.$inject = ['$rootScope', '$scope', '$element', '$mdDialog', 'api', 'filesystemService', 'entityService'];

    function FileCardDirectiveController($rootScope, $scope, $element, $mdDialog, api, filesystemService, entityService) {
        var t = this;

        t.onTouchStart = function (e) {
            if (e) {
                e.preventDefault();
            }

            // Remove any existing selections.
            $(document).find('.art-file-card.selected').removeClass('selected');

            // Mark the card as selected.
            $element.find('.art-file-card').addClass('selected');
        }

        t.onMouseEnter = function (e) {
            // Remove any existing selections.
            $(document).find('.art-file-card.selected').removeClass('selected');
        }

        t.onDragStart = function () {
            $scope.$emit('dragStarted');
        }

        t.onDragStop = function () {
            $scope.$emit('dragStopped');
        }

        t.toggleImportant = function(e, file) {
            if(file) {
                if(file.important === true) {
                    api.setFileIsImportant(file.uri, false).then(function() {
                        file.important = false;
                    });
                } else {
                    api.setFileIsImportant(file.uri, true).then(function() {
                        file.important = true;
                    })
                }
            }
        }

        t.editFile = function (e, file) {
            e.stopPropagation();

            if (file && file.folder && file.label) {
                api.editFile(file.folder + '/' + file.label);
            }
        }

        t.publishFile = function (e) {
            $mdDialog.show({
                templateUrl: 'share-file-dialog.html',
                parent: angular.element(document.body),
                targetEvent: e,
                controller: function ($scope) {
                    $scope.file = t.file;

                    $scope.ok = function () {
                        $mdDialog.hide();
                    }

                    $scope.cancel = function () {
                        $mdDialog.cancel();
                    }
                }
            })
                .then(function (answer) {
                    entityService.publishLatestRevisionFromFileUri(t.file.uri).then(function () {
                        t.file.synchronizationEnabled = true;

                        syncService.synchronize();
                    }, function () { });
                }, function () { });
        }

        t.$postLink = function () {
            $element.on('touchstart', t.onTouchStart);

            $scope.$watch('t.file', function () {
                if (t.file) {
                    var label = t.file.label;

                    t.fileName = filesystemService.getFileNameWithoutExtension(label);
                    t.fileExtension = filesystemService.getFileExtension(label);

                    $element.find('.file-extension').addClass(t.fileExtension);
                }
            });
        }

        t.$onDestroy = function () {
            $element.off('touchstart', t.onTouchStart);
        }
    }
})();