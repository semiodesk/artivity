﻿(function () {
    angular.module('app').factory('entityService', entityService);

    entityService.$inject = ['api'];

    function entityService(api) {
        var endpoint = apid.endpointUrl + "files";

        var service = {};
        service.getAll = getAll;
        service.getById = getById;
        service.getRevisions = getRevisions;
        service.getLatestRevisionFromFileUri = getLatestRevisionFromFileUri;
        service.selectedEntity = null;

        return service;

        function getAll() {
            return new Promise(function (resolve, reject) {
                api.get(endpoint).then(function (result) {
                    var data = result.data;
                    resolve(data);
                }, handleError('Error when getting all projects.'));
            });
        }

        function getById(uri) {
            return api.get(endpoint + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error when getting entity from id.'));
        }

        function getByFile(uri, offset, limit, sort) {
            // offset and limit should be ints
            // sort should either be 'asc' or 'desc'
            // TODO: handle res.count in handleSuccess
            return api.get(endpoint + "?offset=" + offset + "&limit=" + limit + "&sort=" + sort + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error when getting entity from id.'));
        }

        function getRevisions(fileUri) {
            return api.get(endpoint + "/revisions?fileUri=" + fileUri).then(function (response) {
                if (response.data.length) {
                    return response.data;
                }
            }, handleError('Error when retrieving revision from file URI.'));
        }

        function getLatestRevisionFromFileUri(fileUri) {
            return api.get(endpoint + "/revisions/latest?fileUri=" + fileUri).then(function (response) {
                if (response.data.length) {
                    return response.data[0];
                }
            }, handleError('Error when retrieving latest revision from file URI.'));
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