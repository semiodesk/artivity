(function () {
    'use strict';

    angular
        .module('explorerApp')
        .factory('UpdateService', UpdateService);

    UpdateService.$inject = [];
    function UpdateService() {

		var endpoint = "https://static.semiodesk.com/artivity/osx/appcast.xml";
        var service = {
			canUpdate: canUpdate,
			download: download,
			update: update

		};
		initService();
        return service;

		//////////////////////////////

		function initService(){
			service.checker = new UpdateChecker(endpoint);
		}

		//////////////////////////////

        function canUpdate(){
			return service.checker.canUpdate();
		}

		function download() {
			return service.checker.downloadUpdate();
		}

		function update(){
			return service.checker.executeUpdate();
		}
    }

})();

// TODO: pull out of this file to keep it unit testable
//angular.module('explorerApp').run(['UpdateService', function(UpdateService) {
//;
//}]);