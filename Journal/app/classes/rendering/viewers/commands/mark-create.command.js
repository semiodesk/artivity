function CreateMarkCommand(viewer, service) {
    var t = this;

    ViewerCommand.call(t, viewer, 'createMark');

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

CreateMarkCommand.prototype = Object.create(ViewerCommand.prototype);

CreateMarkCommand.prototype.canExecute = function(param) {
    var t = this;

    return t.viewer && param && param.length > 0;
};

CreateMarkCommand.prototype.execute = function (param) {
    var t = ViewerCommand.prototype.execute.call(this, param);

    t.viewer.selectCommand(t, param);

    t.stage.cursor = 'crosshair';

    t.param = param;
};

CreateMarkCommand.prototype.onMouseDown = function (e) {
    var t = this;

    if (t.viewer.selectedCommand === t) {
        // Remember mouse down position for creating new markers.
        t.x1 = e.stageX;
        t.y1 = e.stageY;

        t.stage.on("pressmove", t.mouseDragHandler);
        $(window).on("mouseup", t.mouseUpHandler);
    }
};

CreateMarkCommand.prototype.onMouseUp = function (e) {
    var t = this;

    if (t.mark !== null) {
        t.createMark(t.mark);

        t.mark = null;
        t.markVisual = null;
    }

    t.stage.off("pressmove", t.mouseDragHandler);
    $(window).off("mouseup", t.mouseUpHandler);
};

CreateMarkCommand.prototype.onMouseDrag = function (e) {
    var t = this;

    if (t.viewer.selectedCommand === t) {
        // Create a new marker at the mouse down position.
        t.x2 = e.stageX;
        t.y2 = e.stageY;

        if (t.mark === null) {
            // Create a new mark.
            t.mark = new Marker();

            // Create a new marker with the geometry set in local (scene) coordinates..
            t.mark.p1 = t.viewer.scene.globalToLocal(t.x1, t.y1);
            t.mark.p2 = t.viewer.scene.globalToLocal(t.x2, t.y2);

            t.viewer.raise('markAdded', t.mark);
        } else {
            // Update the lower right point of the rectangle when dragging.
            t.mark.p2 = t.viewer.scene.globalToLocal(t.x2, t.y2);

            t.viewer.raise('markInvalidated', t.mark);
        }
    }
};

CreateMarkCommand.prototype.createMark = function (mark) {
    var t = this;

    var m = {
        agent: t.viewer.user.Uri,
        entity: t.param,
        startTime: mark.startTime,
        endTime: new Date(),
        x: mark.p1.x,
        y: mark.p1.y,
        width: mark.p2.x - mark.p1.x,
        height: mark.p2.y - mark.p1.y
    }

    if (m.width === 0 || m.height === 0) {
        return false;
    }

    t.service.createMark(m).then(function (data) {
        // The URI is needed for subsequent updates of the mark.
        mark.uri = data.uri;

        t.viewer.raise('itemCommited', mark);
    });

    return true;
};