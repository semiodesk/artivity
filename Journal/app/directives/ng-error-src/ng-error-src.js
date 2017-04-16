(function () {
    angular.module('app').directive('ngErrorSrc', ErrorSourceDirective);

    function ErrorSourceDirective() {
        return {
            link: function (scope, element, attr) {
                element.bind('error', function () {
                    $(element).attr('src', attr.ngErrorSrc);
                });
            }
        }
    }
})();