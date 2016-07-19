
angular.module('explorerApp').config(function ($translateProvider) {
    $translateProvider.useSanitizeValueStrategy('escape');

    $translateProvider.translations('en', {
        SETTINGS_TITLE: 'Preferences',

        SETTINGS_PROFILE: 'Profile',
        SETTINGS_APPS: 'Apps',
        SETTINGS_ACCOUNTS: 'Accounts',
        SETTINGS_PROFILE_PICTURE: 'Click to select your profile picture.',
        SETTINGS_NAME: 'Name',
        SETTINGS_EMAIL: 'E-Mail Address',
        SETTINGS_ORGANIZATION: 'Organization',

        'http://w3id.org/art/terms/1.0/position': 'Moved',
        'http://w3id.org/art/terms/1.0/hadBoundaries': 'Scaled',
        'http://www.w3.org/2000/01/rdf-schema#label': 'Renamed'

    });

    $translateProvider.translations('de', {
        SETTINGS_TITLE: 'Einstellungen',

        SETTINGS_PROFILE: 'Profil',
        SETTINGS_APPS: 'Apps',
        SETTINGS_ACCOUNTS: 'Konten',
        SETTINGS_PROFILE_PICTURE: 'Klicken Sie hier um Ihr Profilbild auszuwählen.',
        SETTINGS_NAME: 'Name',
        SETTINGS_EMAIL: 'E-Mail Adresse',
        SETTINGS_ORGANIZATION: 'Organisation',
        'http://w3id.org/art/terms/1.0/position': 'Verschoben',
        'http://w3id.org/art/terms/1.0/hadBoundaries': 'Skaliert',
        'http://www.w3.org/2000/01/rdf-schema#label': 'Umbenannt'
    });

    //$translateProvider.determinePreferredLanguage();
    $translateProvider.preferredLanguage('en');
});