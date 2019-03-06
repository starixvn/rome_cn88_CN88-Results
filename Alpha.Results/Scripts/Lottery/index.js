$(function () {
    var linkLotteryResult = $('#result-link');
    var url = linkLotteryResult.attr('href');

    linkLotteryResult.click(function (e) {
        e.preventDefault();
        window.open(url, 'Lottery_Result', 'width=900,height=700,scrollbars=1,directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0');
    });
})


$(function () {
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

    setInterval(extendToken, 60 * 1000);
});