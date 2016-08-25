
$(function () {
    var autocompleteUrl = '@Url.RouteUrl("AllArticles", new {action = "Find"})';
    $("input#article").autocomplete({
        source: autocompleteUrl,
        minLength: 2,
        select: function (event, ui) {
            window.location.href = '@Url.RouteUrl("AllArticles", new {action = "All" })/' + ui.item.id;
        }
    })
    .data("autocomplete")._renderItem = function (ul, item) {
        return $("<li>")
            .data("item.autocomplete", item)
            .append("<a> Title: " + item.label + "<br> Tags: " + item.tags + "</a>")
            .appendTo(ul);
    }
});