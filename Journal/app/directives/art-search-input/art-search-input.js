(function () {
    angular.module('app').directive('artSearchInput', artSearchInputDirective);

    function artSearchInputDirective() {
        return {
            restrict: 'A',
            controller: SearchInputDirectiveController
        }
    }

    angular.module('app').controller('SearchInputDirectiveController', SearchInputDirectiveController);

    SearchInputDirectiveController.$inject = ['$element', '$timeout'];

    function SearchInputDirectiveController($element, $timeout) {
        var t = this;

        t.$onInit = function () {
            $element.addClass('search-input');

            $timeout(function () {
                var input = $element.find('input');

                input.focus(function (e) {
                    $element.addClass('focused');
                });

                input.blur(function (e) {
                    $element.removeClass('focused');
                });
            }, 0);
        }
    }
})();