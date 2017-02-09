(function () {
    angular.module('app').controller('StartController', StartController);

    function StartController($location, api, $http, windowService, appService) {
        var t = this;

        windowService.setClosable(true);
        windowService.setMinimizable(false);
        windowService.setMaximizable(false);

        t.showSpinner = true;
        t.showError = false;
        t.retry = retry;

        t.connectTime = 0;
        t.connectTimeout = appService.connectionTimeout();
        t.connectInterval = null;
        t.connectIntervalMs = appService.connectionInterval();

        function init() {
            showLoadingSpinner();

            tryConnect();
        }

        function tryConnect() {
            $http.get(apid.endpointUrl + '/setup').then(
                function (response) {
                    stopConnect();
                    showStartView(response);
                },
                function () {
                    if (t.connectInterval === null) {
                        t.connectInterval = setInterval(tryConnect, t.connectIntervalMs);
                    } else if (t.connectTime < t.connectTimeout) {
                        t.connectTime += t.connectIntervalMs;
                    } else {
                        stopConnect();
                        showConnectionError();
                    }
                });
        }

        function stopConnect() {
            if (t.connectInterval !== null) {
                clearInterval(t.connectInterval);

                t.connectInterval = null;
                t.connectTime = 0;
            }
        }

        function showStartView(response) {
            t.showSpinner = false;

            if (response.data) {
                windowService.setWidth(991);

                $location.path("/setup");
            } else {
                $location.path("/files");
            }
        }

        function showLoadingSpinner() {
            t.showSpinner = true;
            t.showError = false;
        }

        function showConnectionError() {
            t.showSpinner = false;
            t.showError = true;
        }

        function retry() {
            init();
        }

        init();
    };
})();