(function () {
    'use strict';

    angular.module('app').factory('derivationService', derivationService);

    derivationService.$inject = ['$http'];

    function derivationService($http) {
        var endpoint = apid.endpointUrl + "entity/derivations";

        var service = {};
        service.getAll = getAll;
        service.getById = getById;
        service.getImageUrl = getImageUrl;
        service.selectedDerivation = null;

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

        function getImageUrl(uri){
            return endpoint + '/rendering?uri=' + encodeURIComponent(uri);
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