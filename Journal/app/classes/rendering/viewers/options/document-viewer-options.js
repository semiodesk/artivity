function DocumentViewerRenderOptions() {
    var t = this;

    t.reset();
}

DocumentViewerRenderOptions.prototype.reset = function () {
    var t = this;

    // Number of pages to render.
    t.maxPages = 1;

    // Optionally constrain the layout of the pages (horizontal/vertical).
    t.layout = 'horizontal';

    // Spacing between horizontally or vertically layouted pages.
    t.gutter = {
        x: 1,
        y: 1
    }
};

DocumentViewerRenderOptions.prototype.validate = function () {
    var t = this;

    var result = true;
    result &= (t.layout === undefined || t.layout === 'horizontal' || t.layout === 'vertical');

    return result;
};