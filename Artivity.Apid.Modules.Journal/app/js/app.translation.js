angular.module('explorerApp').config(function ($translateProvider) {
	$translateProvider.useSanitizeValueStrategy('escape');

	$translateProvider.translations('en', {

		FILELIST: {
			NOFILES: 'Your recent files will appear here.',

		},

		FILEVIEW: {
			'HISTORY': 'History',
			'COMMENTS': 'Comments',
			'STATISTICS': 'Statistics',
			'EDITING': 'Editing',
			'CONFIDENCE': 'Confidence',
			'STEPS': 'Steps',
			'UNDOS': 'Undos',
			'REDOS': 'Redos',
			'COMPOSITION': 'Composition',
			'LAYERS': 'Layers',
			'GROUPS': 'Groups',
			'MASKED': 'Masked',
			'CLIPPED': 'Clipped',
			'COLOURS': 'Colours',
			'UNKNOWN': 'Unknown',
			'http://www.w3.org/ns/prov#Generation': 'Created Element',
			'http://www.w3.org/ns/prov#Invalidation': 'Removed Element',
			'http://w3id.org/art/terms/1.0/Save': 'Saved',
			'http://www.w3.org/ns/prov#Derivation': 'Saved Copy',
			'http://w3id.org/art/terms/1.0/position': 'Moved',
			'http://w3id.org/art/terms/1.0/hadBoundaries': 'Scaled',
			'http://www.w3.org/2000/01/rdf-schema#label': 'Renamed',
			'http://w3id.org/art/terms/1.0/textValue': 'Changed Text',
			'http://w3id.org/art/terms/1.0/aboveLayer': 'Moved Layer',
			'http://w3id.org/art/terms/1.0/strokeWidth': 'Chagned Stroke',
			'http://w3id.org/art/terms/1.0/lengthUnit': 'Chagned Length Unit',
			'http://w3id.org/art/terms/1.0/Comment': 'Comment'
		},
		
		SETTINGS: {
			TITLE: 'Preferences',

			PROFILE: 'Profile',
			APPS: 'Apps',
			ACCOUNTS: 'Accounts',
			PROFILE_PICTURE: 'Click to select your profile picture.',
			NAME: 'Name',
			EMAIL: 'E-Mail Address',
			ORGANIZATION: 'Organization',
		},
	});

	$translateProvider.translations('de', {

		FILELIST: {
			NOFILES: 'Die zuletzt verwendeten Dateien erscheinen hier.',

		},

		FILEVIEW: {
			'HISTORY': 'Verlauf',
			'COMMENTS': 'Kommentare',
			'STATISTICS': 'Statistik',
			'EDITING': 'Bearbeitung',
			'CONFIDENCE': 'Confidence',
			'STEPS': 'Schritte',
			'UNDOS': 'Undos',
			'REDOS': 'Redos',
			'COMPOSITION': 'Komposition',
			'LAYERS': 'Ebenen',
			'GROUPS': 'Gruppen',
			'MASKED': 'Maskiert',
			'CLIPPED': 'Clipped',
			'COLOURS': 'Farben',
			'UNKNOWN': 'Unbekannt',
			'http://www.w3.org/ns/prov#Generation': 'Element erzeugt',
			'http://www.w3.org/ns/prov#Invalidation': 'Element entfernt',
			'http://www.w3.org/ns/prov#Derivation': 'Kopie gespeichert',
			'http://w3id.org/art/terms/1.0/position': 'Verschoben',
			'http://w3id.org/art/terms/1.0/hadBoundaries': 'Skaliert',
			'http://www.w3.org/2000/01/rdf-schema#label': 'Umbenannt',
			'http://w3id.org/art/terms/1.0/textValue': 'Text geändert',
			'http://w3id.org/art/terms/1.0/aboveLayer': 'Ebene verschoben',
			'http://w3id.org/art/terms/1.0/strokeWidth': 'Strichsträke geändert',
			'http://w3id.org/art/terms/1.0/lengthUnit': 'Einheit geändert',
			'http://w3id.org/art/terms/1.0/Comment': 'Kommentar',
			'http://w3id.org/art/terms/1.0/Save': 'Gespeichert',
		},

		SETTINGS: {
			TITLE: 'Einstellungen',
			PROFILE: 'Profil',
			APPS: 'Apps',
			ACCOUNTS: 'Konten',
			PROFILE_PICTURE: 'Klicken Sie hier um Ihr Profilbild auszuwählen.',
			NAME: 'Name',
			EMAIL: 'E-Mail Adresse',
			ORGANIZATION: 'Organisation',
		},
	});

	//$translateProvider.determinePreferredLanguage();
	$translateProvider.preferredLanguage('en');
});