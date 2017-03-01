(function () {
    angular.module('app').directive('artTabControl', TabControlDirective);

    function TabControlDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-tab-control/art-tab-control.html',
            transclude: true,
            bindToController: true,
            scope: {
                type: '@',
                vertical: '@',
                justified: '@'
            },
            controller: TabControlDirectiveController,
            controllerAs: 't'
        }
    }

    angular.module('app').controller('TabControlDirectiveController', TabControlDirectiveController);

    TabControlDirectiveController.$inject = ['$scope'];

    function TabControlDirectiveController($scope) {
        var t = this;

        t.tabs = [];
        t.classes = {};

        initialize();

        function initialize() {
            if (t.type === 'pills') {
                t.classes['nav-pills'] = true;
            } else {
                t.classes['nav-tabs'] = true;
            }

            if (t.vertical) {
                t.classes['nav-stacked'] = true;
            }

            if (t.justified) {
                t.classes['nav-justified'] = true;
            }
        }

        t.addTab = function (tab) {
            t.tabs.push(tab);

            if (t.tabs.length === 1) {
                tab.active = true;
            }
        }

        t.selectTab = function (tab) {
            angular.forEach(t.tabs, function (x) {
                if (x.active && x !== tab) {
                    x.active = false;
                }
            })

            tab.active = true;
        }
    }
})();