(function () {
    angular.module('app').directive('artChatPanel', artChatPanelDirective);

    function artChatPanelDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-chat-panel/art-chat-panel.html',
            scope: {
                topicUri: '=?'
            },
            controller: ChatPanelDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('ChatPanelDirectiveController', ChatPanelDirectiveController);

    ChatPanelDirectiveController.$inject = ['$scope', '$element', '$mdPanel', 'agentService', 'entityService', 'commentService', 'selectionService', 'formattingService'];

    function ChatPanelDirectiveController($scope, $element, $mdPanel, agentService, entityService, commentService, selectionService) {
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
                    comment.agent = d.agent;
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
        t.onInit = function () {
            // Enable ASCII smileys.
            emojione.ascii = true;

            $scope.$watch('t.entityUri', function () {
                if (t.entityUri) {
                    // Load the comments for the given primary source..
                    t.loadCommentsForPrimarySource(t.entityUri);
                }
            });
        }

        t.onInit();
    }
})();