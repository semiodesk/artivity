(function () {
    angular.module('explorerApp').directive('artEditingHistory', EditingHistoryDirective);

    function EditingHistoryDirective() {
        return {
            restrict: 'E',
            templateUrl: 'partials/directives/art-editing-history.html',
            controller: EditingHistoryDirectiveController,
            controllerAs: 't',
            scope: {
                agent: '=agent',
                influences: '=influences',
                activities: '=activities'
            },
            link: function (scope, element, attr, ctrl) {
                ctrl.init(element);
            }
        }
    }

    angular.module('explorerApp').controller('EditingHistoryDirectiveController', EditingHistoryDirectiveController);

    function EditingHistoryDirectiveController($rootScope, $scope, selectionService, translationService, formattingService) {
        var t = this;

        t.autoScroll = true;

        selectionService.on('selectedItemChanged', function (influence) {
            // Highlight the currently selected item.
            updateSelection(influence);

            if (t.autoScroll) {
                scrollIntoView(influence);
            }

            // Re-enable auto scroll if it was deactivated.
            t.autoScroll = true;
        });

        t.selectInfluence = function (influence, scroll = true) {
            t.autoScroll = scroll;

            selectionService.selectedItem(influence);

            $rootScope.$broadcast('redraw');
        }

        t.getInfluenceLabel = translationService.getInfluenceLabel;
        t.getInfluenceIcon = translationService.getInfluenceIcon;
        t.getFormattedTime = formattingService.getFormattedTime;
        t.getFormattedDate = formattingService.getFormattedDate;
        t.getFormattedTimeFromNow = formattingService.getFormattedTimeFromNow;

        t.init = function (element) {
            // Update the scrollviewer to the controller everytime the directive is re-used.
            t.scrollViewer = $(element).closest('.scroll-container.scroll-y');

            $(t.scrollViewer.keydown(function (e) {
                if (e.which == 40) { // Arrow key down
                    selectionService.selectNext();

                    $rootScope.$broadcast('redraw');

                    e.preventDefault();
                } else if (e.which === 38) { // Arrow up
                    selectionService.selectPrev();

                    $rootScope.$broadcast('redraw');
                    
                    e.preventDefault();
                }
            }));
        }

        /* We scroll in percent of the entire scrollable surface to have the scrollbar 
           in sync with the timeline bar. The first selected element scrolls to top and
           the last element scrolls to bottom. */
        function scrollIntoView(influence) {
            if ($scope.influences === undefined) {
                return;
            }

            var n = influence.id;
            var N = $scope.influences.length - 1;

            if (N > 0 && n <= N) {
                var h = t.scrollViewer[0].scrollHeight;
                var top = (n / N) * h;

                t.scrollViewer.scrollTop(top);
            }
        }

        function updateSelection(influence) {
            $('li.selected').each(function (i, element) {
                $(element).removeClass('selected');
            });

            $('li[data-id="' + influence.id + '"]').each(function (i, element) {
                $(element).addClass('selected');
            });
        }
    }
})();