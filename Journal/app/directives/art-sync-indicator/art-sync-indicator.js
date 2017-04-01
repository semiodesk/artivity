(function () {
    angular.module('app').directive('artSyncIndicator', SyncIndicatorDirective);

    function SyncIndicatorDirective() {
        return {
            restrict: 'E',
            template: '<i ng-show="synchronizing" class="loader loader-xs"></i>',
            controller: SyncIndicatorDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attributes, ctrl) {
                $("#btn-sync").click(function () {
                    ctrl.synchronize();
                });
            }
        }
    }

    angular.module('app').controller('SyncIndicatorDirectiveController', SyncIndicatorDirectiveController);

    SyncIndicatorDirectiveController.$inject = ['$scope', '$uibModal', 'settingsService', 'syncService'];

    function SyncIndicatorDirectiveController($scope, $uibModal, settingsService, syncService) {
        var t = this;

        $scope.synchronizing = false;

        syncService.on('syncBegin', function() {
            $scope.synchronizing = true;
        });

        syncService.on('syncEnd', function() {
            $scope.synchronizing = false;
        });

        t.synchronize = function() {
            syncService.synchronize();
        }
    }
})();