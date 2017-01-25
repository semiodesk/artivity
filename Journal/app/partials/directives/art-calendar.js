(function () {
    angular.module('explorerApp').directive('artCalendar', CalendarDirective);

    function CalendarDirective() {
        return {
            scope: true,
            template: '',
            link: function (scope, element, attributes, api, $log, $uibModal) {
                var s = scope;

                var options = {
                    header: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'month,agendaWeek,agendaDay'
                    },
                    defaultView: 'agendaWeek',
                    businessHours: {
                        dow: [1, 2, 3, 4, 5], // Monday - Friday
                        start: '9:00',
                        end: '17:00',
                    },
                    height: function () {
                        return $('.modal-body').innerHeight() - 20;
                    }
                };

                var element = $(element);
                element.fullCalendar(options);

                s.getActivities().then(function (data) {
                    var activities = [];

                    for (var i = 0; i < data.length; i++) {
                        var activity = data[i];
                        activity.title = '';
                        activity.start = new Date(activity.startTime);
                        activity.color = activity.agentColor;

                        if (activity.endTime) {
                            activity.end = new Date(activity.endTime);
                        } else if (activity.maxTime) {
                            activity.end = new Date(activity.maxTime);
                        } else {
                            activity.end = new Date(activity.startTime);
                            activity.end.setSeconds(activity.end.getSeconds() + 30);
                        }

                        activities.push(activity);
                    }

                    console.log("Activities:", activities);

                    if (activities && activities.length > 0) {
                        element.fullCalendar('render');
                        element.fullCalendar('addEventSource', activities);
                        element.fullCalendar('gotoDate', activities[0].start);
                    }

                    s.dialog.rendered.then(function () {
                        s.isLoading = false;

                        element.fullCalendar('render');
                    });
                });
            }
        }
    };
})();