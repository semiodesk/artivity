(function () {
    angular.module('app').directive('artTabHeading', TabHeadingDirective);

    function TabHeadingDirective() {
        return {
            restrict: 'E',
            template: '<div class="tab-heading" ng-transclude></div>',
            transclude: true,
            scope: {},
            require: '^art-tab',
            controllerAs: 't',
            link: function(scope, elem, attr, ctrl) {
            }
        }
    }
})();