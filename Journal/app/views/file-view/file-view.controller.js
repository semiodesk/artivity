(function () {
    angular.module('app').controller('FileViewController', FileViewController);

    FileViewController.$inject = ['$rootScope', '$scope', '$location', '$routeParams', '$uibModal', 'api', 'agentService', 'selectionService', 'hotkeys'];

    function FileViewController($rootScope, $scope, $location, $routeParams, $uibModal, api, agentService, selectionService, hotkeys) {
        var t = this;
        var fileUri = $location.search().uri;
        var entityUri = $location.search().entityUri;

        t.entity = {
            uri: fileUri,
            entityUri: entityUri
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
            if( data != null ){
                t.agent = data;
                t.agent.iconUrl = api.getAgentIconUrl(data.agent);

                console.log("Agent: ", t.agent);
            }
        });

        t.initView = function () {
            // Make the left and right panes resizable.
            $(".ui-pane-left").resizable({
                handles: "e"
            });
            $(".ui-pane-left").resize(t.updateCanvas);
            $(".ui-pane-right").resizable({
                handles: "w",
                resize: function (event, ui) {
                    // Only resize the width of the element.
                    if (ui.position.left < 0) {
                        ui.size.width = ui.originalSize.width + Math.abs(ui.position.left);
                    }

                    // Do not use the left CSS property at all because it breaks the table layout.
                    ui.position.left = 0;
                }
            });
            $(".ui-pane-right").resize(t.updateCanvas);

            // Prevent navigation when the user clicks on tab headers.
            $('.tablist a').click(function (e) {
                e.preventDefault();
                $(this).tab('show');
            });

            $(document).ready(function () {
                t.updateCanvas();

                $(".ctl-history").focus();

                // Update the canvas every time the window is resized.
                window.addEventListener('resize', t.updateCanvas, false);
            });
        }

        // Load the user data.
        t.user = {};

        agentService.getAccountOwner().then(function (data) {
            t.user = data;
            t.user.photoUrl = api.getUserPhotoUrl();

            console.log("User: ", t.user);
        });

        // RENDERING
        var canvas = document.getElementById('canvas');
        var renderer = new DocumentHistoryViewer(t.user, canvas, api.getRenderingUrl(entityUri));
        renderer.addCommand(new PanCommand(renderer));

        $rootScope.$on('redraw', function () {
            t.renderInfluence(t.selectedInfluence);
        });

        // INFLUENCES
        t.loading = false;
        t.influences = [];
        t.selectedInfluence = null;
        t.previousInfluence = null;

        selectionService.on('selectedItemChanging', function (influence) {
            if (influence.time !== undefined) {
                // Set the labels of the layers at the time of the influence.
                each(influence.stats.layers, function (i, layer) {
                    layer.label = layer.getLabel(influence.time);
                });
            }
        });

        selectionService.on('selectedItemChanged', function (influence) {
            t.selectedInfluence = influence;
        });

        // ACTIVITIES
        t.activities = [];

        t.loadActivities = function () {
            t.loading = true;

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

                                            selectionService.dataContext(t.influences);
                                            selectionService.selectedItem(t.influences[0]);

                                            values(layers, function (uri, layer) {
                                                // TODO: The layer state should be recorded and returned by the API.
                                                layer.visible = true;

                                                console.log(layer);
                                            });

                                            // Trigger loading the bitmaps.
                                            api.getRenderings(fileUri).then(function (data) {
                                                renderer.renderCache.load(data, function () {
                                                    console.log("Loaded renderings: ", renderer.renderCache);

                                                    $rootScope.$broadcast('redraw');
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

                                                if (t.influences.length > 0) {
                                                    selectionService.selectedItem(t.influences[0]);
                                                }
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

                            t.loading = false;
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
        };

        t.renderInfluence = function (influence) {
            if (influence !== undefined) {
                renderer.influences = t.influences;
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

        t.updateCanvas = function () {
            // This seems to be the only reliable way to compute the correct height of the body area.
            var bodyHeight = $('#window-view-container').height();

            $('.layout-root > .row').each(function (n, row) {
                if (!$(row).hasClass('expand')) {
                    bodyHeight -= $(row).height();
                }
            });

            var buffer = document.getElementById("canvas");

            if (buffer !== null) {
                // To prevent flickering, store the current context before setting the size.
                var bufferContext = buffer.getContext('2d');

                // Fit the canvas into its parent.
                var container = $(buffer);

                // Create temp canvas and context
                var temp = document.createElement('canvas');
                temp.width = container.width();
                temp.height = bodyHeight;

                // Draw current canvas to temp canvas.
                var tempContext = temp.getContext("2d");
                tempContext.drawImage(bufferContext.canvas, 0, 0);

                // Now resize the canvas.					
                buffer.width = container.width();
                buffer.height = bodyHeight;
                buffer.style.height = bodyHeight + 'px';

                // Draw temp canvas back to the current canvas
                bufferContext.drawImage(tempContext.canvas, 0, 0);

                // Update the buffer.
                var t = angular.element(buffer).controller();

                if (t && t.selectedInfluence) {
                    t.renderInfluence(t.selectedInfluence);
                }
            }
        }

        hotkeys.add({
            combo: 'f9',
            description: 'Toggle rendering of influenced regions.',
            callback: function () {
                t.toggleInfluencedRegions();
            }
        });

        t.toggleInfluencedRegions = function () {
            renderer.renderInfluencedRegions = !renderer.renderInfluencedRegions;

            t.renderInfluence(t.selectedInfluence);
        }

        hotkeys.add({
            combo: 'f10',
            description: 'Toggle rendering of the editing frequency of influenced regions.',
            callback: function () {
                t.toggleEditingFrequency();
            }
        });

        t.toggleEditingFrequency = function () {
            renderer.renderEditingFrequency = !renderer.renderEditingFrequency;

            t.renderInfluence(t.selectedInfluence);
        }

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
                templateUrl: 'app/dialogs/publish-file-dialog/publish-file-dialog.html',
                controller: 'PublishFileDialogController',
                controllerAs: 't',
                scope: $scope
            }).closed.then(function () {
                selectionService.selectedItem(influence);
                selectionService.unmute();
            });
        }

        // Initialize the view.
        t.initView();
    }
})();