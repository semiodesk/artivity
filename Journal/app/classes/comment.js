function Comment(uri) {
    var t = this;

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

    var result = t.agentId
        && typeof(t.agentId) === 'string' // Needs to be URI
        && t.primarySource
        && typeof(t.primarySource) === 'string' // Needs to be URI
        && t.startTime
        && t.startTime <= t.endTime
        && typeof(t.message) === 'string'
        && t.message.length > 0;

    return result;
};