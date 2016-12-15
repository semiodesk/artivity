/**
 * Caches a set of canvases.
 */
function CanvasCache() {
    var t = this;

    EntityCache.call(t);

    t.canvases = [];
};

CanvasCache.prototype = Object.create(EntityCache.prototype);

CanvasCache.prototype.load = function (data, complete) {
    var t = this;

    each(data, function (i, influence) {
        var time = new Date(influence.time);
        var canvas = t.entities[influence.uri];

        if (canvas === undefined) {
            canvas = new Entity(influence.uri);

            t.entities[influence.uri] = canvas;
        };

        if (influence.type === 'http://www.w3.org/ns/prov#Generation') {
            canvas.creationTime = time;
        }

        if (influence.property !== undefined) {
            canvas.pushValue(time, influence.property, influence.value);
        }

        canvas.pushValue(time, 'http://w3id.org/art/terms/1.0/x', influence.x);
        canvas.pushValue(time, 'http://w3id.org/art/terms/1.0/y', influence.y);
        canvas.pushValue(time, 'http://w3id.org/art/terms/1.0/width', influence.w);
        canvas.pushValue(time, 'http://w3id.org/art/terms/1.0/height', influence.h);
    });

    if (complete !== undefined) {
        complete(t.entities);
    }
};

CanvasCache.prototype.getAll = function (time, fn) {
    var t = this;

    if (fn) {
        values(t.entities, function (key, canvas) {
            if (time >= canvas.creationTime) {
                // Set the proper values for the bounds at the givent time.
                canvas.x = canvas.getValue(time, 'http://w3id.org/art/terms/1.0/x');
                canvas.y = canvas.getValue(time, 'http://w3id.org/art/terms/1.0/y');
                canvas.w = canvas.getValue(time, 'http://w3id.org/art/terms/1.0/width');
                canvas.h = canvas.getValue(time, 'http://w3id.org/art/terms/1.0/height');

                fn(canvas);
            }
        });
    };
};