(function () {
    angular.module('app').factory('entityService', entityService);

    entityService.$inject = ['$http'];

    function entityService($http) {
        var endpoint = apid.endpointUrl + "entity/images";

        var service = {};
        service.getAll = getAll;
        service.getById = getById;
        service.getRecentByFile = getRecentByFile;
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
            return $http.get(endpoint + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error when getting entity from id.'));
        }

        function getLatestDerivationFromFile(uri) {
            return $http.get(endpoint + "/derivations/latest?fileUri=" + encodeURIComponent(uri)).then(handleSuccess, handleError('Error when getting entity from id.'));
        }

        function getByFile(uri, offset, limit, sort) {
            // offset and limit should be ints
            // sort should either be 'asc' or 'desc'
            // TODO: handle res.count in handleSuccess
            return $http.get(endpoint + "?offset="+offset+"&limit="+limit+"&sort="+sort + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error when getting entity from id.'));
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