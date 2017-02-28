/**
 * Renders a document onto a canvas.
 */
function DocumentViewer(user, canvas, endpointUrl, selectionService) {
    var t = this;

    // Call the base constructor.
    ViewerBase.call(t, user, canvas, endpointUrl);

    // Manages the selection of items.
    t.selection = selectionService;

    // Loads and caches the document pages and renderings.
    t.pageCache = new ViewerCache();
    t.pageCache.endpointUrl = endpointUrl;

    // Lists all the features which are enabled for this viewer.
    t.commands = [];

    // Handle selection changes.
    t.on('itemSelected', function(item) { t.onItemSelected(item); });
    t.on('itemModified', function(item) { t.onItemModified(item); });
}

DocumentViewer.prototype = new ViewerBase();

DocumentViewer.prototype.onItemSelected = function (item) {
    var t = this;

    var other = t.selection.selectedItem();

    if (other && typeof other.deselect === 'function') {
        other.deselect();
    }

    if (item && typeof item.select === 'function') {
        item.select();
    }

    t.selection.selectedItem(item);

    console.log("Selected:", item);
}

DocumentViewer.prototype.onItemModified = function (item) {
    var t = this;

    console.log("Modified:", item);
}

DocumentViewer.prototype.render = function () {
    var t = this;

    t.clearStage();

    // Return if there's nothing to render.
    if (t.entity === undefined || t.pageCache === undefined) {
        t.stage.update();

        return;
    }

    t.pageCache.get(new Date(), t.entity, function (p) {
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