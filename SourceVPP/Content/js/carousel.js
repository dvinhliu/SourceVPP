$(document).ready(function () {
    var carousel = $("#carouselSidebar");
    var carouselWidth = carousel.find(".carousel-inner")[0].scrollWidth;
    var cardWidth = carousel.find(".carousel-item").width();

    var scrollPosition = 0;

    carousel.find(".carousel-control-next").on("click", function () {
        if (scrollPosition < (carouselWidth - cardWidth * 4)) {
            scrollPosition += cardWidth;
            carousel.find(".carousel-inner").animate({ scrollLeft: scrollPosition }, 600);
        }
    });

    carousel.find(".carousel-control-prev").on("click", function () {
        if (scrollPosition > 0) {
            scrollPosition -= cardWidth;
            carousel.find(".carousel-inner").animate({ scrollLeft: scrollPosition }, 600);
        }
    });
});