(function () {
    angular.module('app').directive('artWindowTitle', WindowTitleDirective);

    function WindowTitleDirective() {
        return {
            restrict: 'A',
            controller: WindowTitleDirectiveController
        }
    }

    angular.module('app').controller('WindowTitleDirectiveController', WindowTitleDirectiveController);

    WindowTitleDirectiveController.$inject = ['$scope', '$element'];

    function WindowTitleDirectiveController($scope, $element) {
        this.$postLink = function() {
            var title = $(document).find('.art-window-titlebar .art-window-title-container');

            if(title && title.length) {
                // Remove the element from DOM.
                $element.remove();

                // Insert the element into the title area.
                title.prepend($element);
            }
        }
    }
})();