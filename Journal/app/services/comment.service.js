(function () {
    'use strict';

    angular.module('app').factory('commentService', commentService);

    commentService.$inject = ['$http'];

    function commentService($http) {
        var endpoint = apid.endpointUrl + "entity/derivations/comments";

        var service = {};

        
        service.post = post;
        service.get = get;

        return service;

        function get(derivationUri) {
            var uri = encodeURIComponent(derivationUri);
            return $http.get(endpoint+'?uri='+uri).then(function (response) {
                return response.data;
            }, handleError('Error while retrieving comments.'));
           
        }

        function post(commentCollection) {
            return $http.post(endpoint, commentCollection).then(function (response) {
                return response.data;
            }, handleError('Error when pushing comments.'));
           
        }

        // private functions
        function handleSuccess(res) {
            return res.data;
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