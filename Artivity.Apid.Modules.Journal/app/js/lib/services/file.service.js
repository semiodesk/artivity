(function () {
	angular.module('explorerApp').factory('fileService', fileService);

	function fileService() {
		return {
			getFileName: getFileName,
			getFileNameWithoutExtension: getFileNameWithoutExtension,
			getFileExtension: getFileExtension
		};

		function getFileName(fileUrl) {
			var url = fileUrl;

			// Remove the anchor at the end, if there is one
			url = url.substring(0, (url.indexOf("#") == -1) ? url.length : url.indexOf("#"));

			// Remove the query after the file name, if there is one
			url = url.substring(0, (url.indexOf("?") == -1) ? url.length : url.indexOf("?"));

			// Remove everything before the last slash in the path
			url = url.substring(url.lastIndexOf("/") + 1, url.length);

			return decodeURIComponent(url);
		}

		function getFileNameWithoutExtension(fileName) {
			var components = fileName.split('.');

			// Return the file name which does not contain any dot.
			return components[0];
		}

		function getFileExtension(fileName) {
			var components = fileName.split('.');

			if (components.length == 1) {
				// Return the file name which does not contain any dot.
				return components[0];
			} else {
				// Return all components which are seperated by a dot. i.e. 'tar.gz'
				return components.slice(1).join('.');
			}
		}
	}
})();