/**
 * Loads a set of images from a server and keeps a cached version of the bitmaps.
 */
function ViewerCache() {
    var t = this;

    t.endpointUrl;
    t.renders = {};
    t.loading = false;
    t.loaded = false;
}

ViewerCache.prototype.load = function (data, complete) {
    var t = this;

    // Sanitize parameters.
    if (data === undefined) {
        return;
    }
    if (data.length === undefined) {
        data = [data];
    }

    t.loading = true;
    t.loaded = false;

    var count = data.length;

    // This callback counts down the items to be loaded.
    var completed = function (data, i) {
        count--;

        if (0 === count && complete !== undefined) {
            t.loading = false;
            t.loaded = true;

            complete(t.renders);
        }
    };

    // Invoke each action and await callback.
    for (var i = 0; i < data.length; i++) {
        t.loadRender(data, i, completed);
    }
};

ViewerCache.prototype.loadRender = function (data, i, complete) {
    var t = this;
    var d = data[i];
    var r = new Image();

    r.onerror = function () {
        complete(data, i);
    };

    r.onload = function () {
        r.onload = undefined;

        var R = [];

        if (d.layer in t.renders) {
            R = t.renders[d.layer];
        } else {
            t.renders[d.layer] = R;
        }

        var render = {
            img: r,
            time: new Date(d.time),
            type: d.type,
            x: d.x,
            y: d.y,
            w: d.w,
            h: d.h
        };

        R.push(render);

        // Ensure that the array is sorted for quicker access when rendering.
        R.sort(function (a, b) {
            return b.time - a.time;
        });

        complete(data, i);
    };

    r.crossOrigin = 'Anonymous'; // Needed for color thief.
    r.src = t.endpointUrl + d.file;
};

ViewerCache.prototype.get = function (time, layer, fn) {
    var t = this;
    var uri = layer;

    if (layer.uri) {
        uri = layer.uri;
    }

    if (uri !== undefined && uri in t.renders) {
        var R = t.renders[uri];

        for (var i = 0; i < R.length; i++) {
            var r = R[i];

            if (r.time <= time) {
                if (fn) {
                    fn(r);
                } else {
                    return r;
                }
            }
        }
    }
};