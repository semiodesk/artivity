(function () {
    angular.module('app').factory('markService', markService);

    markService.$inject = ['api'];

    function markService(api) {
        var endpoint = apid.endpointUrl + "marks";

        return {
            getMarksForEntity: getMarksForEntity,
            createMark: createMark,
            updateMark: updateMark,
            deleteMark: deleteMark
        };

        function getMarksForEntity(entityUri) {
            var uri = encodeURIComponent(entityUri);

            return api.get(endpoint + '?entityUri=' + uri).then(function (response) {
                return response.data;
            }, function () {
                console.error('Error while retrieving marks for entity: ' + uri)
            });
        }

        function createMark(mark) {
            return api.post(endpoint, mark).then(function (response) {
                return response.data;
            }, function () {
                console.error('Error when creating mark.')
            });
        }

        function updateMark(mark) {
            return api.put(endpoint, mark).then(function (response) {
                return response.data;
            }, function () {
                console.error('Error when updating mark.')
            });
        }

        function deleteMark(uri) {
            return api.delete(endpoint + '?uri=' + uri).then(function (response) {
                return response.data;
            }, function () {
                console.error('Error when deleting mark.')
            });
        }
    }
})();