function SelectCommand(viewer, service) {
    var t = this;

    t.id = 'viewer-select';

    t.viewer = viewer;
    t.service = service;
}

SelectCommand.prototype.canExecute = function (e) {
    var t = this;

    return t.viewer && t.viewer.selection;
}

SelectCommand.prototype.execute = function (e) {
    var t = this;

    t.viewer.selection.clear();
    
    t.viewer.stage.cursor = 'default';
    t.viewer.stage.update();

    return true;
}