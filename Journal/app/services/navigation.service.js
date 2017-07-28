(function () {
    angular.module('app').factory('navigationService', navigationService);

    function navigationService($state) {
        var t = this;

        t.initialized = false;

        t.initialize = function (rootScope, state, stateParams) {
            t.history = [{
                state: state,
                stateParams: stateParams
            }];

            if (!t.initialized) {
                t.initialized = true;

                // Listen to state changes.
                rootScope.$on('$stateChangeSuccess', onStateChangeSuccess);
                rootScope.$on('$stateChangeError', onStateChangeError);
            }
        }

        function onStateChangeSuccess(e, state, stateParams) {
            console.info(state, stateParams);

            if (t.history) {
                if (t.history.length > 1) {
                    var item = t.history[t.history.length - 1];

                    if ($state.is(item.state, state)) {
                        // We're navigating back..
                        return;
                    }
                }

                t.history.push({
                    state: state,
                    stateParams: stateParams
                });
            }
        }

        function onStateChangeError(e, state, stateParams) {
            console.error(state, stateParams);
        }

        t.canNavigateBack = function() {
            return !t.history || t.history.length > 1;
        }

        t.navigateBack = function (e) {
            if (!t.history) {
                $state.go('start');
            } else if (t.history.length > 1) {
                // Remove the last element from the history.
                t.history.splice(t.history.length - 1, 1);

                // Retrieve the last state from the history.
                var item = t.history[t.history.length - 1];

                // Navigate to the last element in the history.
                $state.go(item.state.name, item.stateParams);
            }
        }

        t.navigateForward = function (e) {}

        return t;
    }
})();