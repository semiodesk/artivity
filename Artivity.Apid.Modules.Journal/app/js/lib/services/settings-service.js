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
		}
	};
});