(function () {
    angular.module('app').controller('FileListController', FileListController);

    FileListController.$inject = ['$scope', '$uibModal', 'api', 'windowService', 'hotkeys'];

    function FileListController($scope, $uibModal, api, windowService, hotkeys) {
        var t = this;

        windowService.setMinimizable();
        windowService.setMaximizable();
    }
})();