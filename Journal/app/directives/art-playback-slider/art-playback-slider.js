(function () {
    angular.module('app').directive('artPlaybackSlider', PlaybackSliderDirective);

    function PlaybackSliderDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-playback-slider/art-playback-slider.html',
            controller: PlaybackSliderDirectiveController,
            controllerAs: 't',
            bindToController: true,
            scope: {}
        }
    };

    PlaybackSliderDirectiveController.$inject = ['$rootScope', '$scope', '$element'];

    function PlaybackSliderDirectiveController($rootScope, $scope, $element) {
        var t = this;

        // DATA
        t.activities = {};
        t.influences = [];
        t.influence = null;

        t.fileLoadedListener = null;

        t.onFileLoaded = function (e, data) {
            t.activities = data.activities;
            t.influences = data.influences;
            t.influence = t.influences[0];

            t.totalTime = 0;

            if (t.activities) {
                for (var i = 0; i < t.activities.length; i++) {
                    var a = new Activity(t.activities[i], t.totalTime);

                    t.activities.push(a);
                    t.totalTime += a.timeRange.length;
                    t.color = a.color;
                }

                // The activities are ordered backward in time.
                t.firstActivity = t.activities[t.activities.length - 1];
                t.lastActivity = t.activities[0];

                t.timeRange[0] = 0;
                t.timeRange[1] = t.totalTime;

                // Update x time scale to current range and control size.
                var container = $element.find('.timeline');

                if (container) {
                    t.xScale = d3.scaleTime().domain(t.timeRange).range([0, container.innerWidth()]).clamp(true);
                }

                // Set the current position text.
                var time = moment.duration(t.totalTime, "milliseconds").format("hh:mm:ss", {
                    trim: false
                });

                t.durationLabel.text(time);
                t.positionLabel.text(time);
            }
        }

        t.influenceSelectedListener = null;

        t.onInfluenceSelected = function (e, influence) {
            t.influence = influence;

            t.setPositionFromInfluence(influence);
        }

        // DRAG & DROP
        t.dragging = false;

        t.onDrag = function (e) {
            // It can happen that the cursor re-enters the track while dragging. This would
            // enable the position preview and cause flickering. Hiding the preview on drag
            // prevents the flickering from happening.
            t.trackPreview.css('visibility', 'collapse');

            var trackX = t.track.offset().left;
            var maxX = t.track.outerWidth() + t.thumbKnob.outerWidth();

            var x = e.clientX - trackX;

            if (x <= maxX) {
                t.trackIndicator.css('width', x + 'px');
            }

            var y = t.track.innerWidth() / t.influences.length;
            var i = t.influences.length - Math.floor(x / y);

            if (-1 < i && i < t.influences.length) {
                t.selectedIndex = i;

                var influence = t.influences[t.selectedIndex];

                // Snap the thumb to the position of the currently selected influence.
                t.updatePositionLabels(influence);

                $rootScope.$broadcast('influenceSelected', influence);
            }
        }

        t.onDragStart = function (e) {
            t.dragging = true;

            // Stop listening to influence selected events.
            t.influenceSelectedListener();

            t.thumb.addClass('no-transition');
            t.trackIndicator.addClass('no-transition');

            var x = e.clientX - t.track.offset().left;

            if (x) {
                var c = Math.ceil(t.thumbKnob.outerWidth() / 2);

                t.trackIndicator.css('width', (x + c) + 'px');
            }
        }

        t.onDragStop = function (e) {
            var x = e.clientX - t.track.offset().left;

            if (x) {
                var c = Math.ceil(t.thumbKnob.outerWidth() / 2);

                t.trackIndicator.css('width', (x + c) + 'px');
            }

            t.thumb.removeClass('no-transition');
            t.trackIndicator.removeClass('no-transition');

            var maxX = t.track.outerWidth() + t.thumbKnob.outerWidth();

            if (e.clientX <= maxX) {
                t.trackPreview.css('visibility', 'visible');
            }

            var influence = t.influences[t.selectedIndex];

            // Snap the thumb to the position of the currently selected influence.
            t.updatePositionLabels(influence);
            t.updateTrackPreview(influence);

            $rootScope.$broadcast('influenceSelected', influence);

            t.dragging = false;

            // Resume listening to influence selected events.
            t.influenceSelectedListener = $scope.$on('influenceSelected', t.onInfluenceSelected);
        }

        // TRACK
        t.onTrackMouseMove = function (e) {
            if (!t.dragging) {
                // e.offsetX does for some reason deliver unsteady values.
                var x = e.clientX - t.track.offset().left;

                if (x > 0 && x <= t.track.innerWidth()) {
                    var influence = t.getInfluenceFromPosition(x);

                    t.updatePositionLabels(influence);

                    t.trackPreview.css('visibility', 'visible');
                    t.trackPreview.css('width', x + "px");
                }
            }
        }

        t.onTrackMouseEnter = function (e) {
            if (!t.dragging) {
                t.trackPreview.css('visibility', 'visible');
            }
        }

        t.onTrackMouseOut = function (e) {
            if (!t.dragging) {
                t.trackPreview.css('visibility', 'collapse');
            }
        }

        t.onTrackClick = function (e) {
            if (!t.dragging) {
                // e.offsetX does for some reason deliver unsteady values.
                var x = e.clientX - t.track.offset().left;

                var influence = t.getInfluenceFromPosition(x);

                var i = t.influences.indexOf(influence);

                if (-1 < i && i !== t.selectedIndex) {
                    $rootScope.$broadcast('influenceSelected', influence);
                }
            }
        }

        t.onTrackResize = function () {
            t.thumb.addClass('no-transition');
            t.trackIndicator.addClass('no-transition');

            t.setPositionFromInfluence(t.influence);

            t.thumb.removeClass('no-transition');
            t.trackIndicator.removeClass('no-transition');
        }

        t.onThumbMouseEnter = function () {
            t.trackPreview.css('visibility', 'collapse');
        }

        t.onThumbMouseOut = function () {
            t.trackPreview.css('visibility', 'visible');
        }

        t.getInfluenceFromPosition = function (x) {
            if (x > 0 && x <= t.track.innerWidth()) {
                var y = t.track.innerWidth() / t.influences.length;
                var i = t.influences.length - Math.ceil(x / y);

                if (i > -1 && i != t.selectedIndex) {
                    var influence = t.influences[i];

                    return influence;
                }
            }
        }

        t.getPositionFromInfluence = function (influence) {
            var i = t.influences.indexOf(influence);

            if (i > -1) {
                // We subtract 1 from the number of influences to receive values 
                // between 0 and 1 when calculating the progress percentage.
                var I = t.influences.length - 1;

                var x = Math.ceil(t.track.innerWidth() * (I - i) / I);

                return x;
            }

            return 0;
        }

        t.setPositionFromInfluence = function (influence) {
            if (t.dragging) {
                return;
            }

            if (t.xScale) {
                t.selectedIndex = t.influences.indexOf(influence);

                t.updatePositionLabels(influence);
                t.updateTrackPreview(influence);
            } else {
                console.log('Warning: Timline activities are not initialized:', t.activities);

                t.trackIndicator.css('background', t.getColorFromInfluence(influence));
                t.trackIndicator.css('width', 0);
            }
        }

        t.updateTrackPreview = function (influence) {
            var x = t.getPositionFromInfluence(influence);

            t.thumbKnob.css('background', t.getColorFromInfluence(influence));
            t.thumb.css('left', (x - Math.ceil(t.thumbKnob.outerWidth() / 2)) + 'px');

            t.trackIndicator.css('background', t.getColorFromInfluence(influence));
            t.trackIndicator.css('width', x + 'px');
        }

        t.getColorFromInfluence = function (influence) {
            if (!influence || influence.agentColor === '#FF0000') {
                return t.lastColor;
            }

            t.lastColor = influence.agentColor;

            return t.lastColor;
        }

        // LABELS
        t.updatePositionLabels = function (influence) {
            if (influence) {
                var time = new Date(influence.time);
                var position = 0;

                for (var i = 0; i < t.activities.length; i++) {
                    var a = t.activities[i];

                    if (a.endTime >= time && time >= a.startTime) {
                        position += (time - a.startTime);
                    } else if (time >= a.startTime) {
                        position += a.timeRange.length;
                    }
                }

                t.positionLabel.text(moment.duration(position, 'milliseconds').format('hh:mm:ss', {
                    trim: false
                }));
            }
        }

        // INIT
        t.$onInit = function () {
            t.timeRange = new Array(2);

            $scope.$on('fileLoaded', function (e, data) {
                t.activities = data.activities;
                t.influences = data.influences;
            });
        }

        t.$postLink = function () {
            t.trackColumn = $element.find(".track-col");
            t.trackColumn.mouseenter(t.onTrackMouseEnter);
            t.trackColumn.mousemove(t.onTrackMouseMove);
            t.trackColumn.mouseout(t.onTrackMouseOut);
            t.trackColumn.click(t.onTrackClick);

            t.track = $element.find(".track");
            t.trackContainer = $element.find(".track-container");
            t.trackPreview = $element.find(".track-preview");
            t.trackIndicator = $element.find(".track-indicator");

            t.thumb = $element.find(".thumb");
            t.thumbKnob = $element.find(".thumb-knob");
            t.thumb.mouseenter(t.onThumbMouseEnter);
            t.thumb.mouseout(t.onThumbMouseOut);
            t.thumb.draggable({
                axis: 'x',
                containment: '.thumb-container',
                scroll: false,
                drag: t.onDrag,
                start: t.onDragStart,
                stop: t.onDragStop
            });

            t.positionLabel = $($(".position label")[0]);
            t.durationLabel = $($(".duration label")[0]);

            $(window).resize(function () {
                t.trackPreview.css('visibility', 'collapse');
                t.onTrackResize();
            });

            // Register the data change event listeners.
            t.fileLoadedListener = $scope.$on('fileLoaded', t.onFileLoaded);
            t.influenceSelectedListener = $scope.$on('influenceSelected', t.onInfluenceSelected);
        }

        t.$onDestroy = function () {
            // Unregister the data change event listeners.
            t.fileLoadedListener();
            t.influenceSelectedListener();
        }
    }
})();