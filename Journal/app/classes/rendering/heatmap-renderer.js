function HeatmapRenderer(canvas) {
    var t = this;

    t.canvas = canvas;
    t.context = canvas.getContext('2d');
    t.extents = {
        x1: 0,
        y1: 0,
        x2: 0,
        y2: 0
    };
}

HeatmapRenderer.prototype.sample = function (bounds) {
    var t = this;

    // The color palette of the heatmap.
    t.palette = new Array(bounds.length);

    // Counts the number of overlayed rectangles per pixel in the canvas.
    t.map = new Array(t.canvas.width * t.canvas.height);

    // Iterate over the bounding rectangles and increase the intersection count
    // for each pixel in the map that is covered by the rectangle.
    for (var n = 0; n < bounds.length; n++) {
        var rect = bounds[n];

        if (n > 0) {
            t.extents.x1 = Math.min(t.extents.x1, rect.x);
            t.extents.y1 = Math.min(t.extents.y1, rect.y);
            t.extents.x2 = Math.max(t.extents.x2, rect.x + rect.w);
            t.extents.y2 = Math.max(t.extents.y2, rect.y + rect.h);
            t.extents.w = t.extents.x2 - t.extents.x1;
            t.extents.h = t.extents.y2 - t.extents.y1;
        } else {
            ext = {
                x1: rect.x,
                y1: rect.y,
                x2: rect.x + rect.w,
                y2: rect.y + rect.h,
                w: t.extents.x2 - t.extents.x1,
                h: t.extents.y2 - t.extents.y1
            };
        }

        var stride = t.canvas.width - rect.w;
        var p = rect.x - 1 + rect.y * t.canvas.width;

        for (var y = 0; y < rect.h; y++) {
            for (var x = 0; x < rect.w; x++) {
                var c = t.map[p];

                if (c > 0) {
                    t.map[p] += 1;
                } else {
                    t.map[p] = 1;
                }

                p += 1;
            }

            p += stride;
        }

        // Initialize the color palette.
        t.palette[n + 1] = {
            r: 255,
            g: 255 / (n * n + 1),
            b: 0,
            a: 128
        };
    }

    console.log(t.map);
};

HeatmapRenderer.prototype.render = function (bounds) {
    var t = this;

    console.log(t.bounds);

    t.sample(bounds);

    console.log(t.extents);

    var img = t.context.createImageData(t.extents.w, t.extents.h);
    var data = img.data;

    var stride = t.canvas.width - t.extents.w;
    var p = t.extents.x1 - 1 + t.extents.y1 * t.canvas.width;

    var n = 0;

    for (var y = 0; y < t.extents.h; y++) {
        for (var x = 0; x < t.extents.w; x++) {
            var c = t.map[p];

            if (c !== undefined) {
                data[n + 0] = t.palette[c].r;
                data[n + 1] = t.palette[c].g;
                data[n + 2] = t.palette[c].b;
                data[n + 3] = t.palette[c].a;
            }

            n += 4;
            p += 1;
        }

        p += stride;
    }

    t.context.putImageData(img, t.extents.x1, t.extents.y1);
};