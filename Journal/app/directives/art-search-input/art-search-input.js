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
        t.sourceElement = undefined;

        t.onFocused = function (e) {
            if (e.originalEvent && e.originalEvent.srcElement) {
                t.sourceElement = $(e.originalEvent.srcElement);
            }
        }

        t.onBlurred = function (e) {
            t.sourceElement = undefined;
        }

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
            if (t.sourceElement && t.sourceElement.length > 0) {
                t.sourceElement.get(0).focus();
            }
        }

        t.$onInit = function () {
            if(!t.query) {
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