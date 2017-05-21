
$(document).ready(function () {
    $('#uploadForm').submit(function (e) {
        if (window.FormData !== undefined) {
            var id = $("#userId").val();
            var fileUpload = $("#uploadImage").get(0);
            var files = fileUpload.files;

            var fileData = new FormData();

            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            $("#loading").show('slow');
            var url = $('#updatePictureUrl').val();

            e.preventDefault();
            $.ajax({
                url: url,
                type: "POST",
                contentType: false,
                processData: false,
                data: fileData,
                success: function (result) {
                    if ($.type(result.ProfilePicture) !== "undefined") {
                        var results = $('#userPic');
                        results.empty();

                        results
                            .append("<img style='width: 153px;height: 152px;margin-left: 57px' src=\"data:image/jpeg;base64," + result.ProfilePicture + "\" />");

                        $('#picError').text('');
                    } else {
                        $('#picError').text(result.ErrorMessage);
                    }
                },
                error: function (err) {
                    alert("Cannot upload image.");
                }
            });

        } else {
            alert("FormData is not supported.");
        }

        $("#loading").hide('slow');
    });
});