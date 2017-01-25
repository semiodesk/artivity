(function () {
    angular.module('explorerApp').directive('ngEsc', EscapeKeyBindingDirective);

    function EscapeKeyBindingDirective() {
        return function (scope, element, attrs) {
            element.bind('keydown keypress', function (event) {
                if (event.which === 27) { // 27 = esc key
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEsc);
                    });

                    event.preventDefault();
                }
            });
        };
    }
})();