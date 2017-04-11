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

    ChatInputDirectiveController.$inject = ['$scope', '$element', '$sce', 'agentService', 'commentService', 'projectService', 'viewerService'];

    function ChatInputDirectiveController($scope, $element, $sce, agentService, commentService, projectService, viewerService) {
        var t = this;

        // FOCUS
        t.input = '';
        t.inputElement = null;
        t.hasInput = false;
        t.focused = false;

        t.onFocused = function () {
            t.focused = true;
        }

        t.onBlurred = function () {
            t.focused = false;
        }

        t.onEscape = function () {
            if (t.showEmoticonPanel) {
                t.showEmoticonPanel = false;
            }
        }

        // COMMENT
        t.comment = new Comment();

        t.postComment = function (e, comment) {
            if (!comment.endTime) {
                comment.endTime = new Date();
            }

            if (comment.validate()) {
                commentService.postComment(comment).then(function (data) {
                    comment.uri = data.uri;

                    // Show the comment in the list of comments.
                    t.comments.push(comment);

                    console.log("Posted comment: ", comment);

                    t.resetComment();

                    if(t.showEmoticonPanel) {
                        t.toggleEmoticonPanel();
                    }
                }, function (response) {
                    console.error(response);
                });
            } else {
                console.log("Invalid comment: ", comment);
            }
        }

        t.postRequest = function (e, request) {
            if (!request.endTime) {
                request.endTime = new Date();
            }

            if (request.validate()) {
                request.type = 'ApprovalRequest';

                for (i = 0; i < t.members.length; i++) {
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

                t.input = input;

                t.showMembers = false;
            }
        }

        // EMOTICONS
        t.showEmoticonPanel = false;

        t.toggleEmoticonPanel = function () {
            if (!t.focused) {
                t.inputElement.focus();
            }

            t.showEmoticonPanel = !t.showEmoticonPanel;
        }

        // TOOLS
        t.createMark = function (e) {
            if (comment) {
                viewerService.createMark(comment);
            }
        }

        // PARSING
        var toUnicode = function (html) {
            var result = '';
            var input = $.parseHTML(html);

            $.each(input, function (i, element) {
                if (element.nodeType === 3) {
                    result += element.textContent;
                } else if (element.localName === 'img' && element.className === 'emojione') {
                    // We *should* be storing the emoticons in unicode. However, there seems 
                    // to be a problem when loading the comments from the db.
                    //result += emojione.shortnameToUnicode(element.title);
                    result += element.title;
                }
            });

            return result;
        }

        var toHtml = function (unicode) {
            return emojione.toImage(unicode);
        }

        var sanitizeHtml = function (html) {
            return toHtml(toUnicode(html));
        }

        var resetCursor = function (element) {
            var range, selection;

            if (document.createRange) //Firefox, Chrome, Opera, Safari, IE 9+
            {
                range = document.createRange(); //Create a range (a range is a like the selection but invisible)
                range.selectNodeContents(element); //Select the entire contents of the element with the range
                range.collapse(false); //collapse the range to the end point. false means collapse to end rather than the start
                selection = window.getSelection(); //get the selection object (allows you to change selection)
                selection.removeAllRanges(); //remove any selections already made
                selection.addRange(range); //make the range you have just created the visible selection
            } else if (document.selection) //IE 8 and lower
            {
                range = document.body.createTextRange(); //Create a range (a range is a like the selection but invisible)
                range.moveToElementText(element); //Select the entire contents of the element with the range
                range.collapse(false); //collapse the range to the end point. false means collapse to end rather than the start
                range.select(); //Select the range (make it the visible selection
            }
        }

        // INIT
        t.$onInit = function () {
            if (projectService.currentProject) {
                var project = projectService.currentProject;

                projectService.getMembers(project.Uri).then(function (members) {
                    if (members && members.length > 0) {
                        members.sort(function (a, b) {
                            return a.Agent.Name.localeCompare(b.Agent.Name);
                        });
                    }

                    t.members = members;
                });
            }
        }

        t.$postLink = function () {
            $scope.$watch('t.entityUri', function () {
                t.resetComment();
            });

            $scope.$watch('t.comment', function (e) {
                t.input = t.comment.message;
            });

            t.inputElement = $('.textarea.textarea-editable');

            if (t.inputElement) {
                $scope.$watch('t.input', function () {
                    // Set the comment start time.
                    if (!t.comment.startTime) {
                        t.comment.startTime = new Date();
                    }

                    t.comment.message = toUnicode(t.inputElement.html());

                    t.hasInput = t.comment.message.length;
                });

                t.inputElement.on('keydown', function (e) {
                    var keyCode = e.keyCode || e.which;

                    if (keyCode == 9) {
                        e.preventDefault();

                        $scope.$apply(function () {
                            t.toggleEmoticonPanel();
                        });
                    }
                });

                t.inputElement.on('input', function () {
                    var html = sanitizeHtml(t.inputElement.html());

                    if (html != t.inputElement.html()) {
                        $scope.$apply(function () {
                            t.input = $sce.trustAsHtml(html);

                            resetCursor(t.inputElement[0]);
                        });
                    }
                });

                $scope.$on('emoticonSelected', function (e, unicode) {
                    var html = sanitizeHtml(t.inputElement.html() + emojione.toImage(unicode));

                    // Sanitize the input and replace ASCII smileys.
                    t.input = $sce.trustAsHtml(html);
                });
            }
        }
    }
})();