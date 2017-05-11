function PreviousPageCommand(viewer) {
    var t = this;

    ViewerCommand.call(t, viewer, 'viewerPagePrevious');
}

PreviousPageCommand.prototype = Object.create(ViewerCommand.prototype);

PreviousPageCommand.prototype.canExecute = function (e, options) {
    var t = this;

    return t.viewer && t.viewer.previousPage;
};

PreviousPageCommand.prototype.execute = function (e, options) {
    var t = ViewerCommand.prototype.execute.call(this, e);

    t.viewer.previousPage();
};