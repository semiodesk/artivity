(function () {

	angular
		.module('explorerApp')
		.factory('UpdateService', UpdateService);

	UpdateService.$inject = [];
	function UpdateService() {

		var downloadRunning = false;
		var downloadPromise;

		var endpoint = "https://static.semiodesk.com/artivity/win/appcast.xml";
		var service = {
			canUpdate: canUpdate,
			download: download,
			update: update


		};
		initService();
		return service;

		//////////////////////////////

		function initService() {
			service.checker = new UpdateChecker(endpoint);

		}

		//////////////////////////////

		function canUpdate() {
			return service.checker.canUpdate();
		}

		function download(data) {
			if (!downloadRunning) {
				downloadRunning = true
				downloadPromise = service.checker.downloadUpdate(data);
				downloadPromise.then(function () {
					downloadRunning = false;
				})
			}
			return downloadPromise;
		}

		function update(data) {
			return service.checker.executeUpdate(data);
		}
	}

})();

// TODO: pull out of this file to keep it unit testable
angular.module('explorerApp').run(['UpdateService', function (UpdateService) {
	UpdateService.canUpdate().then(function (val) {
		UpdateService.download(val).then(function (val) {
			UpdateService.update(val);
		});
}, function(msg) {
		console.log("no update");
	});
}]);