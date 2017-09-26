(function () {
    angular.module('app').directive('artProjectMemberList', function () {
        return {
            restrict: 'E',
            templateUrl: 'app/directives/art-project-member-list/art-project-member-list.html',
            controller: ProjectMemberListController,
            controllerAs: 't',
            bindToController: true,
            scope: {
                project: "=",
            }
        }
    });

    ProjectMemberListController.$inject = ['$scope', '$state', '$mdDialog', 'hotkeys', 'agentService', 'cookieService', 'projectService'];

    function ProjectMemberListController($scope, $state, $mdDialog, hotkeys, agentService, cookieService, projectService) {
        var t = this;

        // MEMBERS
        t.getPhotoUrl = agentService.getPhotoUrl;

        t.addMember = function (e) {
            if (t.project) {
                agentService.newPerson().then(function (member) {
                    if (member) {
                        $mdDialog.show({
                            attachTo: angular.element(document.body),
                            templateUrl: 'app/dialogs/edit-person-dialog/edit-person-dialog.html',
                            controller: 'EditPersonDialogController',
                            controllerAs: 't',
                            bindToController: true,
                            hasBackdrop: true,
                            trapFocus: true,
                            zIndex: 150,
                            targetEvent: e,
                            disableParentScroll: true,
                            clickOutsideToClose: false,
                            escapeToClose: true,
                            preserveScope: true,
                            focusOnOpen: true,
                            locals: {
                                project: t.project,
                                person: member
                            }
                        }).then(function (member) {
                            if (member) {
                                projectService.addMember(t.project.Uri, member.Uri).then(function () {
                                    projectService.getMembers(t.project.Uri).then(function (result) {
                                        if (result.length > 0) {
                                            t.project.members = result;
                                        }
                                    });
                                });
                            }
                        });
                    }
                });
            }
        }

        t.editMember = function (e, member) {
            if (t.project) {
                $mdDialog.show({
                    attachTo: angular.element(document.body),
                    templateUrl: 'app/dialogs/edit-person-dialog/edit-person-dialog.html',
                    controller: 'EditPersonDialogController',
                    controllerAs: 't',
                    bindToController: true,
                    hasBackdrop: true,
                    trapFocus: true,
                    zIndex: 150,
                    targetEvent: e,
                    disableParentScroll: true,
                    clickOutsideToClose: false,
                    escapeToClose: true,
                    preserveScope: true,
                    focusOnOpen: true,
                    locals: {
                        project: t.project,
                        person: member.Agent
                    }
                }).then(function (member) {
                    if (member) {
                        projectService.getMembers(t.project.Uri).then(function (result) {
                            if (result.length > 0) {
                                t.project.members = result;

                                syncService.synchronize();
                            }
                        });
                    }
                });
            }
        }

        t.removeMember = function (member) {
            if (t.project) {
                projectService.removeMember(t.project.Uri, member.Agent.Uri).then(function () {
                    var i = t.project.members.indexOf(member);

                    if (i > -1) {
                        t.project.members.splice(i, 1);
                    }
                });
            }
        }

        t.$onInit = function () {
            if (t.project) {
                projectService.getMembers(t.project.Uri).then(function (result) {
                    if (result.length > 0) {
                        t.project.members = result;
                    }
                });
            }
        }
    }
})();