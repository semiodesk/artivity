(function () {
    angular.module('app').directive('artFolderPicker', FolderPickerDirective);

    function FolderPickerDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-folder-picker/art-folder-picker.html',
            controller: FolderPickerDirectiveController,
            controllerAs: 't',
            scope: {
                folder: '=folder',
                changed: '=?'
            },
            link: function (scope, element, attributes) {
                var button = $(element).find('.btn-select-folder');

                if (button) {
                    button.click(function () {
                        scope.t.selectFolder();
                    });
                }

                scope.$watch('folder', function (newValue, oldValue) {
                    if (newValue) {
                        scope.folderPath = new URL(newValue).pathname;
                    }
                }, true);
            }
        }
    }

    FolderPickerDirectiveController.$inject = ['$rootScope', '$scope', 'selectionService', 'windowService'];

    function FolderPickerDirectiveController($rootScope, $scope, selectionService, windowService) {
        var t = this;

        t.selectFolder = function () {
            var f = windowService.selectFolder();

            if (f && f !== $scope.folder) {
                $scope.folder = f;
                $scope.$apply();

                if ($scope.changed) {
                    $scope.changed("file:///" + f.replace(/\\/g, '/'));
                }
            }
        }
    };
})();