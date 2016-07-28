function Activity(activity, timeOffset) {
	var t = this;

	t.color = activity.agentColor;
	t.startTime = new Date(activity.startTime);
	t.endTime = new Date(activity.endTime == undefined ? activity.maxTime : activity.endTime);

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
		var x = parseInt($(e.target).css('left'));

		if (x) {
			var c = Math.ceil(parseInt(t.thumb.css('width')) / 2);

			t.trackIndicator.css('width', (x + c) + 'px');

			var y = t.track.innerWidth() / t.influences.length;
			var i = t.influences.length - Math.ceil(x / y);

			if (i > -1 && i != t.selectedIndex) {
				t.selectedIndex = i;

				var influence = t.influences[t.selectedIndex];

				// Snap the thumb to the position of the currently selected influence.
				t.updatePositionLabels(influence);
				t.updatePositionIndicator(influence);

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
		
		var x = parseInt($(e.target).css('left'));

		if (x) {
			var c = Math.ceil(parseInt(t.thumb.css('width')) / 2);

			t.trackIndicator.css('width', (x + c) + 'px');
		}
	};

	var dragStop = function (e) {
		var x = parseInt($(e.target).css('left'));

		if (x) {
			var c = Math.ceil(parseInt(t.thumb.css('width')) / 2);

			t.trackIndicator.css('width', (x + c) + 'px');
		}

		t.thumb.removeClass('no-transition');
		t.trackIndicator.removeClass('no-transition');
		
		var influence = t.influences[t.selectedIndex];

		// Snap the thumb to the position of the currently selected influence.
		t.updatePositionLabels(influence);
		t.updatePositionIndicator(influence);

		t.dragging = false;
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
	t.trackContainer = $(t.control.find(".track-container")[0]);
	t.track = $(t.control.find(".track")[0]);
	t.trackIndicator = $(t.control.find(".track-indicator")[0]);
	t.thumb = $(t.control.find(".thumb")[0]);
	t.thumb.draggable({
		axis: 'x',
		containment: '.track-container',
		scroll: false,
		drag: drag,
		start: dragStart,
		stop: dragStop
	});
	t.positionLabel = $(t.control.find(".position label")[0]);
	t.durationLabel = $(t.control.find(".duration label")[0]);
	t.activitiesContainer = $(t.control.find(".activities-container")[0]);
	t.commentsContainer = $(t.control.find(".comments-container")[0]);
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

		t.timeRange[0] = t.firstActivity.startTime;
		t.timeRange[1] = t.lastActivity.endTime;

		// Update x time scale to current range and control size.
		t.xScale = d3.scaleTime().domain(t.timeRange).range([0, t.control.innerWidth()]).clamp(true);

		// Set the current position text.
		var time = moment.duration(t.totalTime, "milliseconds").format("hh:mm:ss", {
			trim: false
		});

		t.durationLabel.text(time);
		t.positionLabel.text(time);

		// TODO: Could be combined with previous loop.
		t.activitiesContainer.empty();

		for (var i = 0; i < t.activities.length - 1; i++) {
			var x = (Math.ceil(t.xScale(t.activities[i].startTime)) / t.totalTime) * 100;

			var d = $('<div class="activity"></div>');
			d.css('left', x + '%');

			t.activitiesContainer.append(d);
		}
	} else {
		data = undefined;
		t.xScale = undefined;
	}
};

TimelineControl.prototype.setInfluences = function (influences) {
	var t = this;

	t.influences = influences;
};

TimelineControl.prototype.setPosition = function (influence) {
	var t = this;

	if (t.dragging) {
		return;
	}

	if (t.xScale) {
		t.selectedIndex = t.influences.indexOf(influence);

		t.updatePositionLabels(influence);
		t.updatePositionIndicator(influence);
	} else {
		console.log('Warning: Timline activities are not initialized:', t.activities);

		t.trackIndicator.css('background', influence.agentColor);
		t.trackIndicator.css('width', 0);
	}
};

TimelineControl.prototype.updatePositionLabels = function (influence) {
	var t = this;

	if (influence) {
		var position = new Date(influence.time) - t.firstActivity.startTime;

		t.positionLabel.text(moment.duration(position, 'milliseconds').format('hh:mm:ss', {
			trim: false
		}));

		var duration = t.lastActivity.endTime - t.firstActivity.startTime;

		t.durationLabel.text(moment.duration(duration, 'milliseconds').format('hh:mm:ss', {
			trim: false
		}));
	}
};

TimelineControl.prototype.updatePositionIndicator = function (influence) {
	var t = this;

	if (t.selectedIndex > -1) {
		// We subtract 1 from the number of influences to receive values 
		// between 0 and 1 when calculating the progress percentage.
		var I = t.influences.length - 1;

		var x = Math.ceil(((I - t.selectedIndex) / I) * 100);

		t.thumb.css('background', influence.agentColor);
		t.thumb.css('left', x + '%');

		t.trackIndicator.css('background', influence.agentColor);
		t.trackIndicator.css('width', x + '%');
	}
};