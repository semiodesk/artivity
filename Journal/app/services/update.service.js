(function () {
	angular.module('app').factory('updateService', updateService);

	updateService.$inject = ['appService'];

	function updateService(appService) {
		var appcastUrl = "https://static.semiodesk.com/artivity/win/appcast.xml";
		var checker = new UpdateChecker(appcastUrl, undefined, appService);

		var downloading = false;
		var currentDownload;
		var updateAvailable;

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
				if (service.update !== null) {
					resolve(service.update);
				} else {
					checker.isUpdateAvailable().then(function (update) {
						service.update = update;

						resolve(update);
					}, function () {
						service.update = null;

						reject(null);
					});
				}
			});
		}

		function isUpdateDownloaded() {
			return new Promise(function (resolve, reject) {
				if (service.update != null) {
					checker.isUpdateDownloaded(service.update).then(function () {
						resolve(service.update);
					}).catch(function () {
						reject();
					});
				}
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