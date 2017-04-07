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
            bindToController: true
        }
    }

    angular.module('app').controller('ChatInputDirectiveController', ChatInputDirectiveController);

    ChatInputDirectiveController.$inject = ['$scope', '$element', 'agentService', 'commentService', 'projectService', 'viewerService'];

    function ChatInputDirectiveController($scope, $element, agentService, commentService, projectService, viewerService) {
        var t = this;

        // FOCUS
        t.focused = false;

        t.onFocused = function () {
            t.focused = true;
        }

        t.onBlurred = function () {
            t.focused = false;
        }

        // COMMENT
        t.comment = new Comment();

        t.onCommentChanged = function (comment) {
            if (!comment.startTime) {
                comment.startTime = new Date();
            }

            if (comment.message && projectService.currentProject) {
                t.handleMemberSelection(comment.message, projectService.currentProject);
            }
        }

        t.postComment = function (e, comment) {
            if(!comment.endTime) {
                comment.endTime = new Date();
            }

            if (comment.validate()) {
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

        t.postRequest = function (e, request) {
            if(!request.endTime) {
                request.endTime = new Date();
            }

            if (request.validate()) {
                request.type = 'ApprovalRequest';

                for(i = 0; i < t.members.length; i++) {
                    var association = {
                        agent: t.members[i].Agent.Uri,
                        role: t.members[i].RoleUri
                    }

                    request.associations.push(association);
                }

                console.log('Requesting approval from: ', request.associations);

                commentService.postRequest(request).then(function (data) {
                    request.uri = data.uri;

                    // Show the comment in the list of comments.
                    t.comments.push(request);

                    console.log("Posted request: ", request);

                    t.resetComment();
                }, function (response) {
                    console.error(response);
                });
            } else {
                console.log("Invalid request: ", request);
            }
        }

        t.resetComment = function (clearText) {
            t.comment = new Comment();
            t.comment.agent = agentService.currentUser.Uri;
            t.comment.primarySource = t.entityUri;

            console.log("Reset comment: ", t.comment);
        }

        // FEEDBACK
        t.atExpression = new RegExp('@(.*)');
        t.members = [];
        t.showMembers = false;

        t.handleMemberSelection = function (input, project) {
            var match = input.match(t.atExpression);

            t.showMembers = match ? true : false;

            if (match) {
                var q = match[1];

                if (q.length > 0) {
                    for (i = 0; i < t.members.length; i++) {
                        var m = t.members[i];

                        m.visible = m.Agent.Name.toLowerCase().startsWith(q);
                    }
                } else {
                    for (i = 0; i < t.members.length; i++) {
                        var m = t.members[i];

                        m.visible = true;
                    }
                }
            }
        }

        t.selectMember = function (input, member) {
            var match = input.match(t.atExpression);

            if (match) {
                var q = match[1];

                input = input.substring(0, input.indexOf(q));
                input += '@' + member.Agent.Name;

                t.comment.message = input;

                t.showMembers = false;
            }
        }

        // TOOLS
        t.createMark = function (e) {
            if (comment) {
                viewerService.createMark(comment);
            }
        }

        t.selectEmoticon = function (e) {
        }

        // INIT
        t.$onInit = function () {
            if (projectService.currentProject) {
                var project = projectService.currentProject;

                projectService.getMembers(project.Uri).then(function (members) {
                    members.sort(function (a, b) {
                        return a.Agent.Name.localeCompare(b.Agent.Name);
                    });

                    t.members = members;
                });
            }
        }

        t.$postLink = function () {
            $scope.$watch('t.entityUri', function () {
                t.resetComment();
            });
        }
    }
})();