/**
 * Renders a document onto a canvas.
 */
function DocumentViewerBase(canvas, endpointUrl) {
    var t = this;

    // Indicates if debug information like scene extents and center points should be rendered.
    t.enableDebug = false;

    // HTML5 canvas element.
    t.canvas = canvas;

    // Shadow that is drawn below the canvases / artboards / pages.
    t.pageShadow = new createjs.Shadow('rgba(0,0,0,.2)', 3, 3, 6);

    t.initializeEventDispatcher();
    t.initializeScene();
    t.initializeCanvasDragMove();
    t.initializeCanvasResize();
    t.initializeCanvasZoom();
}

DocumentViewerBase.prototype.initializeEventDispatcher = function () {
    var t = this;

    // Adds the event handling methods 'on', 'off', 'raise', etc to this class.
    t.dispatcher = new EventDispatcher(t);
}

DocumentViewerBase.prototype.initializeScene = function () {
    var t = this;

    // EaselJS drawing context.
    t.stage = new createjs.Stage("canvas");
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
}

DocumentViewerBase.prototype.initializeCanvasZoom = function () {
    var t = this;

    t.canvas.addEventListener("wheel", function (e) {
        if (!t.isPanning) {
            var dZ = 0.1;
            var delta = -e.deltaY > 0 ? dZ : -dZ;

            if ((t.scene.scaleX + delta) > dZ) {
                t.scene.scaleX += delta;
                t.scene.scaleY += delta;

                t.stage.update();

                t.dispatcher.raise('zoom');
            }
        }
    });

    window.addEventListener("keydown", function (e) {
        console.log(e.key);

        if (e.key === 'Home' || e.ctrlKey && e.key === '1') {
            e.preventDefault();
            t.zoomToFit();
            t.dispatcher.raise('zoom');
        } else if (e.ctrlKey && e.key === '+') {
            e.preventDefault();
            t.scene.zoom(1);
            t.stage.update();
            t.dispatcher.raise('zoom');
        } else if (e.ctrlKey && e.key === '-') {
            e.preventDefault();
            t.scene.zoom(-1);
            t.stage.update();
            t.dispatcher.raise('zoom');
        } else if (e.ctrlKey && e.key === 'F5') {
            t.enableDebug = !t.enableDebug;
            t.stage.update();
        }
    });
}

DocumentViewerBase.prototype.initializeCanvasDragMove = function () {
    var t = this;

    // Indicates if we are in pan mode.
    t.isPanning = false;

    t.stage.on("mousedown", function (e) {
        t.stage.autoFit = false;

        this.downX = e.stageX;
        this.downY = e.stageY;
        this.stageX = this.x;
        this.stageY = this.y;

        // TODO: Implement x-browser.
        if (e.nativeEvent.button === 1) {
            t.startPanning();
        }
    });

    // Listen to mouse up events on the window because while dragging
    // the cursor might leave the canvas.
    window.addEventListener('mouseup', function (e) {
        // TODO: Implement x-browser.
        if (e.button === 1) {
            t.stopPanning();
        }
    });

    t.stage.on("pressmove", function (e) {
        if (t.isPanning) {
            this.x = this.stageX + (e.stageX - this.downX);
            this.y = this.stageY + (e.stageY - this.downY);
            t.stage.update();
        }
    });

    window.addEventListener("keydown", function (e) {
        if (e.key === ' ') {
            // Prevent electron from resizing the window when pressing spacebar.
            e.preventDefault();
            t.startPanning();
        }
    });

    window.addEventListener("keyup", function (e) {
        if (e.key === ' ') {
            // Prevent electron from resizing the window when pressing spacebar.
            e.preventDefault();
            t.stopPanning();
        }
    });
}

DocumentViewerBase.prototype.clearStage = function () {
    var t = this;

    // Clear the stage.
    t.stage.removeAllChildren();
    t.stage.addChild(t.background);
    t.stage.addChild(t.scene);
    t.stage.addChild(t.overlay);

    // Clear the scene.
    t.scene.removeAllChildren();
}

DocumentViewerBase.prototype.startPanning = function () {
    var t = this;
    t.isPanning = true;
    t.cursor('move');
}

DocumentViewerBase.prototype.stopPanning = function () {
    var t = this;
    t.isPanning = false;
    t.cursor('crosshair');
}

DocumentViewerBase.prototype.initializeCanvasResize = function () {
    var t = this;

    // Finally, adjust the canvas size when the window is being resized.
    t.onResize();

    window.addEventListener("resize", function (e) {
        t.onResize();
    });
}

DocumentViewerBase.prototype.onResize = function () {
    var t = this;

    // This seems to be the only reliable way to compute the correct height of the body area.
    var bodyHeight = $('#window-view-container').height();

    $('.layout-root > .row').each(function (n, row) {
        if (!$(row).hasClass('expand')) {
            bodyHeight -= $(row).height();
        }
    });

    var buffer = t.canvas;

    if (buffer !== null) {
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
    }

    // Update the buffer.
    t.stage.update();
}

DocumentViewerBase.prototype.onDrawStart = function (t, e) {
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
}

DocumentViewerBase.prototype.zoomToFit = function () {
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
}

DocumentViewerBase.prototype.measureExtents = function (x, y, w, h) {
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
}

DocumentViewerBase.prototype.drawSceneMarkers = function (extents, canvasCenter, extentsCenter) {
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
}

DocumentViewerBase.prototype.cursor = function (cursor) {
    var t = this;

    t.stage.cursor = cursor;
}