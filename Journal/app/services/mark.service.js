(function () {
    'use strict';

    angular.module('app').factory('markService', markService);

    markService.$inject = ['$http'];

    function markService($http) {
        var endpoint = apid.endpointUrl + "marks";

        return {
            getMarksForEntity: getMarksForEntity,
            createMark: createMark,
            updateMark: updateMark,
            deleteMark: deleteMark
        };

        function getMarksForEntity(entityUri) {
            var uri = encodeURIComponent(entityUri);

            return $http.get(endpoint + '?entityUri=' + uri).then(function (response) {
                return response.data;
            }, function () {
                console.error('Error while retrieving marks for entity: ' + uri)
            });
        }

        function createMark(mark) {
            return $http.post(endpoint, mark).then(function (response) {
                return response.data;
            }, function () {
                console.error('Error when creating mark.')
            });
        }

        function updateMark(mark) {
            return $http.put(endpoint, mark).then(function (response) {
                return response.data;
            }, function () {
                console.error('Error when updating mark.')
            });
        }

        function deleteMark(uri) {
            return $http.delete(endpoint + '?uri=' + uri).then(function (response) {
                return response.data;
            }, function () {
                console.error('Error when deleting mark.')
            });
        }
    }
})();