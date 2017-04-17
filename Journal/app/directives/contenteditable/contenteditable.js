(function () {
    angular.module('app').directive('contenteditable', ContentEditableDirective);

    function ContentEditableDirective() {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, element, attr, ngModel) {
                function updateViewValue() {
                    var html = this.innerHTML;

                    ngModel.$setViewValue(html);
                }

                //Binding it to keyup will bind it to any other events of interest like change etc..
                element.on('keyup', updateViewValue);

                scope.$on('$destroy', function () {
                    element.off('keyup', updateViewValue);
                });

                ngModel.$render = function () {
                    element.html(ngModel.$viewValue);
                }
            }
        }
    }
})();