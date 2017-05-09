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

        t.getFormattedTime = formattingService.getFormattedTime;
        t.getFormattedDate = formattingService.getFormattedDate;
        t.getFormattedTimeFromNow = formattingService.getFormattedTimeFromNow;

        t.selectInfluence = function (influence, scroll = true) {
            t.autoScroll = scroll;

            $rootScope.$broadcast('influenceSelected', {
                sourceScope: t,
                data: influence
            });
        }

        // We scroll in percent of the entire scrollable surface to have the scrollbar 
        // in sync with the timeline bar. The first selected element scrolls to top and
        // the last element scrolls to bottom.
        t.scrollIntoView = function (influence) {
            if (t.influences) {
                var scrollViewer = $element.find('.md-virtual-repeat-scroller');

                if (scrollViewer.length) {
                    var n = influence.id;
                    var N = t.items.length - 1;

                    if (N > 0 && n <= N) {
                        var h = scrollViewer[0].scrollHeight;
                        var top = (n / N) * h;

                        if (top >= 0) {
                            scrollViewer.scrollTop(top);
                        }
                    }
                }
            }
        }

        t.$onInit = function () {
            t.fileLoadedListener = $scope.$on('fileLoaded', function (e, data) {
                var items = [];

                var activity = null;

                for (var i = 0; i < data.influences.length; i++) {
                    var influence = data.influences[i];

                    if (!activity || influence.activity !== activity.uri) {
                        activity = data.activities[influence.activity];

                        items.push({
                            data: activity,
                            selected: false,
                            header: true
                        });
                    }

                    items.push({
                        id: i,
                        data: influence,
                        selected: false,
                        header: false
                    });
                }

                t.items = items;
                t.activities = data.activities;
                t.influences = data.influences;
            });

            t.influenceSelectedListener = $scope.$on('influenceSelected', function (e, args) {
                var influence = args.data;

                if (influence && args.sourceScope !== t) {
                    // Highlight the currently selected item.
                    $('.item.selected').each(function (i, element) {
                        $(element).removeClass('selected');
                    });

                    var id = t.influences.indexOf(influence);

                    if (id >= 0) {
                        $('.item[data-id="' + influence.id + '"]').each(function (i, element) {
                            $(element).addClass('selected');
                        });

                        // Scroll into view.
                        if (t.autoScroll) {
                            t.scrollIntoView(influence);
                        }
                    }

                    // Re-enable auto scroll if it was deactivated.
                    t.autoScroll = true;
                }
            });
        }

        t.$onDestroy = function () {
            t.fileLoadedListener();
            t.influenceSelectedListener();
        }
    }
})();