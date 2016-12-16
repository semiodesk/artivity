angular.module('explorerApp').controller('StartController', StartController);

function StartController($location, api, $http, windowService) {
    var t = this;

    windowService.setMinimizable(false);
    windowService.setMaximizable(false);

    t.showSpinner = true;
    t.showError = false;
    t.retry = retry;

    init();

    function init() {
        showLoadingSpinner();

        $http.get(apid.endpointUrl + '/setup').then(
            function (response) {
                if(response.data) {
                    windowService.setWidth(991);
                    
                    $location.path("/setup");
                } else {
                    $location.path("/files");
                }
            },
            function () {
                showConnectionError();
            });
    }

    function retry() {
        init();
    }

    function showLoadingSpinner() {
        t.showSpinner = true;
        t.showError = false;
    }

    function showConnectionError() {
        t.showSpinner = false;
        t.showError = true;
    }
};