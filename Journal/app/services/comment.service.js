(function () {
    'use strict';

    angular.module('explorerApp').factory('commentService', commentService);

    commentService.$inject = ['$http'];

    function commentService($http) {
        var endpoint = apid.endpointUrl + "entity/derivations/comments";

        var service = {};

        
        service.post = post;



        return service;

        

        function post() {
            /*
            var uri = encodeURIComponent(project.Uri);

            return $http.post(endpoint + '?uri=' + uri, project).then(function (response) {
                service.projects = null;
                return response.data;
            }, handleError('Error when updating project.'));
            */
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