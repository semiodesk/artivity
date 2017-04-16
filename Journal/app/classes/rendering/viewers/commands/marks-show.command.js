function ShowMarksCommand(viewer, service) {
    var t = this;

    ViewerCommand.call(t, viewer, 'showMarks');

    // The entity to be marked.
    t.param = null;

    // The mark service instance.
    t.service = service;
}

ShowMarksCommand.prototype = Object.create(ViewerCommand.prototype);

ShowMarksCommand.prototype.canExecute = function (param) {
    var t = this;

    if (t.viewer) {
        var renderer = t.viewer.getRenderer('marksRenderer');

        return renderer && param && param.length > 0;
    }

    return false;
};

ShowMarksCommand.prototype.execute = function (param) {
    var t = ViewerCommand.prototype.execute.call(this, param);

    var renderer = t.viewer.getRenderer('marksRenderer');

    renderer.addMarks(param);
};