var interval = null;

function extendToken() {
    $.ajax({
        type: 'POST',
        url: $('#extend-token-url').val(),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (rs) { },
        error: function (e) { },
        complete: function (e) { }
    });
}

$(function () {
    $('#result-link').on('click touchend', function () {
        if (interval == null) {
            setTimeout(function () {
                extendToken();
                interval = setInterval(function () { extendToken(); }, 60 * 1000);
            }, 30 * 1000);
        }
    });
});