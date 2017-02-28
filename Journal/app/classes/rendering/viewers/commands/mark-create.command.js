function CreateMarkCommand(viewer, service) {
    var t = this;

    ViewerCommand.call(t, viewer, 'mark-create');

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
}

CreateMarkCommand.prototype = new ViewerCommand();

CreateMarkCommand.prototype.canExecute = function (e) {
    return true;
}

CreateMarkCommand.prototype.startExecute = function (e) {
    var t = ViewerCommand.prototype.startExecute.call(this, e);

    t.viewer.stage.cursor = 'crosshair';

    t.attachEventHandlers();
}

CreateMarkCommand.prototype.stopExecute = function (e) {
    var t = ViewerCommand.prototype.stopExecute.call(this, e);

    t.viewer.stage.cursor = 'default';

    t.removeEventHandlers();
}

CreateMarkCommand.prototype.attachEventHandlers = function () {
    var t = this;

    t.viewer.stage.on("mousedown", t.mouseDownHandler);
    t.viewer.stage.on("pressmove", t.mouseDragHandler);
    t.viewer.stage.on("pressup", t.mouseUpHandler);
}

CreateMarkCommand.prototype.removeEventHandlers = function () {
    var t = this;

    t.viewer.stage.off("mousedown", t.mouseDownHandler);
    t.viewer.stage.off("pressmove", t.mouseDragHandler);
    t.viewer.stage.off("pressup", t.mouseUpHandler);
}

CreateMarkCommand.prototype.onMouseDown = function (e) {
    var t = this;

    // Remember mouse down position for creating new markers.
    t.x1 = e.stageX;
    t.y1 = e.stageY;
}

CreateMarkCommand.prototype.onMouseUp = function (e) {
    var t = this;

    if (t.mark !== null) {
        t.createMark(t.mark);

        t.mark = null;
        t.markVisual = null;

        t.stopExecute(e);
    }
}

CreateMarkCommand.prototype.onMouseDrag = function (e) {
    var t = this;

    if (t.viewer.isPanning) {
        return;
    }

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

CreateMarkCommand.prototype.createMark = function (mark) {
    var t = this;

    var m = {
        agent: t.viewer.user.Uri,
        entity: t.viewer.entity,
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
}