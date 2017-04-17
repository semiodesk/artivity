function SelectCommand(viewer, service) {
    var t = this;

    ViewerCommand.call(t, viewer, 'viewerSelect');

    t.service = service;
}

SelectCommand.prototype = Object.create(ViewerCommand.prototype);

SelectCommand.prototype.canExecute = function (e) {
    var t = this;

    return t.viewer && t.viewer.selection;
};

SelectCommand.prototype.execute = function (e) {
    var t = ViewerCommand.prototype.execute.call(this, e);

    t.viewer.selectCommand(t);

    t.stage.cursor = 'default';
};