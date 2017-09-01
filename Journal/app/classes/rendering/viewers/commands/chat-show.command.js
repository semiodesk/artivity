function ShowChatCommand(viewer, service) {
    var t = this;

    ViewerCommand.call(t, viewer, 'showChat');

    // The entity to be marked.
    t.param = null;

    // The mark service instance.
    t.service = service;

    viewer.on('itemMouseOver', function(e) {
        if(t.canExecute(e.target)) {
            $(document).trigger('showChatPanel', e);
        }
    });

    viewer.on('itemMouseOut', function(e) {
        if(t.canExecute(e.target)) {
            $(document).trigger('hideChatPanel', e);
        }
    });
}

ShowChatCommand.prototype = Object.create(ViewerCommand.prototype);

ShowChatCommand.prototype.canExecute = function (param) {
    var t = this;

    return param && param.mark;
};

ShowChatCommand.prototype.execute = function (param) {
    var t = ViewerCommand.prototype.execute.call(this, param);

    console.log(param);
};