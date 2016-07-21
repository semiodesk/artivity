function TimelineControl(element) {
	var t = this;

	t.svg = d3.select(element.find("svg")[0]);;

	t.timeRange = new Array(2);

	t.track = t.svg.append("rect")
		.attr("class", "timeline-track")
		.attr("y", 0)
		.attr("x", 0)
		.attr("height", 2)
		.attr("width", 200);

	t.trackIndicator = t.svg.append("rect")
		.attr("class", "timeline-track-indicator")
		.attr("y", 0)
		.attr("x", 0)
		.attr("height", 2)
		.attr("width", 200);
};

TimelineControl.prototype.setData = function (data) {
	var t = this;

	t.data = data;

	if (t.data && t.data.length > 0) {
		var first = t.data[0];
		var last = t.data[t.data.length - 1];

		t.timeRange[0] = new Date(first.startTime);
		t.timeRange[1] = new Date(last.endTime == undefined ? last.maxTime : last.endTime);
		
		// Update x time scale to current range and control size.
		t.xScale = d3.scaleTime().domain(t.timeRange).range([0, t.getWidth() - 1]).clamp(true);
	} else {
		t.data = undefined;
		t.xScale = undefined;
	}
};

TimelineControl.prototype.getWidth = function () {
	return parseInt(this.svg.style("width"));
}

TimelineControl.prototype.setWidth = function (width) {
	var t = this;

	t.svg.style("width", width);
	t.track.attr("width", width);
	t.trackIndicator.attr("x", width - t.trackIndicator.attr("width"));

	// Update the x time scale to the new width.
	if (width) {
		t.setData(t.data);
	}
};

TimelineControl.prototype.getHeight = function () {
	return parseInt(this.svg.style("height"));
}

TimelineControl.prototype.setHeight = function (height) {
	var t = this;
	var y = Math.floor(height / 2);
	
	t.svg.style("height", height);
	t.track.attr("y", y);
	t.trackIndicator.attr("y", y);
};

TimelineControl.prototype.setPosition = function (pos) {
	var t = this;
	var time = new Date(pos);
	
	if (t.xScale) {
		var trackWidth = t.track.attr("width");
		var indicatorWidth = t.xScale(time);
		
		t.trackIndicator.transition().attr("width", indicatorWidth).attr("x", trackWidth - indicatorWidth);
	} else {
		console.log("Warning: Timline data not initialized:", t.data);
		
		t.trackIndicator.transition().attr("width", 0);
	}
};