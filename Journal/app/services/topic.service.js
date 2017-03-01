(function () {
    angular.module('app').factory('topicService', topicService);

    topicService.$inject = ['$http'];

    function topicService($http) {
        var endpoint = apid.endpointUrl + "topics";

        return {
            get: get,
            post: post
        };

        function get(entityUri) {
            var uri = encodeURIComponent(entityUri);

            return $http.get(endpoint + '?entityUri=' + uri).then(function (response) {
                return response.data;
            }, handleError('Error while retrieving topics.'));
        }

        function post(topic) {
            return $http.post(endpoint, topic).then(function (response) {
                return response.data;
            }, handleError('Error when pushing topics.'));
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