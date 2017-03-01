(function () {
    // See: https://thinkster.io/angular-tabs-directive

    angular.module('app').directive('artTab', TabDirective);

    function TabDirective() {
        return {
            restrict: 'E',
            template: '<div class="tab-content" role="tabpanel" ng-show="active" ng-transclude></div>',
            transclude: true,
            scope: {
                heading: '@'
            },
            require: '^art-tab-control',
            controllerAs: 't',
            link: function(scope, elem, attr, ctrl) {
                // Tabs are intially inactive.
                scope.active = false;

                // Register the tab with its parent tab control.
                ctrl.addTab(scope);
            }
        }
    }
})();