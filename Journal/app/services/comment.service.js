(function () {
    angular.module('app').factory('commentService', commentService);

    commentService.$inject = ['api'];

    function commentService(api) {
        var endpoint = apid.endpointUrl + "comments";

        return {
            getCommentsForPrimarySource: getCommentsForPrimarySource,
            postComment: postComment,
            postRequest: postRequest,
            postResponse: postResponse
        };

        function getCommentsForPrimarySource(entityUri) {
            var uri = encodeURIComponent(entityUri);

            return api.get(endpoint + '?entityUri=' + uri).then(function (r) {
                return r.data;
            }, handleError('Error while retrieving comments.'));
        }

        function postComment(comment) {
            return api.post(endpoint, comment).then(function (r) {
                return r.data;
            }, handleError('Error when pushing comment.'));
        }

        function postRequest(request) {
            return api.post(endpoint + '/requests', request).then(function (r) {
                return r.data;
            }, handleError('Error when pushing request.'));
        }

        function postResponse(response) {
            return api.post(endpoint + '/responses', response).then(function (r) {
                return r.data;
            }, handleError('Error when pushing response.'));         
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