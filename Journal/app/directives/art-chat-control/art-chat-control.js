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

    ChatControlDirectiveController.$inject = ['$scope', '$element', 'agentService', 'entityService', 'commentService', 'selectionService', 'formattingService'];

    function ChatControlDirectiveController($scope, $element, agentService, entityService, commentService, selectionService) {
        var t = this;

        // COMMENTS
        t.comments = [];

        t.loadCommentsForPrimarySource = function (entityUri) {
            console.log('Loading comments: ', entityUri);

            commentService.getCommentsForPrimarySource(t.entityUri).then(function (data) {
                var comments = [];

                for (i = 0; i < data.length; i++) {
                    var d = data[i];

                    var comment = new Comment();
                    comment.type = d.type;
                    comment.uri = d.uri;
                    comment.agentId = d.agentId;
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
        }
    }
})();