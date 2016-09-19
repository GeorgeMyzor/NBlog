
function page_cleanup() {
    var target = $("#pageNum");
    var pageNum = target.attr("value");

    var currentPage = $("a:contains('" + pageNum + "')");
    var url = currentPage.attr("href");
    currentPage.addClass('selected');

    window.history.pushState("all articles", "", url);
};