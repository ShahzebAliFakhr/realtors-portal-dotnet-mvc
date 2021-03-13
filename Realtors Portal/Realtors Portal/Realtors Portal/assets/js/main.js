$(document).ready(function () {
    
    // Product Carousel
    $('.owl-carousel').owlCarousel({
        loop: false,
        autoplay: true,
        autoplayTimeout: 5000,
        autoplayHoverPause: true,
        margin: 30,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3
            }
        }
    })

    $(document).ready(function () {
        $('[data-youtube]').youtube_background();
    });
	
});