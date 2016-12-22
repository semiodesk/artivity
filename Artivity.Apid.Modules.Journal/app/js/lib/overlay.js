function showOverlay(id) {
    var element = $(id);

    if (element !== undefined) {
        $('.overlay .msg').each(function(i, element) {
            $(element).addClass('invisible');
        });
        
        element.removeClass('invisible');

        var overlay = $('#overlay');
        overlay.css('opacity', 0);
        overlay.removeClass('collapse');
        overlay.animate({
            opacity: 1.0
        }, 500);
    }
}

function hideOverlays() {
    var overlay = $('#overlay');

    overlay.animate({
        opacity: 0
    }, 500, function() {
        overlay.addClass('collapse');

        $('.overlay .msg').each(function(i, element) {
            $(element).addClass('invisible');
        });
    });
}