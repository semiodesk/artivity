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
			'EDITING_SESSION': 'Edited File',
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
			'WRITE_MSG': 'Write a message',
			'http://www.w3.org/2000/01/rdf-schema#label': 'Renamed',
			'http://www.w3.org/ns/prov#Generation': 'Created Element',
			'http://www.w3.org/ns/prov#Invalidation': 'Removed Element',
			'http://www.w3.org/ns/prov#Revision': 'Changed Element',
			'http://www.w3.org/ns/prov#Derivation': 'Saved Copy',
			'http://w3id.org/art/terms/1.0/SaveAs': 'Saved Copy',
			'http://w3id.org/art/terms/1.0/Save': 'Saved',
			'http://w3id.org/art/terms/1.0/Undo': 'Undo',
			'http://w3id.org/art/terms/1.0/Redo': 'Redo',
			'http://w3id.org/art/terms/1.0/position': 'Moved',
			'http://w3id.org/art/terms/1.0/hadBoundaries': 'Scaled',
			'http://w3id.org/art/terms/1.0/textValue': 'Changed Text',
			'http://w3id.org/art/terms/1.0/aboveLayer': 'Moved Layer',
			'http://w3id.org/art/terms/1.0/strokeWidth': 'Chagned Stroke',
			'http://w3id.org/art/terms/1.0/lengthUnit': 'Changed Length Unit',
			'http://w3id.org/art/terms/1.0/Comment': 'Comment'
		},
		FILEPUBLISH: {
			'NOACCOUNTS_TITLE': 'No Accounts',
			'NOACCOUNTS_SUBTITLE': 'You have not yet added any accounts which can be used for publication.',
			'NOACCOUNTS_MSG': 'Go to <a href="#/settings">Preferences</a> <i class="zmdi zmdi-arrow-right"></i> Accounts to add one.',
			'PUBLISH_FILE_TITLE': 'Publish Editing History',
			'PUBLISH_FILE_SUBTITLE': 'Create a dataset for your file and upload it into a digital repository.',
			'SELECT_ACCOUNT_TITLE': 'Select Account',
			'SELECT_ACCOUNT_SUBTITLE': 'Choose the account used for publication and authorize the upload by logging in.',
			'UPLOAD_FILE_TITLE': 'Publishing Dataset',
			'UPLOAD_FILE_SUBTITLE': '',
			'TITLE': 'Title',
			'TITLE_SUFFIX': ' Artivity Dataset',
			'DESCRIPTION': 'Description',
			'LICENSE': 'License',
			'FILE': 'File',
			'EDITING_HISTORY': 'Editing History',
			'BROWSING_HISTORY': 'Browsing History',
			'COMMENTS': 'Comments',
			'http://w3id.org/art/terms/1.0/features/ExportArchive#description': 'Exporting..',
			'http://w3id.org/art/terms/1.0/features/UploadArchive#description': 'Uploading..'
		},
		UPDATE: {
			'TITLE': 'New Release',
			'DESCRIPTION': 'An update for Artivity is available for download:'
		},
		SETTINGS: {
			TITLE: 'Preferences',
			PROFILE: {
				LABEL: 'Profile',
				TITLE: 'User Profile',
				DESCRIPTION: 'Edit the profile information that is published when you share files with others:',
				LABEL_PICTURE: 'Click to select your profile picture.',
				LABEL_NAME: 'Name',
				LABEL_EMAIL: 'E-Mail',
				LABEL_ORGANIZATION: 'Organisation',
				LABEL_BACKUP_CREATE: 'Backup profile',
				LABEL_BACKUP_RESTORE: 'Restore profile',
				LABEL_BACKUP_CREATING: 'Creating backup',
				LABEL_BACKUP_SUCCESS: 'Successfully created backup.',
				LABEL_BACKUP_ERROR: 'Error when creating backup',
			},
			ACCOUNTS: {
				LABEL: 'Accounts',
				TITLE: 'Accounts',
				DESCRIPTION: 'Connect your online accounts to benefit from additional features:',
				CONNECT_DIALOG: {
					TITLE: 'Connect Account',
					TITLE_X: 'Connect {0}-Account'
				}
			},
			APPS: {
				LABEL: 'Apps',
				TITLE: 'Applications',
				DESCRIPTION: 'Enable or disable the applications which will log data into Artivity:'
			}
		},
		'http://eprints.org': {
			'title': 'EPrints',
			'description': 'Publish your files to online digital archives.'
		},
		'http://orcid.org': {
			'title': 'ORCiD',
			'description': 'Assign a persistent digital identifier to your published files.'
		}
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
			'EDITING_SESSION': 'Datei bearbeitet',
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
			'WRITE_MSG': 'Schreibe eine Nachricht',
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
			'http://w3id.org/art/terms/1.0/SaveAs': 'Kopie gespeichert',
			'http://www.w3.org/ns/prov#Derivation': 'Kopie gespeichert',
			'http://w3id.org/art/terms/1.0/Undo': 'Rückgängig',
			'http://w3id.org/art/terms/1.0/Redo': 'Wiederhergestellt',
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