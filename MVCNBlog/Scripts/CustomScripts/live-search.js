
$(document).ready(function () {
    $(document).on('keyup', '#findArticle', (function (e) {
        var findInputVal = $("#findArticle").val();
        var urlVal;
        if (findInputVal.startsWith("#")) {
            findInputVal = findInputVal.replace('#', '-hashtag-');
        }

        var url = $('#FindUrl').val() + '/' + findInputVal;

        $.ajax({
            url: url,
            cache: false,
            type: "GET",
            success: function (data) {

                $('#allArticles').html(data);

                if (findInputVal === '') {
                    $('.content_title h2').text('Latest Blog Post');
                    $('.pagination').show();
                    window.history.pushState("finded articles", "", "/articles");
                } else {
                    $('.content_title h2').text('Finded Posts');
                    $('.pagination').hide();
                    window.history.pushState("finded articles", "", "/articles/find/" + findInputVal);
                }


            },
            error: function (reponse) {
            }
        });
    }));
});