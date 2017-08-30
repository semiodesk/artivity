function ShowChatCommand(viewer, service) {
    var t = this;

    ViewerCommand.call(t, viewer, 'showChat');

    // The entity to be marked.
    t.param = null;

    // The mark service instance.
    t.service = service;

    viewer.on('markSelected', function(e) {
        if(e.target && t.canExecute(e.target)) {
            $(document).trigger('showChatPanel', e);
        }
    });
}

ShowChatCommand.prototype = Object.create(ViewerCommand.prototype);

ShowChatCommand.prototype.canExecute = function (param) {
    var t = this;

    return param && param.downX && param.downY;
};

ShowChatCommand.prototype.execute = function (param) {
    var t = ViewerCommand.prototype.execute.call(this, param);

    console.log(param);
};