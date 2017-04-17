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
            bindToController: true,
            link: function (scope, element, attr, t) {
                scope.$watch('t.entityUri', function () {
                    if (t.entityUri) {
                        t.loadTopcisForEntity(t.entityUri);
                    }
                });
            }
        }
    }

    angular.module('app').controller('TopicControlDirectiveController', TopicControlDirectiveController);

    TopicControlDirectiveController.$inject = ['$scope', 'agentService', 'entityService', 'topicService', 'viewerService'];

    function TopicControlDirectiveController($scope, agentService, entityService, topicService, viewerService) {
        var t = this;

        // TOPICS
        t.topics = [];
        t.newTopic = null;
        t.selectedTopic = null;

        t.loadTopcisForEntity = function (entityUri) {
            console.log("Loading topics:", entityUri);

            topicService.getTopicsForEntity(entityUri).then(function (data) {
                t.topics = [];

                for (i = 0; i < data.length; i++) {
                    var d = data[i];

                    // Insert at the beginning of the list.
                    t.topics.unshift({
                        uri: d.uri,
                        agent: d.agent,
                        time: d.time,
                        title: d.title,
                        completed: d.completed
                    });
                }
            });
        }

        t.validateTopic = function (topic) {
            return topic.agent && topic.entity && topic.startTime && topic.title.length > 0;
        }

        t.updateTopic = function (topic) {
            if (!topic.startTime) {
                topic.startTime = new Date();

                console.log("Started topic: ", t.newTopic);
            }
        };

        t.postTopic = function (topic) {
            if (t.validateTopic(topic)) {
                topic.endTime = new Date();

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
            t.newTopic = {
                agent: agentService.currentUser.Uri,
                entity: t.entityUri,
                startTime: null,
                endTime: null,
                title: '',
                markers: []
            };

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
    }
})();