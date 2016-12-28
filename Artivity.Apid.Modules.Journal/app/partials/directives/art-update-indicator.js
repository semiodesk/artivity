
angular.module('explorerApp').directive('artUpdateIndicator', UpdateIndicatorDirective);

function UpdateIndicatorDirective() {
    return {
        template: '<i class="zmdi zmdi-cast" uib-tooltip="Update available." tooltip-placement="bottom"></i><span class="btn-label"></span>',
        controller: UpdateIndicatorDirectiveController,
        controllerAs: 't',
        link: function (scope, element, attributes, ctrl) {
            $("#btn-update").click(function () {
                ctrl.showUpdateDialog();
            });
        }
    }
}

angular.module('explorerApp').controller('UpdateIndicatorDirectiveController', UpdateIndicatorDirectiveController);

function UpdateIndicatorDirectiveController(api, $scope, settingsService, $uibModal, updateService) {
    var t = this;
    t.modalInstance = null;

    updateService.on('updateAvailable', function() {
        $("#btn-update").fadeIn();

        updateService.isUpdateDownloaded().then(function() {
            t.setProgressValue(100);
        }).catch(function() {});
    });

    updateService.on('progress', function(progress) {
        t.setProgressValue(progress.percentComplete);
    });

    t.showUpdateDialog = function () {
        if (!t.modalInstance) {
            t.modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'partials/dialogs/update-dialog.html',
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

    t.setProgressValue = function(value) {
        var label = $("#btn-update .btn-label");

        if(label) {
            label.show();
            label.text(value + '%');
        }
    }
}