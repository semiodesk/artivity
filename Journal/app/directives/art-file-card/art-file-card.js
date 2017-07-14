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

    FileCardDirectiveController.$inject = ['$rootScope', '$scope', '$element', '$mdDialog', 'filesystemService', 'entityService'];

    function FileCardDirectiveController($rootScope, $scope, $element, $mdDialog, filesystemService, entityService) {
        var t = this;

        t.onDragStart = function () {
            $scope.$emit('dragStarted');
        }

        t.onDragStop = function () {
            $scope.$emit('dragStopped');
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
                    }, function () {});
                }, function () {});
        }

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