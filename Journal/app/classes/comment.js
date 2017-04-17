function Comment(uri) {
    var t = this;

    t.agent = null;
    t.agentId = null;
    t.primarySource = null;

    t.reset();
}

Comment.prototype.init = function() {
    var t = this;
    t.uri = null;
    t.type = 'Comment';
    t.startTime = null;
    t.endTime = null;
    t.message = '';
    t.markers = [];
    t.associations = [];
};

Comment.prototype.reset = function () {
    var t = this;

    t.init();
};

Comment.prototype.validate = function () {
    var t = this;

    var result = true;
    result = result && (typeof(t.agent) === 'string') && (t.agent.length > 0); // Needs to be URI
    result = result && (typeof(t.agentId) === 'string') && (t.agentId.length > 0); // Needs to be URI
    result = result && (typeof(t.primarySource) === 'string') && (t.primarySource.length > 0); // Needs to be URI
    result = result && (typeof(t.message) === 'string') && (t.message.length > 0);
    result = result && t.startTime && (t.startTime <= t.endTime);

    return result;
};