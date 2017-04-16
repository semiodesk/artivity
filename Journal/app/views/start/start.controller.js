(function () {
    angular.module('app').controller('StartController', StartController);

    StartController.$inject = ['$scope', '$location', '$http', 'appService', 'agentService', 'windowService'];

    function StartController($scope, $location, $http, appService, agentService, windowService) {
        var t = this;

        windowService.setClosable(true);
        windowService.setMinimizable(false);
        windowService.setMaximizable(false);

        t.initialized = false;
        t.showSpinner = true;
        t.showError = false;
        t.retry = retry;

        t.connectTime = 0;
        t.connectTimeout = appService.connectionTimeout();
        t.connectInterval = null;
        t.connectIntervalMs = appService.connectionInterval();

        function tryConnect() {
            $http.get(apid.endpointUrl + '/setup').then(
                function (response) {
                    stopConnect();

                    agentService.initialize(function () {
                        showStartView(response);
                    }, function () {
                        console.warn('Failed to initialize agent service.');
                    });
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

        function setupLinkHandler() {
            if (!t.initialized) {
                // Open external links with the system default browser.
                $(document).on('click', 'a[target="_blank"]', function (event) {
                    event.preventDefault();
                    windowService.openExternalLink(this.href);
                });

                t.initialized = true;
            }
        }

        function retry() {
            showLoadingSpinner();

            tryConnect();
        }

        function init() {
            setupLinkHandler();
            
            showLoadingSpinner();

            tryConnect();
        }

        init();
    };
})();