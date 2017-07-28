(function () {
    angular.module('app').directive('artTaskControl', TaskControlDirective);

    function TaskControlDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-task-control/art-task-control.html',
            scope: {
                entityUri: '='
            },
            controller: TaskControlDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('TaskControlDirectiveController', TaskControlDirectiveController);

    TaskControlDirectiveController.$inject = ['$scope', 'agentService', 'entityService', 'taskService', 'viewerService'];

    function TaskControlDirectiveController($scope, agentService, entityService, taskService, viewerService) {
        var t = this;

        // TASKS
        t.newTask = null;

        t.loadTasksForEntity = function (entityUri) {
            taskService.get(entityUri).then(function (data) {
                var tasks = [];

                for (i = 0; i < data.length; i++) {
                    var d = data[i];

                    // Insert at the beginning of the list.
                    tasks.unshift({
                        uri: d.uri,
                        agent: d.agent,
                        time: d.time,
                        name: d.name
                    });
                }

                t.tasks = tasks;
            });
        }

        t.validateTask = function (task) {
            return task.agent && task.entity && task.startTime && task.name.length > 0;
        }

        t.updateTask = function (task) {
            if (!task.startTime) {
                task.startTime = new Date();

                console.log("Started task: ", task);
            }
        };

        t.putTask = function (task) {
            // Set the start time and entity.
            t.updateTask(task);

            if (!task.endTime) {
                task.endTime = new Date();
            }

            if (t.validateTask(task)) {
                taskService.put(task).then(function (data) {
                    console.log("Updated task: ", task);
                }, function (response) {
                    console.error(response);
                });
            } else {
                console.log("Invalid task: ", task);
            }
        }

        t.postTask = function (task) {
            // Set the start time and entity.
            t.updateTask(task);

            if (!task.endTime) {
                task.endTime = new Date();
            }

            if (t.validateTask(task)) {
                task.endTime = new Date();

                taskService.post(task).then(function (data) {
                    task.uri = data.uri;

                    // Show the task in the list of tasks.
                    t.tasks.push(task);

                    console.log("Posted task: ", task);

                    // Clear the text and create a new task.
                    t.resetTask();
                }, function (response) {
                    console.error(response);
                });
            } else {
                console.log("Invalid task: ", task);
            }
        }

        t.resetTask = function () {
            t.newTask = {
                agent: t.currentUser.Uri,
                entity: t.entityUri,
                startTime: null,
                endTime: null,
                name: '',
                markers: []
            };

            console.log("Reset task: ", t.newTask);
        }

        // MARKS
        t.createMark = viewerService.createMark;
        t.showMarks = viewerService.showMarks;
        t.hideMarks = viewerService.hideMarks;

        // INIT
        t.$postLink = function () {
            agentService.getCurrentUser().then(function (currentUser) {
                t.currentUser = currentUser;
                
                $scope.$watch('t.entityUri', function () {
                    if (t.entityUri) {
                        t.loadTasksForEntity(t.entityUri);

                        t.resetTask();
                    }
                });
            });
        }
    }
})();