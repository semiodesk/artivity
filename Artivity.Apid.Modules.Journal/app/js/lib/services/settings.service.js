(function () {
	angular.module('explorerApp').factory('settingsService', settingsService);

	function settingsService() {
		var controllers = [];

		return {
			controllers: controllers,
			registerController: registerController,
			submitAll: submitAll,
			resetAll: resetAll
		};

		function controllers(callback) {
			if (callback) {
				for (var i = 0; i < controllers.length; i++) {
					callback(controllers[i]);
				}
			}
		}

		function registerController(c) {
			if (controllers.indexOf(c) === -1) {
				controllers.push(c);
			}
		}

		function submitAll() {
			for (var i = 0; i < controllers.length; i++) {
				var c = controllers[i];

				if (c.submit) {
					c.submit();
				}
			}
		}

		function resetAll() {
			for (var i = 0; i < controllers.length; i++) {
				var c = controllers[i];

				if (c.reset) {
					c.reset();
				}
			}
		}
	}
})();