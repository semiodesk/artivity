function ToggleTwoPageLayoutCommand(viewer) {
    var t = this;

    ViewerCommand.call(t, viewer, 'viewerToggleTwoPageLayout');
}

ToggleTwoPageLayoutCommand.prototype = Object.create(ViewerCommand.prototype);

ToggleTwoPageLayoutCommand.prototype.canExecute = function (e, options) {
    var t = this;

    return t.viewer && t.viewer.nextPage;
};

ToggleTwoPageLayoutCommand.prototype.execute = function (e, options) {
    var t = ViewerCommand.prototype.execute.call(this, e);

    t.viewer.toggleTwoPageLayout();
};