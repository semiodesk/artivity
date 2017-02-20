/**
 * Renders a document onto a canvas.
 */
function DocumentViewer(canvas, endpointUrl) {
    var t = this;

    // HTML5 canvas element.
    t.canvas = canvas;

    // Loads and caches the document pages and renderings.
    t.viewerCache = new DocumentRendererCache();

    // Scene element which is being manipulated when zooming or panning.
    t.scene = new createjs.Container();

    t.scene.on("mousedown", function (e) {
        t.stage.autoFit = false;

        this.downX = e.stageX;
        this.downY = e.stageY;
        this.stageX = this.x;
        this.stageY = this.y;
    });

    t.scene.on("pressmove", function (e) {
        this.x = this.stageX + (e.stageX - this.downX);
        this.y = this.stageY + (e.stageY - this.downY);

        t.stage.update();
    });

    t.scene.zoom = function (delta) {
        var dZ = 0.1;

        var delta = delta > 0 ? dZ : -dZ;

        if ((t.scene.scaleX + delta) > dZ) {
            t.scene.scaleX += delta;
            t.scene.scaleY += delta;

            t.stage.update();
        }
    };

    t.canvas.addEventListener("wheel", function (e) {
        t.scene.zoom(-e.deltaY);
    });

    window.addEventListener("keydown", function (e) {
        if (e.ctrlKey && e.key === '1') {
            t.stage.autoFit = true;

            if (t.influence !== undefined) {
                t.render(t.influence);
            }
        } else if (e.ctrlKey && e.key === '+') {
            e.preventDefault();
            t.scene.zoom(1);
        } else if (e.ctrlKey && e.key === '-') {
            e.preventDefault();
            t.scene.zoom(-1);
        }
    });

    window.addEventListener("visibilitychange", function (e) {
        // Redraw the scene to prevent blank scenes when switching windows.		
        if (!document.hidden && t.influence !== undefined) {
            t.render(t.influence);
        }
    });

    // EaselJS drawing context.
    t.stage = new createjs.Stage("canvas");
    t.stage.autoClear = true;
    t.stage.autoFit = true;

    // Shadow that is drawn below the canvases / artboards / pages.
    t.pageShadow = new createjs.Shadow('rgba(0,0,0,.3)', 5, 5, 15);
}

DocumentViewer.prototype.render = function (revision) {
    var t = this;

    // Clear the scene.
    t.scene.removeAllChildren();

    // Clear the stage.
    t.stage.removeAllChildren();
    t.stage.addChild(t.scene);

    // Return if there's nothing to render.
    if (revision === undefined || t.viewerChache === undefined || !t.viewerChache.ready) {
        t.stage.update();

        return;
    }

    var extents = {
        t: 0,
        l: 0,
        b: 0,
        r: 0
    };

    t.viewerCache.getAllPages(revision, function (p) {
        // Render the page sheets with a drop shadow.
        var s = new createjs.Shape();
        s.shadow = t.pageShadow;

        var g = s.graphics;
        g.beginFill('white');
        g.drawRect(p.x, -p.y, p.w, p.h);

        t.measureExtents(extents, p.x, -p.y, p.w, p.h);

        t.scene.addChild(s);

        // Render the page contents, if already loaded.
        if(p.img !== undefined) {
            var b = new createjs.Bitmap(p.img);
            b.x = p.x;
            b.y = -p.y;

            // For performance reasons, the renderings might be smaller than the
            // original size of the image. Therefore, we need to scale the image
            // to it's actual size in the picture.
            if (p.img.width > 0) {
                b.scaleX = p.w / p.img.width;
            }

            if (r.img.height > 0) {
                b.scaleY = p.h / p.img.height;
            }

            t.scene.addChild(b);
        }
    });

    if (extents != null) {
        // Draw extents position marker.
        var ce = {
            x: extents.x + extents.width / 2,
            y: -extents.y + extents.height / 2
        };

        // Draw center position marker.
        var cc = {
            x: t.canvas.width / 2,
            y: t.canvas.height / 2
        };

        // Uncomment this when debugging:
        t.drawSceneMarkers(extents, cc, ce);

        if (t.stage.autoFit) {
            // Set the registration point of the scene to the center of the extents (currently the canvas center).
            t.scene.regX = ce.x;
            t.scene.regY = ce.y;

            // Move the scene into the center of the stage.
            t.scene.x = cc.x;
            t.scene.y = cc.y;

            t.zoomToFit(extents);
        }
    }

    t.stage.update();
}

DocumentViewer.prototype.zoomToFit = function (extents) {
    var t = this;

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
}

DocumentViewer.prototype.measureExtents = function (extents, x, y, w, h) {
    extents.l = Math.min(extents.l, x);
    extents.r = Math.max(extents.r, x + w);
    extents.t = Math.min(extents.t, -y);
    extents.b = Math.max(extents.b, -y + h);
    extents.x = extents.l;
    extents.y = extents.t;
    extents.width = extents.r - extents.l;
    extents.height = extents.b - extents.t;
}

DocumentViewer.prototype.drawSceneMarkers = function (extents, cc, ce) {
    var t = this;

    // Draw the extents.
    var s = new createjs.Shape();

    var g = s.graphics;
    g.beginStroke('red');
    g.setStrokeStyle(1);
    g.drawRect(extents.l, -extents.t, extents.width, extents.height);

    t.scene.addChild(s);

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

        t.scene.addChild(s);
    }

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

        t.stage.addChild(s);
    }

    // Draw null position.
    s = new createjs.Shape();

    g = s.graphics;
    g.beginStroke('green');
    g.setStrokeStyle(1);
    g.drawCircle(0, 0, 5);

    t.stage.addChild(s);
}