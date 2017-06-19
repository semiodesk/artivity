(function () {
    angular.module('app').directive('artTopicControl', TopicControlDirective);

    function TopicControlDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-topic-control/art-topic-control.html',
            scope: {
                entityUri: '='
            },
            controller: TopicControlDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('TopicControlDirectiveController', TopicControlDirectiveController);

    TopicControlDirectiveController.$inject = ['$scope', 'agentService', 'entityService', 'topicService', 'viewerService'];

    function TopicControlDirectiveController($scope, agentService, entityService, topicService, viewerService) {
        var t = this;

        // TOPICS
        t.topics = [];
        t.newTopic = new Topic();
        t.selectedTopic = null;

        t.loadTopcisForEntity = function (entityUri) {
            console.log("Loading topics:", entityUri);

            topicService.getTopicsForEntity(entityUri).then(function (data) {
                t.topics = [];

                for (i = 0; i < data.length; i++) {
                    var d = data[i];

                    var topic = new Topic();
                    topic.uri = d.uri;
                    topic.agentId = d.agent;
                    topic.startTime = d.time;
                    topic.title = d.title;
                    topic.closed = d.completed;

                    // Insert at the beginning of the list.
                    t.topics.unshift(topic);
                }
            });
        }

        t.updateTopic = function (topic) {
            if (!topic.startTime) {
                topic.startTime = new Date();

                console.log("Started topic: ", topic);
            }
        }

        t.postTopic = function (topic) {
            if (!topic.endTime) {
                topic.endTime = new Date();
            }

            if (topic.validate()) {
                topicService.postTopic(topic).then(function (data) {
                    topic.uri = data.uri;

                    // Show the topic in the list of topics.
                    t.topics.push(topic);

                    console.log("Posted topic: ", topic);

                    // Clear the text and create a new topic.
                    t.resetTopic();
                }, function (response) {
                    console.error(response);
                });
            } else {
                console.log("Invalid topic: ", topic);
            }
        }

        t.resetTopic = function () {
            var topic = new Topic();
            topic.agent = t.currentUser.Uri;
            topic.primarySource = t.entityUri;

            t.newTopic = topic;

            console.log("Reset topic: ", t.newTopic);
        }

        t.deleteTopic = function (topic) {
            if (topic && topic.uri) {
                topicService.deleteTopic(topic.uri);

                console.log("Deleted topic: ", topic);
            }
        }

        t.selectTopic = function (topic) {
            t.selectedTopic = topic;
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
                        t.loadTopcisForEntity(t.entityUri);

                        t.resetTopic();
                    }
                });
            });
        }
    }
})();