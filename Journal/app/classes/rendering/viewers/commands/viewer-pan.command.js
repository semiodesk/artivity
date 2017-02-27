function PanCommand(viewer) {
    var t = this;

    t.id = 'viewer-pan';

    t.viewer = viewer;
    t.stage = viewer.stage;

    // Indicates if we are in pan mode.
    t.viewer.isPanning = false;

    t.stage.on('mousedown', function (e) {
        t.onMouseDown(e);
    });

    t.stage.on('pressmove', function (e) {
        t.onMouseDrag(e);
    });

    // Listen to mouse up events on the window because while dragging
    // the cursor might leave the canvas.
    window.addEventListener('mouseup', function (e) {
        t.onMouseUp(e);
    });

    window.addEventListener('keydown', function (e) {
        if (e.key === ' ') {
            t.startPanning();
        }
    });

    window.addEventListener('keyup', function (e) {
        if (e.key === ' ') {
            t.stopPanning();
            e.preventDefault(); // Prevent electron from resizing the window when pressing spacebar.
        }
    }, true);
}

PanCommand.prototype.canExecute = function (e) {
    var t = this;

    return t.viewer;
}

PanCommand.prototype.execute = function (e) {
    var t = this;

    t.stage.cursor = 'move';
    t.stage.update();

    return true;
}

PanCommand.prototype.onMouseDown = function (e) {
    var t = this;

    t.stage.autoFit = false;

    t.downX = e.stageX;
    t.downY = e.stageY;
    t.stageX = t.stage.x;
    t.stageY = t.stage.y;

    // TODO: Implement x-browser.
    if (e.nativeEvent.button === 1) {
        t.startPanning();
    }
}

PanCommand.prototype.onMouseDrag = function (e) {
    var t = this;

    if (t.viewer.isPanning) {
        t.stage.x = t.stageX + (e.stageX - t.downX);
        t.stage.y = t.stageY + (e.stageY - t.downY);
        t.stage.update();
    }
}

PanCommand.prototype.onMouseUp = function (e) {
    var t = this;

    // TODO: Implement x-browser.
    if (e.button === 1) {
        t.stopPanning();
    }
}

PanCommand.prototype.startPanning = function () {
    var t = this;

    t.viewer.isPanning = true;
    t.stage.cursor = 'move';
    t.stage.update();
}

PanCommand.prototype.stopPanning = function () {
    var t = this;

    t.viewer.isPanning = false;
    t.stage.cursor = 'crosshair';
    t.stage.update();
}