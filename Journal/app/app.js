angular.module('app', [
    'ngInputModified',
    'ngCookies',
    'ngMaterial',
    'angularMoment',
    'ui.layout',
    'ui.bootstrap',
    'ui.bootstrap.modal',
    'ui.bootstrap.progressbar',
    'ui.bootstrap.tabs',
    'ui.toggle',
    'ui.router',
    'cfp.hotkeys',
    'ngDragDrop',
    'angular-emojione',
    'luegg.directives',
    'gettext'
]).config(function ($mdThemingProvider) {
    $mdThemingProvider.theme('default')
        .primaryPalette('grey')
        .accentPalette('deep-orange');
});