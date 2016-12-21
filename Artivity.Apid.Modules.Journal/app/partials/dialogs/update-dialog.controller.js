angular.module('explorerApp').controller('UpdateDialogController', UpdateDialogController);

function UpdateDialogController(api, $scope, $filter, $uibModalInstance, $sce, updateService) {
    var t = this;

    t.title = $filter('translate')('UPDATE.TITLE');
    t.update = {};
    t.progress = null;
    t.progressAnimate = false;
    t.initialized = false;

    updateService.isUpdateAvailable().then(init);

    function init() {
        var update = updateService.update;

        if(update.releaseNotesUrl && update.releaseNotesUrl.indexOf('https') === 0) {
            update.releaseNotesUrl = $sce.trustAsResourceUrl(update.releaseNotesUrl);
        }

        t.update = update;

        updateService.on('progress', onDownloadProgress);
        updateService.isUpdateDownloaded().then(function () {
            // Set the download progress in case the update was downloaded prior to checking.
            t.progress = {
                percentComplete: 100
            };

            $scope.$apply();
        });

        $uibModalInstance.closed.then(function () {
            updateService.off('progress', onDownloadProgress);
        });

        t.initialized = true;
    }

    function onDownloadProgress(progress) {
        t.progress = progress;

        $scope.$apply();
    }

    t.downloadUpdate = function () {
        console.log('Downloading Update..');

        t.progressAnimate = true;

        updateService.downloadUpdate(t.update).then(function () {
            console.log('Downloaded Update');
        });
    }

    t.installUpdate = function () {
        updateService.installUpdate(t.update).then(function () {
            $uibModalInstance.dismiss('ok');
        });
    }

    t.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
};