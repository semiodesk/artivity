/**
 * Renders a document onto a canvas.
 */
function DocumentHistoryViewer(canvas, endpointUrl) {
    var t = this;

    // Call the base constructor.
    DocumentViewerBase.call(t, canvas, endpointUrl);
    
    t.canvas = canvas;
    t.canvasCache = new CanvasCache();

    t.renderCache = new DocumentViewerCache();
    t.renderCache.endpointUrl = endpointUrl;
    t.renderInfluencedRegions = false;

    t.layerCache = new LayerCache();

    t.renderedLayers = [];
}

DocumentHistoryViewer.prototype = Object.create(DocumentViewerBase.prototype);

DocumentHistoryViewer.prototype.render = function (influence) {
    var t = this;

    t.influence = influence;

    t.stage.removeAllChildren();
    t.stage.addChild(t.scene);

    t.scene.removeAllChildren();

    if (influence === undefined || t.canvasCache === undefined) {
        t.stage.update();

        return;
    }

    var time = new Date(influence.time);

    var extents = {
        t: 0,
        l: 0,
        b: 0,
        r: 0
    };

    t.canvasCache.getAll(time, function (c) {
        var s = new createjs.Shape();
        s.shadow = t.pageShadow;

        var g = s.graphics;
        g.beginFill('white');
        g.drawRect(c.x, -c.y, c.w, c.h);

        t.measureExtents(c.x, -c.y, c.w, c.h);

        t.scene.addChild(s);
    });

    if (t.layerCache === undefined || t.renderCache === undefined) {
        t.stage.update();

        return;
    }

    t.renderedLayers = [];

    // Only render the newest version of every layer.
    t.layerCache.getAll(time, function (layer) {
        if (layer.visible) {
            var r = t.renderCache.get(time, layer);

            if (r !== undefined) {
                t.renderedLayers.push(r);

                var b = new createjs.Bitmap(r.img);
                b.x = r.x;
                b.y = -r.y;

                // For performance reasons, the renderings might be smaller than the
                // original size of the image. Therefore, we need to scale the image
                // to it's actual size in the picture.
                if (r.img.width > 0) {
                    b.scaleX = r.w / r.img.width;
                }

                if (r.img.height > 0) {
                    b.scaleY = r.h / r.img.height;
                }

                t.scene.addChild(b);

                /*
                if (layer.uri == influence.layer) {
                	context.font = "1em Roboto";
                	context.fillStyle = "blue";
                	context.textAlign = "left";
                	context.fillText(layer.getLabel(time), r.x, -r.y - 10);

                	context.lineJoin = 'miter';
                	context.lineWidth = 1;
                	context.strokeStyle = 'blue';
                	context.setLineDash([1, 2]);
                	context.strokeRect(r.x, -r.y, r.w, r.h);
                }
                */
            }
        }
    });

    // Draw a line around the influenced region.
    if (influence.w > 0 && influence.h > 0) {
        var s = new createjs.Shape();

        var g = s.graphics;
        g.beginFill('rgba(255,0,0,.2)');
        g.drawRect(influence.x, -influence.y, influence.w, influence.h);

        t.scene.addChild(s);
    }

    if (t.influences && (t.renderInfluencedRegions || t.renderEditingFrequency)) {
        var entities = {};

        // Layer for drawing the edited regions.
        var s = new createjs.Shape();
        var g = s.graphics;

        // Layer for drawing the editing frequency indicators.
        var s2 = new createjs.Shape();
        var g2 = s2.graphics;

        var x = Math.ceil(255 / t.influences.length);
        var r = 0;
        var b = 255;

        for (var i = t.influences.length - 1; i > 0; i--) {
            var inf = t.influences[i];

            if (inf.time > time) {
                break;
            }

            if (t.renderInfluencedRegions && inf.w > 0 && inf.h > 0) {
                g.setStrokeStyle(1);
                g.beginFill('rgba(' + r + ',0,' + b + ',.2)');
                g.drawRect(inf.x, -inf.y, inf.w, inf.h);
            }

            if (t.renderEditingFrequency && inf.changes) {
                g2.setStrokeStyle(1);

                for (j in inf.changes) {
                    var entity = inf.changes[j].entity;

                    if (entity) {
                        var editCount = entities[entity];

                        if (editCount) {
                            editCount++;
                        } else {
                            editCount = 1;
                        }

                        entities[entity] = editCount;

                        g2.beginStroke('rgba(0,255,255,.25)');
                        g2.drawRect(inf.x, -inf.y, inf.w, inf.h);
                        g2.endStroke();

                        g2.beginFill('rgba(0,255,255,1)');
                        g2.drawCircle(inf.x, -inf.y, editCount);
                        g2.endFill();
                    } else {
                        g2.beginStroke('rgba(0,255,255,.25)');
                        g2.drawRect(inf.x, -inf.y, inf.w, inf.h);
                        g2.endStroke();

                        g2.beginFill('rgba(0,255,255,1)');
                        g2.drawCircle(inf.x, -inf.y, 1);
                        g2.endFill();
                    }
                }
            }

            r = Math.min(r + x, 255);
            b = Math.max(b - x, 0);
        }

        t.scene.addChild(s);
        t.scene.addChild(s2);
    }

    t.stage.update();
}

DocumentHistoryViewer.prototype.getPalette = function () {
    var t = this;

    // Used for extracting colors from images.
    var thief = new ColorThief();

    // Extract the color palette from the rendered layers.
    var palette = [];

    for (var n = 0; n < t.renderedLayers.length; n++) {
        var r = t.renderedLayers[n];

        // Extract colors from the current image.
        var p = thief.getPalette(r.img, 8);

        palette = palette.concat(p);

        // Remove duplicate colors.
        palette = palette.filter(function (val, j, array) {
            for (var i = 0; i < p.length; i++) {
                var c = p[i];

                // Remove all other values with equal color, except the color itself.
                if (c !== val && c.equals(val)) {
                    return false;
                }
            }

            return true;
        });
    }

    return palette;
}