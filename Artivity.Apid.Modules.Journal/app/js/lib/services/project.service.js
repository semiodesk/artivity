(function () {
    'use strict';

    angular
        .module('explorerApp')
        .factory('projectService', projectService);

    projectService.$inject = ['$http'];
    function projectService($http) {
        var service = {};

        var endpoint = apid.endpointUrl + "projects";

        service.getAll = getAll;
        service.getById = getById;
        service.create = create;
        service.update = update;
        service.remove = remove;
        service.addFileToProject = addFileToProject;
        service.selectedProject = null;
        service.projects = null;

        return service;

        function getAll() {
            if( service.Projects != null) {
                return new Promise(function( resolve, reject) { resolve(service.Projects);})
            }

            return new Promise(function(resolve, reject){
                 $http.get(endpoint).then(function(result){
                     var data = result.data;
                     service.Projects = data;
                     resolve(data);
                 } , handleError('Error getting all projects'));
            });
        }

        function getById(uri) {
            return $http.get(endpoint + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error getting projects by id'));

        }

        function create() {
            return new Promise(function(resolve, reject){
             $http.get(endpoint + '/new').then(function(result){
                 resolve(result.data);
             }, handleError('Error creating projects'));
            });

        }

        function update(project) {
            var uri = encodeURIComponent(project.Uri);
            return $http.put(endpoint + '?uri=' + uri, project).then(handleSuccess, handleError('Error updating projects'));

        }

        function remove(uri) {
            return $http.delete(endpoint + '?uri=' + encodeURIComponent(uri)).then(handleSuccess, handleError('Error deleting projects'));
        }

        function addFileToProject(projectUri, fileUri) {
            return $http.get(endpoint + '/addFileToProject?projectUri=' + encodeURIComponent(projectUri)+"&fileUri="+encodeURIComponent(fileUri)).then(handleSuccess, handleError('Error deleting projects'));
        }


        // private functions
        function handleSuccess(res) {
            return res.data;
        }

        function handleError(error) {
            return function () {
                return { success: false, message: error };
            };
        }
    }

})();
