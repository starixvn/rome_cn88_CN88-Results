$(function () {
    $pokerGamesIframe = $(".poker-games-iframe");

    if ($pokerGamesIframe.length) {
        $(window).resize(function () {
            var height = $(window).height() - 10;
            $pokerGamesIframe.height(height < 0 ? 0 : height);
        });
    }
});