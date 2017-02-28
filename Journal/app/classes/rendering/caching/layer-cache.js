/**
 * Caches a set of layers.
 */
function LayerCache() {
    var t = this;

    EntityCache.call(t);
};

LayerCache.prototype = new EntityCache();

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
                    } else {
                        tree.insertLayer(layer);
                    }
                }
            }
        }

        tree.traverse(function (node, parent, i, d) {
            fn(node.layer, d);
        });
    }
}