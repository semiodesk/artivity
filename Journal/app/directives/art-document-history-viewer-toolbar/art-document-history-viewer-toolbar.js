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

            if (!t.playloop && t.selectedIndex < t.maxIndex) {
                t.playloop = setInterval(function () {
                    if (t.selectedIndex < t.maxIndex) {
                        $scope.$apply(function () {
                            t.skipNext();
                        });
                    } else {
                        $scope.$apply(function () {
                            t.pause();
                        });
                    }
                }, 500);
                t.playing = true;
            }
        };

        t.pause = function () {
            if (t.playloop) {
                clearInterval(t.playloop);

                t.playloop = undefined;
                t.playing = false;
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
            if (t.selectedIndex > t.minIndex) {
                t.selectedIndex--;
            }
        };

        t.skipNext = function () {
            if (t.selectedIndex < t.maxIndex) {
                t.selectedIndex++;
            }
        };

        t.getFormattedDuration = function (index) {
            var result = undefined;

            // The index is inverted because the latest / right most influence is the one with index 0.
            var influence = t.influences[t.maxIndex - index];

            for (var i = 0; i < t.activities.length; i++) {
                var activity = t.activities[i];

                if (activity.uri === influence.activity) {
                    result = new Date(influence.time) - new Date(activity.startTime);
                } else if (result !== undefined) {
                    result += new Date(activity.maxTime) - new Date(activity.startTime);
                }
            }

            if (result === undefined) {
                result = 0;
            }

            return moment.duration(result, "milliseconds").format("hh:mm:ss", {
                trim: false
            });
        }

        t.$onInit = function () {
            t.activities = [];
            t.influences = [];
            t.minIndex = 0;
            t.maxIndex = 0;
            t.selectedIndex = -1;

            t.fileLoadedListener = $scope.$on('fileLoaded', function (e, data) {
                t.activities = [];
                t.influences = data.influences;
                t.minIndex = 0;
                t.maxIndex = data.influences.length - 1;
                t.selectedIndex = t.maxIndex;

                for (var uri in data.activities) {
                    var activity = data.activities[uri];

                    t.activities.push(activity);
                }

                t.activities.sort(function (a, b) {
                    return b.StartTime < a.StartTime;
                });
            });

            t.influenceSelectedListener = $scope.$on('influenceSelected', function (e, args) {
                var influence = args.data;

                if (influence && args.sourceScope !== t) {
                    var i = t.influences.indexOf(influence);

                    if (i >= 0) {
                        // The index is inverted because the latest / right most influence is the one with index 0.
                        t.selectedIndex = t.maxIndex - i;
                    }
                }
            });

            $scope.$watch('t.selectedIndex', function (index) {
                var i = t.selectedIndex;
                var n = t.influences.length - 1;

                if (0 <= i && i < t.influences.length) {
                    // The index is inverted because the latest / right most influence is the one with index 0.
                    var influence = t.influences[n - i];

                    $rootScope.$broadcast('influenceSelected', {
                        sourceScope: t,
                        data: influence
                    });
                }
            });
        }

        t.$onDestroy = function () {
            if (t.fileLoadedListener) {
                t.fileLoadedListener();
            }

            if (t.influenceSelectedListener) {
                t.influenceSelectedListener();
            }
        }
    };
})();