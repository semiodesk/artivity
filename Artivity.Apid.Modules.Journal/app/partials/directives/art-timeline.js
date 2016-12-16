angular.module('explorerApp').directive('artTimeline', function () {
	return {
		scope: true,
		template: '\
		<div class="timeline">\
			<div class="timeline-control"> \
				<div class="position"><label></label></div> \
				<div class="duration"><label></label></div> \
				<div class="track-col"> \
					<div class="thumb-container"></div> \
					<div class="track-container"> \
						<div class="track"></div> \
						<div class="track-preview"></div> \
						<div class="track-indicator"></div> \
						<div class="thumb draggable"><div class="thumb-knob"></div></div> \
					</div> \
					<div class="comments"></div> \
					<div class="activities"></div> \
				</div> \
			</div> \
		</div>',
		link: function (scope, element, attributes) {
			var t = scope.$parent.t; // Bad hack. Needs to be fixed when refactoring the control.
			
			var timeline = new TimelineControl(element);

			timeline.setActivities(getValue(scope, attributes.artActivitiesSrc));
			timeline.setInfluences(getValue(scope, attributes.artInfluencesSrc));

			timeline.selectedInfluenceChanged = function (influence) {
				t.previewInfluence(influence);
			};

			t.scope.$watchCollection(attributes.artActivitiesSrc, function () {
				timeline.setActivities(getValue(scope, attributes.artActivitiesSrc));
			});

			t.scope.$watchCollection(attributes.artInfluencesSrc, function () {
				timeline.setInfluences(getValue(scope, attributes.artInfluencesSrc));
			});

			// TODO: Implement using selectionService.
			t.onSelectedInfluenceChanged = function(influence) {
				if (influence !== undefined) {
					timeline.setPosition(influence);
				}
			}
		}
	}
});

function Activity(activity, timeOffset) {
	var t = this;

	t.color = activity.agentColor;
	t.lastColor = t.color;

	t.startTime = new Date(activity.startTime);
	t.endTime = new Date(activity.endTime === undefined ? activity.maxTime : activity.endTime);

	t.timeRange = {};
	t.timeRange.start = timeOffset;
	t.timeRange.length = t.endTime - t.startTime;
	t.timeRange.end = t.timeRange.start + t.timeRange.length;
};

Activity.prototype.getOffset = function (timestamp) {
	var t = this;
	var time = new Date(timestamp);

	return (time - t.startTime) + t.timeRange.start;
};

function TimelineControl(element) {
	var t = this;

	t.dragging = false;
	t.selectedIndex = 0;

	var drag = function (e) {
		var x = parseInt($(e.target).css('left')) + $(e.target).outerWidth() / 2;

		if (x) {
			//$(e.target).css('left', x);
			
			var c = Math.ceil(t.thumbKnob.outerWidth() / 2);

			t.trackIndicator.css('width', (x + c) + 'px');
			t.trackPreview.css('visibility', 'collapse');

			var y = t.track.innerWidth() / t.influences.length;
			var i = t.influences.length - Math.ceil(x / y);

			if (i > -1 && i !== t.selectedIndex) {
				t.selectedIndex = i;

				var influence = t.influences[t.selectedIndex];

				// Snap the thumb to the position of the currently selected influence.
				t.updatePositionLabels(influence);
				t.updateTrackPreview(influence);

				if (t.selectedInfluenceChanged) {
					t.selectedInfluenceChanged(t.influences[i]);
				}
			}
		}
	};

	var dragStart = function (e) {
		t.dragging = true;

		t.thumb.addClass('no-transition');
		t.trackIndicator.addClass('no-transition');
		t.trackPreview.css('visibility', 'collapse');

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

		t.thumb.removeClass('no-transition');
		t.trackIndicator.removeClass('no-transition');
		t.trackPreview.css('visibility', 'visible');

		var influence = t.influences[t.selectedIndex];

		// Snap the thumb to the position of the currently selected influence.
		t.updatePositionLabels(influence);
		t.updateTrackPreview(influence);

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

			if (i > -1 && i !== t.selectedIndex) {
				t.setPosition(influence);

				if (t.selectedInfluenceChanged) {
					t.selectedInfluenceChanged(influence);
				}
			}
		}
	};

	var trackResize = function() {		
		t.thumb.addClass('no-transition');
		t.trackIndicator.addClass('no-transition');
		
		t.setPosition(t.influences[t.selectedIndex]);
		
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

	t.control = $(element);
	t.trackColumn = $(t.control.find(".track-col")[0]);
	t.trackColumn.mouseenter(trackMouseEnter);
	t.trackColumn.mousemove(trackMouseMove);
	t.trackColumn.mouseout(trackMouseOut);
	t.trackColumn.click(trackClick);
	t.trackContainer = $(t.control.find(".track-container")[0]);
	t.track = $(t.control.find(".track")[0]);
	t.trackPreview = $(t.control.find(".track-preview")[0]);
	t.trackIndicator = $(t.control.find(".track-indicator")[0]);
	t.thumb = $(t.control.find(".thumb")[0]);
	t.thumbKnob = $(t.control.find(".thumb-knob")[0]);
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
	t.positionLabel = $(t.control.find(".position label")[0]);
	t.durationLabel = $(t.control.find(".duration label")[0]);
	t.activitiesContainer = $(t.control.find(".activities-container")[0]);
	t.commentsContainer = $(t.control.find(".comments-container")[0]);
	
	$(window).resize(trackResize);
};

TimelineControl.prototype.selectedInfluenceChanged = undefined;

TimelineControl.prototype.setActivities = function (data) {
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
		t.xScale = d3.scaleTime().domain(t.timeRange).range([0, t.control.innerWidth()]).clamp(true);

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

TimelineControl.prototype.setInfluences = function (influences) {
	var t = this;

	t.influences = influences;
};

TimelineControl.prototype.getInfluence = function (x) {
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

TimelineControl.prototype.setPosition = function (influence) {
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

TimelineControl.prototype.updatePositionLabels = function (influence) {
	var t = this;

	if (influence) {
		var time = new Date(influence.time);
		var position = 0;
		
		for (var i = 0; i < t.activities.length; i++) {
			var a = t.activities[i];
			
			if(a.endTime >= time && time >= a.startTime) {
				position += (time - a.startTime);
			} else if(time >= a.startTime) {
				position += a.timeRange.length;
			}
		}

		t.positionLabel.text(moment.duration(position, 'milliseconds').format('hh:mm:ss', {
			trim: false
		}));

		/*
		var duration = t.lastActivity.endTime - t.firstActivity.startTime;

		t.durationLabel.text(moment.duration(duration, 'milliseconds').format('hh:mm:ss', {
			trim: false
		}));
		*/
	}
};

TimelineControl.prototype.getTrackPosition = function (influence) {
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

TimelineControl.prototype.updateTrackPreview = function (influence) {
	var t = this;

	var x = t.getTrackPosition(influence);

	t.thumbKnob.css('background', t.getColor(influence));
	t.thumb.css('left', (x - Math.ceil(t.thumbKnob.outerWidth() / 2)) + 'px');

	t.trackIndicator.css('background', t.getColor(influence));
	t.trackIndicator.css('width', x + 'px');
};

TimelineControl.prototype.getColor = function (influence) {
	var t = this;

	if (influence.agentColor === '#FF0000') {
		return t.lastColor;
	}

	t.lastColor = influence.agentColor;

	return t.lastColor;
};