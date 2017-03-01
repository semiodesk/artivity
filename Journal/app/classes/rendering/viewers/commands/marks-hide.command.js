function HideMarksCommand(viewer, service) {
    var t = this;

    ViewerCommand.call(t, viewer, 'hideMarks');

    // The entity to be marked.
    t.param = null;

    // The mark service instance.
    t.service = service;
}

HideMarksCommand.prototype = Object.create(ViewerCommand.prototype);

HideMarksCommand.prototype.canExecute = function (param) {
    var t = this;

    if (t.viewer) {
        var renderer = t.viewer.getRenderer('marksRenderer');

        return renderer && param && param.length > 0;
    }

    return false;
}

HideMarksCommand.prototype.execute = function (param) {
    var t = ViewerCommand.prototype.execute.call(this, param);

    var renderer = t.viewer.getRenderer('marksRenderer');

    renderer.removeMarks(param);
}