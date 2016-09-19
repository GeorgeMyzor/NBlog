
function processData(data) {
    var target = $("#createCommentForm");

    if (data.toString() === "ModelError") {
        $('#comment_error').text("Comment should be from 1 to 200 length.");
        $('#comment_error').show();
    }
    else {
        $(target).each(function () {
            this.reset();
        });
        $('#comment_error').hide();
    }
}