function MarkRenderer(viewer, service) {
    var t = this;

    t.id = "marksRenderer";

    // Contains the marks to be rendered.
    t.marks = [];

    t.initialize(viewer, service);
}

MarkRenderer.prototype.initialize = function (viewer, service) {
    var t = this;

    // The mark service instance.
    t.service = service;

    // Keep a reference to the current viewer;
    t.viewer = viewer;
    t.viewer.marks = new createjs.Container();

    // Add the marks container just below the overlay container.
    var i = t.viewer.stage.getChildIndex(t.viewer.overlay);

    t.viewer.stage.addChildAt(t.viewer.marks, i);

    // Set the new entity if the entity of the viewer changes.
    t.viewer.on("revisionChanged", function (revision) {
        t.setEntity(revision);
    });

    // Listen to viewer events for re-rendering the marks.
    t.viewer.on('markAdded', function (mark) {
        if (t.onMarkAdded(mark)) {
            t.render();
        }
    });

    t.viewer.on('markRemoved', function (mark) {
        if (t.onMarkRemoved(mark)) {
            t.render();
        }
    });

    t.viewer.on('markInvalidated', function (mark) {
        if (t.onMarkInvalidated(mark)) {
            t.viewer.stage.update();
        }
    });

    t.viewer.on('zoom', function () {
        t.render();
    });
};

MarkRenderer.prototype.setEntity = function (entity) {
    var t = this;

    if (!t.service) {
        return;
    }

    t.service.getMarksForEntity(entity).then(function (data) {
        t.addMarks(data);
    });
};

MarkRenderer.prototype.addMarks = function (marks) {
    var t = this;

    for (var i = 0; i < marks.length; i++) {
        t.onMarkAdded(marks[i]);
    }

    t.render();
};

MarkRenderer.prototype.removeMarks = function (marks) {
    var t = this;

    var M = {};

    for (var i = 0; i < marks.length; i++) {
        var m = marks[i];

        if (m.uri) {
            M[m.uri] = m;
        }
    }

    t.marks = t.marks.filter(function (m) {
        return m.uri && M[m.uri] === undefined;
    });

    t.render();
};

MarkRenderer.prototype.render = function () {
    var t = this;

    t.viewer.marks.removeAllChildren();

    for (var i = 0; i < t.marks.length; i++) {
        var m = t.marks[i];
        var r = new MarkRectangle(t.viewer, t.viewer.scene, m);

        t.viewer.marks.addChild(r);
    }

    t.viewer.stage.update();
};

MarkRenderer.prototype.onMarkAdded = function (mark) {
    var t = this;

    if (!mark || (!mark.uri && !mark.p1)) {
        console.warn("Invalid mark:", mark);
        return false;
    }

    if (mark.uri && (mark.w === 0 || mark.h === 0)) {
        console.warn("Ingoring zero-sized mark:", mark.uri);
        return false;
    }

    var m;

    if (mark.uri) {
        m = {
            uri: mark.uri,
            p1: {
                x: mark.x,
                y: mark.y
            },
            p2: {
                x: mark.x + mark.w,
                y: mark.y + mark.h
            }
        };
    } else {
        m = mark;
    }

    t.marks.push(m);

    t.viewer.marks.addChild(new MarkRectangle(t.viewer, t.viewer.scene, m));

    return true;
};

MarkRenderer.prototype.onMarkRemoved = function (mark) {
    var t = this;

    for (var i = 0; i < t.marks.length; i++) {
        if (t.marks[i] === mark) {
            t.marks.splice(i, 1);

            return true;
        }
    }

    return false;
};

MarkRenderer.prototype.onMarkInvalidated = function (mark) {
    var t = this;

    for (var i = 0; i < t.viewer.marks.children.length; i++) {
        var m = t.viewer.marks.getChildAt(i);

        if (m.mark === mark) {
            // Recalculate the rectangle dimensions because the marker might have changed.
            m.initializeGeometry();
            m.redraw();

            return true;
        }
    }

    return false;
};