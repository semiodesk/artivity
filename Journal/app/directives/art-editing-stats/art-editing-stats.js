(function () {
    angular.module('app').directive('artEditingStats', function () {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-editing-stats/art-editing-stats.html',
            controller: EditingStatsDirectiveController,
            controllerAs: 't',
            scope: {
                stats: "=stats"
            }
        }
    });

    EditingStatsDirectiveController.$inject = ['$scope', 'selectionService'];

    function EditingStatsDirectiveController($scope, selectionService) {
        var t = this;
    };
})();