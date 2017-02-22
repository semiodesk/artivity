function MarkerRectangle(viewer, container, marker) {
    createjs.Container.call(this);

    var t = this;

    t.viewer = viewer;
    t.container = container;
    t.marker = marker;

    t.initializeGeometry();
    t.initializeContainer();
}

MarkerRectangle.prototype = Object.create(createjs.Container.prototype);

MarkerRectangle.prototype.initializeGeometry = function () {
    var t = this;

    // The markers are created on a unscaled drawing context.
    // Therefore, we need to translate from the scene coordinate system to the global one.
    var p1 = t.container.localToGlobal(t.marker.p1.x, t.marker.p1.y);
    var p2 = t.container.localToGlobal(t.marker.p2.x, t.marker.p2.y);

    t.resize(p1, p2);
}

MarkerRectangle.prototype.initializeContainer = function () {
    var t = this;

    t.on('mouseover', function (e) {
        t.hitTest(e);
        t.container.stage.update();
    });

    t.on('mouseout', function (e) {
        t.hitTest(e);
        t.container.stage.update();
    });

    t.fillRectangle = t.createFillRectangle();
    t.borderRectangle = t.createBorderRectangle();

    t.resizeHandles = [];
    t.resizeHandles = t.resizeHandles.concat(t.createResizeBars());
    t.resizeHandles = t.resizeHandles.concat(t.createResizeGrips());
}

MarkerRectangle.prototype.hitTest = function (e) {
    var t = this;

    if (!t.marker.isNew) {
        var x = e.stageX - t.container.stage.x;
        var y = e.stageY - t.container.stage.y;

        var mouseover = t.hitTestObject(t.fillRectangle, x, y);

        for (i = 0; i < t.resizeHandles.length; i++) {
            mouseover |= t.hitTestObject(t.resizeHandles[i], x, y);
        }

        for (i = 0; i < t.resizeHandles.length; i++) {
            t.resizeHandles[i].visible = mouseover;
        }
    }
}

MarkerRectangle.prototype.hitTestObject = function (object, x, y) {
    var hit = object.hitTest(x, y);

    if (hit) {
        if (object.highlight) {
            object.highlight();
        }
    } else {
        if (object.normal) {
            object.normal();
        }
    }

    return hit;
}

MarkerRectangle.prototype.createFillRectangle = function () {
    var t = this;

    // Note: We position the marker at a half-pixel position to get a crisp 1px border.
    // See: https://groups.google.com/forum/#!msg/createjs-discussion/TQC-jIyZjD4/7lWZg7qFlQ8J
    var s = new createjs.Shape();

    s.normal = function (init) {
        s.graphics.clear()
            .setStrokeStyle(2, 'square')
            .beginStroke('rgba(255,255,255,0.1)')
            .beginFill('rgba(252,202,0,0.1)')
            .drawRect(t.x1, t.y1, t.w, t.h)
            .endFill()
            .endStroke();
    }

    s.highlight = function () {
        s.graphics.clear()
            .setStrokeStyle(2, 'square')
            .beginStroke('rgba(255,255,255,0.1)')
            .beginFill('rgba(252,202,0,0.15)')
            .drawRect(t.x1, t.y1, t.w, t.h)
            .endFill()
            .endStroke();
    }

    s.normal();

    t.enableDragMove(s);

    t.addChild(s);

    return s;
}

MarkerRectangle.prototype.createBorderRectangle = function () {
    var t = this;

    // Note: We position the marker at a half-pixel position to get a crisp 1px border.
    // See: https://groups.google.com/forum/#!msg/createjs-discussion/TQC-jIyZjD4/7lWZg7qFlQ8J
    var s = new createjs.Shape();

    s.normal = function () {
        var g = s.graphics;
        g.clear();
        g.setStrokeStyle(1, 'square');
        g.beginStroke('rgba(252,202,0,1)');
        g.drawRect(t.x1 + .5, t.y1 + .5, t.w - 1, t.h - 1);
        g.endStroke();
    }

    s.normal();

    t.addChild(s);

    return s;
}

MarkerRectangle.prototype.createResizeBars = function () {
    var t = this;

    var result = [];

    result.push(t.createResizeBar('n-resize'));
    result.push(t.createResizeBar('e-resize'));
    result.push(t.createResizeBar('s-resize'));
    result.push(t.createResizeBar('w-resize'));

    return result;
}

MarkerRectangle.prototype.createResizeBar = function (cursor) {
    var t = this;

    var s = new createjs.Shape();
    s.cursor = cursor;
    s.visible = false;

    s.geometry = function () {
        var bt = 10; // Bar thickness / width

        switch (s.cursor) {
            case 'n-resize':
                return {
                    x: t.x1 + bt,
                    y: t.y1,
                    w: t.w - 2 * bt,
                    h: bt
                };
            case 'e-resize':
                return {
                    x: t.x1 + t.w - bt,
                    y: t.y1 + bt,
                    w: bt,
                    h: t.h - 2 * bt
                };
            case 's-resize':
                return {
                    x: t.x1 + bt,
                    y: t.y1 + t.h - bt,
                    w: t.w - 2 * bt,
                    h: bt
                };
            case 'w-resize':
                return {
                    x: t.x1,
                    y: t.y1 + bt,
                    w: bt,
                    h: t.h - 2 * bt
                };
        }
    };

    s.normal = function (init) {
        var g = s.geometry();

        s.graphics.clear()
            .beginFill('rgba(252,202,0,0.01)')
            .drawRect(g.x, g.y, g.w, g.h)
            .endFill();
    }

    s.highlight = function () {
        var g = s.geometry();

        s.graphics.clear()
            .beginFill('rgba(252,202,0,0.15)')
            .drawRect(g.x, g.y, g.w, g.h)
            .endFill();
    }

    s.normal();

    t.enableDragResize(s);

    t.addChild(s);

    return s;
}

MarkerRectangle.prototype.createResizeGrips = function () {
    var t = this;

    var result = [];

    result.push(t.createResizeGrip('nw-resize'));
    result.push(t.createResizeGrip('ne-resize'));
    result.push(t.createResizeGrip('se-resize'));
    result.push(t.createResizeGrip('sw-resize'));

    return result;
}

MarkerRectangle.prototype.createResizeGrip = function (cursor) {
    var t = this;

    var s = new createjs.Shape();
    s.cursor = cursor;
    s.visible = false;

    s.geometry = function () {
        var r = 4; // Radius of the circle.

        switch (s.cursor) {
            case 'nw-resize':
                return {
                    x: t.x1,
                    y: t.y1,
                    r: r
                };
            case 'ne-resize':
                return {
                    x: t.x1 + t.w - 1,
                    y: t.y1,
                    r: r
                };
            case 'se-resize':
                return {
                    x: t.x1 + t.w - 1,
                    y: t.y1 + t.h - 1,
                    r: r
                };
            case 'sw-resize':
                return {
                    x: t.x1,
                    y: t.y1 + t.h - 1,
                    r: r
                };
        }
    };

    s.normal = function () {
        var g = s.geometry();

        s.graphics.clear()
            .setStrokeStyle(4 * g.r)
            .beginStroke('rgba(255,255,255,0.1)')
            .beginFill('rgba(252,202,0,1)')
            .drawCircle(g.x, g.y, g.r)
            .endFill()
            .endStroke();
    }

    s.highlight = function () {
        var g = s.geometry();

        s.graphics.clear()
            .setStrokeStyle(4 * g.r)
            .beginStroke('rgba(252,202,0,0.25)')
            .beginFill('rgba(252,202,0,1)')
            .drawCircle(g.x, g.y, g.r)
            .endFill()
            .endStroke();
    }

    s.normal();

    t.enableDragResize(s);

    t.addChild(s);

    return s;
}

MarkerRectangle.prototype.enableDragMove = function (s) {
    var t = this;

    // Set the move cursor.
    s.cursor = 'move';

    s.on('mousedown', function (e) {
        if (!t.viewer.isPanning) {
            t.downX = e.stageX;
            t.downY = e.stageY;
        }
    });

    s.on('pressmove', function (e) {
        if (!t.viewer.isPanning) {
            t.regX = t.downX - e.stageX;
            t.regY = t.downY - e.stageY;

            t.container.stage.update();
        }
    });

    s.on('pressup', function (e) {
        if (!t.viewer.isPanning) {
            var dx = t.regX - t.container.stage.x;
            var dy = t.regY - t.container.stage.y;

            t.marker.p1 = t.container.globalToLocal(t.x1 - dx, t.y1 - dy);
            t.marker.p2 = t.container.globalToLocal(t.x2 - dx, t.y2 - dy);
        }
    });
}

MarkerRectangle.prototype.enableDragResize = function (s) {
    var t = this;

    s.on('mousedown', function (e) {
        if (!t.viewer.isPanning) {
            t.downX = e.stageX;
            t.downY = e.stageY;
        }
    });

    s.on('pressmove', function (e) {
        if (!t.viewer.isPanning) {
            var dx = t.downX - e.stageX;
            var dy = t.downY - e.stageY;

            if (dx != 0 || dy != 0) {
                t.onResize(s, dx, dy);
            }
        }
    });

    s.on('pressup', function (e) {
        if (!t.viewer.isPanning) {
            var x1 = t.x1 - t.container.stage.x;
            var y1 = t.y1 - t.container.stage.y;
            var x2 = t.x2 - t.container.stage.x;
            var y2 = t.y2 - t.container.stage.y;

            t.marker.p1 = t.container.globalToLocal(x1, y1);
            t.marker.p2 = t.container.globalToLocal(x2, y2);
        }
    });
}

MarkerRectangle.prototype.onResize = function (src, dx, dy) {
    var t = this;
    var c = src.cursor;

    var p1 = t.container.localToGlobal(t.marker.p1.x, t.marker.p1.y);

    // Resize west: p1.x
    if (c == 'nw-resize' || c == 'w-resize' || c == 'sw-resize') {
        p1.x = p1.x - dx;
    }

    // Resize north: p1.y
    if (c == 'nw-resize' || c == 'n-resize' || c == 'ne-resize') {
        p1.y = p1.y - dy;
    }

    var p2 = t.container.localToGlobal(t.marker.p2.x, t.marker.p2.y);

    // Resize east: p2.x
    if (c == 'se-resize' || c == 'e-resize' || c == 'ne-resize') {
        p2.x = p2.x - dx;
    }

    // Resize south: p2.y
    if (c == 'se-resize' || c == 's-resize' || c == 'sw-resize') {
        p2.y = p2.y - dy;
    }

    t.resize(p1, p2);
    t.redraw(src);

    t.container.stage.update();
}

MarkerRectangle.prototype.resize = function (p1, p2) {
    var t = this;

    // Subtracting the stage's position (p.x - t.container.stage.x) corrects the panning offset.
    t.x1 = Math.round(p1.x - t.container.stage.x, 0);
    t.y1 = Math.round(p1.y - t.container.stage.y, 0);
    t.x2 = Math.round(p2.x - t.container.stage.x, 0);
    t.y2 = Math.round(p2.y - t.container.stage.y, 0);

    t.w = t.x2 - t.x1;
    t.h = t.y2 - t.y1;
}

MarkerRectangle.prototype.redraw = function(src) {
    var t = this;

    t.fillRectangle.normal();
    t.borderRectangle.normal();

    // Redraw the resize handles.
    for (i = 0; i < t.resizeHandles.length; i++) {
        var handle = t.resizeHandles[i];

        if (src && src === handle) {
            handle.highlight();
        } else {
            handle.normal();
        }
    }
}