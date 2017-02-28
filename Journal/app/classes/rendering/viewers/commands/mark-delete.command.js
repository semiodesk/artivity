function DeleteMarkCommand(viewer, service) {
    var t = this;

    t.id = 'markDelete';

    // The mark service instance.
    t.service = service;

    // Keep a reference to the current viewer;
    t.viewer = viewer;

    // Listen to key events for deleting selected marks.
    $(window).keyup(function (e) {
        if (e.key === 'Delete') {
            if (t.canExecute(e)) {
                t.execute(e);
            }
        }
    });
}

DeleteMarkCommand.prototype.canExecute = function (e) {
    var t = this;

    if (t.viewer && t.viewer.selection && t.viewer.marks) {
        var item = t.viewer.selection.selectedItem();

        return item && item.mark;
    }

    return false;
}

DeleteMarkCommand.prototype.execute = function (e) {
    var t = this;

    var mark = t.viewer.selection.selectedItem().mark;

    if (mark.uri) {
        // Delete the mark if it has been persisted.
        t.service.deleteMark(mark.uri).then(function () {
            // The 'markRemoved' event will trigger the re-rendering of the marks.
            t.viewer.raise('markDeleted', mark);
            t.viewer.raise('markRemoved', mark);
        });
    } else {
        // The 'markRemoved' event will trigger the re-rendering of the marks.
        t.viewer.raise('markRemoved', mark);
    }
}