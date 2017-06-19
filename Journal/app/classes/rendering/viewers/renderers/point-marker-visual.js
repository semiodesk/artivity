function PointMarkerVisual(viewer, container, mark) {
    createjs.Container.call(this);

    var t = this;

    t.cursor = 'default';
    t.modified = false;
    t.viewer = viewer;
    t.container = container;
    t.mark = mark;

    t.initializeGeometry();
    t.initializeContainer();
}

PointMarkerVisual.prototype = Object.create(createjs.Container.prototype);

PointMarkerVisual.prototype.initializeGeometry = function () {
    var t = this;

    // The markers are created on a unscaled drawing context.
    // Therefore, we need to translate from the scene coordinate system to the global one.
    var p1 = t.container.localToGlobal(t.mark.p1.x, t.mark.p1.y);

    t.resize(p1);
};

PointMarkerVisual.prototype.initializeContainer = function () {
    var t = this;

    t.on('mouseover', function (e) {
        t.hitTestPoint(e.stageX, e.stageY);
        t.container.stage.update();
    });

    t.on('mouseout', function (e) {
        t.hitTestPoint(e.stageX, e.stageY);
        t.container.stage.update();
    });

    t.on('mousedown', function (e) {
        t.viewer.raise('itemSelected', this);
        t.viewer.raise('markSelected', this.mark);
    });

    t.fillShape = t.createFillShape();
};

PointMarkerVisual.prototype.hitTestPoint = function (x, y) {
    var t = this;

    if (!t.mark.new || t.selected) {
        var p = {
            x: x - t.container.stage.x,
            y: y - t.container.stage.y
        };

        // Highlight the marker if it's not being created and when the mouse hovers it.
        var mouseover = t.hitTestObject(t.fillShape, p.x, p.y);
    }
};

PointMarkerVisual.prototype.hitTestObject = function (object, x, y) {
    var hit = object.hitTest(x, y);

    if (hit) {
        if (typeof object.highlight === 'function') {
            object.highlight();
        }
    } else {
        if (typeof object.normal === 'function') {
            object.normal();
        }
    }

    return hit;
};

PointMarkerVisual.prototype.createFillShape = function () {
    var t = this;

    // Note: We position the marker at a half-pixel position to get a crisp 1px border.
    // See: https://groups.google.com/forum/#!msg/createjs-discussion/TQC-jIyZjD4/7lWZg7qFlQ8J
    var s = new createjs.Shape();

    s.normal = function (init) {
        s.graphics.clear()
            .setStrokeStyle(2, 'square')
            .beginFill('rgba(252,202,0,1)')
            .drawCircle(t.x1, t.y1 - 20, 10)
            .endFill()
            .beginFill('rgba(252,202,0,1)')
            .moveTo(t.x1, t.y1)
            .lineTo(t.x1 - 10, t.y1 - 17)
            .lineTo(t.x1 + 10, t.y1 - 17)
            .lineTo(t.x1, t.y1)
            .endFill();
    }

    s.highlight = function () {
        s.graphics.clear()
            .setStrokeStyle(2, 'square')
            .beginFill('rgba(255,205,3,1)')
            .drawCircle(t.x1, t.y1 - 20, 10)
            .endFill()
            .beginFill('rgba(255,205,3,1)')
            .moveTo(t.x1, t.y1)
            .lineTo(t.x1 - 10, t.y1 - 17)
            .lineTo(t.x1 + 10, t.y1 - 17)
            .lineTo(t.x1, t.y1)
            .endFill();
    }

    s.normal();

    t.enableDragMove(s);

    t.addChild(s);

    return s;
};

PointMarkerVisual.prototype.enableDragMove = function (s) {
    var t = this;

    s.on('mousedown', function (e) {
        t.downX = e.stageX;
        t.downY = e.stageY;
    });

    s.on('pressmove', function (e) {
        t.regX = t.downX - e.stageX;
        t.regY = t.downY - e.stageY;

        t.container.stage.update();
    });

    s.on('pressup', function (e) {
        var dx = t.regX - t.container.stage.x;
        var dy = t.regY - t.container.stage.y;

        t.mark.p1 = t.container.globalToLocal(t.x1 - dx, t.y1 - dy);

        t.modified = dx != 0 || dy != 0;

        if (t.modified) {
            t.viewer.raise('itemModified', t);
            t.viewer.raise('markModified', t.mark);
        }
    });
};

PointMarkerVisual.prototype.resize = function (p1, p2) {
    var t = this;

    // Subtracting the stage's position (p.x - t.container.stage.x) corrects the panning offset.
    t.x1 = Math.round(p1.x - t.container.stage.x, 0);
    t.y1 = Math.round(p1.y - t.container.stage.y, 0);

    t.w = 20;
    t.h = 20;
};

PointMarkerVisual.prototype.redraw = function (src) {
    var t = this;

    t.fillShape.normal();
};

PointMarkerVisual.prototype.select = function () {
    var t = this;

    t.selected = true;

    t.viewer.cursor(t, 'move');

    var stage = t.container.stage;

    t.hitTestPoint(stage.mouseX, stage.mouseY);

    stage.update();
};

PointMarkerVisual.prototype.deselect = function () {
    var t = this;

    t.selected = false;

    t.viewer.cursor(t, 'default');

    var stage = t.container.stage;

    t.hitTestPoint(stage.mouseX, stage.mouseY);

    stage.update();
};