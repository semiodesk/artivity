angular.module('explorerApp').directive('artClientStateInfo', ClientStateInfoDirective);

function ClientStateInfoDirective() {
    return {
        scope: {},
        templateUrl: 'partials/directives/art-client-state-info.html',
        controller: ClientStateInfoDirectiveController,
        controllerAs: 't',
        link: function(scope, element, attr, ctrl) {
            attr.$observe('state', function(value) {
                ctrl.setClientState(value);
            });
        }
    }
}

app.controller('ClientStateInfoDirectiveController', ClientStateInfoDirectiveController);

function ClientStateInfoDirectiveController ($scope) {
    var t = this;

    t.setClientState = function(clientState) {
        t.clientState = clientState;
    }
}