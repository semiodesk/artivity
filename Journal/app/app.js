angular.module('app', [
    'ngInputModified',
    'ngCookies',
    'ngMaterial',
    'ngDragDrop',
    'angularMoment',
    'ui.layout',
    'ui.bootstrap',
    'ui.bootstrap.modal',
    'ui.bootstrap.progressbar',
    'ui.bootstrap.tabs',
    'ui.toggle',
    'ui.router',
    'cfp.hotkeys',
    'angular-emojione',
    'luegg.directives',
    'gettext',
    'ngSanitize',
    'btford.markdown',
    'wu.masonry'
]).config(function ($mdThemingProvider) {
    // Extend the red theme with a different color and make the contrast color black instead of white.
    // For example: raised button text will be black instead of white.
    var lightMap = $mdThemingProvider.extendPalette('orange', {
        '500': '#4f4f4f',
        '700': '#3f3f3f',
        '800': '#2f2f2f',
        'A200': '#2f2f2f',
        'A700': '#2f2f2f',
        'contrastDefaultColor': 'light'
    });

    // Register the new color palette map with the name <code>neonRed</code>
    $mdThemingProvider.definePalette('artivity-light', lightMap);

    $mdThemingProvider.theme('default')
        .primaryPalette('artivity-light', {
            'default': '800'
        })
        .accentPalette('artivity-light');
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