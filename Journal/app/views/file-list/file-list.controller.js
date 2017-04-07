(function () {
    angular.module('app').controller('FileListController', FileListController);

    FileListController.$inject = ['$scope', '$uibModal', 'windowService', 'hotkeys'];

    function FileListController($scope, $uibModal, windowService, hotkeys) {
        var t = this;

        windowService.setMinimizable();
        windowService.setMaximizable();
    }
})();