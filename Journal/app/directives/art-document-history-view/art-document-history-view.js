(function () {
    angular.module('app').directive('artDocumentHistoryView', DocumentHistoryViewDirective);

    function DocumentHistoryViewDirective() {
        return {
            restrict: 'E',
            scope: {
                'file': '=file'
            },
            templateUrl: 'app/directives/art-document-history-view/art-document-history-view.html',
            controller: DocumentHistoryViewDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('DocumentHistoryViewDirectiveController', DocumentHistoryViewDirectiveController);

    DocumentHistoryViewDirectiveController.$inject = ['$rootScope', '$scope'];

    function DocumentHistoryViewDirectiveController($rootScope, $scope) {
        var t = this;
    }
})();