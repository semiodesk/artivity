function each(array, fn) {
    for(var i = 0; i < array.length; i++) {
        fn(i, array[i]);
    }
};

function values(array, fn) {	
    for(k in array) {
        if(array.hasOwnProperty(k)) {
            fn(k, array[k]);
        }
    }
};

/**
 * A generic entity which is identified by a URI. The class
 * provides methods for accessing the values of properties
 * at a given point in time.
 */
function Entity(uri) {
    var t = this;
    
    t.uri = uri;
    t.properties = {};
    t.creationTime = undefined;
	t.deletionTime = new Date();
};

Entity.prototype.pushValue = function (time, property, value) {
    var t = this;
    var P = t.properties[property];
    
    if (P === undefined) {
        P = [];
        
        t.properties[property] = P;
    }
    
    P.push({ time: time, value: value });
};

Entity.prototype.getValue = function (time, property) {
    var t = this;
    var P = t.properties[property];
    	
    if (P === undefined) {
        return undefined;
    }
    
    for (var i = 0; i < P.length; i++) {
        var p = P[i];
        
        if(p.time <= time) {
            return p.value;
        }
    }
    
    return undefined;
};

Entity.prototype.getLabel = function(time) {
    return this.getValue(time, 'http://www.w3.org/2000/01/rdf-schema#label');
};

/**
 * Caches a set of entities. The class provides methods for 
 * accessing a subset of the stored entities depending on
 * the time they were created.
 */
function EntityCache() {
    var t = this;

    t.entities = {};
};

EntityCache.prototype.length = function() {
    var t = this;

    return t.entities.length; 
};

EntityCache.prototype.load = function(data, complete) {
    var t = this;

    each(data, function(i, influence) {
        if(influence.time !== undefined && influence.uri !== undefined) {
            var time = new Date(influence.time);
            var entity = t.entities[influence.uri];

            if(entity === undefined) {
                entity = new Entity(influence.uri);

                t.entities[influence.uri] = entity;
            };

            if(influence.type === 'http://www.w3.org/ns/prov#Generation') {
                entity.creationTime = time;
            }
			else if(influence.type === 'http://www.w3.org/ns/prov#Invalidation') {
				entity.deletionTime = time;
			}

			if(influence.property !== undefined) {
            	entity.pushValue(time, influence.property, influence.value);
			}
        }
    });
	
	if(complete !== undefined) {
		complete(t.entities);
	}
};

EntityCache.prototype.get = function(uri) {
    return this.entities[uri];
};

EntityCache.prototype.getAll = function(time, fn) {
    var t = this;

    values(t.entities, function(key, entity) {
        if(time >= entity.creationTime) {
            fn(entity);
        }
    });
};

/**
 * Caches a set of canvases.
 */
function CanvasCache() {
    var t = this;

    EntityCache.call(t);

    t.canvases = [];
};

CanvasCache.prototype = Object.create(EntityCache.prototype);

CanvasCache.prototype.load = function(data, complete) {
    var t = this;

    each(data, function(i, influence) {
		var time = new Date(influence.time);
        var canvas = t.entities[influence.uri];
        
        if(canvas === undefined) {
            canvas = new Entity(influence.uri);
            
            t.entities[influence.uri] = canvas;
        };
        		
        if(influence.type === 'http://www.w3.org/ns/prov#Generation') {
            canvas.creationTime = time;
        }
        
		if(influence.property !== undefined) {
			canvas.pushValue(time, influence.property, influence.value);
		}
		
		canvas.x = influence.x;
		canvas.y = influence.y;
		canvas.w = influence.w;
		canvas.h = influence.h;
    });
	
	if(complete !== undefined) {
		complete(t.entities);
	}
};

/**
 * A level at which you can place an object or image file.
 */
function Layer(uri) {
};

Layer.prototype = Object.create(Entity.prototype);

/**
 * Caches a set of layers.
 */
function LayerCache() {
    var t = this;

    EntityCache.call(t);
};

LayerCache.prototype = Object.create(EntityCache.prototype);

/**
 * Enumerates the layers in rendering order: from bottom to top.
 */
LayerCache.prototype.getAll = function(time, fn) {
    var t = this;

    var map = new Map();
    var count = 0;
    var current = null;

    values(t.entities, function(uri, layer) {	
        if(layer.creationTime <= time && time < layer.deletionTime) {    			
            var lowerLayer = layer.getValue(time, 'http://w3id.org/art/terms/1.0/aboveLayer');
						
            if(lowerLayer !== undefined) {
                map.set(lowerLayer, uri);

                count++;
            } else {
                current = uri;
								
                fn(layer);
            }
        }
    });

    var i = 0;

    while (i < count) {
        var next = t.entities[map.get(current)];
		
        if(next !== undefined) {
            fn(next);
		
            current = next.uri;
        }

        i++;
    }
}

/**
 * Renders a document onto a canvas.
 */
function DocumentRenderer(canvas, endpointUrl) {
    var t = this;
	
	t.canvas = canvas;
	t.canvasCache = new CanvasCache();
	
	t.layerCache = new LayerCache();
	
	t.renderCache = new DocumentRendererCache();
	t.renderCache.endpointUrl = endpointUrl;
	
	t.renderedLayers = [];
}

DocumentRenderer.prototype.render = function(influence) {
	var t = this;
		
	var context = t.canvas.getContext('2d');
	
    context.clearRect(0, 0, t.canvas.width, t.canvas.height);

	if(influence === undefined || t.canvasCache === undefined) {
		return;
	}
	
	var time = new Date(influence.time);
	
    // Save the untransformed canvas state.
    context.save();

	var shadowOffsetX = 3;
    var shadowOffsetY = 3;
	
	var zoom = 1.0;
    var extents = { width: 0, height: 0 };
	
    // Measure the extents of the drawing.
	t.canvasCache.getAll(time, function(c) {		
		// Fit the rendered document into the available viewport.
        extents.width = Math.max(extents.width, c.x + c.w + shadowOffsetX);
        extents.height = Math.max(extents.height, c.y + c.h + shadowOffsetY);
	});

    // After measuring, determine the zoom level to contain all the canvases.
    if (extents.width > t.canvas.width) {
        zoom = Math.min(zoom, t.canvas.width / extents.width);
    }

    if (extents.height > t.canvas.height) {
        zoom = Math.min(zoom, t.canvas.height / extents.height);
    }

    // Translate the document to be centered on the canvas.            
    if (t.canvas.width !== undefined && t.canvas.width > extents.width) {
        context.translate((t.canvas.width - extents.width) / 2, 0);
    }

    context.scale(zoom, zoom);

    // Render the canvases.
	t.canvasCache.getAll(time, function(c) {
		// Fit the rendered document into the available viewport.
        context.save();
		
        context.shadowBlur = 10;
        context.shadowOffsetX = shadowOffsetX;
        context.shadowOffsetY = shadowOffsetY;
        context.shadowColor = "black";
        context.fillStyle = 'white';
        context.fillRect(c.x, c.y, c.w, c.h);
		
        context.restore();
	});
	
	if(t.layerCache === undefined || t.renderCache === undefined) {
		context.restore();
		
		return;
	}
	
	t.renderedLayers = [];
	
    // Only render the newest version of every layer.
    t.layerCache.getAll(time, function(layer) {
        var r = t.renderCache.get(time, layer);
		
        if(r !== undefined) {
			t.renderedLayers.push(r);
			
			context.save();
            context.drawImage(r.img, r.x, -r.y, r.w, r.h);

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
			
			context.restore();
        }
    });
	
    // Draw a line around the influenced region.
    if (influence.w > 0 && influence.h > 0) {
        context.fillStyle = "rgba(255,0,0,.2)";
        context.fillRect(influence.x, -influence.y, influence.w, influence.h);
    }

    // Restore the untransformed canvas state.
    context.restore();
}

DocumentRenderer.prototype.getPalette = function() {
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

/**
 * Loads a set of images from a server and keeps a cached version of the bitmaps.
 */
function DocumentRendererCache() {
    var t = this;

    t.endpointUrl;
    t.renders = {};
    t.loading = false;
    t.loaded = false;
};

DocumentRendererCache.prototype.load = function(data, complete) {
	var t = this;
	
    // Sanitize parameters.
	if (data === undefined) { return; }
    if (data.length === undefined) { data = [data]; }

    t.loading = true;
    t.loaded = false;
	
	var count = data.length;

	// This callback counts down the items to be loaded.
	var completed = function (data, i) {
		count--;

		if (0 === count && complete !== undefined) {
            t.loading = false;
            t.loaded = true;

			complete(t.renders);
		}
	};

	// Invoke each action and await callback.
	for (var i = 0; i < data.length; i++) {
		t.loadRender(data, i, completed);
	}
};

DocumentRendererCache.prototype.loadRender = function (data, i, complete) {
	var t = this;
    var d = data[i];
    var r = new Image();
	
    r.onerror = function () {
        complete(data, i);
    };

    r.onload = function () {
        r.onload = undefined;

        var R = [];

        if(d.layer in t.renders) {
            R = t.renders[d.layer];
        } else {
            t.renders[d.layer] = R;
        }

        var render = {
            img: r,
            time: new Date(d.time),
            type: d.type,
            x: d.x,
            y: d.y,
            w: d.w,
            h: d.h
        };
		
        R.push(render);
		
		// Ensure that the array is sorted for quicker access when rendering.
		R.sort(function(a,b) { return b.time - a.time; });

        complete(data, i);
    };
	
    r.crossOrigin = 'Anonymous'; // Needed for color thief.
    r.src = t.endpointUrl + d.file;
};

DocumentRendererCache.prototype.get = function(time, layer, fn) {
	var t = this;
	
    if(layer !== undefined && layer.uri in t.renders) {
        var R = t.renders[layer.uri];
		
		console.log(R);
		
        for(var i = 0; i < R.length; i++) {
            var r = R[i];
			
            if(r.time <= time) {
                return r;
            }
        }
    }

    return undefined;
};

// HEATMAP

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
};

HeatmapRenderer.prototype.sample = function(bounds) {
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

HeatmapRenderer.prototype.render = function(bounds) {
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