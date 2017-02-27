function ViewerCommand(viewer, id) {
    this.viewer = viewer;
    this.id = id;
}

ViewerCommand.prototype.canExecute = function(param) {
    return this.viewer;
}

ViewerCommand.prototype.execute = function(param) {
    this.startExecute(param);
}

ViewerCommand.prototype.startExecute = function(param) {
    this.viewer.raise('startExecute', this);

    return this;
}

ViewerCommand.prototype.stopExecute = function(param) {
    this.viewer.raise('stopExecute', this);

    return this;
}