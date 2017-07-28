(function () {
    angular.module('app').controller('SetupStateController', SetupStateController);

    SetupStateController.$inject = ['$rootScope', '$scope', '$state', 'api', 'settingsService', 'windowService'];

    function SetupStateController($rootScope, $scope, $state, api, settingsService, windowService) {
        var t = this;

        $scope.stepCount = 2;
        $scope.stepIndex = 0;
        $scope.percentComplete = 50;

        $scope.navigateNext = function () {
            if ($scope.stepIndex < $scope.stepCount - 1) {
                $scope.stepIndex += 1;

                $scope.onActiveTabChanged();
            }
        }

        $scope.navigatePrevious = function () {
            if ($scope.stepIndex > 0) {
                $scope.stepIndex -= 1;
                
                $scope.onActiveTabChanged();
            }
        }

        $scope.onActiveTabChanged = function () {
            $scope.percentComplete = ($scope.stepCount / ($scope.stepIndex + 1)) * 100;
            $scope.hasPreviousButton = $scope.stepIndex > 0;
            $scope.navigatePreviousDisable = false;

            $scope.hasNextButton = $scope.stepIndex < $scope.stepCount - 1;
            $scope.navigateNextDisabled = true;

            $scope.setupComplete = $scope.setupComplete | !$scope.hasNextButton;
        }

        $scope.submitAndReturn = function () {
            // Submit all setup pages.
            settingsService.submitAll();

            api.setRunSetup(false).then(function () {
                windowService.setWidth(992);

                // After the setup has been disabled, go to the homepage.
                $state.go('main.view.recently-used', {
                    index: 0
                });
            });
        }

        t.onInit = function () {
            windowService.setMinimizable(false);
            windowService.setMaximizable(false);

            $rootScope.$on('navigateNext', function () {
                $scope.navigateNext();
            });

            $rootScope.$on('navigateNextEnabled', function () {
                $scope.navigateNextDisabled = false;
            });
        }

        t.onInit();
    }
})();