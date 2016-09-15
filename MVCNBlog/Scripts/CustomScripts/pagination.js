
$(document).ready(function () {
    $(document).on('click', '.page', (function (e) {
        var newVal = $(e.target).attr("value");
        var url = '/articles/page' + newVal;

        $.ajax({
            url: url,
            cache: false,
            type: "GET",
            success: function (data) {
                $('#artcilesOutput').replaceWith(data);
                window.history.pushState("all articles", "", "/articles/page" + newVal);
            },
            error: function (reponse) {
            }
        });
        e.preventDefault();
    }));
});