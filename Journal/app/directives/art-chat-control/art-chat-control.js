(function () {
    angular.module('app').directive('artChatControl', artChatControlDirective);

    function artChatControlDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-chat-control/art-chat-control.html',
            scope: {
                entity: '@'
            },
            controller: ChatControlDirectiveController,
            controllerAs: 't',
            link: function (scope, element, attributes, ctrl) {
                ctrl.element = element;
            }
        }
    }

    angular.module('app').controller('ChatControlDirectiveController', ChatControlDirectiveController);

    ChatControlDirectiveController.$inject = ['$scope', 'api', 'agentService', 'entityService', 'commentService', 'selectionService', 'formattingService'];

    function ChatControlDirectiveController($scope, api, agentService, entityService, commentService, selectionService, formattingService) {
        var t = this;

        t.user = null;
        t.entity = null;
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

        initialize();

        function initialize() {
            initializeData();
            initializeView();
        }

        function initializeData() {
            agentService.getUser().then(function (data) {
                t.user = data;

                // Set the user URI for the new comments.
                t.comment.agent = t.user.Uri;

                if ($scope.entity) {
                    // Make sure the user is properly initialized before retrieving the entity derivations.
                    entityService.getById($scope.entity).then(function (response) {
                        var entity = response;

                        if (entity.RevisionUris && entity.RevisionUris.length > 0) {
                            t.entity = entity.RevisionUris[0];

                            // Set the entity URI as primary source for the comments.
                            t.comment.entity = t.entity;

                            commentService.get(t.entity).then(function (data) {
                                t.comments = [];

                                for (i = 0; i < data.length; i++) {
                                    var c = data[i];

                                    // Insert at the beginning of the list.
                                    t.comments.unshift({
                                        agent: c.agent,
                                        time: c.time,
                                        text: c.message
                                    });
                                }
                            });
                        }
                    });
                }
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
            if (!t.comment.startTime) {
                t.comment.startTime = new Date();

                console.log("Start comment: ", t.comment);
            }
        };

        function postComment() {
            if (!validateComment()) {
                console.log("Invalid comment: ", t.comment);

                return;
            }

            t.comment.endTime = new Date();

            commentService.post(t.comment).then(function (response) {
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
                entity: t.entity,
                startTime: null,
                endTime: null,
                text: '',
                markers: []
            };

            console.log("Reset comment: ", t.comment);
        }
    }
})();