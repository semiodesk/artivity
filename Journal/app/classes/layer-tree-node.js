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
}