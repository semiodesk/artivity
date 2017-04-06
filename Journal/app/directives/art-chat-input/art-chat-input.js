(function () {
    angular.module('app').directive('artChatInput', artChatInputDirective);

    function artChatInputDirective() {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-chat-input/art-chat-input.html',
            scope: {
                entityUri: '=',
                comments: '='
            },
            controller: ChatInputDirectiveController,
            controllerAs: 't',
            bindToController: true,
            link: function (scope, element, attr, t) {
                t.element = element;

                scope.$watch('t.entityUri', function () {
                    // Reset the primary source on the new comment.
                    t.resetComment();
                });
            }
        }
    }

    angular.module('app').controller('ChatInputDirectiveController', ChatInputDirectiveController);

    ChatInputDirectiveController.$inject = ['$scope', 'agentService', 'entityService', 'commentService', 'markService', 'projectService', 'viewerService'];

    function ChatInputDirectiveController($scope, agentService, entityService, commentService, markService, projectService, viewerService) {
        var t = this;

        // COMMENT
        t.comment = null;

        t.validateComment = function (comment) {
            return comment.agent && comment.entity && comment.startTime && comment.text.length > 0;
        }

        t.updateComment = function (comment) {
            if (!comment.startTime) {
                comment.startTime = new Date();

                console.log("Started comment: ", comment);
            }
        }

        t.postComment = function (e, comment) {
            if (t.validateComment(comment)) {
                comment.endTime = new Date();

                if (e && e.ctrlKey) {
                    comment.agent = 'http://artivity.online/test';
                }

                commentService.postComment(comment).then(function (data) {
                    comment.uri = data.uri;

                    // Show the comment in the list of comments.
                    t.comments.push(comment);

                    console.log("Posted comment: ", comment);

                    t.resetComment();
                }, function (response) {
                    console.error(response);
                });
            } else {
                console.log("Invalid comment: ", comment);
            }
        }

        t.resetComment = function (clearText) {
            t.comment = {
                agent: agentService.currentUser.Uri,
                entity: t.entityUri,
                startTime: null,
                endTime: null,
                text: '',
                markers: [],
                requestType: undefined
            };

            console.log("Reset comment: ", t.comment);
        }

        // TOOLS

        t.selectedTool = null;

        t.createMark = function (e) {
            t.selectedTool = 'mark';

            if (comment && comment.uri) {
                viewerService.executeCommand('createMark', comment.uri);
            }
        }

        t.createRequest = function (e) {
            if (projectService.currentProject) {
                t.selectedTool = 'request';

                t.comment.requestType = 'Request';
                t.comment.associations = [];

                projectService.getMembers(projectService.currentProject.Uri).then(function (result) {
                    if (result.length > 0) {
                        for (i = 0; i < result.length; i++) {
                            var member = result[i];

                            var association = {
                                agent: member.Agent.Uri,
                                role: member.RoleUri
                            };

                            t.comment.associations.push(association);
                        }

                        console.log('Requesting feedback from: ', t.comment.associations);
                    }
                });
            }
        }

        t.selectEmoticon = function (e) {
            t.selectedTool = 'emoticon';
        }

        // FOCUS
        t.focused = false;

        t.onFocused = function () {
            t.focused = true;
        }

        t.onBlurred = function () {
            t.focused = false;
        }
    }
})();