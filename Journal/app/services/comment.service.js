(function () {
    angular.module('app').factory('commentService', commentService);

    commentService.$inject = ['api'];

    function commentService(api) {
        var endpoint = apid.endpointUrl + "comments";

        return {
            get: get,
            post: post
        };

        function get(entityUri) {
            var uri = encodeURIComponent(entityUri);

            return api.get(endpoint + '?entityUri=' + uri).then(function (response) {
                return response.data;
            }, handleError('Error while retrieving comments.'));
        }

        function post(comment) {
            return api.post(endpoint, comment).then(function (response) {
                return response.data;
            }, handleError('Error when pushing comments.'));
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