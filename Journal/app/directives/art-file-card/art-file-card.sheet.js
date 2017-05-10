(function () {
    angular.module('app').controller('FileCardDirectiveSheetController', FileCardDirectiveSheetController);

    FileCardDirectiveSheetController.$inject = ['$rootScope', '$scope', 'file'];

    function FileCardDirectiveSheetController($rootScope, $scope, file) {
        var t = this;

        t.viewFile = function (e) {
            if (file) {
                $rootScope.$broadcast('viewFile', file);
            }
        }

        t.viewFileHistory = function (e) {
            if (file) {
                $rootScope.$broadcast('viewFileHistory', file);
            }
        }
    }
})();