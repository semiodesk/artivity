(function () {
    angular.module('app').controller('EditPersonDialogController', EditPersonDialogController);

    EditPersonDialogController.$inject = ['$scope', '$filter', '$uibModalInstance', '$sce', 'api', 'agentService', 'person'];

    function EditPersonDialogController($scope, $filter, $uibModalInstance, $sce, api, agentService, person) {
        var t = this;

        t.person = person;
        t.photo = null;
        t.photoUrl = api.getUserPhotoUrl(t.person.Uri);

        t.onPhotoChanged = function (file) {
            t.photo = file;
        };

        t.commit = function () {
            console.log(t.person);
            
            agentService.putPerson(t.person).then(function () {
                if (t.photo) {
                    agentService.putPhoto(t.person.Uri, t.photo).then(function(response) {
                        console.log(response);
                    });
                }

                $uibModalInstance.close();
            });
        };

        t.cancel = function () {
            $uibModalInstance.dismiss('cancel');

            if (interval) {
                window.clearInterval(interval);
            }
        };
    };
})();