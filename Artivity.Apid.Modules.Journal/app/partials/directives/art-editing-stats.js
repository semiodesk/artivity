angular.module('explorerApp').directive('artEditingStats', function () {
	return {
		restrict: 'E',
		templateUrl: 'partials/directives/art-editing-stats.html',
		controller: EditingStatsDirectiveController,
		controllerAs: 't',
        scope: {
            stats: "=stats"
        }
	}
});

function EditingStatsDirectiveController($scope, selectionService) {
	var t = this;

    selectionService.on('selectionChanged', function(influence) {
        try {
            if (!t.$$phase) {
                $scope.$digest();
            }
        } catch (error) {}
    });
};