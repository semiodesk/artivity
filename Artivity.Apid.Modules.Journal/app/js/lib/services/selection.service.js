(function () {
    angular.module('explorerApp').factory('selectionService', selectionService);

    function selectionService() {
        // TODO: Add change notification support.
        return {
            items: []
        }
    }
})();