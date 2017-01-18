(function () {
	angular.module('explorerApp').factory('artFileService', artFileService);

	artFileService.$inject = ['api'];
	function artFileService(api) {


		var service = {
			loadRecentFiles: loadRecentFiles,
            files: [],
            hasFiles: false
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
                }

                service.hasFiles = data.length > 0;
            });
        };
	}
})();