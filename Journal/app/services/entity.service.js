(function () {
    'use strict';

    angular.module('explorerApp').factory('entityService', entityService);

    entityService.$inject = ['$http'];

    function entityService($http) {
        var endpoint = apid.endpointUrl + "entity/images";

        var service = {};
        service.getAll = getAll;
        service.getById = getById;
        service.selectedEntity = null;

        return service;

        function getAll() {

            return new Promise(function (resolve, reject) {
                $http.get(endpoint).then(function (result) {
                    var data = result.data;
                    resolve(data);
                }, handleError('Error when getting all projects.'));
            });
        }

        function getById(uri) {
            return $http.get(endpoint + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error when getting projects by id.'));
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