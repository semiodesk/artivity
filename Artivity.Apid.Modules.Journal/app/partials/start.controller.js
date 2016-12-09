(function () {
    'use strict';

    angular.module('explorerApp').controller('StartController', StartController);

    function StartController($location, api, $http) {
        var t = this;

        t.showSpinner = true;
        t.showError = false;
        t.retry = retry;

        init();

        function init() {
            showLoadingSpinner();
            
            $http.get(apid.endpointUrl + '/agents').then(
				function (response) {
					$location.path("/files")
				}, 
                function() {
                    showConnectionError();
                });
        }
       
        function retry()
        {
            init();
        }

        function showLoadingSpinner()
        {
            t.showSpinner = true;
            t.showError = false;
        }
        
        function showConnectionError()
        {
            t.showSpinner = false;
            t.showError = true;
        }
    };
})();