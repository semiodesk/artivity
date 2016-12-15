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