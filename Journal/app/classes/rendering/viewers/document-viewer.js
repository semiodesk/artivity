/**
 * Renders a document onto a canvas.
 */
function DocumentViewer(user, canvas, endpointUrl, selectionService) {
    var t = this;

    // Call the base constructor.
    ViewerBase.call(t, user, canvas, endpointUrl);

    // Manages the selection of items.
    t.selection = selectionService;

    // Options that influence how the pages are being rendered and layouted.
    t.renderOptions = new DocumentViewerRenderOptions();

    // Stores the bitmaps of the currently loaded pages.
    t.pages = [];

    // The currently rendered page. Set to -1 for rendering all pages.
    t.pageIndex = 0;

    // Loads and caches the document pages and renderings.
    t.pageCache = new ViewerCache();
    t.pageCache.endpointUrl = endpointUrl;

    // Handle selection changes.
    t.on('itemSelected', function (item) {
        t.onItemSelected(item);
    });

    t.on('itemModified', function (item) {
        t.onItemModified(item);
    });
}

DocumentViewer.prototype = Object.create(ViewerBase.prototype);

DocumentViewer.prototype.setRenderOptions = function (options) {
    var t = this;

    if (options) {
        t.renderOptions = options;
    }
}

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
};

DocumentViewer.prototype.onItemModified = function (item) {
    var t = this;

    console.log("Modified:", item);
};

DocumentViewer.prototype.setRevision = function (revision) {
    var t = this;

    t.revision = revision;
    t.entity = revision;

    // Rebuild the scene.
    t.render();

    // Raise the entity changed event.
    t.raise('revisionChanged', revision);
};

DocumentViewer.prototype.render = function () {
    var t = this;

    t.clearStage();

    // Return if there's nothing to render.
    if (!t.revision || !t.pageCache) {
        t.stage.update();

        return;
    }

    t.pages = t.pageCache.getRenderingsAtTime(new Date(), t.revision);

    var pages = t.pages;

    if (t.pageIndex > -1) {
        var i = t.pageIndex;
        var n = t.pageIndex;

        if (t.renderOptions.maxPages > 0) {
            n += t.renderOptions.maxPages;
        } else {
            n += 1;
        }

        pages = t.pages.slice(i, n);
    }

    if (pages.length) {
        t.layoutPages(pages);
    }

    t.stage.update();
};

DocumentViewer.prototype.layoutPages = function (pages) {
    var t = this;

    var gutter = t.renderOptions.gutter;
    var position = {
        x: 0,
        y: 0
    };

    var layout = t.renderOptions.layout;

    for (var i = 0; i < pages.length; i++) {
        var page = pages[i];

        // If the layout is undefined, we layout with the page cooridates from the database.
        if (!layout) {
            position.x = page.x;
            position.y = page.y;
        }

        t.renderPage(page, position);

        if (layout === 'horizontal') {
            position.x += (gutter.x + page.w);
        } else if (layout === 'vertical') {
            position.y -= (gutter.y + page.h);
        }
    }
}

DocumentViewer.prototype.renderPage = function (page, position) {
    var t = this;

    // Render the page sheets with a drop shadow.
    var s = new createjs.Shape();
    s.shadow = t.pageShadow;

    var x = Math.round(position.x, 0);
    var y = Math.round(position.y, 0);

    var g = s.graphics;
    g.setStrokeStyle(1);
    g.beginStroke('rgba(0,0,0,.1)');
    g.beginFill('white');

    // Note: The half pixel offset is required for the rendering to be sharp.
    g.drawRect(position.x + 0.5, -position.y + 0.5, page.w, page.h);

    t.measureExtents(position.x, -position.y, page.w, page.h);

    t.scene.addChild(s);

    // Render the page contents, if already loaded.
    if (page.img !== undefined) {
        var b = new createjs.Bitmap(page.img);
        b.x = position.x;
        b.y = -position.y;

        // For performance reasons, the renderings might be smaller than the
        // original size of the image. Therefore, we need to scale the image
        // to it's actual size in the picture.
        if (page.img.width > 0) {
            b.scaleX = page.w / page.img.width;
        }

        if (page.img.height > 0) {
            b.scaleY = page.h / page.img.height;
        }

        t.scene.addChild(b);
    }
}

DocumentViewer.prototype.nextPage = function (options) {
    var t = this;

    var n = t.pages.length - 1;

    if (t.pageIndex < n) {
        t.pageIndex += t.renderOptions.maxPages;

        t.render();
    }
}

DocumentViewer.prototype.previousPage = function (options) {
    var t = this;

    if (t.pageIndex >= t.renderOptions.maxPages) {
        t.pageIndex -= t.renderOptions.maxPages;

        t.render();
    }
}

DocumentViewer.prototype.setPageLayout = function (layout) {
    var t = this;

    switch (layout) {
        case 'all':
            t.pageIndex = -1;
            t.renderOptions.maxPages = -1;
            t.renderOptions.layout = undefined;
            break;
        case 'dual':
            if (t.pageIndex === -1) {
                t.pageIndex = 0;
            }
            t.renderOptions.maxPages = 2;
            t.renderOptions.layout = 'horizontal';
            break;
        default:
            if (t.pageIndex === -1) {
                t.pageIndex = 0;
            }
            t.renderOptions.maxPages = 1;
            t.renderOptions.layout = 'vertical';
            break;
    }

    t.render();
    t.zoomToFit();
}