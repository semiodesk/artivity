(function () {
	angular.module('explorerApp').factory('updateService', updateService);

	updateService.$inject = [];

	function updateService() {
		var appcastUrl = "https://static.semiodesk.com/artivity/win/appcast.xml";
		var checker = new UpdateChecker(appcastUrl);

		var downloading = false;
		var currentDownload;

		var service = {
			update: null,
			isUpdateAvailable: isUpdateAvailable,
			isUpdateDownloaded: isUpdateDownloaded,
			isDownloading: isDownloading,
			downloadUpdate: downloadUpdate,
			installUpdate: installUpdate,
			on: on,
			off: off
		};

		return service;

		function isUpdateAvailable() {
			return new Promise(function (resolve, reject) {
				if (service.update) {
					resolve(service.update);
				} else {
					checker.isUpdateAvailable().then(function (update) {
						service.update = update;

						resolve(update);
					}, function () {
						reject();
					});
				}
			});
		}

		function isUpdateDownloaded() {
			return new Promise(function (resolve, reject) {
				return isUpdateAvailable().then(function () {
					checker.isUpdateDownloaded(service.update).then(function() {
						resolve(service.update);
					}).catch(function() {
						reject();
					});
				});
			});
		}

		function isDownloading() {
			return downloading;
		}

		function downloadUpdate() {
			if (downloading) {
				return currentDownload;
			}

			downloading = true;

			currentDownload = checker.downloadUpdate(service.update);
			currentDownload.then(function () {
				downloading = false;
			}, function () {
				downloading = false;
			});

			return currentDownload;
		}

		function installUpdate() {
			return checker.installUpdate(service.update);
		}

		function on(event, callback) {
			checker.on(event, callback);
		}

		function off(event, callback) {
			checker.off(event, callback);
		}
	}
})();