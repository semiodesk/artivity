angular.module('explorerApp').controller('QueryController', QueryController);

function QueryController(api, $location) {
    var t = this;

    // Generate the SPARQL query endpoint with the correct port from the current URI.
	var endpoint = apid.endpointUrl + 'sparql';
	
    var editor = document.getElementById("editor");

	console.log("SPARQL Endpoint:", endpoint);

	// Initialize the SPARQL query GUI.
	var yasgui = YASGUI(editor);

	for (tab in yasgui.tabs) {
		var ts = yasgui.tabs[tab];

		ts.persistentOptions.yasqe.sparql.endpoint = endpoint;
		ts.yasqe.addPrefixes({
			art: 'http://w3id.org/art/terms/1.0/',
			dc: 'http://purl.org/dc/elements/1.1/',
			prov: 'http://www.w3.org/ns/prov#',
			nie: 'http://www.semanticdesktop.org/ontologies/2007/01/19/nie#',
			nfo: 'http://www.semanticdesktop.org/ontologies/2007/03/22/nfo#'
		});

		console.log('Tab settings:', ts);
	}

	console.log('Loaded YASGUI:', yasgui);

	// Change the position of the 'execute query' button.
	var queryButton = $('.yasqe_queryButton.query_valid');
	queryButton.remove();
	queryButton.find('svg').remove();
	queryButton.append($.parseHTML('<div class="btn"><i class="zmdi zmdi-play"></i></div>'));

	$('.yasr .yasr_header').prepend(queryButton);
};