(function () {
    angular.module('app').controller('StartStateController', StartStateController);

    StartStateController.$inject = ['$scope', '$state', '$http', 'appService', 'agentService', 'windowService', 'cookieService'];

    function StartStateController($scope, $state, $http, appService, agentService, windowService, cookieService) {
        var t = this;

        t.tryConnect = function() {
            $http.get(apid.endpointUrl + '/setup').then(
                function (response) {
                    t.stopConnect();

                    agentService.initialize(function () {
                        t.showStartView(response);
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
                        t.stopConnect();
                        t.showConnectionError();
                    }
                });
        }

        t.stopConnect = function() {
            if (t.connectInterval !== null) {
                clearInterval(t.connectInterval);

                t.connectInterval = null;
                t.connectTime = 0;
            }
        }

        t.showStartView = function(response) {
            t.showSpinner = false;

            if (response.data) {
                windowService.setWidth(991);

                $state.go("setup");
            } else {
                // Restore the selected tab after an app restart.
                var i = cookieService.get('tabs.selectedTab', 0);

                $state.go("main.view.recently-used", {index: i});
            }
        }

        t.showLoadingSpinner = function() {
            t.showSpinner = true;
            t.showError = false;
        }

        t.showConnectionError = function() {
            t.showSpinner = false;
            t.showError = true;
        }

        t.setupLinkHandler = function() {
            if (!t.initialized) {
                // Open external links with the system default browser.
                $(document).on('click', 'a[target="_blank"]', function (event) {
                    event.preventDefault();
                    windowService.openExternalLink(this.href);
                });

                t.initialized = true;
            }
        }

        t.retry = function() {
            t.showLoadingSpinner();
            t.tryConnect();
        }

        t.onInit = function () {
            windowService.setClosable(true);
            windowService.setMinimizable(false);
            windowService.setMaximizable(false);

            t.initialized = false;
            t.showSpinner = true;
            t.showError = false;

            t.connectTime = 0;
            t.connectTimeout = appService.connectionTimeout();
            t.connectInterval = null;
            t.connectIntervalMs = appService.connectionInterval();

            t.setupLinkHandler();
            t.showLoadingSpinner();
            t.tryConnect();
        }

        t.onInit();
    };
})();