/**
 * @author Dimitry Kudrayvtsev
 * @version 2.0
 */

d3.gantt = function () {
	// CANVAS
	var _svg = undefined;

	var _size = {
		width: 500,
		minWidth: 500,
		height: 30,
		minHeight: 30
	};

	var _autoSize = true;

	// TASKS
	var _tasks = [];

	// TIME RANGE
	var _timeRangeStart = d3.time.day.offset(new Date(), -6);
	var _timeRangeEnd = d3.time.hour.offset(new Date(), +6);

	var _fitTimeRange = true;

	// AXIS
	var _tickIndicator = undefined;

	var _tickFormat = "%H:%M";

	var _x = d3.time.scale();

	var _xAxis = d3.svg.axis()
		.orient("bottom")
		.tickSubdivide(true)
		.tickSize(8)
		.tickPadding(8);

	var _y = d3.scale.ordinal();

	var _yAxis = d3.svg.axis()
		.orient("left")
		.tickSize(0);

	var _labels = [];

	var _labelAreaWidth = 120;

	var _barHeight = 30;

	function gantt() {
	}

	var keyFunction = function (task) {
		return task.name + task.startTime + task.endTime;
	};

	var rectTransform = function (task) {
		return "translate(" + _x(new Date(task.startTime)) + "," + _y(task.name) + ")";
	};

	var updateAxes = function () {
		// Update the time range, if enabled.
		if (_fitTimeRange) {
			if (_tasks === undefined || _tasks.length === 0) {
				// Set a time range of 12h around now.
				_timeRangeStart = d3.time.day.offset(new Date(), -6);
				_timeRangeEnd = d3.time.hour.offset(new Date(), +6);
			} else {
				// Determine the min and max date time values of the given tasks.
				_timeRangeStart = new Date(_tasks[0].startTime);
				_timeRangeEnd = new Date(_tasks[0].endTime);

				for(var i = 1; i < _tasks.length; i++) {
					var t = _tasks[i];

					var startTime = new Date(t.startTime);
					var endTime = new Date(t.endTime);

					if(startTime < _timeRangeStart){ _timeRangeStart = startTime; }
					if(endTime > _timeRangeEnd) { _timeRangeEnd = endTime; }
				}
			}
		}

		if(_autoSize) {
			_size.height = _labels.length * _barHeight + _barHeight;
		}

		var width = Math.max(_size.width, _size.minWidth);
		var height = Math.max(_size.height, _size.minHeight);

		// Update x time scale to current range and control size.
		_x = d3.time.scale().domain([_timeRangeStart, _timeRangeEnd]).range([0, width - _labelAreaWidth - 1]).clamp(true);

		// Update the scale and tick format of the x-axis.
		_xAxis = d3.svg.axis()
			.orient("bottom")
			.tickSubdivide(true)
			.tickSize(8)
			.tickPadding(8)
			.scale(_x)
			.tickFormat(d3.time.format(_tickFormat));

		// Update the y values to current tasks and control size.
		_y = d3.scale.ordinal().domain(_labels).rangeRoundBands([0, _labels.length * _barHeight], .1);

		// Update the scale of the y-axis.
		_yAxis = d3.svg.axis()
			.orient("left")
			.tickSize(0)
			.scale(_y);
	};

	var updatePosition = function(time) {
		var rect = _svg.select(".overlay").select(".indicator");

		rect.transition()
			.attr("transform", "translate(" + _x(new Date(time)) + ", 0)");
	};

	gantt.init = function (element, tasks) {
		// Set the tasks.
		gantt.tasks(tasks);

		// Find the svg canvas.
		_svg = d3.select(element.find("svg")[0]);

		updateAxes();

		// Initialize the canvas.
		var body = _svg.append("g")
			.attr("class", "body")
			.attr("transform", "translate(" + _labelAreaWidth + ",0)");

		body.selectAll(".body")
			.data(_tasks, keyFunction)
			.enter()
			.append("rect")
			.attr("class", "task")
			.attr("rx", 2)
			.attr("ry", 2)
			.attr("y", 0)
			.attr("transform", rectTransform)
			.attr("height", function () { return 20; })
			.attr("width", function (task) { return (_x(new Date(task.endTime)) - _x(new Date(task.startTime))); })
			.attr("fill", function (task) { return task.agentColor; });

		body.append("g")
			.attr("class", "axis x")
			.attr("transform", "translate(0, " + _size.height + ")")
			.transition()
			.call(_xAxis);

		var header = _svg.append("g")
			.attr("class", "header")
			.attr("transform", "translate(" + _labelAreaWidth + ",0)");

		header.append("g")
			.attr("class", "axis y")
			.transition()
			.call(_yAxis);

		var overlay = _svg.append("g")
			.attr("class", "overlay");

		overlay.append("rect")
			.attr("class", "indicator accent-fill")
			.attr("y", 0)
			.attr("x", _labelAreaWidth)
			.attr("height", _size.height)
			.attr("width", 2);

		// Update the chart when the window size changes.
		$(window).resize(function() {
			gantt.update(_tasks);
		});

		return gantt;
	};

	gantt.update = function (tasks) {
		// Set the tasks.
		if(arguments.length) {
			gantt.tasks(tasks);
		}

		updateAxes();

		var rect = _svg.select(".body").selectAll("rect").data(_tasks, keyFunction);

		rect.enter()
			.insert("rect", ":first-child")
			.attr("rx", 2)
			.attr("ry", 2)
			.transition()
			.attr("y", 0)
			.attr("transform", rectTransform)
			.attr("height", function () { return _y.rangeBand(); })
			.attr("width", function (task) { return (_x(new Date(task.endTime)) - _x(new Date(task.startTime))); })
			.attr("fill", function (task) { return task.agentColor; });

		rect.transition()
			.attr("transform", rectTransform)
			.attr("height", function() { return _y.rangeBand(); })
			.attr("width", function(task) { return (_x(new Date(task.endTime)) - _x(new Date(task.startTime))); })
			.attr("fill", function (task) { return task.agentColor; });

		rect.exit().remove();

		_svg.select(".x")
			.transition()
			.call(_xAxis);

		_svg.select(".y")
			.transition()
			.call(_yAxis);

		return gantt;
	};

	gantt.tasks = function (value) {
		if (!arguments.length) {
			return _tasks;
		} else {
			_tasks = value;
			_tasks.forEach(function (task) {
				if(_labels.indexOf(task.name) == -1) {
					_labels.push(task.name);
				}
			});

			return gantt;
		}
	};

	gantt.timeRange = function (value) {
		if (!arguments.length) {
			return [_timeRangeStart, _timeRangeEnd];
		} else {
			_timeRangeStart = +value[0];
			_timeRangeEnd = +value[1];
			return gantt;
		}
	};

	gantt.fitTimeRange = function (value) {
		if (!arguments.length) {
			return _fitTimeRange;
		} else {
			_fitTimeRange = value;
			return gantt;
		}
	};

	gantt.barHeight = function(value) {
		if (!arguments.length) {
			return _barHeight;
		} else {
			_barHeight = +value;
			return gantt;
		}
	};

	gantt.width = function (value) {
		if (!arguments.length) {
			return _size.width;
		} else {
			_size.width = +value;
			return gantt;
		}
	};

	gantt.minWidth = function (value) {
		if (!arguments.length) {
			return _size.minWidth;
		} else {
			_size.minWidth = +value;
			return gantt;
		}
	};

	gantt.height = function (value) {
		if (!arguments.length) {
			return _size.height;
		} else {
			_size.height = +value;
			return gantt;
		}
	};

	gantt.minHeight = function (value) {
		if (!arguments.length) {
			return _size.minHeight;
		} else {
			_size.minHeight = +value;
			return gantt;
		}
	};

	gantt.tickFormat = function (formatString) {
		if (!arguments.length) {
			return _tickFormat;
		} else {
			_tickFormat = formatString;
			return gantt;
		}
	};

	gantt.position = function(time) {
		updatePosition(time);
	}

	return gantt;
};