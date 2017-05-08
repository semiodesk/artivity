(function () {
    angular.module('app').controller('DocumentHistoryViewController', DocumentHistoryViewController);

    DocumentHistoryViewController.$inject = ['$scope', '$state', '$stateParams', 'api', 'entityService', 'navigationService'];

    function DocumentHistoryViewController($scope, $state, $stateParams, api, entityService, navigationService) {
        var t = this;

        t.loadFile = function (file) {
            if (file && file.uri) {
                var e = {
                    file: file,
                    activities: {},
                    influences: [],
                    statistics: {
                        stepCount: 0,
                        undoCount: 0,
                        redoCount: 0
                    }
                };

                api.getActivities(file.uri).then(function (data) {
                    var activities = data;

                    console.log("Loaded activities: ", activities);

                    api.getInfluences(file.uri).then(function (data) {
                        var influences = data;

                        e.influences = influences;

                        console.log("Loaded influences:", influences);

                        // Add the loaded influences to the activities for easier access in the frontend.
                        var i = 0;

                        var activity = activities[i];
                        activity.showDate = true;
                        activity.influences = [];

                        // Keep a dictionary of activities so that we can access them when required.
                        e.activities[activity.uri] = activity;

                        // NOTE: We assume that the influences and activities are ordered by descending time.
                        for (var j = 0; j < influences.length; j++) {
                            var influence = influences[j];

                            // TODO: Move into control.
                            // renderer.layerCache.getAll(influence.time, function (layer, depth) {
                            //     influence.stats.layers.push(layer);
                            // });

                            // Add an identifier to the influence.
                            influence.id = j;

                            while (activity.uri !== influence.activity && i < activities.length - 1) {
                                if (e.activities[influence.activity]) {
                                    // The influence belongs to previous activity.
                                    activity = e.activities[influence.activity];
                                } else {
                                    var a = activities[++i];

                                    var t1 = new Date(a.startTime);
                                    var t2 = new Date(activity.startTime);

                                    a.showDate = t1.getDay() != t2.getDay() || t1.getMonth() != t2.getMonth() || t1.getYear() != t2.getYear();

                                    activity = a;
                                    activity.influences = [];

                                    e.activities[activity.uri] = activity;
                                }
                            }

                            if (influence.activity === activity.uri) {
                                activity.influences.push(influence);
                            }

                            if (activity.endTime < activity.maxTime) {
                                activity.endTime = activity.maxTime;
                            }
                        }

                        $scope.$broadcast('fileLoaded', e);
                    });
                });
            }
        }

        t.publishLatestRevision = function (file) {
            if (file && file.uri) {
                entityService.publishLatestRevisionFromFileUri(file.uri).then(function (data) {
                    syncService.synchronize();
                });
            }
        }

        t.navigateBack = function () {
            $state.go('main.view.project-dashboard', $stateParams);
        }

        t.$onInit = function () {
            if ($stateParams.fileUri) {
                entityService.get($stateParams.fileUri).then(function (data) {
                    t.file = data;
                    t.loadFile(t.file);
                });
            }
        }
    }
})();