function PanCommand(viewer) {
    var t = this;

    ViewerCommand.call(t, viewer, 'viewerPan');

    // Indicates if the viewer command should be reset on a mouse up event.
    t.resetCommand = false;

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

    window.addEventListener('keydown', function (e) {
        if (e.key === ' ') {
            if (t.viewer.selectedCommand !== t) {
                // If the command has been triggered by a key event, reset the command after release.
                t.resetCommand = true;
                t.execute();
            }
        }
    });

    window.addEventListener('keyup', function (e) {
        if (e.key === ' ') {
            // Prevent electron from resizing the window when pressing spacebar.
            e.preventDefault();

            // Only reset the command if panning has not been executed.
            if (!t.viewer.isPanning) {
                t.viewer.resetCommand();
            }
        }
    }, true);
}

PanCommand.prototype = Object.create(ViewerCommand.prototype);

PanCommand.prototype.execute = function (e) {
    var t = ViewerCommand.prototype.execute.call(this, e);

    if (t.viewer.selectedCommand !== t) {
        t.viewer.selectCommand(t);
        t.stage.cursor = 'move';
    }
}

PanCommand.prototype.onMouseDown = function (e) {
    var t = this;

    // Allow triggering the execution of this command by pressing the middle mouse button.
    var native = e.nativeEvent;
    var handle = t.viewer.selectedCommand === t || (native && (native.which == 2 || native.button == 2));

    if (handle) {
        t.stage.autoFit = false;

        t.downX = e.stageX;
        t.downY = e.stageY;
        t.stageX = t.stage.x;
        t.stageY = t.stage.y;

        t.stage.on("pressmove", t.mouseDragHandler);
        $(window).on("mouseup", t.mouseUpHandler);

        if(t.viewer.selectedCommand !== t) {
            // If the command has been triggered by a mouse event, reset the command after release.
            t.resetCommand = true;
            t.execute();
        }
    }
}

PanCommand.prototype.onMouseDrag = function (e) {
    var t = this;

    if (t.viewer.selectedCommand === t) {
        t.viewer.isPanning = true;

        t.stage.x = t.stageX + (e.stageX - t.downX);
        t.stage.y = t.stageY + (e.stageY - t.downY);
        t.stage.update();
    }
}

PanCommand.prototype.onMouseUp = function (e) {
    var t = this;

    t.stage.off("pressmove", t.mouseDragHandler);
    $(window).off("mouseup", t.mouseUpHandler);

    t.viewer.isPanning = false;

    if (t.resetCommand) {
        t.viewer.resetCommand();
    }
}