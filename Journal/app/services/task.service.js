(function () {
    angular.module('app').factory('taskService', taskService);

    taskService.$inject = ['api'];

    function taskService(api) {
        var endpoint = apid.endpointUrl + "tasks";

        return {
            get: get,
            post: post,
            put: put
        };

        function get(entityUri) {
            var uri = encodeURIComponent(entityUri);

            return api.get(endpoint + '?entityUri=' + uri).then(function (response) {
                return response.data;
            }, handleError('Error while retrieving tasks.'));
        }

        function post(task) {
            return api.post(endpoint, task).then(function (response) {
                return response.data;
            }, handleError('Error when pushing tasks.'));
        }

        function put(task) {
            return api.put(endpoint, task).then(function (response) {
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