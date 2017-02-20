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

    function ChatControlDirectiveController(api, $scope, selectionService, formattingService, entityService, derivationService, commentService) {
        var t = this;

        t.element = null;

        t.entity = null;
        t.derivation = null;
        t.comment = null;
        t.comments = [];
        t.selectedComment = null;
        t.selectComment = selectComment;
        t.postComment = postComment;
        t.validateComment = validateComment;
        t.removeComment = removeComment;
        t.getFormattedTime = formattingService.getFormattedTime;
        t.getFormattedDate = formattingService.getFormattedDate;
        t.getFormattedTimeFromNow = formattingService.getFormattedTimeFromNow;

        initialize();

        function initialize() {
            initializeData();
            initializeView();
        }

        function initializeData() {
            entityService.getById($scope.entity).then(function (response) {
                var entity = response;

                if (entity.Revisions.length > 0) {
                    t.entity = entity;
                    t.derivation = entity.Revisions[0];

                    commentService.get(t.derivation).then(function (data) {
                        t.comments = [];

                        for (i = 0; i < data.length; i++) {
                            var c = data[i];

                            t.comments.unshift({
                                time: c.Time,
                                text: c.Message,
                                isUser: t.comments.length % 2 == 1
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

        function loadComments(deriv) {
            if (!t.comments.hasOwnProperty(deriv.Uri)) {
                commentService.get(deriv.Uri).then(function (data) {
                    t.comments[deriv.Uri] = data;
                });
            }
        }

        function postComment(comment) {
            if (validateComment(comment)) {
                comment.isUser = t.comments.length % 2 == 1;
                comment.time = new Date();

                t.comments.push(comment);

                resetComment();

                var collection = {
                    influence: t.derivation,
                    entity: t.entity.Uri,
                    startTime: comment.time, // TODO: record actual start time of comment writing.
                    endTime: new Date(),
                    comments: [comment]
                }

                commentService.post(collection).then(function (response) {
                    console.log(response);
                }, function (response) {
                    console.log(response);

                    // TODO: Show error message in frontend.
                });
            }
        }

        function validateComment(comment) {
            return comment.text.length > 0 &&
                t.entity != undefined &&
                t.derivation != undefined;
        }

        function resetComment() {
            t.comment = {
                time: new Date(),
                text: '',
                markers: []
            };

            t.selectedComment = t.comment;
        }

        function selectComment(i) {
            if (i < t.comments.length) {
                var comment = t.comments[i];

                t.selectedComment = comment;

                selectionService.selectedItem(comment);
            }
        }

        function removeComment(i) {
            if (i < t.comments.length) {
                var comment = t.comments[i];

                t.comments.splice(i, 1);

                if (selectionService.selectedItem() === comment) {
                    selectionService.clear();
                }
            }
        }

        function addMarker(marker) {
            t.selectedComment.marker.push(marker);
        }

        function removeMarker(markerName) {
            for (i = t.selectedComment.marker.length - 1; i >= 0; i--) {
                if (t.selectedComment.marker[i].name == markerName) {
                    t.selectedComment.marker.splice(i, 1);
                }
            }
        }
    }
})();