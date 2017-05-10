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
}).config(function config($provide) {
    // We hide the auto-complete menu for the search input because we're filterting the file list live.
    // See: https://github.com/angular/material/issues/8393
    
    // add additional function to md-autocomplete
    $provide.decorator('mdAutocompleteDirective', mdAutoCompleteDirectiveOverride);

    mdAutoCompleteDirectiveOverride.$inject = ['$delegate'];

    function mdAutoCompleteDirectiveOverride($delegate) {
        var directive = $delegate[0];
        // need to append to base compile function
        var compile = directive.compile;

        // add our custom attribute to the directive's scope
        angular.extend(directive.scope, {
            menuContainerClass: '@?mdMenuContainerClass'
        });

        // recompile directive and add our class to the virtual repeat container
        directive.compile = function (element, attr) {
            var template = compile.apply(this, arguments);
            var menuContainerClass = attr.mdMenuContainerClass ? attr.mdMenuContainerClass : '';
            var menuContainer = element.find('md-virtual-repeat-container');

            menuContainer.addClass(menuContainerClass);

            // recompile the template
            return function (e, a) {
                template.apply(this, arguments);
            };
        };

        return $delegate;
    }
});