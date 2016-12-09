var reloadUrl = 'http://localhost:35729/livereload.js';

$.get(reloadUrl, function() {
    var script = $('#livereload');

    if(script !== undefined) {
	    $('#livereload').attr('src', reloadUrl);

        console.log('Enabled live-reload:', reloadUrl);
    }
});