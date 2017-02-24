(function () {
    angular.module('app').controller('CalendarDialogController', CalendarDialogController);

    CalendarDialogController.$inject = ['$scope', '$filter', '$uibModalInstance', '$sce', 'api'];

    function CalendarDialogController($scope, $filter, $uibModalInstance, $sce, api) {
        $scope.isLoading = true;
        $scope.dialog = $uibModalInstance;
        $scope.getActivities = api.getActivities;

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();