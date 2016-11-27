(function () {
    'use strict';

    angular
        .module('explorerApp')
        .controller('LoadController', LoadController);

    LoadController.$inject = ['$location', '$rootScope', 'api', '$http'];
    function LoadController( $location, $rootScope, api, $http) {
        var vm = this;

        vm.showSpinner = true;
        vm.showError = false;

        vm.retry = retry;
        initController();

        function initController() {
            showLoadingSpinner();
            $http.get(apid.endpointUrl + '/agents').then(
				function (response) {
					$location.path("/files")
				}, 
                function() {
                    showConnectionError();
                });
        }

       
        function showLoadingSpinner()
        {
            vm.showSpinner = true;
            vm.showError = false;
        }
        
        function showConnectionError()
        {
           vm.showSpinner = false;
           vm.showError = true;
        }

        function retry()
        {
            initController();
        }


    }

})();	
    
