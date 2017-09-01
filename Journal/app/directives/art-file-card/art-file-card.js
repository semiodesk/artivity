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

    FileCardDirectiveController.$inject = ['$rootScope', '$scope', '$element', '$mdDialog', 'api', 'filesystemService', 'entityService', 'syncService'];

    function FileCardDirectiveController($rootScope, $scope, $element, $mdDialog, api, filesystemService, entityService, syncService) {
        var t = this;

        t.onTouchStart = function (e) {
            // On touch, select the element first to reveal any context relevant controls.
            if (e && !$element.find('.art-file-card-container').hasClass('selected')) {
                e.preventDefault();

                // Remove any existing selections.
                $(document).find('.art-file-card-container.selected').removeClass('selected');

                // Mark the card as selected.
                $element.find('.art-file-card-container').addClass('selected');
            }
        }

        t.onMouseEnter = function (e) {
            // Remove any existing selections.
            $(document).find('.art-file-card-container.selected').removeClass('selected');
        }

        t.onDragStart = function () {
            $scope.$emit('dragStarted');
        }

        t.onDragStop = function () {
            $scope.$emit('dragStopped');
        }

        t.toggleImportant = function (e, file) {
            if (file) {
                var value = !file.important;

                api.setFileIsImportant(file.uri, value).then(function () {
                    file.important = value;
                });
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
            $element.find('.art-file-card').on('touchstart', t.onTouchStart);

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
            $element.find('.art-file-card').off('touchstart', t.onTouchStart);
        }
    }
})();