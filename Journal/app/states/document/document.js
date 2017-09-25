(function () {
    angular.module('app').controller("DocumentViewController", DocumentViewController);

    DocumentViewController.$inject = ['$scope', '$state', '$stateParams', '$element', '$mdPanel', 'entityService', 'viewerService', 'syncService'];

    function DocumentViewController($scope, $state, $stateParams, $element, $mdPanel, entityService, viewerService, syncService) {
        var t = this;

        t.latestRevisionUri = null;

        t.loadLatestRevision = function (file) {
            if (file && file.uri) {
                entityService.getLatestRevisionFromFileUri(file.uri).then(function (data) {
                    if (data.revision) {
                        t.latestRevisionUri = data.revision;
                    }
                });
            }
        }

        t.publishLatestRevision = function (file) {
            if (file && file.uri) {
                entityService.publishLatestRevisionFromFileUri(file.uri).then(function (data) {
                    syncService.synchronize();
                });
            }
        }

        t.$onInit = function () {
            if ($stateParams.fileUri) {
                entityService.get($stateParams.fileUri).then(function (data) {
                    t.file = data;

                    t.loadLatestRevision(t.file);
                });
            }

            var panel = null;

            $(document).on('showPanel', function(e, params) {
                var container = angular.element($('.viewer-container'));

                var offset = $('.viewer-container').offset();

                var position = $mdPanel.newPanelPosition()
                    .absolute()
                    .left(offset.left + params.event.localX + 'px')
                    .top(offset.top + params.event.localY + 'px');

                var animation = $mdPanel.newPanelAnimation()
                    .withAnimation($mdPanel.animation.FADE);

                var config = {
                    animation: animation,
                    attachTo: container,
                    controller: 'ChatPanelDirectiveController',
                    controllerAs: 't',
                    templateUrl: 'app/directives/art-chat-panel/art-chat-panel.html',
                    panelClass: 'art-chat-panel',
                    position: position,
                    trapFocus: true,
                    focusOnOpen: true,
                    zIndex: 150,
                    clickOutsideToClose: true,
                    clickEscapeToClose: true,
                    propagateContainerEvents: false,
                    hasBackdrop: false,
                    locals: {
                        entityUri: params.target.mark.uri
                    }
                };

                $mdPanel.open(config).then(function(p) {
                    panel = p;
                });
            });

            $(document).on('hidePanel', function(e, params) {
                if(panel && !panel.pinned) {
                    //panel.close();
                }
            });

            $(document).on('pinPanel', function(e, params) {
                if(panel) {
                    panel.pinned = true;
                }
            });
        }
    }
})();