(function () {
    angular.module('app').directive('artDocumentHistoryViewerToolbar', artDocumentHistoryViewerToolbar);

    function artDocumentHistoryViewerToolbar() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-document-history-viewer-toolbar/art-document-history-viewer-toolbar.html',
            scope: {},
            controller: DocumentHistoryViewerToolbarDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('DocumentHistoryViewerToolbarDirectiveController', DocumentHistoryViewerToolbarDirectiveController);

    DocumentHistoryViewerToolbarDirectiveController.$inject = ['$rootScope', '$scope', '$element'];

    function DocumentHistoryViewerToolbarDirectiveController($rootScope, $scope, $element) {
        var t = this;

        t.playloop = undefined;
        t.playing = false;

        t.play = function () {
            var t = this;
            var end = t.influences.indexOf(t.selectionService.selectedItem()) === 0;

            if (!t.playloop && !end) {
                t.playloop = setInterval(function () {
                    t.skipNext();
                }, 500);
                t.playing = true;
            }
        };

        t.pause = function () {
            if (t.playloop) {
                clearInterval(t.playloop);

                t.playloop = undefined;
                t.playing = false;

                try {
                    if (!$scope.$$phase) {
                        $scope.$digest();
                    }
                } catch (error) {}
            }
        };

        t.togglePlay = function () {
            if (t.playloop) {
                t.pause();
            } else {
                t.play();
            }
        };

        t.skipPrev = function () {
            if (t.influences === undefined) {
                return;
            }

            t.selectionService.selectNext();

            t.rootScope.$broadcast('redraw');

            if (t.playloop) {
                var i = t.selectionService.selectedIndex();

                if (i === t.influences.length - 1) {
                    t.pause();
                }
            }
        };

        t.skipNext = function () {
            t.selectionService.selectPrev();

            t.rootScope.$broadcast('redraw');

            if (t.playloop) {
                var i = t.selectionService.selectedIndex();

                if (i === 0) {
                    t.pause();
                }
            }
        };
    };
})();