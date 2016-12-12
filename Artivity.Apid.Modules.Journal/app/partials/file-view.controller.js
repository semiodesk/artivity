angular.module('explorerApp').controller('FileViewController', FileViewController);

function FileViewController(api, $scope, $location, $routeParams, $translate, $uibModal) {
    var t = this;
    var s = $scope;

    var fileUri = $location.search().uri;

    s.entity = {
        uri: fileUri
    };

    // File metadata
    s.file = {};

    api.getFile(fileUri).then(function (data) {
        s.file = data;

        console.log("Entity: ", s.file);
    });

    // Agent metadata
    s.agent = {
        iconUrl: ''
    };

    api.getAgent(fileUri).then(function (data) {
        s.agent = data;
        s.agent.iconUrl = api.getAgentIconUrl(data.agent);

        console.log("Agent: ", s.agent);
    });

    // Load the user data.
    s.user = {};

    api.getUser().then(function (data) {
        s.user = data;
        s.user.photoUrl = api.getUserPhotoUrl();

        console.log("User: ", s.user);
    });

    // RENDERING
    var canvas = document.getElementById('canvas');

    var renderer = new DocumentRenderer(canvas, api.getRenderingUrl(fileUri));

    // INFLUENCES
    s.influences = [];
    s.previousInfluence;
    s.selectedInfluence;

    // ACTIVITIES
    s.activities = [];

    s.loadActivities = function () {
        api.getActivities(fileUri).then(function (data) {
            console.log("Loaded activities: ", data);

            s.activities = data;

            if (data.length > 0) {
                api.getInfluences(fileUri).then(function (data) {
                    console.log("Loaded influences:", data.length, data);

                    s.influences = data;

                    if (data.length > 0) {
                        // Canvases in the file.
                        api.getCanvases(fileUri).then(function (data) {
                            renderer.canvasCache.load(data, function () {
                                console.log("Loaded canvases: ", renderer.canvasCache);

                                api.getLayers(fileUri).then(function (data) {
                                    renderer.layerCache.load(data, function (layers) {
                                        console.log("Loaded layers: ", layers);

                                        // Trigger loading the bitmaps.
                                        api.getRenderings(fileUri).then(function (data) {
                                            renderer.renderCache.load(data, function () {
                                                console.log("Loaded renderings: ", renderer.renderCache);

                                                s.previewInfluence(s.selectedInfluence);
                                            });
                                        }).then(function () {
                                            s.statistics = [];

                                            var stepCount = 0;
                                            var undoCount = 0;
                                            var redoCount = 0;

                                            for (var i = s.influences.length - 1; i >= 0; i--) {
                                                var influence = s.influences[i];

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

                                                s.statistics.push(influence.stats);
                                            }

                                            console.log("Loaded stats:", s.statistics);
                                        });;
                                    });
                                });
                            });
                        });

                        // Add the loaded influences to the activities for easier access in the frontend.
                        var i = 0;

                        var activity = s.activities[i];
                        activity.showDate = true;
                        activity.influences = [];

                        // Keep a dictionary of activities so that we can access them when required.
                        var activities = {};
                        activities[activity.uri] = activity;

                        // NOTE: We assume that the influences and activities are ordered by descending time.
                        for (var j = 0; j < data.length; j++) {
                            var influence = data[j];

                            // Initialize empty statistics.
                            influence.stats = new EditingStatistics();

                            while (activity.uri !== influence.activity && i < s.activities.length - 1) {
                                if (activities[influence.activity]) {
                                    // The influece belongs to previous activity.
                                    activity = activities[influence.activity];
                                } else {
                                    var a = s.activities[++i];

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
                                activity.title = s.file.label; // Set for fullcalendar.
                                activity.start = activity.startTime; // Alias for fullcalendar.
                                activity.end = activity.endTime; // Alias for fullcalendar.
                            }

                            //activity.startTime = new Date(activity.startTime);
                            //activity.endTime = new Date(activity.endTime);
                            //activity.totalTime = moment(activity.endTime) - moment(activity.startTime);
                        }

                        s.previewInfluence(data[0]);
                    }
                });
            }
        });
    };

    s.loadActivities();

    s.selectInfluence = function (influence) {
        s.selectedInfluence = influence;
        s.previousInfluence = undefined;
    };

    s.previewInfluence = function (influence) {
        s.previousInfluence = s.selectedInfluence;
        s.selectedInfluence = influence;

        if (influence.time !== undefined) {
            s.renderInfluence(influence);

            // Set the labels of the layers at the time of the influence.
            each(influence.stats.layers, function (i, layer) {
                layer.label = layer.getLabel(influence.time);
            });

            // Trigger the processing of change notifications, if necessary.
            // Note: $$phase should NOT be used, but currently solves the problem.
            if (!s.$$phase) {
                s.$digest();
            }

            // Note: this is experimental.
            //var heatmap = new HeatmapRenderer(canvas);
            //heatmap.render(s.influences);
        }
    };

    s.renderInfluence = function (influence) {
        if (influence !== undefined) {
            renderer.render(influence);

            // Warning: this is slow.
            //s.palette = renderer.getPalette();
        }
    };

    s.resetInfluence = function () {
        if (s.previousInfluence) {
            s.selectedInfluence = s.previousInfluence;
            s.previousInfluence = undefined;

            s.renderInfluence(s.selectedInfluence);
        }
    };

    // PLAYBACK
    s.playing = false;

    var playloop = undefined;

    s.togglePlay = function () {
        if (playloop) {
            s.pause();
        } else {
            s.play();
        }
    };

    s.play = function () {
        var end = s.influences.indexOf(s.selectedInfluence) === 0;

        if (!playloop && !end) {
            playloop = setInterval(s.skipNext, 500);

            s.playing = playloop !== undefined;
        }
    };

    s.pause = function () {
        console.log(playloop);

        if (playloop) {
            clearInterval(playloop);

            playloop = undefined;

            s.playing = playloop !== undefined;

            s.$digest();
        }
    };

    s.skipPrev = function () {
        if (s.influences === undefined) {
            return;
        }

        var i = s.influences.indexOf(s.selectedInfluence) + 1;

        if (0 < i && i < s.influences.length) {
            s.selectedInfluence = s.influences[i];

            console.log(s.selectedInfluence.offsetTop);

            s.renderInfluence(s.selectedInfluence);
        }

        if (playloop) {
            s.$digest();

            if (i === s.influences.length) {
                s.pause();
            }
        }
    };

    s.skipNext = function () {
        var i = s.influences.indexOf(s.selectedInfluence);

        if (0 < i && i < s.influences.length) {
            s.selectedInfluence = s.influences[i - 1];

            s.renderInfluence(s.selectedInfluence);
        }

        if (playloop) {
            s.$digest();

            if (i === 0) {
                s.pause();
            }
        }
    };

    s.historyKeyDown = function (e) {
        if (e.which == 40) { // Arrow key down
            s.skipPrev();

            e.preventDefault();
        } else if (e.which === 38) { // Arrow up
            s.skipNext();

            e.preventDefault();
        }
    };

    // FORMATTING
    s.getFormattedTime = function (time) {
        return moment(time).format('hh:mm:ss');
    };

    s.getFormattedDate = function (time) {
        return moment(time).format('dddd, Do MMMM YYYY');
    };

    s.getFormattedTimeFromNow = function (time) {
        var result = moment(time).fromNow();

        return result;
    };

    // EXPORT
    s.exportFile = function () {
        api.exportFile(fileUri, s.file.label);
    };

    // SHARING
    s.publishFile = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'partials/dialogs/publish-file-dialog.html',
            controller: 'PublishFileDialogController',
            scope: $scope
        });
    }

    // PRINT LABEL
    var getChangedProperty = function (influence) {
        for (var i = 0; i < influence.changes.length; i++) {
            var change = influence.changes[i];

            if (change.entityType !== 'http://w3id.org/art/terms/1.0/Layer' && change.property) {
                return change.property;
            }
        }

        return '';
    };

    s.getLabel = function (influence) {
        var key;

        switch (influence.type) {
            case 'http://www.w3.org/ns/prov#Generation':
                {
                    key = 'FILEVIEW.http://www.w3.org/ns/prov#Generation';
                    break;
                }
            case 'http://www.w3.org/ns/prov#Invalidation':
                {
                    key = 'FILEVIEW.http://www.w3.org/ns/prov#Invalidation';
                    break;
                }
            default:
                {
                    // TODO: pluralize
                    key = 'FILEVIEW.' + getChangedProperty(influence);
                    break;
                }
        }

        var result;

        // Only translate if we actually found a property in the previous loop.
        if (key && key !== 'FILEVIEW.') {
            result = $translate.instant(key)
        } else if (influence.description) {
            result = influence.description;
        } else {
            result = $translate.instant('FILEVIEW.' + influence.type);
        }

        return result;
    };

    s.getIcon = function (influence) {
        switch (influence.type) {
            /*
            case 'http://www.w3.org/ns/prov#Generation':
                return 'zmdi-plus';
            case 'http://www.w3.org/ns/prov#Invalidation':
                return 'zmdi-delete';
            */
            case 'http://www.w3.org/ns/prov#Derivation':
                return 'zmdi-floppy';
            case 'http://www.w3.org/ns/prov#Undo':
                return 'zmdi-undo';
            case 'http://www.w3.org/ns/prov#Redo':
                return 'zmdi-redo';
            case 'http://w3id.org/art/terms/1.0/Save':
                return 'zmdi-floppy';
            case 'http://w3id.org/art/terms/1.0/SaveAs':
                return 'zmdi-floppy';
        }

        /*
        var property = getChangedProperty(influence);

        if (property !== '') {
            switch (property) {
            case 'http://w3id.org/art/terms/1.0/position':
                return 'zmdi-arrows';
            case 'http://w3id.org/art/terms/1.0/hadBoundaries':
                return 'zmdi-border-style';
            case 'http://www.w3.org/2000/01/rdf-schema#label':
                return 'zmdi-format-color-text';
            case 'http://w3id.org/art/terms/1.0/textValue':
                return 'zmdi-format-color-text';
            case 'http://w3id.org/art/terms/1.0/strokeWidth':
                return 'zmdi-border-color';
            }
        }
        */

        return 'zmdi-brush';
    };

    s.comment = {
        text: ''
    };

    s.updateComment = function () {
        if (!s.comment.startTime) {
            s.comment.activity = s.activities[0].uri;
            s.comment.agent = s.user.Uri;
            s.comment.entity = s.entity.uri;
            s.comment.startTime = new Date();

            console.log("Start comment: ", s.comment);
        }
    };

    s.resetComment = function (clearText) {
        if (clearText) {
            s.comment.text = '';
        }

        if (s.comment.text === '') {
            s.comment.startTime = undefined;
            s.comment.endTime = undefined;
        }

        console.log("Reset comment: ", s.comment);
    };

    s.postComment = function () {
        var comment = s.comment;

        if (comment.agent && comment.text !== '') {
            s.resetComment(true);

            comment.endTime = new Date();

            console.log("Post comment: ", comment);

            api.postComment(comment).then(function (data) {
                s.loadActivities();
            });
        }
    };
}