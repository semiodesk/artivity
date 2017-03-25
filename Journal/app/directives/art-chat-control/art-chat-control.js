(function () {
    angular.module('app').directive('artChatControl', artChatControlDirective);

    function artChatControlDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-chat-control/art-chat-control.html',
            scope: {
                file: '@'
            },
            controller: ChatControlDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attributes, ctrl) {
                ctrl.element = element;
            }
        }
    }

    angular.module('app').controller('ChatControlDirectiveController', ChatControlDirectiveController);

    ChatControlDirectiveController.$inject = ['$scope', 'api', 'viewerService', 'agentService', 'entityService', 'commentService', 'markService', 'selectionService', 'formattingService'];

    function ChatControlDirectiveController($scope, api, viewerService, agentService, entityService, commentService, markService, selectionService, formattingService) {
        var t = this;

        t.user = null;
        t.file = null;
        t.comment = null;
        t.comments = [];

        t.isUserComment = isUserComment;
        t.validateComment = validateComment;
        t.updateComment = updateComment;
        t.postComment = postComment;
        t.resetComment = resetComment;

        t.getFormattedTime = formattingService.getFormattedTime;
        t.getFormattedDate = formattingService.getFormattedDate;
        t.getFormattedTimeFromNow = formattingService.getFormattedTimeFromNow;

        t.marks = [];

        t.createMark = createMark;
        t.showMarks = showMarks;
        t.hideMarks = hideMarks;

        initialize();

        function initialize() {
            initializeData();
            initializeView();
        }

        function initializeData() {
            agentService.getAccountOwner().then(function (data) {
                t.user = data;

                // Set the user URI for the new comments.
                t.comment.agent = t.user.Uri;

                entityService.getLatestRevision($scope.file).then(function (data) {
                    // Set the entity URI as primary source for the comments.
                    t.comment.entity = data.revision;

                    commentService.get(data.revision).then(function (data) {
                        t.comments = [];

                        for (i = 0; i < data.length; i++) {
                            var c = data[i];

                            // Insert at the beginning of the list.
                            t.comments.unshift({
                                uri: c.uri,
                                agent: c.agent,
                                time: c.time,
                                text: c.message
                            });
                        }
                    });
                });
            });

            resetComment();
        }

        function initializeView() {
            $("textarea.ui-autosize").on('change keyup keydown paste cut scroll', function () {
                $(this).innerHeight(0).scrollTop(0).innerHeight(this.scrollHeight);
            });
        }

        function isUserComment(comment) {
            return t.user && t.user.Uri === comment.agent;
        }

        function validateComment() {
            return t.comment.agent && t.comment.entity && t.comment.startTime && t.comment.text.length > 0;
        }

        function updateComment() {
            if (!t.comment.agent && t.user) {
                t.comment.agent = t.user.Uri;
            }

            if (!t.comment.startTime) {
                t.comment.startTime = new Date();

                console.log("Start comment: ", t.comment);
            }
        };

        function postComment(e) {
            if (!validateComment()) {
                console.log("Invalid comment: ", t.comment);

                return;
            }

            t.comment.endTime = new Date();

            if (e && e.ctrlKey) {
                t.comment.agent = 'http://artivity.online/faubulous';
            }

            commentService.post(t.comment).then(function (data) {
                t.comment.uri = data.uri;

                // Show the comment in the list of comments.
                t.comments.push(t.comment);

                // Clear the text and create a new comment.
                resetComment();

                console.log("Posted comment: ", t.comment);
            }, function (response) {
                console.error(response);
            });
        }

        function resetComment(clearText) {
            t.comment = {
                agent: null,
                entity: t.file,
                startTime: null,
                endTime: null,
                text: '',
                markers: []
            };

            if (t.user) {
                t.comment.agent = user.Uri;
            }

            console.log("Reset comment: ", t.comment);
        }

        function createMark(comment) {
            if (comment && comment.uri) {
                viewerService.executeCommand('createMark', comment.uri);
            }
        }

        function showMarks(comment) {
            if (comment && comment.uri) {
                markService.getMarksForEntity(comment.uri).then(function (data) {
                    t.marks = data;

                    viewerService.executeCommand('showMarks', t.marks);
                });
            }
        }

        function hideMarks(comment) {
            if (comment && comment.uri && t.marks.length > 0) {
                viewerService.executeCommand('hideMarks', t.marks);

                t.marks = [];
            }
        }
    }
})();