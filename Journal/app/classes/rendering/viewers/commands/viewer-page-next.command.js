function NextPageCommand(viewer) {
    var t = this;

    ViewerCommand.call(t, viewer, 'viewerPageNext');
}

NextPageCommand.prototype = Object.create(ViewerCommand.prototype);

NextPageCommand.prototype.canExecute = function (e, options) {
    var t = this;

    return t.viewer && t.viewer.nextPage;
};

NextPageCommand.prototype.execute = function (e, options) {
    var t = ViewerCommand.prototype.execute.call(this, e);

    t.viewer.nextPage();
};