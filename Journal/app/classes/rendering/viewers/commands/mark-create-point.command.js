function CreatePointMarkCommand(viewer, service) {
    var t = this;

    ViewerCommand.call(t, viewer, 'createPointMark');

    // The entity to be marked.
    t.param = null;

    // The mark service instance.
    t.service = service;

    // Reference to the currently created mark.
    t.mark = null;
    t.markVisual = null;

    // Coordinates of the top-left rectangle point.
    t.x1 = 0;
    t.y1 = 0;

    // Coordinates of the bottom-right rectangle point;
    t.x2 = 0;
    t.y2 = 0;

    // Attachable viewer event handlers.
    t.mouseDownHandler = function (e) {
        t.onMouseDown(e);
    };
    t.mouseDragHandler = function (e) {
        t.onMouseDrag(e);
    };
    t.mouseUpHandler = function (e) {
        t.onMouseUp(e);
    };

    // Start listening to mouse down events.
    t.stage.on("mousedown", t.mouseDownHandler);
}

CreatePointMarkCommand.prototype = Object.create(ViewerCommand.prototype);

CreatePointMarkCommand.prototype.canExecute = function (param) {
    var t = this;

    return t.viewer && param && param.length > 0;
};

CreatePointMarkCommand.prototype.execute = function (param) {
    var t = ViewerCommand.prototype.execute.call(this, param);

    t.viewer.selectCommand(t, param);

    t.stage.cursor = 'crosshair';

    t.param = param;
};

CreatePointMarkCommand.prototype.onMouseDown = function (e) {
    var t = this;

    if (t.viewer.selectedCommand === t) {
        t.x = e.stageX;
        t.y = e.stageY;

        if (t.mark === null) {
            // Create a new mark.
            t.mark = new Mark();
            t.mark.geometryType = 'http://w3id.org/art/terms/1.0/Point';

            // Create a new marker with the geometry set in local (scene) coordinates..
            t.mark.p1 = t.viewer.scene.globalToLocal(t.x, t.y);

            t.viewer.raise('markAdded', t.mark);
        }

        t.stage.on("pressmove", t.mouseDragHandler);
        $(window).on("mouseup", t.mouseUpHandler);
    }
};

CreatePointMarkCommand.prototype.onMouseUp = function (e) {
    var t = this;

    if (t.mark !== null) {
        t.createMark(t.mark);

        t.mark = null;
        t.markVisual = null;
    }

    t.stage.off("pressmove", t.mouseDragHandler);
    $(window).off("mouseup", t.mouseUpHandler);
};

CreatePointMarkCommand.prototype.onMouseDrag = function (e) {
    var t = this;

    if (t.viewer.selectedCommand === t) {
        // Create a new marker at the mouse down position.
        t.x = e.stageX;
        t.y = e.stageY;

        if (t.mark !== null) {
            // Update the lower right point of the rectangle when dragging.
            t.mark.p1 = t.viewer.scene.globalToLocal(t.x, t.y);

            t.viewer.raise('markInvalidated', t.mark);
        }
    }
};

CreatePointMarkCommand.prototype.createMark = function (mark) {
    var t = this;

    var m = {
        agent: t.viewer.user.Uri,
        entity: t.param,
        startTime: mark.startTime,
        endTime: new Date(),
        geometryType: mark.geometryType,
        geometry: {
            x: mark.p1.x,
            y: mark.p1.y
        }
    }

    t.service.createMark(m).then(function (data) {
        // The URI is needed for subsequent updates of the mark.
        mark.uri = data.uri;

        t.viewer.raise('itemCommited', mark);
    });

    return true;
};