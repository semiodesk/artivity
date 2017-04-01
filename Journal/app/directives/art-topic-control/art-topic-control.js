(function () {
    angular.module('app').directive('artTopicControl', TopicControlDirective);

    function TopicControlDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-topic-control/art-topic-control.html',
            bindToController: true,
            scope: {
                file: '='
            },
            controller: TopicControlDirectiveController,
            controllerAs: 't'
        }
    }

    angular.module('app').controller('TopicControlDirectiveController', TopicControlDirectiveController);

    TopicControlDirectiveController.$inject = ['$scope', 'agentService', 'entityService', 'topicService', 'markService', 'viewerService'];

    function TopicControlDirectiveController($scope, agentService, entityService, topicService, markService, viewerService) {
        var t = this;

        t.revision = null;

        t.topics = [];
        t.topic = {
            title: ''
        }

        t.updateTopic = updateTopic;
        t.postTopic = postTopic;
        t.resetTopic = resetTopic;

        t.marks = [];

        t.createMark = createMark;
        t.showMarks = showMarks;
        t.hideMarks = hideMarks;

        initialize();

        function initialize() {
            initializeData();
        }

        function initializeData() {
            agentService.getAccountOwner().then(function (data) {
                t.user = data;

                // Set the user URI for the new comments.
                t.topic.agent = t.user.Uri;

                if (t.file) {
                    // Make sure the user is properly initialized before retrieving the entity derivations.
                    entityService.getLatestRevision(t.file).then(function (data) {
                        t.revision = data.revision;

                        // Set the entity URI as primary source for the comments.
                        t.topic.entity = t.revision;

                        topicService.get(t.revision).then(function (data) {
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
                    });
                }
            });
        }

        function validateTopic() {
            return t.topic.agent && t.topic.entity && t.topic.startTime && t.topic.title.length > 0;
        }

        function updateTopic() {
            if (!t.topic.startTime) {
                t.topic.entity = t.revision;
                t.topic.startTime = new Date();

                console.log("Start topic: ", t.topic);
            }
        };

        function postTopic() {
            if (!validateTopic()) {
                console.log("Invalid topic: ", t.topic);

                return;
            }

            t.topic.endTime = new Date();

            topicService.post(t.topic).then(function (data) {
                t.topic.uri = data.uri;

                // Show the topic in the list of topics.
                t.topics.push(t.topic);

                // Clear the text and create a new topic.
                resetTopic();

                console.log("Posted topic: ", t.topic);
            }, function (response) {
                console.error(response);
            });
        }

        function resetTopic() {
            t.topic = {
                agent: null,
                entity: t.revision,
                startTime: null,
                endTime: null,
                title: '',
                markers: []
            };

            console.log("Reset topic: ", t.topic);
        }

        function createMark(topic) {
            if (topic && topic.uri) {
                viewerService.executeCommand('createMark', topic.uri);
            }
        }

        function showMarks(topic) {
            if (topic && topic.uri) {
                markService.getMarksForEntity(topic.uri).then(function (data) {
                    t.marks = data;

                    viewerService.executeCommand('showMarks', t.marks);
                });
            }
        }

        function hideMarks(topic) {
            if (topic && topic.uri && t.marks.length > 0) {
                viewerService.executeCommand('hideMarks', t.marks);

                t.marks = [];
            }
        }
    }
})();