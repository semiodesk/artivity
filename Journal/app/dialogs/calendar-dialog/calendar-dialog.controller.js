(function () {
    angular.module('app').controller('CalendarDialogController', CalendarDialogController);

    function CalendarDialogController(api, $scope, $filter, $uibModalInstance, $sce) {
        $scope.isLoading = true;
        $scope.dialog = $uibModalInstance;
        $scope.getActivities = api.getActivities;

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();