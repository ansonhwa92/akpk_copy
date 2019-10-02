
function UploadFile(filename) {
    
    var files = document.getElementsByName(filename)[0].files;

    console.log(" file: ", files);

    // Create FormData object
    var formData = new FormData();

    var len = files.length;
    if (len <= 0) {

        console.log("No file selected to upload.");

        return false;
    }

    for (var i = 0; i < len; i++) {

        var curFile = files[i];
        console.log(curFile.type);

        // check file type
        //if (!(
        //    curFile.type === "application/pdf" ||
        //    curFile.type === "image/png" ||
        //    curFile.type === "image/jpg" ||
        //    curFile.type === "image/jpeg"
        //)
        //) {

        //    console.log(
        //        "Only file with format PDF, PNG and JPG can be uploaded.",              
        //    );
        //    return false;
        //}

        // check file size
        var filesize = (curFile.size / 1024 / 1024).toFixed(4); // MB

        if (filesize >= 50) { // 50 MB
            
            console.log("File is too big");
            return false;
        }

        formData.append("files", curFile);

        // You can abort the upload by calling jqxhr.abort();

        var url = '@(WebApiURL)eLearning/Courses/Upload';

        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            processData: false, // tell jQuery not to process the data
            contentType: false, // tell jQuery not to set contentType
            success: function (response) {
                toastr.success("Succsfully uploaded", "Success", { timeOut: 2000 });

                console.log("fileid = ", response.result.fileId);

                $("#PaymentFileId").val(response.result.fileId);

                console.log("payment file name = ", $("#PaymentFileId").val());

                $("#uploadPaymentForm").trigger("submit");
            },
            error: function (jqXHR, status, errorThrown) {
                if (errorThrown === "abort") {
                    toastr.error("Uploading aborted", "Error", { timeOut: 2000 });
                } else {
                    toastr.error("Uploading failed", "Error", { timeOut: 2000 });
                }
            }
        });
    }
}


function truncate(elemClass, elemIdAfter, reqLen) {

    var target = $("." + elemClass)[0];

    var newTxt = jQuery.truncate(target, {
        length: reqLen
    });

    $("#" + elemIdAfter).empty();

    $("#" + elemIdAfter).append(newTxt);

}