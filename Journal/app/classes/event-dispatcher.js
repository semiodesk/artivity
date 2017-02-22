function EventDispatcher() {
    var t = this;

    t.muted = false;
    t.listeners = {};
};

function EventDispatcher(parent) {
    var t = this;

    t.muted = false;
    t.listeners = {};

    if(parent) {
        if(!parent.on) parent.on = function(event, callback) { t.on(event, callback); };
        if(!parent.off) parent.off = function(event, callback) { t.off(event, callback); };
        if(!parent.raise) parent.raise = function(event, params) { t.on(event, params); };
        if(!parent.mute) parent.mute = function() { t.mute(); };
        if(!parent.unmute) parent.unmute = function() { t.unmute(); };
    }
}

EventDispatcher.prototype.on = function (event, callback) {
    var t = this;

    if (callback) {
        if (!t.listeners[event]) {
            t.listeners[event] = [];
        }

        t.listeners[event].push(callback);
    }
};

EventDispatcher.prototype.off = function (event, callback) {
    var t = this;

    if (callback && event in t.listeners) {
        var i = t.listeners[event].indexOf(callback);

        if (i > -1) {
            t.listeners[event].splice(i, 1);
        }
    }
};

EventDispatcher.prototype.raise = function (event, params) {
    var t = this;

    if (t.muted) {
        return;
    }

    if (event in t.listeners) {
        var listeners = t.listeners[event];

        for (var i in listeners) {
            listeners[i](params);
        }
    }
};

EventDispatcher.prototype.mute = function () {
    var t = this;

    t.muted = true;
}

EventDispatcher.prototype.unmute = function () {
    var t = this;

    t.muted = false;
}