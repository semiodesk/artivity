/**
 * Renders a document onto a canvas.
 */
function ViewerBase(user, canvas) {
    var t = this;

    // Indicates if debug information like scene extents and center points should be rendered.
    t.enableDebug = false;

    // The user agent. -- NOTE: why do we need the user here? Does this concern the viewer?
    t.user = user;

    // HTML5 canvas element.
    t.canvas = canvas;

    // The rendered entity or derivation.
    t.entity = null;

    // Renderers provide additional rendering functionality.
    t.renderers = [];

    // Available viewer commands.
    t.commands = {};

    // Default command is executed when the user aborts other commands.
    t.defaultCommand = null;

    // The command which was executed before the selecterd command.
    t.previousCommand = null;

    // The currently selected command mode.
    t.selectedCommand = null;

    // Indicates if the pan command is being executed.
    t.isPanning = false;

    // Shadow that is drawn below the canvases / artboards / pages.
    t.pageShadow = new createjs.Shadow('rgba(0,0,0,.2)', 3, 3, 6);

    // Only initialize if we have a canvas object.
    if (t.canvas) {
        t.initializeEventDispatcher();
        t.initializeScene();
        t.initializeCanvasResize();
        t.initializeCanvasZoom();

        t.raise('initialized', t);
    }
}

ViewerBase.prototype.initializeEventDispatcher = function () {
    var t = this;

    // Adds the event handling methods 'on', 'off', 'raise', etc to this class.
    t.dispatcher = new EventDispatcher(t);
};

ViewerBase.prototype.initializeScene = function () {
    var t = this;

    // EaselJS drawing context.
    t.stage = new createjs.Stage(t.canvas.id);
    t.stage.autoClear = true;
    t.stage.autoFit = true;
    t.stage.debug = new createjs.Container();
    t.stage.enableMouseOver();
    t.stage.on('drawstart', function (e) {
        t.onDrawStart(t, e);
    });

    // Scene element which is being manipulated when zooming or panning.
    t.scene = new createjs.Container();
    t.scene.debug = new createjs.Container();
    t.scene.extents = {
        t: 0, // Top
        l: 0, // Left
        b: 0, // Bottom
        r: 0 // Right
    };

    // Add an 'infinitly' large shape to the background which
    // makes the stage respond to click and drag events.
    var min = -100000;
    var max = 1000000;

    t.background = new createjs.Shape();

    // The fill renders the shape visible for hit tests.
    var g = t.background.graphics;
    g.beginFill('rgba(0,0,0,0.01)');
    g.drawRect(min, min, max, max);

    // An overlay container for elements which are rendered above the scene.
    t.overlay = new createjs.Container();

    // Initialize the stage containers.
    t.stage.addChild(t.background);
    t.stage.addChild(t.scene);
    t.stage.addChild(t.overlay);
};

ViewerBase.prototype.initializeCanvasZoom = function () {
    var t = this;

    t.stage.on('pressup', function (e) {
        if (t.selection) {
            var item = t.selection.selectedItem();

            if (item && typeof item.hitTest === 'function') {
                var p = item.globalToLocal(e.stageX, e.stageY);

                var hit = item.hitTest(p.x, p.y);

                if (hit === false) {
                    t.resetSelection();
                }
            }
        }
    });

    t.canvas.addEventListener('wheel', function (e) {
        if (!t.isPanning) {
            t.zoom(e.deltaY);
        }
    });

    window.addEventListener('keydown', function (e) {
        if (e.key === 'Home' || e.ctrlKey && e.key === '1') {
            e.preventDefault();
            t.zoomToFit();
            t.dispatcher.raise('zoom');
        } else if (e.ctrlKey && e.key === 'F5') {
            t.enableDebug = !t.enableDebug;
            t.stage.update();
        } else if (e.key === 'Escape') {
            t.resetCommand(true);
            t.resetSelection();
        }
    });
};

ViewerBase.prototype.clearStage = function () {
    var t = this;

    if (t.scene) {
        // Clear the scene container.
        t.scene.removeAllChildren();
    }

    if (t.overlay) {
        // Clear the overlay container.
        t.overlay.removeAllChildren();
    }
};

ViewerBase.prototype.setFile = function (file) {
    var t = this;

    t.file = file;

    // Rebuild the scene.
    t.render();

    // Raise the entity changed event.
    t.raise('fileChanged', file);
};

ViewerBase.prototype.addRenderer = function (renderer) {
    var t = this;

    if (renderer.id) {
        t.renderers.push(renderer);

        console.log("Enabled renderer:", renderer.id);
    } else {
        console.error("Invalid renderer:", renderer);
    }
};

ViewerBase.prototype.getRenderer = function (r) {
    var t = this;
    var id = null;

    if (typeof r === 'string') {
        id = r;
    } else if (typeof r.id === 'string') {
        id = r.id;
    }

    if (id) {
        for (i in t.renderers) {
            var r = t.renderers[i];

            if (r.id == id) {
                return r;
            }
        }
    }
};

ViewerBase.prototype.addCommand = function (command, defaultCommand) {
    var t = this;

    if (command.id) {
        t.commands[command.id] = command;

        if (defaultCommand) {
            t.defaultCommand = command;
            t.selectedCommand = command;

            console.log("Enabled command (default):", command.id);
        } else {
            console.log("Enabled command:", command.id);
        }
    } else {
        console.error("Invalid command:", command);
    }
};

ViewerBase.prototype.getCommand = function (c) {
    var t = this;
    var id = null;

    if (typeof c === 'string') {
        id = c;
    } else if (typeof c.id === 'string') {
        id = c.id;
    }

    if (id) {
        for (i in t.commands) {
            var c = t.commands[i];

            if (c.id == id) {
                return c;
            }
        }
    }
};

ViewerBase.prototype.canExecuteCommand = function (id, param) {
    var t = this;
    var c = t.getCommand(id);

    if (c) {
        return c.canExecute(param);
    }

    return false;
};

ViewerBase.prototype.executeCommand = function (id, param) {
    var t = this;
    var c = t.getCommand(id);

    if (c && c.canExecute(param)) {
        return c.execute(param);
    }

    return false;
};

ViewerBase.prototype.selectCommand = function (id, param) {
    var t = this;
    var c = t.getCommand(id);

    if (c && c.canExecute(param)) {
        var e = {
            command: c,
            parameter: param
        };

        if (t.selectedCommand && t.selectedCommand.id != c.id) {
            t.previousCommand = t.selectedCommand;
        }

        t.selectedCommand = c;

        t.raise('commandSelected', e);
    }
};

ViewerBase.prototype.resetCommand = function (resetDefault) {
    var t = this;

    if (!resetDefault && t.previousCommand) {
        t.executeCommand(t.previousCommand.id);
    } else if (t.defaultCommand) {
        t.executeCommand(t.defaultCommand.id);
    }
};

ViewerBase.prototype.resetSelection = function () {
    var t = this;

    var item = t.selection.selectedItem();

    if (item && typeof item.deselect === 'function') {
        item.deselect();

        t.selection.clear();

        t.stage.update();
    }
};

ViewerBase.prototype.initializeCanvasResize = function () {
    var t = this;

    // Finally, adjust the canvas size when the window is being resized.
    t.onResize();

    window.addEventListener("resize", function (e) {
        t.onResize();
    });
};

ViewerBase.prototype.onResize = function () {
    var t = this;

    // This seems to be the only reliable way to compute the correct height of the body area.
    var bodyHeight = $('#window-view-container').height();

    $('.layout-root > .row').each(function (n, row) {
        if (!$(row).hasClass('expand')) {
            bodyHeight -= $(row).height();
        }
    });

    var buffer = t.canvas;

    if (buffer) {
        // To prevent flickering, store the current context before setting the size.
        var bufferContext = buffer.getContext('2d');

        // Fit the canvas into its parent.
        var container = $(buffer);

        // Create temp canvas and context
        var temp = document.createElement('canvas');
        temp.width = container.width();
        temp.height = bodyHeight;

        // Draw current canvas to temp canvas.
        var tempContext = temp.getContext("2d");
        tempContext.drawImage(bufferContext.canvas, 0, 0);

        // Now resize the canvas.					
        buffer.width = container.width();
        buffer.height = bodyHeight;
        buffer.style.height = bodyHeight + 'px';

        // Draw temp canvas back to the current canvas
        bufferContext.drawImage(tempContext.canvas, 0, 0);
    }

    if (t.stage.autoFit) {
        t.zoomToFit();
    } else {
        t.raise('zoom');
    }

    // Update the buffer.
    t.stage.update();
};

ViewerBase.prototype.onDrawStart = function (t, e) {
    t.scene.debug.removeAllChildren();
    t.stage.debug.removeAllChildren();

    if (t.enableDebug && t.scene.extents != null) {
        var extents = t.scene.extents;

        // Draw extents position marker.
        var ce = {
            x: extents.x + extents.width / 2,
            y: -extents.y + extents.height / 2
        };

        // Draw center position marker.
        var cc = {
            x: $(t.canvas).innerWidth() / 2,
            y: $(t.canvas).innerHeight() / 2
        };

        // Uncomment this when debugging:
        t.drawSceneMarkers(extents, cc, ce);
    }
};

ViewerBase.prototype.zoom = function (dY) {
    var t = this;

    var dZ = 0.1;
    var delta = -dY > 0 ? dZ : -dZ;

    if ((t.scene.scaleX + delta) > dZ) {
        t.scene.scaleX += delta;
        t.scene.scaleY += delta;

        t.stage.update();

        t.raise('zoom');
    }
};

ViewerBase.prototype.zoomIn = function () {
    this.zoom(-1);
};

ViewerBase.prototype.zoomOut = function () {
    this.zoom(1);
};

ViewerBase.prototype.zoomToFit = function () {
    var t = this;

    var extents = t.scene.extents;

    // Draw extents position marker.
    var ce = {
        x: extents.x + extents.width / 2,
        y: -extents.y + extents.height / 2
    };

    // Draw center position marker.
    var cc = {
        x: $(t.canvas).innerWidth() / 2,
        y: $(t.canvas).innerHeight() / 2
    };

    // Set the registration point of the scene to the center of the extents (currently the canvas center).
    t.scene.regX = ce.x;
    t.scene.regY = ce.y;

    // Move the scene into the center of the stage.
    t.scene.x = cc.x;
    t.scene.y = cc.y;

    // One zoom factor for scaling both axes.
    var z = 1.0;

    // Padding inside the canvas.
    var p = 30;

    // After measuring, determine the zoom level to contain all the canvases.
    if (extents.width > t.canvas.width) {
        z = Math.min(z, (t.canvas.width - p) / extents.width);
    }

    if (extents.height > t.canvas.height) {
        z = Math.min(z, (t.canvas.height - p) / extents.height);
    }

    t.scene.scaleX = z;
    t.scene.scaleY = z;

    t.stage.x = 0;
    t.stage.y = 0;

    t.stage.update();

    t.raise('zoom');
};

ViewerBase.prototype.measureExtents = function (x, y, w, h) {
    var t = this;

    if (t.scene.extents) {
        var extents = t.scene.extents;
        extents.l = Math.min(extents.l, x);
        extents.r = Math.max(extents.r, x + w);
        extents.t = Math.min(extents.t, y);
        extents.b = Math.max(extents.b, y + h);
        extents.x = extents.l;
        extents.y = extents.t;
        extents.width = extents.r - extents.l;
        extents.height = extents.b - extents.t;
    }
};

ViewerBase.prototype.drawSceneMarkers = function (extents, canvasCenter, extentsCenter) {
    var t = this;

    var cc = canvasCenter;
    var ce = extentsCenter;

    // Elements in this container are in the coordinate space of the scene and get scaled when zooming.
    if (t.scene.debug != null) {
        // Ensure that the debug layer is the top most layer.
        t.scene.addChild(t.scene.debug);

        // Draw extents of the scene.
        var s = new createjs.Shape();

        var g = s.graphics;
        g.beginStroke('red');
        g.setStrokeStyle(1);
        g.drawRect(extents.l, -extents.t, extents.width, extents.height);

        t.scene.debug.addChild(s);

        // Draw null position of the scene.
        s = new createjs.Shape();

        g = s.graphics;
        g.beginStroke('green');
        g.setStrokeStyle(1);
        g.drawCircle(0, 0, 5);

        t.scene.debug.addChild(s);

        var r = 6;

        if (ce !== undefined) {
            s = new createjs.Shape();

            g = s.graphics;
            g.beginStroke('red');
            g.setStrokeStyle(1);
            g.moveTo(ce.x - r, ce.y);
            g.lineTo(ce.x + r, ce.y);
            g.endStroke();

            g.beginStroke('red');
            g.setStrokeStyle(1);
            g.moveTo(ce.x, ce.y - r);
            g.lineTo(ce.x, ce.y + r);
            g.endStroke();

            t.scene.debug.addChild(s);
        }
    }

    // Elements in this container are in global coordinate space and do not get scaled when zooming.
    if (t.stage.debug != null) {
        // Make the stage debug container the top most one.
        t.stage.addChild(t.stage.debug);

        // Draw the actual canvas center marker.
        if (cc !== undefined) {
            s = new createjs.Shape();

            g = s.graphics;
            g.beginStroke('blue');
            g.setStrokeStyle(1);
            g.moveTo(cc.x - r, cc.y - r);
            g.lineTo(cc.x + r, cc.y + r);
            g.endStroke();

            g.beginStroke('blue');
            g.setStrokeStyle(1);
            g.moveTo(cc.x + r, cc.y - r);
            g.lineTo(cc.x - r, cc.y + r);
            g.endStroke();

            t.stage.debug.addChild(s);
        }
    }
};

ViewerBase.prototype.render = function (cursor) {
    var t = this;

    t.clearStage();
};

ViewerBase.prototype.cursor = function (target, cursor) {
    var t = this;

    target.cursor = cursor;

    t.stage.update();

    $(window).css('cursor', cursor);
};