function UpdateMarkCommand(viewer, service) {
    var t = this;

    t.id = 'updateMark';

    // The mark service instance.
    t.service = service;

    // Keep a reference to the current viewer;
    t.viewer = viewer;

    // Persist a mark everything the selection is changing.
    t.viewer.on('markModified', function (e) {
        if (e.target && t.canExecute(e.target.mark)) {
            t.execute(e.target.mark);
        }
    });
}

UpdateMarkCommand.prototype.canExecute = function (e) {
    var t = this;

    if (t.viewer && t.viewer.selection && t.viewer.marks) {
        var item = t.viewer.selection.selectedItem();

        return item && item.mark;
    }

    return false;
};

UpdateMarkCommand.prototype.execute = function (mark) {
    var t = this;

    if (mark && mark.uri) {
        var m = {
            uri: mark.uri,
            agent: t.viewer.user.Uri,
            entity: t.viewer.entity,
            startTime: mark.startTime,
            endTime: new Date(),
            geometryType: mark.geometryType,
            geometry: {
                x: mark.p1.x,
                y: mark.p1.y
            }
        }

        if (m.geometryType.endsWith('Rectangle')) {
            m.geometry.width = mark.p2.x - mark.p1.x;
            m.geometry.height = mark.p2.y - mark.p1.y;

            if (m.width === 0 || m.height === 0) {
                return false;
            }
        }

        t.service.updateMark(m).then(function () {
            t.viewer.raise('markUpdated', mark);
        });

        return true;
    }
};