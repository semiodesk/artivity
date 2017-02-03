(function () {
	angular.module('explorerApp').factory('settingsService', settingsService);

	function settingsService() {
		var controllers = [];

		return {
			controllers: controllers,
			clear: clear,
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

		function clear() {
			controllers = [];
		}

		function registerController(c) {
			for(var i = 0; i < controllers.length; i++) {
				if(controllers[i].constructor.name === c.constructor.name) {
					controllers.splice(i, 1);
				}
			}

			controllers.push(c);
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