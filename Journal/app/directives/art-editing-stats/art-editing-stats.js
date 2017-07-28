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
            $scope.$on('fileLoaded', function (e, data) {
                var influences = data.influences;
                var statistics = data.statistics;

                for (var j = influences.length - 1; j >= 0; j--) {
                    var influence = influences[j];

                    // Compute the statistics.
                    statistics.stepCount++;

                    switch (influence.type) {
                        case 'http://w3id.org/art/terms/1.0/Undo':
                            statistics.undoCount++;
                            break;
                        case 'http://w3id.org/art/terms/1.0/Redo':
                            statistics.redoCount++;
                            break;
                    }

                    influence.stats = new EditingStatistics();
                    influence.stats.stepCount = statistics.stepCount;
                    influence.stats.undoCount = statistics.undoCount;
                    influence.stats.redoCount = statistics.redoCount;
                }
            });

            $scope.$on('influenceSelected', function (e, args) {
                var influence = args.data;

                if (influence && args.sourceScope !== t) {
                    t.stats = influence.stats;
                }
            });
        }
    };
})();