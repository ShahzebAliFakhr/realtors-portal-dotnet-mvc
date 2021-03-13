var tag = document.createElement('script');
tag.src = "https://www.youtube.com/player_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

(function ($) {

    $.fn.youtube_background = function () {
        var YOUTUBE = /(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([^"&?\/ ]{11})/i;

        var $this = $(this);

        function onVideoPlayerReady(event) {
            $(event.target.a).css({ 
               "position": "relative"
            });

            var $root = $(event.target.a).parent();

            function onResize() {
                var h = $root.outerHeight() + 100; // since showinfo is deprecated and ignored after September 25, 2018. we add +100
                var w = $root.outerWidth() + 100;
                var res = 1.77777778;

                if (res > w / h) {
                    $root.find('iframe').width("100%").height("450px");
                } else {
                    $root.find('iframe').width("100%").height("450px");
                }
            }
            $(window).on('resize', onResize);
            onResize();
        }

        function onVideoStateChange(event) {
            event.target.playVideo();
        }

        var ytp = null;
        var yt_event_triggered = false;

        window.onYouTubeIframeAPIReady = function () {
            yt_event_triggered = true;

            //element loop
            for (var i = 0; i < $this.length; i++) {
                var $elem = $($this[i]);

                if ($elem.parent().hasClass('youtube-background')) {
                    continue;
                }

                $elem.wrap('<div class="youtube-background" />');
                var $root = $elem.parent();

                $root.css({
                    "height": "100%",
                    "width": "100%",
                    "z-index": "0",
                    "position": "relative",
                    "overflow": "hidden",
                    "top": 0, // added by @insad
                    "left": 0,
                    "bottom": 0,
                    "right": 0
                });

                $root.parent().parent().css({
                    "position": "relative"
                });

                var ytid = $elem.data('youtube');

                var pts = ytid.match(YOUTUBE);
                if (pts && pts.length) {
                    ytid = pts[1];
                }

                ytp = new YT.Player($elem[0], {
                    height: '100%',
                    width: '450px',
                    videoId: ytid,
                    events: {
                        'onReady': onVideoPlayerReady
                    }
                });
            }
        };

        if (window.hasOwnProperty('YT') && window.YT.loaded && !yt_event_triggered) {
            window.onYouTubeIframeAPIReady();
        }

        return $this;
    };
})(jQuery);