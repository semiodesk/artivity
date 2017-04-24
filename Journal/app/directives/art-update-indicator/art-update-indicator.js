(function () {
    angular.module('app').directive('artUpdateIndicator', UpdateIndicatorDirective);

    function UpdateIndicatorDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-update-indicator/art-update-indicator.html',
            controller: UpdateIndicatorDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attributes, ctrl) {
                $("#btn-update").click(function () {
                    ctrl.showUpdateDialog();
                });
            }
        }
    }

    angular.module('app').controller('UpdateIndicatorDirectiveController', UpdateIndicatorDirectiveController);

    UpdateIndicatorDirectiveController.$inject = ['$scope', '$uibModal', 'api', 'settingsService', 'updateService'];

    function UpdateIndicatorDirectiveController($scope, $uibModal, api, settingsService, updateService) {
        var t = this;
        t.modalInstance = null;
        updateService.isUpdateAvailable();
        updateService.on('updateAvailable', function () {
            $("#btn-update").fadeIn();

            updateService.isUpdateDownloaded().then(function () {
                t.setProgressValue(100);
            }, function() {}).catch(function () {});
        });

        updateService.on('progress', function (progress) {
            t.setProgressValue(progress.percentComplete);
        });

        t.showUpdateDialog = function () {
            if (!t.modalInstance) {
                t.modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: 'app/dialogs/update-dialog/update-dialog.html',
                    controller: 'UpdateDialogController',
                    controllerAs: 't',
                    scope: $scope
                });

                t.disableInfoButton();

                t.modalInstance.closed.then(function () {
                    t.modalInstance = null;

                    t.enableInfoButton();
                });
            }
        }

        t.enableInfoButton = function () {
            $("#btn-update").removeClass('disabled');
        }

        t.disableInfoButton = function () {
            $("#btn-update").addClass('disabled');
        }

        t.setProgressValue = function (value) {
            var label = $("#btn-update .btn-label");

            if (label) {
                label.show();
                label.text(value + '%');
            }
        }
    }
})();