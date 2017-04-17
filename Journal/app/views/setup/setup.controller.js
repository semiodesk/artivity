(function () {
    angular.module('app').controller('SetupController', SetupController);

    SetupController.$inject = ['$rootScope', '$scope', '$location', 'api', 'settingsService', 'windowService'];

    function SetupController($rootScope, $scope, $location, api, settingsService, windowService) {
        var t = this;

        windowService.setMinimizable(false);
        windowService.setMaximizable(false);

        t.onActiveTabChanged = function () {
            var n = t.tabs.indexOf(t.activeTab);

            t.hasPreviousButton = n > 0;
            t.navigatePreviousDisable = false;

            t.hasNextButton = n < t.tabs.length - 1;
            t.navigateNextDisabled = true;

            t.setupComplete = t.setupComplete | !t.hasNextButton;
        }

        t.showTab = function (target) {
            var a = $('a[data-target="' + target + '"]');

            if (a) {
                t.activeTab = target;

                t.onActiveTabChanged();

                a.tab('show');
            }
        }

        t.navigateNext = function () {
            var n = t.tabs.indexOf(t.activeTab);

            if (n < t.tabs.length - 1) {
                t.showTab(t.tabs[n + 1]);
            }
        }

        t.navigatePrevious = function () {
            var n = t.tabs.indexOf(t.activeTab);

            if (n > 0) {
                t.showTab(t.tabs[n - 1]);
            }
        }

        t.submitAndReturn = function () {
            // Submit all setup pages.
            settingsService.submitAll();

            api.setRunSetup(false).then(function () {
                windowService.setWidth(992);

                // After the setup has been disabled, go to the homepage.
                $location.path("/");
            });
        }

        t.onInit = function () {
            t.tabs = [];

            each($('.tab-pane'), function (n, tab) {
                var id = $(tab).attr('id');

                if (id !== undefined) {
                    t.tabs.push('#' + id);
                }
            });

            if (t.tabs.length > 0) {
                t.activeTab = '#' + $('.tab-pane.active').attr('id');

                t.onActiveTabChanged();

                // http://getbootstrap.com/javascript/#tabs
                $('a[data-toggle="tab"]').bind('shown.bs.tab', function (e) {
                    t.activeTab = $(e.target).data('target');

                    t.onActiveTabChanged();

                    // The event is triggered from another thread. Trigger the digest manually for the UI to update.
                    try {
                        if (!$scope.$$phase) {
                            $scope.$digest();
                        }
                    } catch (error) {}
                });
            }

            $rootScope.$on('navigateNext', function () {
                t.navigateNext();
            });

            $rootScope.$on('navigateNextEnabled', function () {
                t.navigateNextDisabled = false;
            });
        }

        t.onInit();
    }
})();