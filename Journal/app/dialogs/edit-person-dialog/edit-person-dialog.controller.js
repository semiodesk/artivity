(function () {
    angular.module('app').controller('EditPersonDialogController', EditPersonDialogController);

    EditPersonDialogController.$inject = ['$scope', '$filter', '$uibModalInstance', '$sce', 'agentService', 'person'];

    function EditPersonDialogController($scope, $filter, $uibModalInstance, $sce, agentService, person) {
        var t = this;

        t.persons = [];
        t.person = person;
        t.photo = null;
        t.photoUrl = agentService.getPhotoUrl(t.person.Uri);
        t.getPhotoUrl = agentService.getPhotoUrl;

        t.onPhotoChanged = function (file) {
            t.photo = file;
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
            if (t.person && (t.person.IsNew || $scope.personForm.$dirty)) {
                agentService.putPerson(t.person).then(function () {
                    if (t.photo) {
                        agentService.putPhoto(t.person.Uri, t.photo).then(function () {
                            $uibModalInstance.close(t.person);
                        });
                    } else {
                        $uibModalInstance.close(t.person);
                    }
                });
            } else {
                $uibModalInstance.close(t.person);
            }
        };

        t.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        function init() {
            if (t.person && t.person.Name === '') {
                t.person.IsNew = true;
            } else if(t.person.Uri) {
                agentService.getPerson(t.person.Uri).then(function(data) {
                    t.person = data;
                    t.person.IsNew = false;
                });
            }
        }

        init();
    };
})();