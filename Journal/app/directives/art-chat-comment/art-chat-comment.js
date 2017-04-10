(function () {
    angular.module('app').directive('artChatComment', artChatCommentDirective);

    function artChatCommentDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-chat-comment/art-chat-comment.html',
            scope: {
                comment: '='
            },
            controller: ChatCommentDirectiveController,
            controllerAs: 't',
            bindToController: true
        }
    }

    angular.module('app').controller('ChatCommentDirectiveController', ChatCommentDirectiveController);

    ChatCommentDirectiveController.$inject = ['$scope', '$element', 'agentService', 'commentService', 'formattingService', 'viewerService'];

    function ChatCommentDirectiveController($scope, $element, agentService, commentService, formattingService, viewerService) {
        var t = this;

        // FORMATTING
        t.getFormattedTime = formattingService.getFormattedTime;
        t.getFormattedDate = formattingService.getFormattedDate;
        t.getFormattedTimeFromNow = formattingService.getFormattedTimeFromNow;

        // MARKS
        t.createMark = viewerService.createMark;
        t.showMarks = viewerService.showMarks;
        t.hideMarks = viewerService.hideMarks;

        // REQUESTS
        t.enableResponse = false;

        t.hasResponded = function() {
            var agent = agentService.currentUser.Uri;

            for(i = 0; i < t.responses.length; i++) {
                var response = t.responses[i];

                if(response.agent === agent) {
                    return true;
                }
            }

            return false;
        }

        t.postYes = function (request) {
            var response = new Comment();
            response.type = 'ApprovalResponse';
            response.agent = agentService.currentUser.Uri;
            response.primarySource = request.uri;
            response.startTime = new Date();
            response.endTime = new Date();
            response.message = 'yes';

            if (response.validate()) {
                commentService.postResponse(response);
            }
        }

        t.postNo = function () {
            var response = new Comment();
            response.type = 'ApprovalResponse';
            response.agent = agentService.currentUser.Uri;
            response.primarySource = request.uri;
            response.startTime = new Date();
            response.endTime = new Date();
            response.message = 'no';

            if (response.validate()) {
                commentService.postResponse(response);
            }
        }

        t.$onInit = function () {
            if (t.comment) {
                t.isUser = t.comment.agent === agentService.currentUser.Uri;
                t.isRequest = t.comment.type.endsWith('Request');
                t.isResponse = t.comment.type.endsWith('Response');

                t.responses = [];

                if (t.isRequest) {
                    commentService.getCommentsForPrimarySource(t.comment.uri).then(function(data) {
                        t.responses = data;
                    });
                }

                t.enableResponse = t.hasResponded();
            }
        }
    }
})();