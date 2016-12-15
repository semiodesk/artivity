(function () {

'use strict';

// Warn if overriding existing method
if (Array.prototype.equals) {
	console.warn("Overriding existing Array.prototype.equals. Possible causes: New API defines the method, there's a framework conflict or you've got double inclusions in your code.");
}

// attach the .equals method to Array's prototype to call it on any array
Array.prototype.equals = function (array) {
	// if the other array is a falsy value, return
	if (!array)
		return false;

	// compare lengths - can save a lot of time 
	if (this.length != array.length)
		return false;

	for (var i = 0, l = this.length; i < l; i++) {
		// Check if we have nested arrays
		if (this[i] instanceof Array && array[i] instanceof Array) {
			// recurse into the nested arrays
			if (!this[i].equals(array[i]))
				return false;
		} else if (this[i] != array[i]) {
			// Warning - two different object instances will never be equal: {x:20} != {x:20}
			return false;
		}
	}
	return true;
}

// Hide method from for-in loops
Object.defineProperty(Array.prototype, "equals", {
	enumerable: false
});
function each(array, fn) {
    if (array !== undefined) {
        for (var i = 0; i < array.length; i++) {
            fn(i, array[i]);
        }
    }
};

function values(array, fn) {
    if (array !== undefined) {
        for (var k in array) {
            if (array.hasOwnProperty(k)) {
                fn(k, array[k]);
            }
        }
    }
};

function serialize(obj, prefix) {
	var str = [];

	for (var p in obj) {
		if (obj.hasOwnProperty(p)) {
			var k = prefix ? prefix + "[" + p + "]" : p,
				v = obj[p];
			str.push(typeof v == "object" ?
				serialize(v, k) :
				encodeURIComponent(k) + "=" + encodeURIComponent(v));
		}
	}

	return str.join("&");
}

function getValue(obj, path) {
	for (var i = 0, path = path.split('.'), len = path.length; i < len; i++) {
		if (obj === undefined) {
			break;
		}

		obj = obj[path[i]];
	}
	return obj;
}

function setValue(obj, path, value) {
	var p;

	for (var i = 0, path = path.split('.'), len = path.length - 1; i < len; i++) {
		obj = obj[path[i]];

		p = path[i + 1];
	}

	if (p) {
		obj[p] = value;
	}
}

function loadItems(items, action, done) {
	if (!items) {
		return;
	}

	// convert single item to array.
	if ("undefined" === items.length) {
		items = [items];
	}

	var count = items.length;

	// this callback counts down the things to do.
	var completed = function (items, i) {
		count--;

		if (0 == count) {
			done(items);
		}
	};

	// invoke each action, and await callback.
	for (var i = 0; i < items.length; i++) {
		action(items, i, completed);
	}
}
var reloadUrl = 'http://localhost:35729/livereload.js?host=localhost';

$.get(reloadUrl, function() {
    var script = $('#livereload');

    if(script !== undefined) {
	    $('#livereload').attr('src', reloadUrl);

        console.log('Enabled live-reload:', reloadUrl);
    }
});
function showOverlay(id) {
    var element = $(id);

    if (element !== undefined) {
        $('.overlay .msg').each(function(i, element) {
            $(element).addClass('invisible');
        });
        
        element.removeClass('invisible');

        var overlay = $('#overlay');
        overlay.css('opacity', 0);
        overlay.removeClass('collapse');
        overlay.animate({
            opacity: 1.0
        }, 500);
    }
}

function hideOverlays() {
    var overlay = $('#overlay');

    overlay.animate({
        opacity: 0
    }, 500, function() {
        overlay.addClass('collapse');

        $('.overlay .msg').each(function(i, element) {
            $(element).addClass('invisible');
        });
    });
}
/*! 
 * quantize.js Copyright 2008 Nick Rabinowitz.
 * Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
 */

// fill out a couple protovis dependencies
/*!
 * Block below copied from Protovis: http://mbostock.github.com/protovis/
 * Copyright 2010 Stanford Visualization Group
 * Licensed under the BSD License: http://www.opensource.org/licenses/bsd-license.php
 */
if (!pv) {
    var pv = {
        map: function(array, f) {
          var o = {};
          return f
              ? array.map(function(d, i) { o.index = i; return f.call(o, d); })
              : array.slice();
        },
        naturalOrder: function(a, b) {
            return (a < b) ? -1 : ((a > b) ? 1 : 0);
        },
        sum: function(array, f) {
          var o = {};
          return array.reduce(f
              ? function(p, d, i) { o.index = i; return p + f.call(o, d); }
              : function(p, d) { return p + d; }, 0);
        },
        max: function(array, f) {
          return Math.max.apply(null, f ? pv.map(array, f) : array);
        }
    }
}
 
/**
 * Basic Javascript port of the MMCQ (modified median cut quantization)
 * algorithm from the Leptonica library (http://www.leptonica.com/).
 * Returns a color map you can use to map original pixels to the reduced
 * palette. Still a work in progress.
 * 
 * @author Nick Rabinowitz
 * @example

// array of pixels as [R,G,B] arrays
var myPixels = [[190,197,190], [202,204,200], [207,214,210], [211,214,211], [205,207,207]
                // etc
                ];
var maxColors = 4;

var cmap = MMCQ.quantize(myPixels, maxColors);
var newPalette = cmap.palette();
var newPixels = myPixels.map(function(p) { 
    return cmap.map(p); 
});
 
 */
var MMCQ = (function() {
    // private constants
    var sigbits = 5,
        rshift = 8 - sigbits,
        maxIterations = 1000,
        fractByPopulations = 0.75;
    
    // get reduced-space color index for a pixel
    function getColorIndex(r, g, b) {
        return (r << (2 * sigbits)) + (g << sigbits) + b;
    }
    
    // Simple priority queue
    function PQueue(comparator) {
        var contents = [],
            sorted = false;
        
        function sort() {
            contents.sort(comparator);
            sorted = true;
        }
        
        return {
            push: function(o) {
                contents.push(o);
                sorted = false;
            },
            peek: function(index) {
                if (!sorted) sort();
                if (index===undefined) index = contents.length - 1;
                return contents[index];
            },
            pop: function() {
                if (!sorted) sort();
                return contents.pop();
            },
            size: function() {
                return contents.length;
            },
            map: function(f) {
                return contents.map(f);
            },
            debug: function() {
                if (!sorted) sort();
                return contents;
            }
        };
    }
    
    // 3d color space box
    function VBox(r1, r2, g1, g2, b1, b2, histo) {
        var vbox = this;
        vbox.r1 = r1;
        vbox.r2 = r2;
        vbox.g1 = g1;
        vbox.g2 = g2;
        vbox.b1 = b1;
        vbox.b2 = b2;
        vbox.histo = histo;
    }
    VBox.prototype = {
        volume: function(force) {
            var vbox = this;
            if (!vbox._volume || force) {
                vbox._volume = ((vbox.r2 - vbox.r1 + 1) * (vbox.g2 - vbox.g1 + 1) * (vbox.b2 - vbox.b1 + 1));
            }
            return vbox._volume;
        },
        count: function(force) {
            var vbox = this,
                histo = vbox.histo;
            if (!vbox._count_set || force) {
                var npix = 0,
                    i, j, k;
                for (i = vbox.r1; i <= vbox.r2; i++) {
                    for (j = vbox.g1; j <= vbox.g2; j++) {
                        for (k = vbox.b1; k <= vbox.b2; k++) {
                             index = getColorIndex(i,j,k);
                             npix += (histo[index] || 0);
                        }
                    }
                }
                vbox._count = npix;
                vbox._count_set = true;
            }
            return vbox._count;
        },
        copy: function() {
            var vbox = this;
            return new VBox(vbox.r1, vbox.r2, vbox.g1, vbox.g2, vbox.b1, vbox.b2, vbox.histo);
        },
        avg: function(force) {
            var vbox = this,
                histo = vbox.histo;
            if (!vbox._avg || force) {
                var ntot = 0,
                    mult = 1 << (8 - sigbits),
                    rsum = 0,
                    gsum = 0,
                    bsum = 0,
                    hval,
                    i, j, k, histoindex;
                for (i = vbox.r1; i <= vbox.r2; i++) {
                    for (j = vbox.g1; j <= vbox.g2; j++) {
                        for (k = vbox.b1; k <= vbox.b2; k++) {
                             histoindex = getColorIndex(i,j,k);
                             hval = histo[histoindex] || 0;
                             ntot += hval;
                             rsum += (hval * (i + 0.5) * mult);
                             gsum += (hval * (j + 0.5) * mult);
                             bsum += (hval * (k + 0.5) * mult);
                        }
                    }
                }
                if (ntot) {
                    vbox._avg = [~~(rsum/ntot), ~~(gsum/ntot), ~~(bsum/ntot)];
                } else {
                    console.log('empty box');
                    vbox._avg = [
                        ~~(mult * (vbox.r1 + vbox.r2 + 1) / 2),
                        ~~(mult * (vbox.g1 + vbox.g2 + 1) / 2),
                        ~~(mult * (vbox.b1 + vbox.b2 + 1) / 2)
                    ];
                }
            }
            return vbox._avg;
        },
        contains: function(pixel) {
            var vbox = this,
                rval = pixel[0] >> rshift;
                gval = pixel[1] >> rshift;
                bval = pixel[2] >> rshift;
            return (rval >= vbox.r1 && rval <= vbox.r2 &&
                    gval >= vbox.g1 && rval <= vbox.g2 &&
                    bval >= vbox.b1 && rval <= vbox.b2);
        }
    };
    
    // Color map
    function CMap() {
        this.vboxes = new PQueue(function(a,b) { 
            return pv.naturalOrder(
                a.vbox.count()*a.vbox.volume(), 
                b.vbox.count()*b.vbox.volume()
            ) 
        });;
    }
    CMap.prototype = {
        push: function(vbox) {
            this.vboxes.push({
                vbox: vbox,
                color: vbox.avg()
            });
        },
        palette: function() {
            return this.vboxes.map(function(vb) { return vb.color });
        },
        size: function() {
            return this.vboxes.size();
        },
        map: function(color) {
            var vboxes = this.vboxes;
            for (var i=0; i<vboxes.size(); i++) {
                if (vboxes.peek(i).vbox.contains(color)) {
                    return vboxes.peek(i).color;
                }
            }
            return this.nearest(color);
        },
        nearest: function(color) {
            var vboxes = this.vboxes,
                d1, d2, pColor;
            for (var i=0; i<vboxes.size(); i++) {
                d2 = Math.sqrt(
                    Math.pow(color[0] - vboxes.peek(i).color[0], 2) +
                    Math.pow(color[1] - vboxes.peek(i).color[1], 2) +
                    Math.pow(color[1] - vboxes.peek(i).color[1], 2)
                );
                if (d2 < d1 || d1 === undefined) {
                    d1 = d2;
                    pColor = vboxes.peek(i).color;
                }
            }
            return pColor;
        },
        forcebw: function() {
            // XXX: won't  work yet
            var vboxes = this.vboxes;
            vboxes.sort(function(a,b) { return pv.naturalOrder(pv.sum(a.color), pv.sum(b.color) )});
            
            // force darkest color to black if everything < 5
            var lowest = vboxes[0].color;
            if (lowest[0] < 5 && lowest[1] < 5 && lowest[2] < 5)
                vboxes[0].color = [0,0,0];
            
            // force lightest color to white if everything > 251
            var idx = vboxes.length-1,
                highest = vboxes[idx].color;
            if (highest[0] > 251 && highest[1] > 251 && highest[2] > 251)
                vboxes[idx].color = [255,255,255];
        }
    };
    
    // histo (1-d array, giving the number of pixels in
    // each quantized region of color space), or null on error
    function getHisto(pixels) {
        var histosize = 1 << (3 * sigbits),
            histo = new Array(histosize),
            index, rval, gval, bval;
        pixels.forEach(function(pixel) {
            rval = pixel[0] >> rshift;
            gval = pixel[1] >> rshift;
            bval = pixel[2] >> rshift;
            index = getColorIndex(rval, gval, bval);
            histo[index] = (histo[index] || 0) + 1;
        });
        return histo;
    }
    
    function vboxFromPixels(pixels, histo) {
        var rmin=1000000, rmax=0, 
            gmin=1000000, gmax=0, 
            bmin=1000000, bmax=0, 
            rval, gval, bval;
        // find min/max
        pixels.forEach(function(pixel) {
            rval = pixel[0] >> rshift;
            gval = pixel[1] >> rshift;
            bval = pixel[2] >> rshift;
            if (rval < rmin) rmin = rval;
            else if (rval > rmax) rmax = rval;
            if (gval < gmin) gmin = gval;
            else if (gval > gmax) gmax = gval;
            if (bval < bmin) bmin = bval;
            else if (bval > bmax)  bmax = bval;
        });
        return new VBox(rmin, rmax, gmin, gmax, bmin, bmax, histo);
    }
    
    function medianCutApply(histo, vbox) {
        if (!vbox.count()) return;
        
        var rw = vbox.r2 - vbox.r1 + 1,
            gw = vbox.g2 - vbox.g1 + 1,
            bw = vbox.b2 - vbox.b1 + 1,
            maxw = pv.max([rw, gw, bw]);
        // only one pixel, no split
        if (vbox.count() == 1) {
            return [vbox.copy()]
        }
        /* Find the partial sum arrays along the selected axis. */
        var total = 0,
            partialsum = [],
            lookaheadsum = [],
            i, j, k, sum, index;
        if (maxw == rw) {
            for (i = vbox.r1; i <= vbox.r2; i++) {
                sum = 0;
                for (j = vbox.g1; j <= vbox.g2; j++) {
                    for (k = vbox.b1; k <= vbox.b2; k++) {
                        index = getColorIndex(i,j,k);
                        sum += (histo[index] || 0);
                    }
                }
                total += sum;
                partialsum[i] = total;
            }
        }
        else if (maxw == gw) {
            for (i = vbox.g1; i <= vbox.g2; i++) {
                sum = 0;
                for (j = vbox.r1; j <= vbox.r2; j++) {
                    for (k = vbox.b1; k <= vbox.b2; k++) {
                        index = getColorIndex(j,i,k);
                        sum += (histo[index] || 0);
                    }
                }
                total += sum;
                partialsum[i] = total;
            }
        }
        else {  /* maxw == bw */
            for (i = vbox.b1; i <= vbox.b2; i++) {
                sum = 0;
                for (j = vbox.r1; j <= vbox.r2; j++) {
                    for (k = vbox.g1; k <= vbox.g2; k++) {
                        index = getColorIndex(j,k,i);
                        sum += (histo[index] || 0);
                    }
                }
                total += sum;
                partialsum[i] = total;
            }
        }
        partialsum.forEach(function(d,i) { 
            lookaheadsum[i] = total-d 
        });
        function doCut(color) {
            var dim1 = color + '1',
                dim2 = color + '2', 
                left, right, vbox1, vbox2, d2, count2=0;
            for (i = vbox[dim1]; i <= vbox[dim2]; i++) {
                if (partialsum[i] > total / 2) {
                    vbox1 = vbox.copy();
                    vbox2 = vbox.copy();
                    left = i - vbox[dim1];
                    right = vbox[dim2] - i;
                    if (left <= right)
                        d2 = Math.min(vbox[dim2] - 1, ~~(i + right / 2));
                    else d2 = Math.max(vbox[dim1], ~~(i - 1 - left / 2));
                    // avoid 0-count boxes
                    while (!partialsum[d2]) d2++;
                    count2 = lookaheadsum[d2];
                    while (!count2 && partialsum[d2-1]) count2 = lookaheadsum[--d2];
                    // set dimensions
                    vbox1[dim2] = d2;
                    vbox2[dim1] = vbox1[dim2] + 1;
                    console.log('vbox counts:', vbox.count(), vbox1.count(), vbox2.count());
                    return [vbox1, vbox2];
                }
            }
        
        }
        // determine the cut planes
        return maxw == rw ? doCut('r') :
            maxw == gw ? doCut('g') :
            doCut('b');
    }

    function quantize(pixels, maxcolors) {
        // short-circuit
        if (!pixels.length || maxcolors < 2 || maxcolors > 256) {
            console.log('wrong number of maxcolors');
            return false;
        }
        
        // XXX: check color content and convert to grayscale if insufficient
        
        var histo = getHisto(pixels),
            histosize = 1 << (3 * sigbits);
        
        // check that we aren't below maxcolors already
        var nColors = 0;
        histo.forEach(function() { nColors++ });
        if (nColors <= maxcolors) {
            // XXX: generate the new colors from the histo and return
        }
        
        // get the beginning vbox from the colors
        var vbox = vboxFromPixels(pixels, histo),
            pq = new PQueue(function(a,b) { return pv.naturalOrder(a.count(), b.count()) });
        pq.push(vbox);
        
        // inner function to do the iteration
        function iter(lh, target) {
            var ncolors = 1,
                niters = 0,
                vbox;
            while (niters < maxIterations) {
                vbox = lh.pop();
                if (!vbox.count())  { /* just put it back */
                    lh.push(vbox);
                    niters++;
                    continue;
                }
                // do the cut
                var vboxes = medianCutApply(histo, vbox),
                    vbox1 = vboxes[0],
                    vbox2 = vboxes[1];
                    
                if (!vbox1) {
                    console.log("vbox1 not defined; shouldn't happen!");
                    return;
                }
                lh.push(vbox1);
                if (vbox2) {  /* vbox2 can be null */
                    lh.push(vbox2);
                    ncolors++;
                }
                if (ncolors >= target) return;
                if (niters++ > maxIterations) {
                    console.log("infinite loop; perhaps too few pixels!");
                    return;
                }
            }
        }
        
        // first set of colors, sorted by population
        iter(pq, fractByPopulations * maxcolors);
        // console.log(pq.size(), pq.debug().length, pq.debug().slice());
        
        // Re-sort by the product of pixel occupancy times the size in color space.
        var pq2 = new PQueue(function(a,b) { 
            return pv.naturalOrder(a.count()*a.volume(), b.count()*b.volume()) 
        });
        while (pq.size()) {
            pq2.push(pq.pop());
        }
        
        // next set - generate the median cuts using the (npix * vol) sorting.
        iter(pq2, maxcolors - pq2.size());
        
        // calculate the actual colors
        var cmap = new CMap();
        while (pq2.size()) {
            cmap.push(pq2.pop());
        }
        
        return cmap;
    }
    
    return {
        quantize: quantize
    }
})();
(function () {
     var dragging = 0;

  window.ondragenter = (ev) => {

    if (ev.dataTransfer.files != null && ev.dataTransfer.files.length > 0 && dragging === 0) {
      var filePath = ev.dataTransfer.files[0].path;

      if (filePath.endsWith(".arta") || filePath.endsWith(".arty"))
        showOverlay("#msg-import");
      else
        showOverlay("#msg-add-app");
    }
    dragging++;
    ev.stopPropagation();
    ev.preventDefault();
    console.log(dragging);
  }

  document.ondrop = (ev) => {

    if (ev.dataTransfer.files != null && ev.dataTransfer.files.length > 0) {
      var fileUrl = require('file-url');
      var filePath = ev.dataTransfer.files[0].path;

      if (filePath.endsWith(".arta") || filePath.endsWith(".arty")) {
        var endpoint = apid.endpointUrl + '/import?fileUrl=' + fileUrl(filePath);
      } else {
        var endpoint = apid.endpointUrl + '/agents/software/paths/add?url=' + fileUrl(filePath);
      }
      $.get(endpoint)
    }
    ev.preventDefault();
    ev.stopPropagation();

    hideOverlays();
    dragging = 0;
    return false;
  }

  document.ondragover = (ev) => {
    ev.preventDefault();
  }

  window.ondragend = (ev) => {
    ev.preventDefault();

    return false;
  }

  window.ondragleave = (ev) => {

    dragging--;

    ev.stopPropagation();
    ev.preventDefault();

    if (dragging === 0) {
      hideOverlays();
      console.log("leaving");
    }

    return false;
  }
})();

(function () {

  const remote = require('electron').remote;

  function init() {
    document.getElementById("min-btn").addEventListener("click", function (e) {
      const window = remote.getCurrentWindow();
      window.minimize();
    });

    document.getElementById("max-btn").addEventListener("click", function (e) {
      const window = remote.getCurrentWindow();
      if (!window.isMaximized()) {
        window.maximize();
      } else {
        window.unmaximize();
      }
    });

    document.getElementById("close-btn").addEventListener("click", function (e) {
      remote.app.exit();
    });
  };

  document.onreadystatechange = function () {
    if (document.readyState == "complete") {
      init();
    }
  };

 
})();

(function () {

function UpdateChecker(appcastUrl, currentVersion) {

    if (currentVersion == undefined) {
        var pjson = require('./package.json');
        this.currentVersion = pjson.version;
        console.log(this.currentVersion);
    } else {
        this.currentVersion = currentVersion;
    }

    this.appcastUrl = appcastUrl;
    this.package;
    this.localPath;

    this.mostRecentUpdate = mostRecentUpdate;
    this.canUpdate = canUpdate;
    this.downloadUpdate = downloadUpdate;
    this.executeUpdate = executeUpdate;
    //    this.validateUpdate = validateUpdate;

    function mostRecentUpdate() {
        return new Promise(function (resolve, reject) {

            var FeedParser = require('feedparser');
            var request = require('request');

            var req = request(appcastUrl);
            var feedparser = new FeedParser();
            var lastItem;
            req.on('error', function (error) {
                reject();
            });

            req.on('response', function (res) {
                var stream = this;

                if (res.statusCode != 200) return this.emit('error', new Error('Bad status code'));

                stream.pipe(feedparser);
            });


            feedparser.on('error', function (error) {
                reject();
            });

            feedparser.on('readable', function () {
                var meta = this.meta; // **NOTE** the "meta" is always available in the context of the feedparser instance
                var item;

                while (item = this.read()) {
                    lastItem = item;
                }


            });

            feedparser.on('end', function () {
                var package = lastItem['rss:enclosure']['@'];
                resolve(package);
            });

        });
    }

    function canUpdate() {
        return new Promise(function (resolve, reject) {
            this.mostRecentUpdate().then(function (package) {
                var updateVer = package["sparkle:version"];
                var semver = require('semver');
                if (semver.gte(updateVer, this.currentVersion)) {
                    this.package = package;
                    resolve(updateVer);
                }
                else {
                    reject();
                }
            });
        });
    }

    function downloadUpdate() {
        return new Promise(function (resolve, reject) {

            var crypto = require("crypto");

            var fs = require("fs");

            //http://stackoverflow.com/questions/667017/how-to-check-if-a-file-has-a-digital-signature
            var signature = this.package['sparkle:dsasignature'];

            var verifier = crypto.createVerify('sha1');

            var https = require('https');
            var fs = require('fs');
            var remote = require('electron').remote;
            var app = remote.app;

            var path = app.getPath('temp') + "\\artivity-update-" + package["sparkle:version"] + ".exe";
            this.localPath = path;

            var pubkeyPath = app.getAppPath() + "/dsa_pub.pem";
            var pubkey = fs.readFileSync(pubkeyPath).toString();

            if (fs.existsSync(path)) {
                var s = fs.ReadStream(path);
                s.on('data', function (d) {
                    verifier.update(d);
                });

                s.on('end', function () {

                    console.log(verifier.verify(pubkey, signature, 'base64'));
                    resolve();
                });



            } else {
                var file = fs.createWriteStream(path);
                var request = https.get(package.url, function (response) {
                    response.pipe(file);
                    response.pipe(verifier);
                });

                request.on('close', function () {
                    verifier.end();
                    console.log(verifier.verify(pubkey, signature, 'base64'));
                    resolve();
                });
            }
        });



    }

    function executeUpdate()
    {

    }
}

})();
/**
 * Caches a set of canvases.
 */
function CanvasCache() {
    var t = this;

    EntityCache.call(t);

    t.canvases = [];
};

CanvasCache.prototype = Object.create(EntityCache.prototype);

CanvasCache.prototype.load = function (data, complete) {
    var t = this;

    each(data, function (i, influence) {
        var time = new Date(influence.time);
        var canvas = t.entities[influence.uri];

        if (canvas === undefined) {
            canvas = new Entity(influence.uri);

            t.entities[influence.uri] = canvas;
        };

        if (influence.type === 'http://www.w3.org/ns/prov#Generation') {
            canvas.creationTime = time;
        }

        if (influence.property !== undefined) {
            canvas.pushValue(time, influence.property, influence.value);
        }

        canvas.pushValue(time, 'http://w3id.org/art/terms/1.0/x', influence.x);
        canvas.pushValue(time, 'http://w3id.org/art/terms/1.0/y', influence.y);
        canvas.pushValue(time, 'http://w3id.org/art/terms/1.0/width', influence.w);
        canvas.pushValue(time, 'http://w3id.org/art/terms/1.0/height', influence.h);
    });

    if (complete !== undefined) {
        complete(t.entities);
    }
};

CanvasCache.prototype.getAll = function (time, fn) {
    var t = this;

    if (fn) {
        values(t.entities, function (key, canvas) {
            if (time >= canvas.creationTime) {
                // Set the proper values for the bounds at the givent time.
                canvas.x = canvas.getValue(time, 'http://w3id.org/art/terms/1.0/x');
                canvas.y = canvas.getValue(time, 'http://w3id.org/art/terms/1.0/y');
                canvas.w = canvas.getValue(time, 'http://w3id.org/art/terms/1.0/width');
                canvas.h = canvas.getValue(time, 'http://w3id.org/art/terms/1.0/height');

                fn(canvas);
            }
        });
    };
};
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

DocumentRendererCache.prototype.load = function (data, complete) {
    var t = this;

    // Sanitize parameters.
    if (data === undefined) {
        return;
    }
    if (data.length === undefined) {
        data = [data];
    }

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

        if (d.layer in t.renders) {
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
        R.sort(function (a, b) {
            return b.time - a.time;
        });

        complete(data, i);
    };

    r.crossOrigin = 'Anonymous'; // Needed for color thief.
    r.src = t.endpointUrl + d.file;
};

DocumentRendererCache.prototype.get = function (time, layer, fn) {
    var t = this;

    if (layer !== undefined && layer.uri in t.renders) {
        var R = t.renders[layer.uri];

        for (var i = 0; i < R.length; i++) {
            var r = R[i];

            if (r.time <= time) {
                return r;
            }
        }
    }

    return undefined;
};
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

    // The scene element which is being manipulated when zooming or panning.
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
            console.log("Redrawing..");

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

DocumentRenderer.prototype.render = function (influence) {
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

        t.measureExtents(extents, c.x, -c.y, c.w, c.h);

        t.scene.addChild(s);
    });

    if (t.layerCache === undefined || t.renderCache === undefined) {
        t.stage.update();

        return;
    }

    t.renderedLayers = [];

    // Only render the newest version of every layer.
    t.layerCache.getAll(time, function (layer) {
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
    });

    // Draw a line around the influenced region.
    if (influence.w > 0 && influence.h > 0) {
        var s = new createjs.Shape();

        var g = s.graphics;
        g.beginFill('rgba(255,0,0,.2)');
        g.drawRect(influence.x, -influence.y, influence.w, influence.h);

        t.scene.addChild(s);
    }

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
        //t.drawSceneMarkers(extents, cc, ce);

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

DocumentRenderer.prototype.zoomToFit = function (extents) {
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

DocumentRenderer.prototype.measureExtents = function (extents, x, y, w, h) {
    extents.l = Math.min(extents.l, x);
    extents.r = Math.max(extents.r, x + w);
    extents.t = Math.min(extents.t, -y);
    extents.b = Math.max(extents.b, -y + h);
    extents.x = extents.l;
    extents.y = extents.t;
    extents.width = extents.r - extents.l;
    extents.height = extents.b - extents.t;
}

DocumentRenderer.prototype.drawSceneMarkers = function (extents, cc, ce) {
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

DocumentRenderer.prototype.getPalette = function () {
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
// STATS
function EditingStatistics() {
    var t = this;

    t.stepCount = 0;
    t.undoCount = 0;
    t.redoCount = 0;
};

EditingStatistics.prototype.confidence = function () {
    var t = this;

    if (t.stepCount > 0) {
        return (100 * (t.stepCount - t.undoCount - t.redoCount) / t.stepCount).toFixed(0);
    } else {
        return 100;
    }
}
/**
 * Caches a set of entities. The class provides methods for 
 * accessing a subset of the stored entities depending on
 * the time they were created.
 */
function EntityCache() {
    var t = this;

    // A lookup table for all entities.
    t.entities = {};

    // The list of entities in the order they were modified, from newest to oldest.
    t.entityHistory = [];
};

EntityCache.prototype.length = function () {
    var t = this;

    return t.entities.length;
};

EntityCache.prototype.load = function (data, complete) {
    var t = this;

    each(data, function (i, influence) {
        if (influence.time !== undefined && influence.uri !== undefined) {
            var time = new Date(influence.time);
            var entity = t.entities[influence.uri];

            if (entity === undefined) {
                entity = new Entity(influence.uri);

                t.entities[influence.uri] = entity;

                t.entityHistory.push(entity);
            };

            if (influence.type === 'http://www.w3.org/ns/prov#Generation') {
                entity.creationTime = time;
            } else if (influence.type === 'http://www.w3.org/ns/prov#Invalidation') {
                entity.deletionTime = time;
            }

            if (influence.property !== undefined) {
                entity.pushValue(time, influence.property, influence.value);
            }
        }
    });

    if (complete !== undefined) {
        complete(t.entities);
    }
};

EntityCache.prototype.get = function (uri) {
    return this.entities[uri];
};

EntityCache.prototype.getAll = function (time, fn) {
    var t = this;

    values(t.entities, function (key, entity) {
        if (time >= entity.creationTime) {
            fn(entity);
        }
    });
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

    P.push({
        time: time,
        value: value
    });
};

Entity.prototype.getValue = function (time, property) {
    var t = this;
    var P = t.properties[property];

    if (P === undefined) {
        return undefined;
    }

    for (var i = 0; i < P.length; i++) {
        var p = P[i];

        if (p.time <= time) {
            return p.value;
        }
    }

    return undefined;
};

Entity.prototype.getLabel = function (time) {
    return this.getValue(time, 'http://www.w3.org/2000/01/rdf-schema#label');
};
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
LayerCache.prototype.getAll = function (time, fn) {
    var t = this;

    if (fn) {
        // We reconstruct the layer tree from the cache.
        var tree = new LayerTree();

        // We iterate the layers from oldest to the newest to ensure,
        // that the nodes for layers already exist in the tree.
        for (var i = t.entityHistory.length - 1; i >= 0; i--) {
            var layer = t.entityHistory[i];

            // Only consider the layers that existed at the time.
            if (layer.creationTime <= time && time < layer.deletionTime) {
                var lowerUri = layer.getValue(time, 'http://w3id.org/art/terms/1.0/aboveLayer');
                var parentUri = layer.getValue(time, 'http://w3id.org/art/terms/1.0/parentLayer');

                // In the beginning, there were no layers..
                if (parentUri === undefined && (lowerUri === undefined || lowerUri === 'http://w3id.org/art/terms/1.0/noLayer')) {
                    tree.insertLayer(layer);

                    continue;
                }

                if (parentUri !== undefined) {
                    var parentLayer = t.entities[parentUri];

                    // It may happen (with Isolation mode layers in Illustrator), 
                    // that the parent layer has been deleted but the layer itself was not. 
                    // In that case, just insert the layer with no parent set.
                    if (parentLayer !== undefined && time < parentLayer.deletionTime) {
                        tree.insertLayer(layer, parentLayer);
                    } else if (parentLayer.deletionTime <= time) {
                        tree.insertLayer(layer);
                    }
                }

                if (lowerUri !== undefined && lowerUri !== 'http://w3id.org/art/terms/1.0/noLayer') {
                    var lowerLayer = t.entities[lowerUri];

                    if (lowerLayer !== undefined) {
                        tree.insertLayerAbove(layer, lowerLayer);
                    }
                }
            }
        }

        tree.traverse(function (node, parent, i, d) {
            fn(node.layer, d);
        });
    }
}
/**
 * A node in a layer tree.
 */
function LayerTreeNode(tree, parent, layer) {
    var t = this;

    t.layer = layer;
    t.parent = parent;

    // The layer children in rendering order, from bottom to top.
    t.children = [];

    // Register the node in the tree.
    if (layer != null && layer.uri !== undefined && tree !== undefined) {
        tree.nodes.set(layer.uri, t);
    }
};
/**
 * A layer tree.
 */
function LayerTree() {
    var t = this;

    // We always have a root not with a null-parent.
    t.root = new LayerTreeNode(t, null, null);

    // Maps layer URIs to tree nodes for fast lookup.
    t.nodes = new Map();
};

/**
 * Traverses the layers in rendering order: from bottom to top.
 */
LayerTree.prototype.traverse = function (callback) {
    var t = this;

    // NOTE: This is a recurse and immediately-invoking function.
    (function recurse(node, parent, i, d) {
        // Traverse the children from bottom to top.
        for (var i = node.children.length - 1; i >= 0; i--) {
            if (recurse(node.children[i], node, i, d + 1)) {
                return true;
            }
        }

        // Do not invoke the callback for the root node.
        if (0 <= d && callback(node, parent, i, d)) {
            return true;
        }
    })
    (t.root, t.root.parent, 0, -1);
};

/**
 * Insert a layer as a child of a parent layer. If no parent layer
 * is given, the layer is inserted at root level.
 */
LayerTree.prototype.insertLayer = function (layer, parentLayer) {
    var t = this;

    var node = t.nodes.get(layer.uri);

    if (node !== undefined) {
        // The node is already in the tree.
        return node;
    }

    // By default, insert the node at the root of the tree.
    var parent = t.root;

    if (parentLayer !== undefined) {
        parent = t.nodes.get(parentLayer.uri);
    }

    if (layer != null && parent !== undefined) {
        node = new LayerTreeNode(t, parent, layer);

        parent.children.splice(0, 0, node);
    }

    return node;
}

/**
 * Insert a layer on top of another layer.
 */
LayerTree.prototype.insertLayerAbove = function (layer, lowerLayer) {
    var t = this;

    var node = t.nodes.get(layer.uri);

    if (node !== undefined) {
        // The node is already in the tree.
        return node;
    }

    var lower = t.nodes.get(lowerLayer.uri);

    if (lower === undefined && lowerLayer.uri !== undefined) {
        lower = t.insertLayer(lowerLayer);
    }

    if (layer != null && lower !== undefined) {
        var parent = lower.parent;

        node = new LayerTreeNode(t, parent, layer);

        var i = parent.children.indexOf(lower);

        if (i === 0) {
            parent.children.splice(0, 0, node);
        } else if (i > 0) {
            parent.children.splice(i - 1, 0, node);
        }
    }

    return node;
}
/**
 * A level at which you can place an object or image file.
 */
function Layer(uri) {};

Layer.prototype = Object.create(Entity.prototype);
angular.module('explorerApp').filter('reverse', ReverseFilter);

function ReverseFilter() {
    return function (items) {
        if (items !== undefined && items.length > 1) {
            return items.slice().reverse();
        } else {
            return items;
        }
    }
}
angular.module('explorerApp').factory('clientService', function() {
	return {
		pollServiceState : function (api, sessionId, handleClientStateChange) {
			// The polling interval hanler.
			var interval = undefined;

			// The polling interval in milliseconds.
			var intervalMs = 500;

			// The maximum number of queries.
			var maxQueries = 500;

			// The number of status queries.
			var n = 0;

			// Stop polling if errors occur.
			var onError = function (data) {
				if (interval) {
					clearInterval(interval);
				}
			};

			// Query the current client status in a regular interval.
			var interval = setInterval(function () {
				api.getAccountClientStatus(sessionId, onError).then(function (data) {
					var client = data;

					if (!client) {
						// The sessionId was not found, no need to query again.
						clearInterval(interval);
					}

					n++;

					if (handleClientStateChange) {
						handleClientStateChange(interval, client);
					}

					if (n === maxQueries) {
						clearInterval(interval);
					}
				});
			}, intervalMs);
		}
	}
});
angular.module('explorerApp').factory('fileService', function () {
	return {
		getFileName: function (fileUrl) {
			var url = fileUrl;

			// Remove the anchor at the end, if there is one
			url = url.substring(0, (url.indexOf("#") == -1) ? url.length : url.indexOf("#"));

			// Remove the query after the file name, if there is one
			url = url.substring(0, (url.indexOf("?") == -1) ? url.length : url.indexOf("?"));

			// Remove everything before the last slash in the path
			url = url.substring(url.lastIndexOf("/") + 1, url.length);

			return decodeURIComponent(url);
		},
		getFileNameWithoutExtension: function (fileName) {
			var components = fileName.split('.');

			// Return the file name which does not contain any dot.
			return components[0];
		},
		getFileExtension: function (fileName) {
			var components = fileName.split('.');

			if (components.length == 1) {
				// Return the file name which does not contain any dot.
				return components[0];
			} else {
				// Return all components which are seperated by a dot. i.e. 'tar.gz'
				return components.slice(1).join('.');
			}
		}
	};
});
angular.module('explorerApp').factory('settingsService', function () {
    var controllers = [];

	return {
		controllers: function(callback) {
			if(callback) {
				for(var i = 0; i < controllers.length; i++) {
					callback(controllers[i]);
				}
			}
		},
		registerController: function (c) {
            if(controllers.indexOf(c) === -1) {
			    controllers.push(c);
            }
		}
	};
});
(function () {
    'use strict';

    angular
        .module('explorerApp')
        .factory('UpdateService', UpdateService);

    UpdateService.$inject = [];
    function UpdateService() {

		var endpoint = "https://static.semiodesk.com/artivity/osx/appcast.xml";
        var service = {
			canUpdate: canUpdate,
			download: download,
			update: update

		};
		initService();
        return service;

		//////////////////////////////

		function initService(){
			service.checker = new UpdateChecker(endpoint);
		}

		//////////////////////////////

        function canUpdate(){
			return service.checker.canUpdate();
		}

		function download() {
			return service.checker.downloadUpdate();
		}

		function update(){
			return service.checker.executeUpdate();
		}
    }

})();

// TODO: pull out of this file to keep it unit testable
//angular.module('explorerApp').run(['UpdateService', function(UpdateService) {
//;
//}]);
angular.module('explorerApp').directive('artCalendar', CalendarDirective);

function CalendarDirective() {
    return {
        scope: true,
        template: '',
        link: function(scope, element, attributes, api, $log, $uibModal) {
            var s = scope;

            var options = {
                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'month,agendaWeek,agendaDay'
                    },
                    defaultView: 'agendaWeek',
                    businessHours: {
                        dow: [1, 2, 3, 4, 5], // Monday - Friday
                        start: '9:00',
                        end: '17:00',
                    },
                    height: function () {
                        return $('.modal-body').innerHeight() - 20;
                    }
                };

                var element = $(element);
                element.fullCalendar(options);

                s.getActivities().then(function (data) {
                    var activities = [];

                    for (var i = 0; i < data.length; i++) {
                        var activity = data[i];
                        activity.title = '';
                        activity.start = new Date(activity.startTime);
                        activity.color = activity.agentColor;

                        if (activity.endTime) {
                            activity.end = new Date(activity.endTime);
                        } else if (activity.maxTime) {
                            activity.end = new Date(activity.maxTime);
                        } else {
                            activity.end = new Date(activity.startTime);
                            activity.end.setSeconds(activity.end.getSeconds() + 30);
                        }

                        activities.push(activity);
                    }

                    console.log("Activities:", activities);

                    if (activities && activities.length > 0) {
                        element.fullCalendar('render');
                        element.fullCalendar('addEventSource', activities);
                        element.fullCalendar('gotoDate', activities[0].start);
                    }

                    s.dialog.rendered.then(function () {
                        s.isLoading = false;

                        element.fullCalendar('render');
                    });
                });
        }
    }
};
angular.module('explorerApp').directive('artColorPicker', ColorPickerDirective);

function ColorPickerDirective() {
    return {
        template: '<button type="button" class="btn btn-colorpicker"><span class="color-fill-icon"><i></i></span></button>',
        link: function (scope, element, attributes) {
            var indicator = $('.color-fill-icon', element);
            indicator.css('background-color', getValue(scope, attributes.selectedColor));

            var button = $(element);

            button.on('changeColor', function (e) {
                if (e.color == null) {
                    //when select transparent color
                    //$('.color-fill-icon', btn).addClass('colorpicker-color');
                } else {
                    //$('.color-fill-icon', btn).removeClass('colorpicker-color');
                    indicator.css('background-color', e.color);

                    // Update the bound value.
                    setValue(scope, attributes.selectedColor, e.color.toHex());
                }
            });

            button.colorpicker({
                customClass: 'colorpicker-lg',
                align: 'right',
                format: 'rgb',
                color: getValue(scope, attributes.selectedColor),
                colorSelectors: {
                    'default': '#777777',
                    'primary': '#337ab7',
                    'success': '#5cb85c',
                    'info': '#5bc0de',
                    'warning': '#f0ad4e',
                    'danger': '#d9534f'
                },
                sliders: {
                    saturation: {
                        maxLeft: 200,
                        maxTop: 200
                    },
                    hue: {
                        maxTop: 200
                    },
                    alpha: {
                        maxTop: 200
                    }
                }
            });
        }
    }
}
angular.module('explorerApp').directive('artKeyboardInputHandler', KeyboardInputHandlerDirective);

function KeyboardInputHandlerDirective() {
    return {
        scope: true,
        template: '',
        controller: KeyboardInputHandlerDirectiveController
    }
};

function KeyboardInputHandlerDirectiveController (api, $scope, hotkeys) {
    $scope.navigateTo = function (path) {
        var url = window.location.href.split('#');

        if (url.length < 2) {
            console.log('Navigation failed; unable to parse fragment from url:' + window.location.href);

            return;
        }

        window.location.href = url[0].replace(/index.html/i, '') + path;
    };

    $scope.navigateToFragment = function (pathFragment) {
        var url = window.location.href.split('#');

        if (url.length < 2) {
            console.log('Navigation failed; unable to parse fragment from url:' + window.location.href);

            return;
        }

        window.location.href = url[0] + '#' + pathFragment;
    };

    hotkeys.add({
        combo: 'backspace',
        description: 'Go back to the previous view.',
        callback: function () {
            window.history.back();
        }
    });

    hotkeys.add({
        combo: 'shift+backspace',
        description: 'Go forward to the next view.',
        callback: function () {
            window.history.forward();
        }
    });

    hotkeys.add({
        combo: 'alt+h',
        description: 'Go to the dashboard view.',
        callback: function () {
            // This will be replaced with the correct home page by the route provider.
            $scope.navigateToFragment('/');
        }
    });

    hotkeys.add({
        combo: 'alt+q',
        description: 'Open the SPARQL query editor view.',
        callback: function () {
            $scope.navigateTo('query.html');
        }
    });
}
angular.module('explorerApp').directive('artPhotoPicker', PhotoPickerDirective);

function PhotoPickerDirective() {
    return {
        link: function (scope, element, attributes) {
            element.bind("change", function (changeEvent) {
                scope.$apply(function () {
                    // Store the selected picture in the model for saving when the changes are applied.
                    scope.userPhoto = changeEvent.target.files[0];
                });
            });
        }
    }
}
var app = angular.module('explorerApp');

app.directive('artSettingsAccounts', AccountsSettingsDirective);

function AccountsSettingsDirective() {
    return {
        scope: {},
        templateUrl: 'partials/directives/art-settings-accounts.html',
        controller: AccountsSettingsDirectiveFormController
    }
};

app.controller('AccountsSettingsDirectiveFormController', AccountsSettingsDirectiveFormController);

function AccountsSettingsDirectiveFormController (api, $scope, settingsService, $uibModal) {
    var t = this;
    var s = $scope;

    if(settingsService) {
        // Register the controller with its parent for global apply/cancel.
        settingsService.registerController(t);
    }

    s.selectedItem = null;

    // Load the user accounts.
    s.accounts = [];

    api.getAccounts().then(function (data) {
        s.accounts = data;
    });

    s.addAccount = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'partials/dialogs/add-account-dialog.html',
            controller: 'AddAccountDialogController'
        });

        modalInstance.result.then(function (account) {
            console.log("Reloading Accounts");

            // Reload the user accounts.
            api.getAccounts().then(function (data) {
                s.accounts = data;
            });
        });
    };

    s.selectAccount = function (account) {
        s.selectedItem = account;
    };

    s.uninstallAccount = function (a) {
        api.uninstallAccount(a.Uri).then(function (data) {
            console.log("Account uninstalled:", a.Uri);

            var i = s.accounts.indexOf(a);

            s.accounts.splice(i, 1);
        });
    };

    t.submit = function () {
        // Changes are handled by the addAccount and uninstallAccount functions.
        // Nothing to sumbmit here, yet.
    };

    t.reset = function () {
        s.form.reset();
    };
};
var app = angular.module('explorerApp');

app.directive('artSettingsApps', AppsSettingsDirective);

function AppsSettingsDirective() {
    return {
        scope: {},
        templateUrl: 'partials/directives/art-settings-apps.html',
        controller: AppsSettingsDirectiveFormController
    }
};

app.controller('AppsSettingsDirectiveFormController', AppsSettingsDirectiveFormController);

function AppsSettingsDirectiveFormController (api, $scope, settingsService) {
    var t = this;
    var s = $scope;

    if(settingsService) {
        // Register the controller with its parent for global apply/cancel.
        settingsService.registerController(t);
    }

    s.agents = [];

    s.hasError = false;
    s.errorType = '';
    s.errorMessage = '';

    s.toggleInstall = function (agent) {
        if (agent.pluginInstalled) {
            api.installAgent(agent.uri).then(function (response) {
                agent.pluginInstalled = response.success;

                s.hasError = !response.success;
                s.errorType = response.error.data.type;
                s.errorMessage = response.error.data.message;
            });
        } else {
            api.uninstallAgent(agent.uri).then(function (response) {
                agent.pluginInstalled = !response.success;

                s.hasError = !response.success;
                s.errorType = response.error.data.type;
                s.errorMessage = response.error.data.message;
            });
        }
    };

    s.reload = function () {
        s.hasError = false;

        api.getAgents().then(function (data) {
            s.agents = [];

            for (var i = 0; i < data.length; i++) {
                var agent = data[i];

                if (agent.IsSoftwareInstalled) {
                    s.agents.push({
                        uri: agent.Manifest.AgentUri,
                        name: agent.Manifest.DisplayName,
                        color: agent.Manifest.DefaultColor,
                        associationUri: agent.AssociationUri,
                        iconSrc: api.getAgentIconUrl(agent.Manifest.AgentUri),
                        softwareInstalled: agent.IsSoftwareInstalled,
                        softwareVersion: agent.DetectedVersion,
                        pluginInstalled: agent.IsPluginInstalled,
                        pluginVersion: agent.Manifest.PluginVersion,
                        pluginEnabled: agent.IsPluginEnabled
                    });
                }
            }

            s.form.$setPristine();
        });
    }

    s.reload();

    t.submit = function () {
        if (s.agents.length > 0) {
            console.log("Submitting Apps");

            api.setAgents(s.agents);
        }
    };

    t.reset = function () {
        s.form.reset();
    };
};
var app = angular.module('explorerApp');

app.directive('artSettingsUser', UserSettingsDirective);

function UserSettingsDirective() {
    return {
        scope: {
            backupEnabled: '@backupEnabled'
        },
        templateUrl: 'partials/directives/art-settings-user.html',
        controller: UserSettingsDirectiveFormController
    }
};

app.controller('UserSettingsDirectiveFormController', UserSettingsDirectiveFormController);

function UserSettingsDirectiveFormController (api, $scope, settingsService) {
    var t = this;
    var s = $scope;

    if(settingsService) {
        // Register the controller with its parent for global apply/cancel.
        settingsService.registerController(t);
    }

    // Load the user data.
    api.getUser().then(function (data) {
        s.user = data;
    });

    // Set the user photo URL.
    s.userPhotoUrl = api.getUserPhotoUrl();

    s.onPhotoChanged = function (e) {
        // Update the preview image..
        var files = window.event.srcElement.files;

        if (FileReader && files.length) {
            var reader = new FileReader();

            reader.onload = function () {
                document.getElementById('photo-img').src = reader.result;
            }

            reader.readAsDataURL(files[0]);

            s.form.$pristine = false;
        }
    };

    // Set attribute default values.
    if(s.backupEnabled === undefined) {
        s.backupEnabled = true;
    }
    
    s.backupStatus = null;
    
    s.createBackup = function () {
        var fileName = 'Unknown';
        
        if(s.user.Name) {
            fileName = s.user.Name;
        }
        
        fileName += '-' + moment().format('DDMMYYYY') + '.artb';
        
        console.log("Creating backup to file:", fileName);
        
        api.backupUserProfile(fileName).then(function (data) {
            s.backupStatus = data;
            
            if(s.backupStatus.Id && s.backupStatus.Error === null) {
                var interval = setInterval(function() {					
                    api.getUserProfileBackupStatus(s.backupStatus.Id).then(function(data) {
                        s.backupStatus.PercentComplete = data.PercentComplete;
                        s.backupStatus.Error = data.Error;
                        
                        if(s.backupStatus.PercentComplete === 100 || s.backupStatus.Error) {
                            clearInterval(interval);
                            
                            if(s.backupStatus.Error) {
                                console.log(s.backupStatus.Error);
                            }
                        }
                    });
                }, 500);
            }
        });
    };

    t.submit = function () {
        console.log("Submitting Profile");

        if (s.user) {
            api.setUser(s.user);
        }

        if (s.userPhoto) {
            api.setUserPhoto(s.userPhoto).then(function () {
                s.userPhotoUrl = '';
                s.userPhotoUrl = api.getUserPhotoUrl();
            });
        }
    };

    t.reset = function () {
        s.form.reset();
    };
};
angular.module('explorerApp').directive('artStyleBinder', StyleBinderDirective);

function StyleBinderDirective() {
    return {
        link: function (scope, element, attributes) {
            var template = $(element).text();

            scope.$watch(attributes.artAccentColor, function () {
                var accentColor = getValue(scope, attributes.artAccentColor);

                if (accentColor !== undefined && accentColor !== "#FF0000") {
                    var text = template.replace(/\$accentColor/g, accentColor);

                    $(element).text(text);
                }
            });
        }
    }
}
angular.module('explorerApp').directive('artTimeline', function () {
	return {
		template: '\
		<div class="timeline">\
			<div class="timeline-control"> \
				<div class="position"><label></label></div> \
				<div class="duration"><label></label></div> \
				<div class="track-col"> \
					<div class="thumb-container"></div> \
					<div class="track-container"> \
						<div class="track"></div> \
						<div class="track-preview"></div> \
						<div class="track-indicator"></div> \
						<div class="thumb draggable"><div class="thumb-knob"></div></div> \
					</div> \
					<div class="comments"></div> \
					<div class="activities"></div> \
				</div> \
			</div> \
		</div>',
		link: function (scope, element, attributes) {
			var timeline = new TimelineControl(element);

			timeline.setActivities(getValue(scope, attributes.artActivitiesSrc));
			timeline.setInfluences(getValue(scope, attributes.artInfluencesSrc));

			timeline.selectedInfluenceChanged = function (influence) {
				scope.previewInfluence(influence);
			};

			scope.$watchCollection(attributes.artActivitiesSrc, function () {
				timeline.setActivities(getValue(scope, attributes.artActivitiesSrc));
			});

			scope.$watchCollection(attributes.artInfluencesSrc, function () {
				timeline.setInfluences(getValue(scope, attributes.artInfluencesSrc));
			});

			scope.$watch('selectedInfluence', function () {
				if (scope.selectedInfluence !== undefined) {
					timeline.setPosition(scope.selectedInfluence);

					console.log(angular.element(scope.selectedInfluence));
				}
			});
		}
	}
});

function Activity(activity, timeOffset) {
	var t = this;

	t.color = activity.agentColor;
	t.lastColor = t.color;

	t.startTime = new Date(activity.startTime);
	t.endTime = new Date(activity.endTime === undefined ? activity.maxTime : activity.endTime);

	t.timeRange = {};
	t.timeRange.start = timeOffset;
	t.timeRange.length = t.endTime - t.startTime;
	t.timeRange.end = t.timeRange.start + t.timeRange.length;
};

Activity.prototype.getOffset = function (timestamp) {
	var t = this;
	var time = new Date(timestamp);

	return (time - t.startTime) + t.timeRange.start;
};

function TimelineControl(element) {
	var t = this;

	t.dragging = false;
	t.selectedIndex = 0;

	var drag = function (e) {
		var x = parseInt($(e.target).css('left')) + $(e.target).outerWidth() / 2;

		if (x) {
			//$(e.target).css('left', x);
			
			var c = Math.ceil(t.thumbKnob.outerWidth() / 2);

			t.trackIndicator.css('width', (x + c) + 'px');
			t.trackPreview.css('visibility', 'collapse');

			var y = t.track.innerWidth() / t.influences.length;
			var i = t.influences.length - Math.ceil(x / y);

			if (i > -1 && i !== t.selectedIndex) {
				t.selectedIndex = i;

				var influence = t.influences[t.selectedIndex];

				// Snap the thumb to the position of the currently selected influence.
				t.updatePositionLabels(influence);
				t.updateTrackPreview(influence);

				if (t.selectedInfluenceChanged) {
					t.selectedInfluenceChanged(t.influences[i]);
				}
			}
		}
	};

	var dragStart = function (e) {
		t.dragging = true;

		t.thumb.addClass('no-transition');
		t.trackIndicator.addClass('no-transition');
		t.trackPreview.css('visibility', 'collapse');

		var x = e.clientX - t.track.offset().left;

		if (x) {
			var c = Math.ceil(t.thumbKnob.outerWidth() / 2);

			t.trackIndicator.css('width', (x + c) + 'px');
		}
	};

	var dragStop = function (e) {
		var x = e.clientX - t.track.offset().left;

		if (x) {
			var c = Math.ceil(t.thumbKnob.outerWidth() / 2);

			t.trackIndicator.css('width', (x + c) + 'px');
		}

		t.thumb.removeClass('no-transition');
		t.trackIndicator.removeClass('no-transition');
		t.trackPreview.css('visibility', 'visible');

		var influence = t.influences[t.selectedIndex];

		// Snap the thumb to the position of the currently selected influence.
		t.updatePositionLabels(influence);
		t.updateTrackPreview(influence);

		t.dragging = false;
	};

	var trackMouseMove = function (e) {
		if (!t.dragging) {
			// e.offsetX does for some reason deliver unsteady values.
			var x = e.clientX - t.track.offset().left;

			if (x > 0 && x <= t.track.innerWidth()) {
				var influence = t.getInfluence(x);

				t.updatePositionLabels(influence);

				t.trackPreview.css('visibility', 'visible');
				t.trackPreview.css('width', x + "px");
			}
		}
	};

	var trackMouseEnter = function (e) {
		if (!t.dragging) {
			t.trackPreview.css('visibility', 'visible');
		}
	};

	var trackMouseOut = function (e) {
		if (!t.dragging) {
			t.trackPreview.css('visibility', 'collapse');
		}
	};

	var trackClick = function (e) {
		if (!t.dragging) {
			// e.offsetX does for some reason deliver unsteady values.
			var x = e.clientX - t.track.offset().left;

			var influence = t.getInfluence(x);

			var i = t.influences.indexOf(influence);

			if (i > -1 && i !== t.selectedIndex) {
				t.setPosition(influence);

				if (t.selectedInfluenceChanged) {
					t.selectedInfluenceChanged(influence);
				}
			}
		}
	};

	var trackResize = function() {		
		t.thumb.addClass('no-transition');
		t.trackIndicator.addClass('no-transition');
		
		t.setPosition(t.influences[t.selectedIndex]);
		
		t.thumb.removeClass('no-transition');
		t.trackIndicator.removeClass('no-transition');
	};
	
	var thumbMouseEnter = function () {
		t.trackPreview.css('visibility', 'collapse');
	}

	var thumbMouseOut = function () {
		t.trackPreview.css('visibility', 'visible');
	};

	t.spacing = {
		horizontal: 10,
		vertical: 5
	};
	t.padding = {
		horizontal: 10,
		vertical: 0
	};

	t.color = "#ffffff";
	t.timeRange = new Array(2);

	t.control = $(element);
	t.trackColumn = $(t.control.find(".track-col")[0]);
	t.trackColumn.mouseenter(trackMouseEnter);
	t.trackColumn.mousemove(trackMouseMove);
	t.trackColumn.mouseout(trackMouseOut);
	t.trackColumn.click(trackClick);
	t.trackContainer = $(t.control.find(".track-container")[0]);
	t.track = $(t.control.find(".track")[0]);
	t.trackPreview = $(t.control.find(".track-preview")[0]);
	t.trackIndicator = $(t.control.find(".track-indicator")[0]);
	t.thumb = $(t.control.find(".thumb")[0]);
	t.thumbKnob = $(t.control.find(".thumb-knob")[0]);
	t.thumb.mouseenter(thumbMouseEnter);
	t.thumb.mouseout(thumbMouseOut);
	t.thumb.draggable({
		axis: 'x',
		containment: '.thumb-container',
		scroll: false,
		drag: drag,
		start: dragStart,
		stop: dragStop
	});
	t.positionLabel = $(t.control.find(".position label")[0]);
	t.durationLabel = $(t.control.find(".duration label")[0]);
	t.activitiesContainer = $(t.control.find(".activities-container")[0]);
	t.commentsContainer = $(t.control.find(".comments-container")[0]);
	
	$(window).resize(trackResize);
};

TimelineControl.prototype.selectedInfluenceChanged = undefined;

TimelineControl.prototype.setActivities = function (data) {
	var t = this;

	if (data && data.length > 0) {
		t.activities = [];
		t.totalTime = 0;

		for (var i = 0; i < data.length; i++) {
			var a = new Activity(data[i], t.totalTime);

			t.activities.push(a);
			t.totalTime += a.timeRange.length;
			t.color = a.color;
		}

		// The activities are ordered backward in time.
		t.firstActivity = t.activities[t.activities.length - 1];
		t.lastActivity = t.activities[0];

		t.timeRange[0] = 0;
		t.timeRange[1] = t.totalTime;

		// Update x time scale to current range and control size.
		t.xScale = d3.scaleTime().domain(t.timeRange).range([0, t.control.innerWidth()]).clamp(true);

		// Set the current position text.
		var time = moment.duration(t.totalTime, "milliseconds").format("hh:mm:ss", {
			trim: false
		});

		t.durationLabel.text(time);
		t.positionLabel.text(time);
	} else {
		data = undefined;
		t.xScale = undefined;
	}
};

TimelineControl.prototype.setInfluences = function (influences) {
	var t = this;

	t.influences = influences;
};

TimelineControl.prototype.getInfluence = function (x) {
	var t = this;

	if (x > 0 && x <= t.track.innerWidth()) {
		var y = t.track.innerWidth() / t.influences.length;
		var i = t.influences.length - Math.ceil(x / y);

		if (i > -1 && i != t.selectedIndex) {
			var influence = t.influences[i];

			return influence;
		}
	}
};

TimelineControl.prototype.setPosition = function (influence) {
	var t = this;

	if (t.dragging) {
		return;
	}

	if (t.xScale) {
		t.selectedIndex = t.influences.indexOf(influence);

		t.updatePositionLabels(influence);
		t.updateTrackPreview(influence);
	} else {
		console.log('Warning: Timline activities are not initialized:', t.activities);

		t.trackIndicator.css('background', t.getColor(influence));
		t.trackIndicator.css('width', 0);
	}
};

TimelineControl.prototype.updatePositionLabels = function (influence) {
	var t = this;

	if (influence) {
		var time = new Date(influence.time);
		var position = 0;
		
		for (var i = 0; i < t.activities.length; i++) {
			var a = t.activities[i];
			
			if(a.endTime >= time && time >= a.startTime) {
				position += (time - a.startTime);
			} else if(time >= a.startTime) {
				position += a.timeRange.length;
			}
		}

		t.positionLabel.text(moment.duration(position, 'milliseconds').format('hh:mm:ss', {
			trim: false
		}));

		/*
		var duration = t.lastActivity.endTime - t.firstActivity.startTime;

		t.durationLabel.text(moment.duration(duration, 'milliseconds').format('hh:mm:ss', {
			trim: false
		}));
		*/
	}
};

TimelineControl.prototype.getTrackPosition = function (influence) {
	var t = this;
	var i = t.influences.indexOf(influence);

	if (i > -1) {
		// We subtract 1 from the number of influences to receive values 
		// between 0 and 1 when calculating the progress percentage.
		var I = t.influences.length - 1;

		var x = Math.ceil(t.track.innerWidth() * (I - i) / I);

		return x;
	}

	return 0;
};

TimelineControl.prototype.updateTrackPreview = function (influence) {
	var t = this;

	var x = t.getTrackPosition(influence);

	t.thumbKnob.css('background', t.getColor(influence));
	t.thumb.css('left', (x - Math.ceil(t.thumbKnob.outerWidth() / 2)) + 'px');

	t.trackIndicator.css('background', t.getColor(influence));
	t.trackIndicator.css('width', x + 'px');
};

TimelineControl.prototype.getColor = function (influence) {
	var t = this;

	if (influence.agentColor === '#FF0000') {
		return t.lastColor;
	}

	t.lastColor = influence.agentColor;

	return t.lastColor;
};
angular.module('explorerApp').directive('bsSwitch', BootstrapSwitchDirective);

function BootstrapSwitchDirective() {
    return {
        restrict: 'A',
        require: '?ngModel',
        link: function (scope, element, attrs, ngModel) {
            element.bootstrapSwitch();

            element.on('switchChange.bootstrapSwitch', function (event, state) {
                if (ngModel) {
                    scope.$apply(function () {
                        ngModel.$setViewValue(state);
                    });
                }
            });

            scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                if (newValue) {
                    element.bootstrapSwitch('state', true, true);
                } else {
                    element.bootstrapSwitch('state', false, true);
                }
            });
        }
    };
}
angular.module('explorerApp').directive('ngDropzone', DropZoneDirective);

function DropZoneDirective() {
    return {
        restrict: "A",
        link: function (scope, elem) {
            elem.bind('drop', function (evt) {
                evt.stopPropagation();
                evt.preventDefault();

                var files = evt.dataTransfer.files;

                alert(files);

                for (var i = 0, f; f = files[i]; i++) {
                    alert(f);
                }
            });
        }
    }
}
angular.module('explorerApp').directive('ngEnter', EnterKeyBindingDirective);

function EnterKeyBindingDirective() {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) { // 13 = enter key
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
}
angular.module('explorerApp').directive('ngEsc', EscapeKeyBindingDirective);

function EscapeKeyBindingDirective() {
    return function (scope, element, attrs) {
        element.bind('keydown keypress', function (event) {
            if (event.which === 27) { // 27 = esc key
                scope.$apply(function () {
                    scope.$eval(attrs.ngEsc);
                });

                event.preventDefault();
            }
        });
    };
}
angular.module('explorerApp').controller('AddAccountDialogController', AddAccountDialogController);

function AddAccountDialogController(api, $scope, $filter, $uibModalInstance, $sce, clientService) {
    var t = this;
    var s = $scope;
    var interval = undefined;

    s.title = $filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE');
    s.clients = [];

    api.getAccountClients().then(function (data) {
        s.clients = data;

        console.log("Available clients:", s.clients);
    });

    s.selectedClient = null;

    s.selectClient = function (client) {
        s.title = ($filter('translate')('SETTINGS.ACCOUNTS.CONNECT_DIALOG.TITLE_X')).replace('{0}', client.Title);
        s.selectedClient = client;
        s.parameter = {
            clientUri: client.Uri,
            authType: client.SupportedAuthenticationClients[0].Uri
        };

        // TODO: Remove hard-wiring. Receive presets and target sites from client.
        if (client.Uri === 'http://orcid.org') {
            s.parameter.presetId = 'orcid.org';

            s.connectAccount(s.selectedClient);
        } else if (client.Uri === 'http://eprints.org') {
            s.parameter.url = 'https://ualresearchonline.arts.ac.uk';
        }

        console.log("Client selected: ", client);
    }

    // Prevent an account from being installed twice.
    s.isInstalling = false;

    // The client state '0' refers to 'None'.	
    s.clientState = 0;

    s.connectAccount = function (client) {
        // The client state '1' refers to 'InProgress'.
        s.clientState = 1;

        api.authorizeAccount(s.parameter).then(function (data) {
            var sessionId = data.Id;

            clientService.pollServiceState(api, sessionId, function (intervalHandle, state) {
                interval = intervalHandle;

                console.log(state);

                for (var i = 0; i < state.Client.SupportedAuthenticationClients.length; i++) {
                    var c = state.Client.SupportedAuthenticationClients[i];

                    // Allow iframes to connect to the URL.
                    s.clientUrl = $sce.trustAsResourceUrl(c.AuthorizeUrl);

                    if (c.ClientState > 1) {
                        clearInterval(interval);

                        s.clientState = c.ClientState;

                        // The client state '2' refers to 'Authorized'.
                        if (!s.isInstalling && c.ClientState == 2) {
                            s.isInstalling = true;

                            api.installAccount(sessionId).then(function (r) {
                                console.log("Account installed:", sessionId);

                                // Close the dialog after the account was successfully connected.
                                setTimeout(function () {
                                    $uibModalInstance.close();
                                }, 1000);
                            });
                        }

                        break;
                    }
                }
            });
        });
    };

    s.cancel = function () {
        $uibModalInstance.dismiss('cancel');

        if (interval) {
            window.clearInterval(interval);
        }
    };
};
angular.module('explorerApp').controller('CalendarDialogController', CalendarDialogController);

function CalendarDialogController(api, $scope, $filter, $uibModalInstance, $sce) {
    $scope.isLoading = true;
    $scope.dialog = $uibModalInstance;
    $scope.getActivities = api.getActivities;

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}
angular.module('explorerApp').controller('PublishFileDialogController', PublishFileDialogController);

function PublishFileDialogController(api, $scope, $filter, $uibModalInstance, $sce, fileService, clientService) {
    $scope.dialog = {
        step: 'publishing-options',
        title: 'Publish File',
        subtitle: 'Create a dataset for your file and upload it into a digital repository.'
    };

    $scope.getFileName = fileService.getFileName;
    $scope.getFileNameWithoutExtension = fileService.getFileNameWithoutExtension;
    $scope.getFileExtension = fileService.getFileExtension;
    $scope.hasFileThumbnail = api.hasThumbnail;
    $scope.getFileThumbnailUrl = api.getThumbnailUrl;

    console.log($scope.entity);

    // Accounts
    $scope.accounts = [];
    $scope.selectedAccount = null;

    // At first, we need to determine if there are any accounts which can be used for publishing.
    api.getAccountsWithFeature('http://w3id.org/art/terms/1.0/features/PublishArchive').then(function (data) {
        console.log("Accounts:", data);

        $scope.accounts = data;

        if (0 < $scope.accounts.length) {
            var account = $scope.accounts[0];

            $scope.selectedAccount = account;

            $scope.authentication = {
                protocol: account.AuthenticationProtocol.Uri,
                parameter: {}
            };

            for (var i = 0; i < account.AuthenticationParameters.length; i++) {
                var p = account.AuthenticationParameters[i];

                $scope.authentication.parameter[p.Name] = p.Value;
            }
        } else {
            $scope.dialog.step = 'no-accounts';
            $scope.dialog.title = 'No Accounts';
            $scope.dialog.subtitle = 'You have not yet added any accounts which can be used for publication.';
        }
    });

    $scope.selectAccount = function () {
        $scope.dialog.step = 'upload-select-account';
        $scope.dialog.title = 'Select Account';
        $scope.dialog.subtitle = 'Choose the account used for publication and authorize the upload by logging in.';
    };

    // Publishing
    $scope.archive = {
        title: 'Artivity data for ' + $scope.file.label,
        description: '',
        creators: [],
        license: null,
        licenseOptions: [{
                uri: 'https://creativecommons.org/licenses/by-nc-nd/4.0/',
                label: 'Creative Commons BY-NC-ND',
                description: 'Attribution, Non-Commercial, No Derivatives'
            }, {
                uri: 'https://creativecommons.org/licenses/by-nc-sa/4.0/',
                label: 'Creative Commons BY-NC-SA',
                description: 'Attribution, Non-Commercial, Share Alike'
            }, {
                uri: 'https://creativecommons.org/licenses/by-nc/4.0/',
                label: 'Creative Commons BY-NC',
                description: 'Attribution, Non-Commercial'
            }, {
                uri: 'https://creativecommons.org/licenses/by-nd/4.0/',
                label: 'Creative Commons BY-ND',
                description: 'Attribution, No Derivatives'
            }, {
                uri: 'https://creativecommons.org/licenses/by-sa/4.0/',
                label: 'Creative Commons BY-SA',
                description: 'Attribution, Share Alike'
            }, {
                uri: 'https://creativecommons.org/licenses/by/4.0/',
                label: 'Creative Commons BY',
                description: 'Attribution'
            }
        ],
        contentOptions: {
            includeFile: true,
            includeEditingHistory: true,
            includeBrowsingHistory: false,
            includeComments: false
        }
    };

    $scope.archive.license = $scope.archive.licenseOptions[0].uri;

    // Load author information.
    $scope.userPhotoUrl = api.getUserPhotoUrl();

    api.getUser().then(function (data) {
        $scope.user = data;

        $scope.archive.creators = [{
            name: data.Name,
            email: data.EmailAddress
        }];
    });

    // Upload
    var interval = undefined;

    $scope.beginUpload = function () {
        $scope.dialog.step = 'upload-progress';
        $scope.dialog.title = "Publishing File";

        $scope.progress = {
            Tasks: [],
            CurrentTask: '',
            PercentComplete: 0
        };

        api.uploadArchive($scope.selectedAccount.Uri, $scope.entity.uri, $scope.authentication.parameter, $scope.archive).then(function (data) {
            var sessionId = data.Id;

            console.log("Session: ", sessionId);

            clientService.pollServiceState(api, sessionId, function (intervalHandle, state) {
                interval = intervalHandle;

                $scope.dialog.subtitle = $filter('translate')(state.Progress.CurrentTask.Id + "#description");

                $scope.progress = state.Progress;

                if (parseInt(state.Progress.PercentComplete) === 100) {
                    clearInterval(interval);

                    // Delay the closing of the window so that the UI can update the progress.
                    setTimeout($scope.endUpload, 2000);
                }
            });
        });
    };

    $scope.endUpload = function () {
        if (interval) {
            clearInterval(interval);
        }

        if ($scope.percentComplete < 100) {
            $scope.percentComplete = 0;
        } else {
            $uibModalInstance.close();
        }
    };

    $scope.cancel = function () {
        $scope.endUpload();

        $uibModalInstance.dismiss('cancel');
    };
};
angular.module('explorerApp').controller('FileListController', FileListController);

function FileListController(api, $scope, $uibModal, fileService, hotkeys) {
    var t = this;
    var s = $scope;

    // USER INFO
    s.user = {};
    s.userPhotoUrl = api.getUserPhotoUrl()+"?q="+Date.now();

    api.getUser().then(function (data) {
        s.user = data;
    });

    s.activities = [];

    // RECENTLY USED FILES
    s.files = [];
    s.hasFiles = false;

    s.loadRecentFiles = function () {
        api.getRecentFiles().then(function (data) {
            for (var i = 0; i < data.length; i++) {
                var file = data[i];

                if (i < s.files.length && s.files[i].uri == file.uri) {
                    break;
                }

                s.files.splice(i, 0, file);
            }

            s.hasFiles = data.length > 0;
        });
    };

    s.getFileName = fileService.getFileName;
    s.getFileNameWithoutExtension = fileService.getFileNameWithoutExtension;
    s.getFileExtension = fileService.getFileExtension;
    s.hasFileThumbnail = api.hasThumbnail;
    s.getFileThumbnailUrl = api.getThumbnailUrl;

    s.loadRecentFiles();

    // CALENDAR
    s.calendar = null;

    s.toggleCalendar = function () {
        if (s.calendar) {
            s.calendar.close();

            s.calendar = null;
        } else {
            s.calendar = $uibModal.open({
                animation: true,
                templateUrl: 'partials/dialogs/calendar-dialog.html',
                controller: 'CalendarDialogController',
                windowClass: 'modal-window-lg',
                scope: $scope
            });
        }
    };

    hotkeys.add({
        combo: 'alt+c',
        description: 'Open the calendar view.',
        callback: function () {
            s.toggleCalendar();
        }
    });

    // REFRESH
    window.addEventListener("focus", function (e) {
        // Redraw the scene to prevent blank scenes when switching windows.		
        if (!document.hidden) {
            s.loadRecentFiles();
        }
    });
}
angular.module('explorerApp').controller('FileViewController', FileViewController);

function FileViewController(api, $scope, $location, $routeParams, $translate, $uibModal) {
    var t = this;
    var s = $scope;

    var fileUri = $location.search().uri;

    s.entity = {
        uri: fileUri
    };

    // File metadata
    s.file = {};

    api.getFile(fileUri).then(function (data) {
        s.file = data;

        console.log("Entity: ", s.file);
    });

    // Agent metadata
    s.agent = {
        iconUrl: ''
    };

    api.getAgent(fileUri).then(function (data) {
        s.agent = data;
        s.agent.iconUrl = api.getAgentIconUrl(data.agent);

        console.log("Agent: ", s.agent);
    });

    // Load the user data.
    s.user = {};

    api.getUser().then(function (data) {
        s.user = data;
        s.user.photoUrl = api.getUserPhotoUrl();

        console.log("User: ", s.user);
    });

    // RENDERING
    var canvas = document.getElementById('canvas');

    var renderer = new DocumentRenderer(canvas, api.getRenderingUrl(fileUri));

    // INFLUENCES
    s.influences = [];
    s.previousInfluence;
    s.selectedInfluence;

    // ACTIVITIES
    s.activities = [];

    s.loadActivities = function () {
        api.getActivities(fileUri).then(function (data) {
            console.log("Loaded activities: ", data);

            s.activities = data;

            if (data.length > 0) {
                api.getInfluences(fileUri).then(function (data) {
                    console.log("Loaded influences:", data.length, data);

                    s.influences = data;

                    if (data.length > 0) {
                        // Canvases in the file.
                        api.getCanvases(fileUri).then(function (data) {
                            renderer.canvasCache.load(data, function () {
                                console.log("Loaded canvases: ", renderer.canvasCache);

                                api.getLayers(fileUri).then(function (data) {
                                    renderer.layerCache.load(data, function (layers) {
                                        console.log("Loaded layers: ", layers);

                                        // Trigger loading the bitmaps.
                                        api.getRenderings(fileUri).then(function (data) {
                                            renderer.renderCache.load(data, function () {
                                                console.log("Loaded renderings: ", renderer.renderCache);

                                                s.previewInfluence(s.selectedInfluence);
                                            });
                                        }).then(function () {
                                            s.statistics = [];

                                            var stepCount = 0;
                                            var undoCount = 0;
                                            var redoCount = 0;

                                            for (var i = s.influences.length - 1; i >= 0; i--) {
                                                var influence = s.influences[i];

                                                // Convert the timestamp into a date object.
                                                influence.time = new Date(influence.time);

                                                // Influences[0] is the first step..
                                                stepCount++;

                                                switch (influence.type) {
                                                    case 'http://w3id.org/art/terms/1.0/Undo':
                                                        undoCount++;
                                                        break;
                                                    case 'http://w3id.org/art/terms/1.0/Redo':
                                                        redoCount++;
                                                        break;
                                                }

                                                influence.stats.stepCount = stepCount;
                                                influence.stats.undoCount = undoCount;
                                                influence.stats.redoCount = redoCount;
                                                influence.stats.layers = [];

                                                renderer.layerCache.getAll(influence.time, function (layer, depth) {
                                                    influence.stats.layers.push(layer);
                                                });

                                                s.statistics.push(influence.stats);
                                            }

                                            console.log("Loaded stats:", s.statistics);
                                        });;
                                    });
                                });
                            });
                        });

                        // Add the loaded influences to the activities for easier access in the frontend.
                        var i = 0;

                        var activity = s.activities[i];
                        activity.showDate = true;
                        activity.influences = [];

                        // Keep a dictionary of activities so that we can access them when required.
                        var activities = {};
                        activities[activity.uri] = activity;

                        // NOTE: We assume that the influences and activities are ordered by descending time.
                        for (var j = 0; j < data.length; j++) {
                            var influence = data[j];

                            // Initialize empty statistics.
                            influence.stats = new EditingStatistics();

                            while (activity.uri !== influence.activity && i < s.activities.length - 1) {
                                if (activities[influence.activity]) {
                                    // The influece belongs to previous activity.
                                    activity = activities[influence.activity];
                                } else {
                                    var a = s.activities[++i];

                                    var t1 = new Date(a.startTime);
                                    var t2 = new Date(activity.startTime);

                                    a.showDate = t1.getDay() != t2.getDay() || t1.getMonth() != t2.getMonth() || t1.getYear() != t2.getYear();

                                    activity = a;
                                    activity.influences = [];
                                    
                                    activities[activity.uri] = activity;
                                }
                            }

                            activity.isComment = influence.comment != null;

                            if (influence.activity === activity.uri) {
                                activity.influences.push(influence);
                            }

                            if (activity.endTime < activity.maxTime) {
                                activity.endTime = activity.maxTime;
                            }

                            if (!activity.title) {
                                activity.title = s.file.label; // Set for fullcalendar.
                                activity.start = activity.startTime; // Alias for fullcalendar.
                                activity.end = activity.endTime; // Alias for fullcalendar.
                            }

                            //activity.startTime = new Date(activity.startTime);
                            //activity.endTime = new Date(activity.endTime);
                            //activity.totalTime = moment(activity.endTime) - moment(activity.startTime);
                        }

                        s.previewInfluence(data[0]);
                    }
                });
            }
        });
    };

    s.loadActivities();

    s.selectInfluence = function (influence) {
        s.selectedInfluence = influence;
        s.previousInfluence = undefined;
    };

    s.previewInfluence = function (influence) {
        s.previousInfluence = s.selectedInfluence;
        s.selectedInfluence = influence;

        if (influence.time !== undefined) {
            s.renderInfluence(influence);

            // Set the labels of the layers at the time of the influence.
            each(influence.stats.layers, function (i, layer) {
                layer.label = layer.getLabel(influence.time);
            });

            // Trigger the processing of change notifications, if necessary.
            // Note: $$phase should NOT be used, but currently solves the problem.
            if (!s.$$phase) {
                s.$digest();
            }

            // Note: this is experimental.
            //var heatmap = new HeatmapRenderer(canvas);
            //heatmap.render(s.influences);
        }
    };

    s.renderInfluence = function (influence) {
        if (influence !== undefined) {
            renderer.render(influence);

            // Warning: this is slow.
            //s.palette = renderer.getPalette();
        }
    };

    s.resetInfluence = function () {
        if (s.previousInfluence) {
            s.selectedInfluence = s.previousInfluence;
            s.previousInfluence = undefined;

            s.renderInfluence(s.selectedInfluence);
        }
    };

    // PLAYBACK
    s.playing = false;

    var playloop = undefined;

    s.togglePlay = function () {
        if (playloop) {
            s.pause();
        } else {
            s.play();
        }
    };

    s.play = function () {
        var end = s.influences.indexOf(s.selectedInfluence) === 0;

        if (!playloop && !end) {
            playloop = setInterval(s.skipNext, 500);

            s.playing = playloop !== undefined;
        }
    };

    s.pause = function () {
        console.log(playloop);

        if (playloop) {
            clearInterval(playloop);

            playloop = undefined;

            s.playing = playloop !== undefined;

            s.$digest();
        }
    };

    s.skipPrev = function () {
        if (s.influences === undefined) {
            return;
        }

        var i = s.influences.indexOf(s.selectedInfluence) + 1;

        if (0 < i && i < s.influences.length) {
            s.selectedInfluence = s.influences[i];

            console.log(s.selectedInfluence.offsetTop);

            s.renderInfluence(s.selectedInfluence);
        }

        if (playloop) {
            s.$digest();

            if (i === s.influences.length) {
                s.pause();
            }
        }
    };

    s.skipNext = function () {
        var i = s.influences.indexOf(s.selectedInfluence);

        if (0 < i && i < s.influences.length) {
            s.selectedInfluence = s.influences[i - 1];

            s.renderInfluence(s.selectedInfluence);
        }

        if (playloop) {
            s.$digest();

            if (i === 0) {
                s.pause();
            }
        }
    };

    s.historyKeyDown = function (e) {
        if (e.which == 40) { // Arrow key down
            s.skipPrev();

            e.preventDefault();
        } else if (e.which === 38) { // Arrow up
            s.skipNext();

            e.preventDefault();
        }
    };

    // FORMATTING
    s.getFormattedTime = function (time) {
        return moment(time).format('hh:mm:ss');
    };

    s.getFormattedDate = function (time) {
        return moment(time).format('dddd, Do MMMM YYYY');
    };

    s.getFormattedTimeFromNow = function (time) {
        var result = moment(time).fromNow();

        return result;
    };

    // EXPORT
    s.exportFile = function () {
        api.exportFile(fileUri, s.file.label);
    };

    // SHARING
    s.publishFile = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'partials/dialogs/publish-file-dialog.html',
            controller: 'PublishFileDialogController',
            scope: $scope
        });
    }

    // PRINT LABEL
    var getChangedProperty = function (influence) {
        for (var i = 0; i < influence.changes.length; i++) {
            var change = influence.changes[i];

            if (change.entityType !== 'http://w3id.org/art/terms/1.0/Layer' && change.property) {
                return change.property;
            }
        }

        return '';
    };

    s.getLabel = function (influence) {
        var key;

        switch (influence.type) {
            case 'http://www.w3.org/ns/prov#Generation':
                {
                    key = 'FILEVIEW.http://www.w3.org/ns/prov#Generation';
                    break;
                }
            case 'http://www.w3.org/ns/prov#Invalidation':
                {
                    key = 'FILEVIEW.http://www.w3.org/ns/prov#Invalidation';
                    break;
                }
            default:
                {
                    // TODO: pluralize
                    key = 'FILEVIEW.' + getChangedProperty(influence);
                    break;
                }
        }

        var result;

        // Only translate if we actually found a property in the previous loop.
        if (key && key !== 'FILEVIEW.') {
            result = $translate.instant(key)
        } else if (influence.description) {
            result = influence.description;
        } else {
            result = $translate.instant('FILEVIEW.' + influence.type);
        }

        return result;
    };

    s.getIcon = function (influence) {
        switch (influence.type) {
            /*
            case 'http://www.w3.org/ns/prov#Generation':
                return 'zmdi-plus';
            case 'http://www.w3.org/ns/prov#Invalidation':
                return 'zmdi-delete';
            */
            case 'http://www.w3.org/ns/prov#Derivation':
                return 'zmdi-floppy';
            case 'http://www.w3.org/ns/prov#Undo':
                return 'zmdi-undo';
            case 'http://www.w3.org/ns/prov#Redo':
                return 'zmdi-redo';
            case 'http://w3id.org/art/terms/1.0/Save':
                return 'zmdi-floppy';
            case 'http://w3id.org/art/terms/1.0/SaveAs':
                return 'zmdi-floppy';
        }

        /*
        var property = getChangedProperty(influence);

        if (property !== '') {
            switch (property) {
            case 'http://w3id.org/art/terms/1.0/position':
                return 'zmdi-arrows';
            case 'http://w3id.org/art/terms/1.0/hadBoundaries':
                return 'zmdi-border-style';
            case 'http://www.w3.org/2000/01/rdf-schema#label':
                return 'zmdi-format-color-text';
            case 'http://w3id.org/art/terms/1.0/textValue':
                return 'zmdi-format-color-text';
            case 'http://w3id.org/art/terms/1.0/strokeWidth':
                return 'zmdi-border-color';
            }
        }
        */

        return 'zmdi-brush';
    };

    s.comment = {
        text: ''
    };

    s.updateComment = function () {
        if (!s.comment.startTime) {
            s.comment.activity = s.activities[0].uri;
            s.comment.agent = s.user.Uri;
            s.comment.entity = s.entity.uri;
            s.comment.startTime = new Date();

            console.log("Start comment: ", s.comment);
        }
    };

    s.resetComment = function (clearText) {
        if (clearText) {
            s.comment.text = '';
        }

        if (s.comment.text === '') {
            s.comment.startTime = undefined;
            s.comment.endTime = undefined;
        }

        console.log("Reset comment: ", s.comment);
    };

    s.postComment = function () {
        var comment = s.comment;

        if (comment.agent && comment.text !== '') {
            s.resetComment(true);

            comment.endTime = new Date();

            console.log("Post comment: ", comment);

            api.postComment(comment).then(function (data) {
                s.loadActivities();
            });
        }
    };
}
angular.module('explorerApp').controller('SettingsController', SettingsController);

function SettingsController(api, $scope, $rootScope, $location, $routeParams, settingsService) {
    var t = this;
    var s = $scope;

    s.submit = function () {
        settingsService.controllers(function(c) {
            if(c.submit) {
                c.submit();
            }
        });
    };

    s.$watch('agent.iconUrl', function () {
        if (s.agent && s.agent.iconUrl !== "") {
            timeline.setUserPhotoUrl(scope.user.photoUrl);
        }
    });

    s.submitAndReturn = function () {
        s.submit();

        // Navigate to dashboard and refresh the page.
        $location.path('/');
    };

    s.reset = function () {
        settingsService.controllers(function(c) {
            if(c.reset) {
                c.reset();
            }
        });
    };
};
angular.module('explorerApp').controller('SetupController', SetupController);

function SetupController(api, $scope, $rootScope, $location, $routeParams, $controller, settingsService) {
    var t = this;
    var s = $scope;

    // Inherit from SettingsController.
    $controller('SettingsController', {
        api: api,
        $scope: $scope,
        $rootScope: $rootScope,
        $location: location,
        $routeParams: $routeParams,
        settingsService: settingsService
    });

    s.steps = ['art-settings-user', 'art-settings-apps', 'art-settings-complete'];
    s.currentStep = s.steps[0];
    s.canGoNext = true;
    s.canGoBack = false;

    s.goNext = function() {
        var n = s.steps.indexOf(s.currentStep);

        if(n < s.steps.length - 1) {
            s.currentStep = s.steps[n + 1];
            s.canGoNext = n + 1 < s.steps.length - 1;

            onStepChanged();
        }
    }

    s.goBack = function() {
        var n = s.steps.indexOf(s.currentStep);

        if(n > 0) {
            s.currentStep = s.steps[n - 1];
            s.canGoBack = n - 1 > 0;

            onStepChanged();
        }
    }

    function onStepChanged() {
        var n = s.steps.indexOf(s.currentStep);

        if(n === s.steps.length - 1) {
            // Submit all setup pages.
            s.submit();

            api.setRunSetup(false).then(function() {
                // After the setup has been disabled, go to the homepage.
                $location.path("/");
            });
        }
    }
}
angular.module('explorerApp').controller('StartController', StartController);

function StartController($location, api, $http) {
    var t = this;

    t.showSpinner = true;
    t.showError = false;
    t.retry = retry;

    init();

    function init() {
        showLoadingSpinner();

        $http.get(apid.endpointUrl + '/setup').then(
            function (response) {
                if(response.data) {
                    $location.path("/setup");
                } else {
                    $location.path("/files");
                }
            },
            function () {
                showConnectionError();
            });
    }

    function retry() {
        init();
    }

    function showLoadingSpinner() {
        t.showSpinner = true;
        t.showError = false;
    }

    function showConnectionError() {
        t.showSpinner = false;
        t.showError = true;
    }
};
})();
//# sourceMappingURL=app.src.js.map