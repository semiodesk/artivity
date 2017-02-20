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
}

DocumentViewer.prototype = Object.create(DocumentViewerBase.prototype);

DocumentViewer.prototype.render = function (revision) {
    var t = this;

    // Clear the scene.
    t.scene.removeAllChildren();

    // Clear the stage.
    t.stage.removeAllChildren();
    t.stage.addChild(t.scene);

    // Return if there's nothing to render.
    if (revision === undefined || t.pageCache === undefined) {
        t.stage.update();

        return;
    }

    t.pageCache.get(new Date(), revision, function (p) {
        // Render the page sheets with a drop shadow.
        var s = new createjs.Shape();
        s.shadow = t.pageShadow;

        var g = s.graphics;
        g.beginFill('white');
        g.drawRect(p.x, -p.y, p.w, p.h);

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