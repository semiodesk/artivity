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
    return this.getValue(time, 'http://purl.org/dc/elements/1.1/title');
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

EntityCache.prototype.load = function(data) {
    var t = this;

    each(data, function(i, influence) {
		var time = new Date(influence.time);
        var entity = t.entities[influence.uri];
        
        if(entity === undefined) {
            entity = new Entity(influence.uri);
            
            t.entities[influence.uri] = entity;
        };
        		
        if(influence.type === 'http://www.w3.org/ns/prov#Generation') {
            entity.creationTime = time;
        }
        
        entity.pushValue(time, influence.property, influence.value);
    });
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

    t.layers = [];
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
        if(time >= layer.creationTime) {            
            var lowerLayer = layer.getValue(time, 'http://w3id.org/art/terms/1.0/aboveLayer');
			
			console.log(uri, lowerLayer);
			
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

		console.log(next);
		
        fn(next);
		
        current = next.uri;

        i++;
    }
}