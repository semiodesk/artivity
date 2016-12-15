angular.module('explorerApp').factory('settingsService', function () {
    var controllers = [];

	return {
		controllers: function(callback) {
			if(callback) {
				for(var i = 0; i < controllers.length; i++) {
					callback(controllers[i]);
				}
			}
		},
		registerController: function (c) {
            if(controllers.indexOf(c) === -1) {
			    controllers.push(c);
            }
		},
		submitAll: function() {
			for(var i = 0; i < controllers.length; i++) {
				var c = controllers[i];

				if(c.submit) {
					c.submit();
				}
			}
		},
		resetAll: function() {
			for(var i = 0; i < controllers.length; i++) {
				var c = controllers[i];

				if(c.reset) {
					c.reset();
				}
			}
		}
	};
});