(function () {
    angular.module('app').directive('artEditingStats', function () {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-editing-stats/art-editing-stats.html',
            controller: EditingStatsDirectiveController,
            controllerAs: 't',
            bindToController: true,
            scope: {}
        }
    });

    EditingStatsDirectiveController.$inject = ['$scope'];

    function EditingStatsDirectiveController($scope) {
        var t = this;

        t.$onInit = function () {
            $scope.$on('influenceSelected', function (e, influence) {
                t.stats = influence.stats;
            });
        }
    };
})();