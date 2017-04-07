(function () {
    angular.module('app').factory('commentService', commentService);

    commentService.$inject = ['api'];

    function commentService(api) {
        var endpoint = apid.endpointUrl + "comments";

        return {
            getCommentsForPrimarySource: getCommentsForPrimarySource,
            postComment: postComment,
            postRequest: postRequest
        };

        function getCommentsForPrimarySource(entityUri) {
            var uri = encodeURIComponent(entityUri);

            return api.get(endpoint + '?entityUri=' + uri).then(function (response) {
                return response.data;
            }, handleError('Error while retrieving comments.'));
        }

        function postComment(comment) {
            return api.post(endpoint, comment).then(function (response) {
                return response.data;
            }, handleError('Error when pushing comment.'));
        }

        function postRequest(request) {
            return api.post(endpoint + '/requests', request).then(function (response) {
                return response.data;
            }, handleError('Error when pushing request.'));
        }

        function handleError(error) {
            return function () {
                return {
                    success: false,
                    message: error
                };
            };
        }
    }
})();