$(document).ready(function () {
    // Carousel Sidebar
    var carouselSidebar = $("#carouselSidebar");
    var carouselWidth = carouselSidebar.find(".carousel-inner")[0].scrollWidth;
    var cardWidth = carouselSidebar.find(".carousel-item").width();

    var scrollPosition = 0;

    carouselSidebar.find(".custom-next").on("click", function () {
        if (scrollPosition < (carouselWidth - cardWidth * 4)) {
            scrollPosition += cardWidth;
            carouselSidebar.find(".carousel-inner").animate({ scrollLeft: scrollPosition }, 600);
        }
    });

    carouselSidebar.find(".custom-prev").on("click", function () {
        if (scrollPosition > 0) {
            scrollPosition -= cardWidth;
            carouselSidebar.find(".carousel-inner").animate({ scrollLeft: scrollPosition }, 600);
        }
    });
});