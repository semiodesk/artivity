angular.module('explorerApp').directive('ngEnter', EnterKeyBindingDirective);

function EnterKeyBindingDirective() {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) { // 13 = enter key
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
}