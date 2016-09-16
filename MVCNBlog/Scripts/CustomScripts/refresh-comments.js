
function processData(data) {
    var target = $("#createCommentForm");

    if (data.toString() === "ModelError") {
        $('#comment_error').text("Comment should be from 1 to 200 length.");
    }
    else {
        $(target).each(function () {
            this.reset();
        });
        $('#comments_output').html(data);
        $('#comment_error').hide();
    }
}