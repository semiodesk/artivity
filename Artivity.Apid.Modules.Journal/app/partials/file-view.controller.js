angular.module('explorerApp').controller('FileViewController', FileViewController);

function FileViewController(api, $scope, $location, $routeParams, $translate, $uibModal, selectionService) {
    var t = this;

    // TODO: Remove. Currently required by timeline control.
    t.scope = $scope;

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
    t.previousInfluence;
    t.selectedInfluence; // TODO: Implement using selectionService.
    t.onSelectedInfluenceChanged = null;

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

                                        // Trigger loading the bitmaps.
                                        api.getRenderings(fileUri).then(function (data) {
                                            renderer.renderCache.load(data, function () {
                                                console.log("Loaded renderings: ", renderer.renderCache);

                                                t.previewInfluence(t.selectedInfluence);
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

                            // Initialize empty statistics.
                            influence.stats = new EditingStatistics();

                            while (activity.uri !== influence.activity && i < t.activities.length - 1) {
                                if (activities[influence.activity]) {
                                    // The influece belongs to previous activity.
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

                        t.previewInfluence(data[0]);
                    }
                });
            }
        });
    };

    t.loadActivities();

    t.selectInfluence = function (influence) {
        t.selectedInfluence = influence;
        t.previousInfluence = undefined;

        if(t.onSelectedInfluenceChanged !== null) {
            t.onSelectedInfluenceChanged(t.selectedInfluence);
        }
    };

    t.previewInfluence = function (influence) {
        t.previousInfluence = t.selectedInfluence;
        t.selectedInfluence = influence;

        if(t.onSelectedInfluenceChanged !== null) {
            t.onSelectedInfluenceChanged(t.selectedInfluence);
        }

        if (influence.time !== undefined) {
            t.renderInfluence(influence);

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
            t.selectedInfluence = t.previousInfluence;
            t.previousInfluence = undefined;

            if(t.onSelectedInfluenceChanged !== null) {
                t.onSelectedInfluenceChanged(t.selectedInfluence);
            }

            t.renderInfluence(t.selectedInfluence);
        }
    };

    // PLAYBACK
    t.playing = false;

    var playloop = undefined;

    t.togglePlay = function () {
        if (playloop) {
            t.pause();
        } else {
            t.play();
        }
    };

    t.play = function () {
        var end = t.influences.indexOf(t.selectedInfluence) === 0;

        if (!playloop && !end) {
            playloop = setInterval(t.skipNext, 500);

            t.playing = playloop !== undefined;
        }
    };

    t.pause = function () {
        console.log(playloop);

        if (playloop) {
            clearInterval(playloop);

            playloop = undefined;

            t.playing = playloop !== undefined;

            t.$digest();
        }
    };

    t.skipPrev = function () {
        if (t.influences === undefined) {
            return;
        }

        var i = t.influences.indexOf(t.selectedInfluence) + 1;

        if (0 < i && i < t.influences.length) {
            t.selectedInfluence = t.influences[i];

            if(t.onSelectedInfluenceChanged !== null) {
                t.onSelectedInfluenceChanged(t.selectedInfluence);
            }

            console.log(t.selectedInfluence.offsetTop);

            t.renderInfluence(t.selectedInfluence);
        }

        if (playloop) {
            t.$digest();

            if (i === t.influences.length) {
                t.pause();
            }
        }
    };

    t.skipNext = function () {
        var i = t.influences.indexOf(t.selectedInfluence);

        if (0 < i && i < t.influences.length) {
            t.selectedInfluence = t.influences[i - 1];

            if(t.onSelectedInfluenceChanged !== null) {
                t.onSelectedInfluenceChanged(t.selectedInfluence);
            }

            t.renderInfluence(t.selectedInfluence);
        }

        if (playloop) {
            t.$digest();

            if (i === 0) {
                t.pause();
            }
        }
    };

    t.historyKeyDown = function (e) {
        if (e.which == 40) { // Arrow key down
            t.skipPrev();

            e.preventDefault();
        } else if (e.which === 38) { // Arrow up
            t.skipNext();

            e.preventDefault();
        }
    };

    // FORMATTING
    t.getFormattedTime = function (time) {
        return moment(time).format('hh:mm:ss');
    };

    t.getFormattedDate = function (time) {
        return moment(time).format('dddd, Do MMMM YYYY');
    };

    t.getFormattedTimeFromNow = function (time) {
        var result = moment(time).fromNow();

        return result;
    };

    // EXPORT
    t.exportFile = function () {
        api.exportFile(fileUri, t.file.label);
    };

    // SHARING
    t.publishFile = function () {
        selectionService.items = [t.file];

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'partials/dialogs/publish-file-dialog.html',
            controller: 'PublishFileDialogController',
            controllerAs: 't',
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

    t.getLabel = function (influence) {
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

    t.getIcon = function (influence) {
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