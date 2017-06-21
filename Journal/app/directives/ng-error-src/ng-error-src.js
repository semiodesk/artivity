(function () {
    angular.module('app').directive('ngErrorSrc', ErrorSourceDirective);

    function ErrorSourceDirective() {
        return {
            link: function (scope, element, attr) {
                element.bind('error', function () {
                    scope.$apply(function () {
                        //$(element).attr('src', attr.ngErrorSrc);
                        scope.$parent.t.src = attr.ngErrorSrc;
                    });
                });
            }
        }
    }
})();