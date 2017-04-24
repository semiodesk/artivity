(function () {
    angular.module('app').directive('artSyncIndicator', SyncIndicatorDirective);

    function SyncIndicatorDirective() {
        return {
            restrict: 'E',
            template: '<span ng-show="synchronizing"><i class="loader loader-xs"></i> <span translate>Synchronizing..</span></span>',
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

        syncService.on('syncBegin', function () {
            $scope.synchronizing = true;
        });

        syncService.on('syncEnd', function () {
            // Let people see when a sync is executed very quickly.
            setTimeout(function () {
                $scope.$apply(function () {
                    $scope.synchronizing = false;
                });
            }, 500);
        });

        t.synchronize = function () {
            syncService.synchronize();
        }
    }
})();