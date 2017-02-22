/**
 * Renders a document onto a canvas.
 */
function DocumentViewer(canvas, endpointUrl) {
    var t = this;

    // Call the base constructor.
    DocumentViewerBase.call(t, canvas, endpointUrl);

    // Loads and caches the document pages and renderings.
    t.pageCache = new DocumentViewerCache();
    t.pageCache.endpointUrl = endpointUrl;

    // Unscaled overlay layer which can be used for drawing dialogs, tools or makers.
    t.markerContainer = new createjs.Container();
    t.markers = [];

    t.initializeMarkers();

    t.cursor('crosshair');
}

DocumentViewer.prototype = Object.create(DocumentViewerBase.prototype);

DocumentViewer.prototype.initializeMarkers = function () {
    var t = this;

    var x1 = 0;
    var y1 = 0;
    var isDragging = false;

    t.scene.on("mousedown", function (e) {
        isDragging = true;

        // Remember mouse down position for creating new markers.
        x1 = e.stageX;
        y1 = e.stageY;
    });

    t.scene.on("pressmove", function (e) {
        if (t.isPanning || !isDragging) {
            return;
        }

        // Create a new marker at the mouse down position.
        var x2 = e.stageX;
        var y2 = e.stageY;

        var m = t.markers[t.markers.length - 1];

        if (!m || !m.isNew) {
            // Create a new marker with the geometry set in local (scene) coordinates..
            m = new Marker();
            m.p1 = t.scene.globalToLocal(x1, y1);
            m.p2 = t.scene.globalToLocal(x2, y2);

            var r = new MarkerRectangle(t, t.scene, m);

            t.markers.push(m);
            t.markerContainer.addChild(r);

            t.stage.update();
        } else {
            // Update the lower right point of the rectangle when dragging.
            m.p2 = t.scene.globalToLocal(x2, y2);

            // Redraw the existing marker rectangle.
            var r = t.markerContainer.getChildAt(t.markers.length - 1);

            if (r) {
                r.initializeGeometry();
                r.redraw();

                t.stage.update();
            }
        }
    });

    $(window).mouseup(function (e) {
        isDragging = false;

        // Finalize the new marker.
        if (t.markers.length > 0) {
            t.markers[t.markers.length - 1].isNew = false;
        }
    });

    t.on("zoom", function () {
        t.markerContainer.removeAllChildren();

        for (i = 0; i < t.markers.length; i++) {
            var m = t.markers[i];
            var r = new MarkerRectangle(t, t.scene, m);

            t.markerContainer.addChild(r);
        }

        t.stage.update();
    });
}

DocumentViewer.prototype.render = function (revision) {
    var t = this;

    t.clearStage();

    t.stage.addChild(t.markerContainer);

    // Return if there's nothing to render.
    if (revision === undefined || t.pageCache === undefined) {
        t.stage.update();

        return;
    }

    t.pageCache.get(new Date(), revision, function (p) {
        // Render the page sheets with a drop shadow.
        var s = new createjs.Shape();
        s.shadow = t.pageShadow;

        var x = Math.round(p.x, 0);
        var y = Math.round(p.y, 0);

        var g = s.graphics;
        g.setStrokeStyle(1);
        g.beginStroke('rgba(0,0,0,.1)');
        g.beginFill('white');
        g.drawRect(p.x + 0.5, -p.y + 0.5, p.w, p.h);

        t.measureExtents(p.x, -p.y, p.w, p.h);

        t.scene.addChild(s);

        // Render the page contents, if already loaded.
        if (p.img !== undefined) {
            var b = new createjs.Bitmap(p.img);
            b.x = p.x;
            b.y = -p.y;

            // For performance reasons, the renderings might be smaller than the
            // original size of the image. Therefore, we need to scale the image
            // to it's actual size in the picture.
            if (p.img.width > 0) {
                b.scaleX = p.w / p.img.width;
            }

            if (p.img.height > 0) {
                b.scaleY = p.h / p.img.height;
            }

            t.scene.addChild(b);
        }
    });

    t.stage.update();
}