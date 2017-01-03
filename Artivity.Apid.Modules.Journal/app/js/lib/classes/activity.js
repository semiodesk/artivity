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