(function () {
	angular.module('explorerApp').directive('artPlaybackControl', function () {
		return {
			restrict: 'E',
			templateUrl: 'partials/directives/art-playback-control.html',
			controller: PlaybackControlDirectiveController,
			controllerAs: 't',
			scope: {
				influences: "@influences",
				selectedInfluence: "@selectedInfluence",
				activities: "@activities"
			}
		}
	});

	function PlaybackControlDirectiveController($rootScope, $scope, selectionService) {
		var t = this;

		t.rootScope = $rootScope;

		t.activities = [];
		t.influences = [];
		t.selectionService = selectionService;

		$scope.$parent.$watchCollection($scope.activities, function () {
			var activities = getValue($scope.$parent, $scope.activities);

			if (activities) {
				t.setActivities(activities);
			} else {
				console.error("Could not bind to ", $scope.activities, " from scope:", $scope.$parent);
			}
		});

		$scope.$parent.$watchCollection($scope.influences, function () {
			var influences = getValue($scope.$parent, $scope.influences);

			if (influences) {
				t.setInfluences(influences);
			} else {
				console.error("Could not bind to ", $scope.influences, " from scope:", $scope.$parent);
			}
		});

		var onSelectedItemChanged = function (influence) {
			if (influence) {
				t.setPosition(influence);
			}
		};

		selectionService.on('selectedItemChanged', onSelectedItemChanged);

		t.playloop = undefined;
		t.playing = false;
		t.dragging = false;
		t.selectedIndex = 0;

		var drag = function (e) {
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

				// Set the position of the newly selected influence;
				selectionService.selectedItem(influence);

				$rootScope.$broadcast('redraw');
			}
		};

		var dragStart = function (e) {
			t.dragging = true;

			selectionService.off('selectedItemChanged', onSelectedItemChanged);

			t.thumb.addClass('no-transition');
			t.trackIndicator.addClass('no-transition');

			var x = e.clientX - t.track.offset().left;

			if (x) {
				var c = Math.ceil(t.thumbKnob.outerWidth() / 2);

				t.trackIndicator.css('width', (x + c) + 'px');
			}
		};

		var dragStop = function (e) {
			var x = e.clientX - t.track.offset().left;

			if (x) {
				var c = Math.ceil(t.thumbKnob.outerWidth() / 2);

				t.trackIndicator.css('width', (x + c) + 'px');
			}

			selectionService.on('selectedItemChanged', onSelectedItemChanged);

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

			$rootScope.$broadcast('redraw');

			t.dragging = false;
		};

		var trackMouseMove = function (e) {
			if (!t.dragging) {
				// e.offsetX does for some reason deliver unsteady values.
				var x = e.clientX - t.track.offset().left;

				if (x > 0 && x <= t.track.innerWidth()) {
					var influence = t.getInfluence(x);

					t.updatePositionLabels(influence);

					t.trackPreview.css('visibility', 'visible');
					t.trackPreview.css('width', x + "px");
				}
			}
		};

		var trackMouseEnter = function (e) {
			if (!t.dragging) {
				t.trackPreview.css('visibility', 'visible');
			}
		};

		var trackMouseOut = function (e) {
			if (!t.dragging) {
				t.trackPreview.css('visibility', 'collapse');
			}
		};

		var trackClick = function (e) {
			if (!t.dragging) {
				// e.offsetX does for some reason deliver unsteady values.
				var x = e.clientX - t.track.offset().left;

				var influence = t.getInfluence(x);

				var i = t.influences.indexOf(influence);

				if (-1 < i && i !== t.selectedIndex) {
					selectionService.selectedItem(influence);

					$rootScope.$broadcast('redraw');
				}
			}
		};

		var trackResize = function () {
			t.thumb.addClass('no-transition');
			t.trackIndicator.addClass('no-transition');

			t.setPosition(selectionService.selectedItem());

			t.thumb.removeClass('no-transition');
			t.trackIndicator.removeClass('no-transition');
		};

		var thumbMouseEnter = function () {
			t.trackPreview.css('visibility', 'collapse');
		}

		var thumbMouseOut = function () {
			t.trackPreview.css('visibility', 'visible');
		};

		t.spacing = {
			horizontal: 10,
			vertical: 5
		};
		t.padding = {
			horizontal: 10,
			vertical: 0
		};

		t.color = "#ffffff";
		t.timeRange = new Array(2);
		t.trackColumn = $($(".track-col")[0]);
		t.trackColumn.mouseenter(trackMouseEnter);
		t.trackColumn.mousemove(trackMouseMove);
		t.trackColumn.mouseout(trackMouseOut);
		t.trackColumn.click(trackClick);
		t.trackContainer = $($(".track-container")[0]);
		t.track = $($(".track")[0]);
		t.trackPreview = $($(".track-preview")[0]);
		t.trackIndicator = $($(".track-indicator")[0]);
		t.thumb = $($(".thumb")[0]);
		t.thumbKnob = $($(".thumb-knob")[0]);
		t.thumb.mouseenter(thumbMouseEnter);
		t.thumb.mouseout(thumbMouseOut);
		t.thumb.draggable({
			axis: 'x',
			containment: '.thumb-container',
			scroll: false,
			drag: drag,
			start: dragStart,
			stop: dragStop
		});
		t.positionLabel = $($(".position label")[0]);
		t.durationLabel = $($(".duration label")[0]);
		t.activitiesContainer = $($(".activities-container")[0]);
		t.commentsContainer = $($(".comments-container")[0]);

		$(window).resize(function () {
			t.trackPreview.css('visibility', 'collapse');
			trackResize();
		});
	};

	PlaybackControlDirectiveController.prototype.setActivities = function (data) {
		var t = this;

		if (data && data.length > 0) {
			t.activities = [];
			t.totalTime = 0;

			for (var i = 0; i < data.length; i++) {
				var a = new Activity(data[i], t.totalTime);

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
			var container = $($('.timeline')[0]);

			if (container) {
				t.xScale = d3.scaleTime().domain(t.timeRange).range([0, container.innerWidth()]).clamp(true);
			}

			// Set the current position text.
			var time = moment.duration(t.totalTime, "milliseconds").format("hh:mm:ss", {
				trim: false
			});

			t.durationLabel.text(time);
			t.positionLabel.text(time);
		} else {
			data = undefined;
			t.xScale = undefined;
		}
	};

	PlaybackControlDirectiveController.prototype.setInfluences = function (influences) {
		var t = this;

		t.influences = influences;
	};

	PlaybackControlDirectiveController.prototype.getInfluence = function (x) {
		var t = this;

		if (x > 0 && x <= t.track.innerWidth()) {
			var y = t.track.innerWidth() / t.influences.length;
			var i = t.influences.length - Math.ceil(x / y);

			if (i > -1 && i != t.selectedIndex) {
				var influence = t.influences[i];

				return influence;
			}
		}
	};

	PlaybackControlDirectiveController.prototype.setPosition = function (influence) {
		var t = this;

		if (t.dragging) {
			return;
		}

		if (t.xScale) {
			t.selectedIndex = t.influences.indexOf(influence);

			t.updatePositionLabels(influence);
			t.updateTrackPreview(influence);
		} else {
			console.log('Warning: Timline activities are not initialized:', t.activities);

			t.trackIndicator.css('background', t.getColor(influence));
			t.trackIndicator.css('width', 0);
		}
	};

	PlaybackControlDirectiveController.prototype.updatePositionLabels = function (influence) {
		var t = this;

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
	};

	PlaybackControlDirectiveController.prototype.getTrackPosition = function (influence) {
		var t = this;
		var i = t.influences.indexOf(influence);

		if (i > -1) {
			// We subtract 1 from the number of influences to receive values 
			// between 0 and 1 when calculating the progress percentage.
			var I = t.influences.length - 1;

			var x = Math.ceil(t.track.innerWidth() * (I - i) / I);

			return x;
		}

		return 0;
	};

	PlaybackControlDirectiveController.prototype.updateTrackPreview = function (influence) {
		var t = this;

		var x = t.getTrackPosition(influence);

		t.thumbKnob.css('background', t.getColor(influence));
		t.thumb.css('left', (x - Math.ceil(t.thumbKnob.outerWidth() / 2)) + 'px');

		t.trackIndicator.css('background', t.getColor(influence));
		t.trackIndicator.css('width', x + 'px');
	};

	PlaybackControlDirectiveController.prototype.getColor = function (influence) {
		var t = this;

		if (!influence || influence.agentColor === '#FF0000') {
			return t.lastColor;
		}

		t.lastColor = influence.agentColor;

		return t.lastColor;
	};

	PlaybackControlDirectiveController.prototype.play = function () {
		var t = this;
		var end = t.influences.indexOf(t.selectionService.selectedItem()) === 0;

		if (!t.playloop && !end) {
			t.playloop = setInterval(function () {
				t.skipNext();
			}, 500);
			t.playing = true;
		}
	};

	PlaybackControlDirectiveController.prototype.pause = function () {
		var t = this;

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

	PlaybackControlDirectiveController.prototype.togglePlay = function () {
		var t = this;

		if (t.playloop) {
			t.pause();
		} else {
			t.play();
		}
	};

	PlaybackControlDirectiveController.prototype.skipPrev = function () {
		var t = this;

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

	PlaybackControlDirectiveController.prototype.skipNext = function () {
		var t = this;

		t.selectionService.selectPrev();
		t.rootScope.$broadcast('redraw');

		if (t.playloop) {
			var i = t.selectionService.selectedIndex();

			if (i === 0) {
				t.pause();
			}
		}
	};
})();