angular.module('explorerApp').controller('FileViewController', FileViewController);

function FileViewController(api, $scope, $location, $routeParams, $uibModal, selectionService) {
    var t = this;
    var fileUri = $location.search().uri;

    t.entity = {
        uri: fileUri
    };

    // File metadata
    t.file = {};

    api.getFile(fileUri).then(function (data) {
        t.file = data;

        console.log("Entity: ", t.file);
    });

    // Agent metadata
    t.agent = {
        iconUrl: ''
    };

    api.getAgent(fileUri).then(function (data) {
        t.agent = data;
        t.agent.iconUrl = api.getAgentIconUrl(data.agent);

        console.log("Agent: ", t.agent);
    });

    // Load the user data.
    t.user = {};

    api.getUser().then(function (data) {
        t.user = data;
        t.user.photoUrl = api.getUserPhotoUrl();

        console.log("User: ", t.user);
    });

    // RENDERING
    var canvas = document.getElementById('canvas');
    var renderer = new DocumentRenderer(canvas, api.getRenderingUrl(fileUri));

    // INFLUENCES
    t.influences = [];
    t.selectedInfluence = null;
    t.previousInfluence = null;

    selectionService.on('selectionChanged', function(item) {
        t.selectedInfluence = item;

        t.renderInfluence(item);
    });

    // ACTIVITIES
    t.activities = [];

    t.loadActivities = function () {
        api.getActivities(fileUri).then(function (data) {
            console.log("Loaded activities: ", data);

            t.activities = data;

            if (data.length > 0) {
                api.getInfluences(fileUri).then(function (data) {
                    console.log("Loaded influences:", data.length, data);

                    t.influences = data;

                    if (data.length > 0) {
                        // Canvases in the file.
                        api.getCanvases(fileUri).then(function (data) {
                            renderer.canvasCache.load(data, function () {
                                console.log("Loaded canvases: ", renderer.canvasCache);

                                api.getLayers(fileUri).then(function (data) {
                                    renderer.layerCache.load(data, function (layers) {
                                        console.log("Loaded layers: ", layers);

                                        values(layers, function(uri, layer) {
                                            // TODO: The layer state should be recorded and returned by the API.
                                            layer.visible = true;

                                            console.log(layer);
                                        });

                                        // Trigger loading the bitmaps.
                                        api.getRenderings(fileUri).then(function (data) {
                                            renderer.renderCache.load(data, function () {
                                                console.log("Loaded renderings: ", renderer.renderCache);

                                                t.previewInfluence(selectionService.selectedItem());
                                            });
                                        }).then(function () {
                                            t.statistics = [];

                                            var stepCount = 0;
                                            var undoCount = 0;
                                            var redoCount = 0;

                                            for (var i = t.influences.length - 1; i >= 0; i--) {
                                                var influence = t.influences[i];

                                                // Convert the timestamp into a date object.
                                                influence.time = new Date(influence.time);

                                                // Influences[0] is the first step..
                                                stepCount++;

                                                switch (influence.type) {
                                                    case 'http://w3id.org/art/terms/1.0/Undo':
                                                        undoCount++;
                                                        break;
                                                    case 'http://w3id.org/art/terms/1.0/Redo':
                                                        redoCount++;
                                                        break;
                                                }

                                                influence.stats.stepCount = stepCount;
                                                influence.stats.undoCount = undoCount;
                                                influence.stats.redoCount = redoCount;
                                                influence.stats.layers = [];

                                                renderer.layerCache.getAll(influence.time, function (layer, depth) {
                                                    influence.stats.layers.push(layer);
                                                });

                                                t.statistics.push(influence.stats);
                                            }

                                            console.log("Loaded stats:", t.statistics);
                                        });;
                                    });
                                });
                            });
                        });

                        // Add the loaded influences to the activities for easier access in the frontend.
                        var i = 0;

                        var activity = t.activities[i];
                        activity.showDate = true;
                        activity.influences = [];

                        // Keep a dictionary of activities so that we can access them when required.
                        var activities = {};
                        activities[activity.uri] = activity;

                        // NOTE: We assume that the influences and activities are ordered by descending time.
                        for (var j = 0; j < data.length; j++) {
                            var influence = data[j];

                            // Add an identifier to the influence.
                            influence.id = j;

                            // Initialize empty statistics.
                            influence.stats = new EditingStatistics();

                            while (activity.uri !== influence.activity && i < t.activities.length - 1) {
                                if (activities[influence.activity]) {
                                    // The influence belongs to previous activity.
                                    activity = activities[influence.activity];
                                } else {
                                    var a = t.activities[++i];

                                    var t1 = new Date(a.startTime);
                                    var t2 = new Date(activity.startTime);

                                    a.showDate = t1.getDay() != t2.getDay() || t1.getMonth() != t2.getMonth() || t1.getYear() != t2.getYear();

                                    activity = a;
                                    activity.influences = [];
                                    
                                    activities[activity.uri] = activity;
                                }
                            }

                            activity.isComment = influence.comment != null;

                            if (influence.activity === activity.uri) {
                                activity.influences.push(influence);
                            }

                            if (activity.endTime < activity.maxTime) {
                                activity.endTime = activity.maxTime;
                            }

                            if (!activity.title) {
                                activity.title = t.file.label; // Set for fullcalendar.
                                activity.start = activity.startTime; // Alias for fullcalendar.
                                activity.end = activity.endTime; // Alias for fullcalendar.
                            }

                            //activity.startTime = new Date(activity.startTime);
                            //activity.endTime = new Date(activity.endTime);
                            //activity.totalTime = moment(activity.endTime) - moment(activity.startTime);
                        }

                        selectionService.dataContext(data);
                        selectionService.selectedItem(data[0]);
                    }
                });
            }
        });
    };

    t.loadActivities();

    t.previewInfluence = function (influence) {
        t.previousInfluence = selectionService.selectedItem();

        // The selected item is being rendered automatically.
        selectionService.selectedItem(influence);

        if (influence.time !== undefined) {
            // Set the labels of the layers at the time of the influence.
            each(influence.stats.layers, function (i, layer) {
                layer.label = layer.getLabel(influence.time);
            });

            // Trigger the processing of change notifications, if necessary.
            // Note: $$phase should NOT be used, but currently solves the problem.
            try {
                if (!t.$$phase) {
                    $scope.$digest();
                }
            }
            catch(error) {
            }

            // Note: this is experimental.
            //var heatmap = new HeatmapRenderer(canvas);
            //heatmap.render(t.influences);
        }
    };

    t.renderInfluence = function (influence) {
        if (influence !== undefined) {
            renderer.render(influence);

            // Warning: this is slow.
            //t.palette = renderer.getPalette();
        }
    };

    t.resetInfluence = function () {
        if (t.previousInfluence) {
            var influence = t.previousInfluence;

            t.previousInfluence = null;

            // The selected item is being rendered automatically.
            selectionService.selectedItem(influence);
        }
    };
    
    // EXPORT
    t.exportFile = function () {
        api.exportFile(fileUri, t.file.label);
    };

    // SHARING
    t.publishFile = function () {
        var influence = selectionService.selectedItem();

        selectionService.mute();
        selectionService.selectedItem({
            uri: t.entity.uri,
            label: t.file.label,
            agentColor: t.agent.color
        });

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'partials/dialogs/publish-file-dialog.html',
            controller: 'PublishFileDialogController',
            controllerAs: 't',
            scope: $scope
        }).closed.then(function() {
            selectionService.selectedItem(influence);
            selectionService.unmute();
        });
    }

    t.comment = {
        text: ''
    };

    t.updateComment = function () {
        if (!t.comment.startTime) {
            t.comment.activity = t.activities[0].uri;
            t.comment.agent = t.user.Uri;
            t.comment.entity = t.entity.uri;
            t.comment.startTime = new Date();

            console.log("Start comment: ", t.comment);
        }
    };

    t.resetComment = function (clearText) {
        if (clearText) {
            t.comment.text = '';
        }

        if (t.comment.text === '') {
            t.comment.startTime = undefined;
            t.comment.endTime = undefined;
        }

        console.log("Reset comment: ", t.comment);
    };

    t.postComment = function () {
        var comment = t.comment;

        if (comment.agent && comment.text !== '') {
            t.resetComment(true);

            comment.endTime = new Date();

            console.log("Post comment: ", comment);

            api.postComment(comment).then(function (data) {
                t.loadActivities();
            });
        }
    };
}