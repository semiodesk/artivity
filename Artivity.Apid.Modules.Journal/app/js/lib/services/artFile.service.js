(function () {
	angular.module('explorerApp').factory('artFileService', artFileService);

	artFileService.$inject = ['api'];
	function artFileService(api) {


		var service = {
			loadRecentFiles: loadRecentFiles,
            filterFilesByProject: filterFilesByProject,
            removeFilter: removeFilter,
            mute: mute,
            unmute: unmute,
            on: on,
            off: off,
            files: [],
            hasFiles: false,
            dispatcher: new EventDispatcher(),
            
		};

        return service;

		function loadRecentFiles () {
            api.getRecentFiles().then(function (data) {
                for (var i = 0; i < data.length; i++) {
                    var file = data[i];

                    if (i < service.files.length && service.files[i].uri == file.uri) {
                        break;
                    }

                    service.files.splice(i, 0, file);
                    service.dispatcher.raise('dataChanged', values);
                }

                service.hasFiles = data.length > 0;
            });
        }

        function clearFiles () {
            service.files = [];
        }

        function removeFilter(){
            clearFiles();
            loadRecentFiles();
        }

        function filterFilesByProject (projectUri) {
            clearFiles();
            api.getProjectFiles(projectUri).then(function (data) {
                for (var i = 0; i < data.length; i++) {
                    var file = data[i];

                    if (i < service.files.length && service.files[i].uri == file.uri) {
                        break;
                    }

                    service.files.splice(i, 0, file);
                    service.dispatcher.raise('dataChanged', values);
                }

                service.hasFiles = data.length > 0;
            });
        }

        
        function mute() {
            service.dispatcher.mute();
        }

        function unmute() {
            service.dispatcher.unmute();
        }

        function on(event, callback) {
            service.dispatcher.on(event, callback);
        }

        function off(event, callback) {
            service.dispatcher.off(event, callback);
        }
        
	}
})();