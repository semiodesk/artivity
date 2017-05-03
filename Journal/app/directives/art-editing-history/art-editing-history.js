(function () {
    angular.module('app').directive('artEditingHistory', EditingHistoryDirective);

    function EditingHistoryDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-editing-history/art-editing-history.html',
            controller: EditingHistoryDirectiveController,
            controllerAs: 't',
            bindToController: true,
            scope: {}
        }
    }

    angular.module('app').controller('EditingHistoryDirectiveController', EditingHistoryDirectiveController);

    EditingHistoryDirectiveController.$inject = ['$rootScope', '$scope', '$element', 'formattingService'];

    function EditingHistoryDirectiveController($rootScope, $scope, $element, formattingService) {
        var t = this;
        
        t.autoScroll = true;
        t.influences = [];

        t.getFormattedTime = formattingService.getFormattedTime;
        t.getFormattedDate = formattingService.getFormattedDate;
        t.getFormattedTimeFromNow = formattingService.getFormattedTimeFromNow;

        t.selectInfluence = function (influence, scroll = true) {
            t.autoScroll = scroll;

            $rootScope.$broadcast('influenceSelected', influence);
        }

        // We scroll in percent of the entire scrollable surface to have the scrollbar 
        // in sync with the timeline bar. The first selected element scrolls to top and
        // the last element scrolls to bottom.
        t.scrollIntoView = function (influence) {
            if (t.influences === undefined) {
                return;
            }

            var n = influence.id;
            var N = t.influences.length - 1;

            if (N > 0 && n <= N) {
                var h = t.scrollViewer[0].scrollHeight;
                var top = (n / N) * h;

                t.scrollViewer.scrollTop(top);
            }
        }

        t.$onInit = function () {
            // Update the scrollviewer to the controller everytime the directive is re-used.
            t.scrollViewer = $element.find('.scroll-container.scroll-y');

            t.scrollViewer.keydown(function (e) {
                if (e.which == 40) { // Arrow key down
                    selectionService.selectNext();

                    $rootScope.$broadcast('redraw');

                    e.preventDefault();
                } else if (e.which === 38) { // Arrow up
                    selectionService.selectPrev();

                    $rootScope.$broadcast('redraw');

                    e.preventDefault();
                }
            });

            t.fileLoadedListener = $scope.$on('fileLoaded', function(e, data) {
                t.activities = data.activities;
                t.influences = data.influences;
            });

            t.influenceSelectedListener = $scope.$on('influenceSelected', function (e, influence) {
                // Highlight the currently selected item.
                $('li.selected').each(function (i, element) {
                    $(element).removeClass('selected');
                });

                $('li[data-id="' + influence.id + '"]').each(function (i, element) {
                    $(element).addClass('selected');
                });

                // Scroll into view.
                if (t.autoScroll) {
                    t.scrollIntoView(influence);
                }

                // Re-enable auto scroll if it was deactivated.
                t.autoScroll = true;
            });
        }

        t.$onDestroy = function() {
            t.fileLoadedListener();
            t.influenceSelectedListener();
        }
    }
})();