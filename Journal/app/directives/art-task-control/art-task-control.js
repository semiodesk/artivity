(function () {
    angular.module('app').directive('artTaskControl', TaskControlDirective);

    function TaskControlDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-task-control/art-task-control.html',
            bindToController: true,
            scope: {
                entity: '@'
            },
            controller: TaskControlDirectiveController,
            controllerAs: 't'
        }
    }

    angular.module('app').controller('TaskControlDirectiveController', TaskControlDirectiveController);

    TaskControlDirectiveController.$inject = ['$scope', 'agentService', 'entityService', 'taskService', 'markService', 'viewerService'];

    function TaskControlDirectiveController($scope, agentService, entityService, taskService, markService, viewerService) {
        var t = this;

        t.tasks = [];
        t.task = {
            name: ''
        }

        t.updateTask = updateTask;
        t.postTask = postTask;
        t.putTask = putTask;
        t.resetTask = resetTask;

        t.marks = [];

        t.createMark = createMark;
        t.showMarks = showMarks;
        t.hideMarks = hideMarks;

        initialize();

        function initialize() {
            initializeData();
        }

        function initializeData() {
            agentService.getUser().then(function (data) {
                t.user = data;

                // Set the user URI for the new comments.
                t.task.agent = t.user.Uri;

                if (t.entity) {
                    // Make sure the user is properly initialized before retrieving the entity derivations.
                    entityService.getById(t.entity).then(function (response) {
                        var entity = response;

                        if (entity.RevisionUris && entity.RevisionUris.length > 0) {
                            t.derivation = entity.RevisionUris[0];

                            // Set the entity URI as primary source for the comments.
                            t.task.entity = t.derivation;

                            taskService.get(t.derivation).then(function (data) {
                                t.tasks = [];

                                for (i = 0; i < data.length; i++) {
                                    var d = data[i];

                                    // Insert at the beginning of the list.
                                    t.tasks.unshift({
                                        uri: d.uri,
                                        agent: d.agent,
                                        time: d.time,
                                        name: d.name
                                    });
                                }
                            });
                        }
                    });
                }
            });
        }

        function validateTask(task) {
            return task.agent && task.entity && task.startTime && task.name.length > 0;
        }

        function updateTask(task) {
            if (!task.startTime) {
                task.entity = t.derivation;
                task.startTime = new Date();

                console.log("Start task: ", t.task);
            }
        };

        function putTask(task) {
            // Set the start time and entity.
            t.updateTask(task);

            if (!validateTask(task)) {
                console.log("Invalid task: ", t.task);

                return;
            }

            taskService.put(task).then(function (data) {
                console.log("Updated task: ", t.task);
            }, function (response) {
                console.error(response);
            });
        }

        function postTask() {
            // Set the start time and entity.
            t.updateTask(t.task);

            if (!validateTask(t.task)) {
                console.log("Invalid task: ", t.task);

                return;
            }

            t.task.endTime = new Date();

            taskService.post(t.task).then(function (data) {
                t.task.uri = data.uri;

                // Show the task in the list of tasks.
                t.tasks.push(t.task);

                // Clear the text and create a new task.
                resetTask();

                console.log("Posted task: ", t.task);
            }, function (response) {
                console.error(response);
            });
        }

        function resetTask() {
            t.task = {
                agent: null,
                entity: t.derivation,
                startTime: null,
                endTime: null,
                name: '',
                markers: []
            };

            console.log("Reset task: ", t.task);
        }

        function createMark(task) {
            if (task && task.uri) {
                viewerService.executeCommand('createMark', task.uri);
            }
        }

        function showMarks(task) {
            if (task && task.uri) {
                markService.getMarksForEntity(task.uri).then(function (data) {
                    t.marks = data;

                    viewerService.executeCommand('showMarks', t.marks);
                });
            }
        }

        function hideMarks(task) {
            if (task && task.uri && t.marks.length > 0) {
                viewerService.executeCommand('hideMarks', t.marks);

                t.marks = [];
            }
        }
    }
})();