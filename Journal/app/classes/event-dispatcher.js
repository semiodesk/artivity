function EventDispatcher() {
    var t = this;

    t.muted = false;
    t.listeners = {};
}

function EventDispatcher(parent) {
    var t = this;

    t.muted = false;
    t.listeners = {};

    if (parent) {
        if (!parent.on) parent.on = function (event, callback) {
            t.on(event, callback);
        };
        if (!parent.off) parent.off = function (event, callback) {
            t.off(event, callback);
        };
        if (!parent.raise) parent.raise = function (event, params) {
            t.raise(event, params);
        };
        if (!parent.mute) parent.mute = function () {
            t.mute();
        };
        if (!parent.unmute) parent.unmute = function () {
            t.unmute();
        };
    }
};

EventDispatcher.prototype.on = function (events, callback) {
    var t = this;

    if (!callback) {
        return;
    }

    var E = null;

    if (Array.isArray(events)) {
        E = events;
    } else if (typeof events === 'string') {
        E = events.split(' ');
    } else {
        return;
    }

    for (i = 0; i < E.length; i++) {
        var event = E[i];

        if (!t.listeners[event]) {
            t.listeners[event] = [];
        }

        t.listeners[event].push(callback);
    }
};

EventDispatcher.prototype.off = function (events, callback) {
    var t = this;

    if (!callback) {
        return;
    }

    var E = null;

    if (Array.isArray(events)) {
        E = events;
    } else if (typeof events === 'string') {
        E = events.split(' ');
    } else {
        return;
    }

    for (i = 0; i < E.length; i++) {
        var event = E[i];
        
        if (event in t.listeners) {
            var n = t.listeners[event].indexOf(callback);

            if (n > -1) {
                t.listeners[event].splice(n, 1);
            }
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
};

EventDispatcher.prototype.unmute = function () {
    var t = this;

    t.muted = false;
};