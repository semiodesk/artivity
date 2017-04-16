function ViewerCommand(viewer, id) {
    this.id = id;

    if (viewer) {
        this.viewer = viewer;
        this.stage = viewer.stage;
        this.selection = viewer.selection;
    } else {
        console.warn("Argument 'viewer' is undefined in command:", id);
    }
}

ViewerCommand.prototype.canExecute = function (param) {
    return this.viewer;
};

ViewerCommand.prototype.execute = function (param) {
    var e = {
        command: this,
        parameter: param
    };

    this.viewer.raise('commandExecuted', e);

    console.log('Executing command:', e);

    return this;
};