function MarkFeature(service) {
    var t = this;

    // The mark service instance.
    t.service = service;
}

MarkFeature.prototype.initialize = function (viewer) {
    var t = this;

    // Unscaled overlay layer which can be used for drawing dialogs, tools or makers.
    t.marks = [];
    t.marksContainer = new createjs.Container();

    t.initializeViewer(viewer);

    var x1 = 0;
    var y1 = 0;
    var isDragging = false;

    t.viewer.scene.on("mousedown", function (e) {
        isDragging = true;

        // Remember mouse down position for creating new markers.
        x1 = e.stageX;
        y1 = e.stageY;
    });

    t.isCreatingMarker = false;

    t.viewer.scene.on("pressmove", function (e) {
        if (t.viewer.isPanning || !isDragging) {
            return;
        }

        // Create a new marker at the mouse down position.
        var x2 = e.stageX;
        var y2 = e.stageY;

        var m = t.marks[t.marks.length - 1];

        if (!m || !m.new) {
            t.isCreatingMarker = true;

            m = new Marker();
            m.startTime = new Date();

            // Create a new marker with the geometry set in local (scene) coordinates..
            m.p1 = t.viewer.scene.globalToLocal(x1, y1);
            m.p2 = t.viewer.scene.globalToLocal(x2, y2);

            t.marks.push(m);

            // The visual representation of the marker.
            var r = new MarkerRectangle(t.viewer, t.viewer.scene, m);

            t.marksContainer.addChild(r);

            t.viewer.stage.update();
        } else {
            // Update the lower right point of the rectangle when dragging.
            m.p2 = t.viewer.scene.globalToLocal(x2, y2);

            // Redraw the existing marker rectangle.
            var r = t.marksContainer.getChildAt(t.marks.length - 1);

            if (r) {
                r.initializeGeometry();
                r.redraw();

                t.viewer.stage.update();
            }
        }
    });

    $(window).mouseup(function (e) {
        isDragging = false;

        if (t.isCreatingMarker && t.marks.length > 0) {
            // Finalize the new marker.
            t.isCreatingMarker = false;

            var mark = t.marks[t.marks.length - 1];

            t.createMark(mark);
        }
    });

    $(window).keyup(function (e) {
        if (e.key === 'Delete') {
            var selectedItem = t.viewer.selection.selectedItem();

            if (selectedItem && selectedItem.mark) {
                var n = t.marks.indexOf(selectedItem.mark);

                if (n > -1) {
                    t.removeMark(n);
                }
            }
        }
    });
}

MarkFeature.prototype.initializeViewer = function (viewer) {
    var t = this;

    // Keep a reference to the current viewer;
    t.viewer = viewer;
    t.viewer.cursor('crosshair');

    // Add the marks container just below the overlay container.
    var i = t.viewer.stage.getChildIndex(t.viewer.overlay);

    t.viewer.stage.addChildAt(t.marksContainer, i);

    // Set the new entity if the entity of the viewer changes.
    t.viewer.on("entityChanged", function (entity) {
        t.setEntity(entity);
    });

    // Handle updates on marks.
    t.viewer.on('markChanged', function (mark) {
        t.updateMark(mark);
    })

    // Render the marks hwne the zoom factor of the viewer changes.
    t.viewer.on("zoom", function () {
        t.renderMarks();
    });
}

MarkFeature.prototype.setEntity = function (entity) {
    var t = this;

    t.entity = entity;

    t.service.getMarksForEntity(entity).then(function (data) {
        for (i = 0; i < data.length; i++) {
            t.addMark(data[i]);
        }

        t.renderMarks();
    });
}

MarkFeature.prototype.addMark = function (mark) {
    var t = this;

    if (mark.w != 0 && mark.h != 0) {
        var m = {
            new: false,
            uri: mark.uri,
            p1: {
                x: mark.x,
                y: mark.y
            },
            p2: {
                x: mark.x + mark.w,
                y: mark.y + mark.h
            }
        };

        t.marks.push(m);

        t.viewer.raise('markAdded', m);
    } else {
        console.error("Invalid mark:", mark);

        return;
    }
}

MarkFeature.prototype.createMark = function (mark) {
    var t = this;

    var m = {
        agent: t.viewer.user.Uri,
        entity: t.viewer.entity,
        startTime: mark.startTime,
        endTime: new Date(),
        x: mark.p1.x,
        y: mark.p1.y,
        width: mark.p2.x - mark.p1.x,
        height: mark.p2.y - mark.p1.y
    }

    if (m.width !== 0 && m.height !== 0) {
        t.service.createMark(m).then(function (data) {
            // The URI is needed for subsequent updates of the mark.
            mark.uri = data.uri;

            t.viewer.raise('markCreated', m);
        });
    }
}

MarkFeature.prototype.updateMark = function (mark) {
    var t = this;

    if (mark && mark.uri) {
        var m = {
            uri: mark.uri,
            agent: t.viewer.user.Uri,
            entity: t.viewer.entity,
            startTime: mark.startTime,
            endTime: new Date(),
            x: mark.p1.x,
            y: mark.p1.y,
            width: mark.p2.x - mark.p1.x,
            height: mark.p2.y - mark.p1.y
        }

        if (m.width !== 0 && m.height !== 0) {
            t.service.updateMark(m).then(function () {
                t.viewer.raise('markUpdated', m);
            });
        }
    }
}

MarkFeature.prototype.removeMark = function (n) {
    var t = this;

    var mark = t.marks[n];

    if (mark) {
        var handleRemove = function (event) {
            t.marks.splice(n, 1);
            t.renderMarks();
            t.viewer.raise(event, mark);
        }

        if (mark.uri) {
            // Delete the mark if it has been persisted.
            t.service.deleteMark(mark.uri).then(function () {
                handleRemove('markDeleted');
            });
        } else {
            // Remove the mark if it has not been persisted yet.
            handleRemove('markRemoved');
        }
    }
}

MarkFeature.prototype.renderMarks = function () {
    var t = this;

    t.marksContainer.removeAllChildren();

    for (i = 0; i < t.marks.length; i++) {
        var m = t.marks[i];
        var r = new MarkerRectangle(t.viewer, t.viewer.scene, m);

        t.marksContainer.addChild(r);
    }

    t.viewer.stage.update();
}