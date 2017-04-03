(function () {
    angular.module('app').directive('artChatControl', artChatControlDirective);

    function artChatControlDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-chat-control/art-chat-control.html',
            scope: {
                entityUri: '=?'
            },
            controller: ChatControlDirectiveController,
            controllerAs: 't',
            bindToController: true,
            link: function (scope, element, attr, t) {
                // Enable ASCII smileys.
                emojione.ascii = true;

                $("textarea.ui-autosize").on('change keyup keydown paste cut scroll', function () {
                    $(this).innerHeight(0).scrollTop(0).innerHeight(this.scrollHeight);
                });

                scope.$watch('t.entityUri', function () {
                    if (t.entityUri) {
                        console.log('Entity changed: ', t.entityUri);

                        // Load the comments for the given primary source..
                        t.loadCommentsForEntity(t.entityUri);
                    }
                });
            }
        }
    }

    angular.module('app').controller('ChatControlDirectiveController', ChatControlDirectiveController);

    ChatControlDirectiveController.$inject = ['$scope', 'viewerService', 'agentService', 'entityService', 'commentService', 'markService', 'selectionService', 'formattingService'];

    function ChatControlDirectiveController($scope, viewerService, agentService, entityService, commentService, markService, selectionService, formattingService) {
        var t = this;

        t.getFormattedTime = formattingService.getFormattedTime;
        t.getFormattedDate = formattingService.getFormattedDate;
        t.getFormattedTimeFromNow = formattingService.getFormattedTimeFromNow;

        // COMMENTS
        t.comments = [];
        t.selectedComment = null;

        t.loadCommentsForEntity = function (entityUri) {
            console.log('Loading comments: ', entityUri);

            commentService.getCommentsForEntity(t.entityUri).then(function (data) {
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
        }

        t.isUserComment = function (comment) {
            return comment.agent === agentService.currentUser.Uri;
        }

        // MARKS
        t.marks = [];

        t.createMark = function (comment) {
            if (comment && comment.uri) {
                viewerService.executeCommand('createMark', comment.uri);
            }
        }

        t.showMarks = function (comment) {
            if (comment && comment.uri) {
                markService.getMarksForEntity(comment.uri).then(function (data) {
                    t.marks = data;

                    viewerService.executeCommand('showMarks', t.marks);
                });
            }
        }

        t.hideMarks = function (comment) {
            if (comment && comment.uri && t.marks.length > 0) {
                viewerService.executeCommand('hideMarks', t.marks);

                t.marks = [];
            }
        }
    }
})();