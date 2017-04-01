(function () {
    angular.module('app').directive('artDocumentView', DocumentViewDirective);

    function DocumentViewDirective() {
        return {
            restrict: 'E',
            scope: {
                'file': '='
            },
            templateUrl: 'app/directives/art-document-view/art-document-view.html',
            controller: DocumentViewDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('DocumentViewDirectiveController', DocumentViewDirectiveController);

    DocumentViewDirectiveController.$inject = ['$rootScope', '$scope', 'navigationService'];

    function DocumentViewDirectiveController($rootScope, $scope, navigationService) {
        var t = this;

        t.back = function() {
            navigationService.navigateBack($scope);
        }
    }
})();