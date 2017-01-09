(function () {
    angular.module('explorerApp').directive('artPhotoPicker', PhotoPickerDirective);

    function PhotoPickerDirective() {
        return {
            link: function (scope, element, attributes) {
                element.bind("change", function (changeEvent) {
                    scope.$apply(function () {
                        // Store the selected picture in the model for saving when the changes are applied.
                        scope.userPhoto = changeEvent.target.files[0];
                    });
                });
            }
        }
    }
})();