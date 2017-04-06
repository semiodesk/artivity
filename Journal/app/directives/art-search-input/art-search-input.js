(function () {
    angular.module('app').directive('artSearchInput', artSearchInputDirective);

    function artSearchInputDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-search-input/art-search-input.html',
            scope: {
                query: '=?'
            },
            controller: SearchInputDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('SearchInputDirectiveController', SearchInputDirectiveController);

    SearchInputDirectiveController.$inject = ['$scope', '$element', 'hotkeys'];

    function SearchInputDirectiveController($scope, $element, hotkeys) {
        var t = this;

        // FOCUS
        t.onKeyUp = function (e) {
            if (e.keyCode === 27) {
                if (t.query.length > 0) {
                    t.clear();
                } else {
                    t.escape();
                }
            }
        }

        t.focus = function () {
            var input = $($element).find('input');

            if (input.length === 1) {
                input.get(0).focus();
            }
        }

        t.clear = function () {
            t.query = '';
        }

        t.escape = function () {
            var input = $($element).find('input');

            if (input.length === 1) {
                input.get(0).blur();
            }
        }

        t.$onInit = function () {
            if (!t.query) {
                t.query = '';
            }

            hotkeys.add({
                combo: 'ctrl+f',
                description: 'Search the current view.',
                callback: function () {
                    t.focus();
                }
            })
        }
    }
})();