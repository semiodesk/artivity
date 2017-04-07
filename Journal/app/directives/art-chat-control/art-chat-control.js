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
            bindToController: true
        }
    }

    angular.module('app').controller('ChatControlDirectiveController', ChatControlDirectiveController);

    ChatControlDirectiveController.$inject = ['$scope', '$element', '$timeout', 'viewerService', 'agentService', 'entityService', 'commentService', 'selectionService', 'formattingService'];

    function ChatControlDirectiveController($scope, $element, $timeout, viewerService, agentService, entityService, commentService, selectionService, formattingService) {
        var t = this;

        t.getFormattedTime = formattingService.getFormattedTime;
        t.getFormattedDate = formattingService.getFormattedDate;
        t.getFormattedTimeFromNow = formattingService.getFormattedTimeFromNow;

        // COMMENTS
        t.comments = [];

        t.isUserComment = function (comment) {
            return comment.agent === agentService.currentUser.Uri;
        }

        t.loadCommentsForPrimarySource = function (entityUri) {
            console.log('Loading comments: ', entityUri);

            commentService.getCommentsForPrimarySource(t.entityUri).then(function (data) {
                var comments = [];

                for (i = 0; i < data.length; i++) {
                    var d = data[i];

                    var comment = new Comment();
                    comment.type = d.type;
                    comment.uri = d.uri;
                    comment.agent = d.agent;
                    comment.startTime = d.startTime;
                    comment.endTime = d.endTime;
                    comment.message = d.message;
                    comment.associations = d.associations;

                    // Insert at the beginning of the list.
                    comments.unshift(comment);
                }

                // Now assign the complete list of comments which triggers ng-repeat.
                t.comments = comments;
            });
        }

        // MARKS
        t.createMark = viewerService.createMark;
        t.showMarks = viewerService.showMarks;
        t.hideMarks = viewerService.hideMarks;

        // INIT
        t.$onInit = function () {
            // Enable ASCII smileys.
            emojione.ascii = true;
        }

        t.$postLink = function () {
            $("textarea.ui-autosize").on('change keyup keydown paste cut scroll', function () {
                $(this).innerHeight(0).scrollTop(0).innerHeight(this.scrollHeight);
            });

            $scope.$watch('t.entityUri', function () {
                if (t.entityUri) {
                    // Load the comments for the given primary source..
                    t.loadCommentsForPrimarySource(t.entityUri);
                }
            });

            // Keep the list scrolled to the bottom when ng-repeat is done.
            $timeout(function () {
                var container = $($element).find('.scroll-container');

                container.scrollTop(50);
            }, 0);
        }
    }
})();