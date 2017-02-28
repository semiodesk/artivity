function ZoomOutCommand(viewer) {
    var t = this;

    ViewerCommand.call(t, viewer, 'viewerZoomOut');

    $(window).on('keydown', function(e) {
        if(e.ctrlKey && e.key === '-') {
            t.execute();
        }
    });
}

ZoomOutCommand.prototype = Object.create(ViewerCommand.prototype);

ZoomOutCommand.prototype.execute = function (e) {
    var t = ViewerCommand.prototype.execute.call(this, e);

    t.viewer.zoomOut();
}