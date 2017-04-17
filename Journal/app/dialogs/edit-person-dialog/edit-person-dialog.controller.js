(function () {
    angular.module('app').controller('EditPersonDialogController', EditPersonDialogController);

    EditPersonDialogController.$inject = ['$scope', '$filter', '$uibModalInstance', '$sce', 'agentService', 'person'];

    function EditPersonDialogController($scope, $filter, $uibModalInstance, $sce, agentService, person) {
        var t = this;

        t.persons = [];
        t.person = person;
        t.photo = null;
        t.photoChanged = false;
        t.photoUrl = agentService.getPhotoUrl(t.person.Uri);
        t.getPhotoUrl = agentService.getPhotoUrl;

        t.onPhotoChanged = function (file) {
            t.photo = file;
            t.photoChanged = true;
        };

        t.findPersons = function (query) {
            if (query.length > 0) {
                agentService.findPersons(query).then(function (result) {
                    t.persons = result;
                });
            } else {
                t.persons = [];
            }
        }

        t.selectPerson = function (person) {
            t.persons = [];
            t.person = person;
            t.photoUrl = agentService.getPhotoUrl(person.Uri);

            $scope.personForm.$dirty = false;
        }

        t.commit = function () {
            if (t.person && $scope.personForm.$valid) {
                if ($scope.personForm.$dirty) {
                    agentService.putPerson(t.person);
                }

                if (t.photoChanged) {
                    agentService.putPhoto(t.person.Uri, t.photo);
                }
            }

            $uibModalInstance.close(t.person);
        };

        t.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        t.$onInit = function() {
            if (t.person.Uri && !t.person.IsNew) {
                agentService.getPerson(t.person.Uri).then(function (data) {
                    t.person = data;
                });
            }
        }
    };
})();