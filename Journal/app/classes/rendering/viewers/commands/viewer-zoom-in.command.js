function ZoomInCommand(viewer) {
    var t = this;

    ViewerCommand.call(t, viewer, 'viewerZoomIn');

    $(window).on('keydown', function (e) {
        if (e.ctrlKey && e.key === '+') {
            t.execute();
        }
    });
}

ZoomInCommand.prototype = Object.create(ViewerCommand.prototype);

ZoomInCommand.prototype.execute = function (e) {
    var t = ViewerCommand.prototype.execute.call(this, e);

    t.viewer.zoomIn();
};