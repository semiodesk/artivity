function Topic(uri) {
    var t = this;

    t.init();
}

Topic.prototype.init = function () {
    var t = this;

    t.uri = null;
    t.agent = null;
    t.primarySource = null;
    t.startTime = new Date();
    t.endTime = null;
    t.title = '';
    t.closed = false;
};

Topic.prototype.reset = function () {
    var t = this;

    t.init();
};

Topic.prototype.validate = function () {
    var t = this;

    var result = true;
    result = result && (typeof (t.agent) === 'string') && (t.agent.length > 0); // Needs to be URI
    result = result && (typeof (t.primarySource) === 'string') && (t.primarySource.length > 0); // Needs to be URI
    result = result && (typeof (t.title) === 'string') && (t.title.length > 0);
    result = result && t.startTime && (t.startTime <= t.endTime);

    return result;
};