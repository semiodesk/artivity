(function () {
    angular.module('app').factory('taskService', taskService);

    taskService.$inject = ['$http'];

    function taskService($http) {
        var endpoint = apid.endpointUrl + "tasks";

        return {
            get: get,
            post: post,
            put: put
        };

        function get(entityUri) {
            var uri = encodeURIComponent(entityUri);

            return $http.get(endpoint + '?entityUri=' + uri).then(function (response) {
                return response.data;
            }, handleError('Error while retrieving tasks.'));
        }

        function post(task) {
            return $http.post(endpoint, task).then(function (response) {
                return response.data;
            }, handleError('Error when pushing tasks.'));
        }

        function put(task) {
            return $http.put(endpoint, task).then(function (response) {
                return response.data;
            }, handleError('Error when pushing tasks.'));
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