
// for summernote image upload
function uploadFile(url, file, editor, welEditable) {


    var data = new FormData();
    data.append("file", file);

    //var url = "/api/eLearning/File";

    console.log("try upload file - ", data, " url -", url);

    url = "/eLearning/File/UploadFile";

    $.ajax({
        url: url,
        data: data,
        cache: false,
        contentType: false,
        processData: false,

        crossDomain: true,
        type: 'POST',
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) myXhr.upload.addEventListener('progress', progressHandlingFunction, false);
            return myXhr;
        },
        success: function (result) {

            //result = result + "'";
            //editor.insertImage(welEditable, url);
                result = result.replace(/\\/g, '');
        //    result2 = result2.replace(/\"/g, '\'');

            var message = "";

            if (result.slice(0, 1) == '"') {
                message = JSON.parse(result.slice(1, -1));
            } else {
                message = JSON.parse(result);
            }

            console.log('result - ', result, ' result2 - ', message);

            //var doc = JSON.parse(message);

            console.log('doc - ', message, ' = ', message.FileNameOnStorage);

            var remoteUrl = url + "/" + message.FileNameOnStorage;

            var image = $('<img>').attr('src', remoteUrl);
            //console.log("success upload file - ", result, " - ", remoteUrl);


            $('#summernote').summernote('insertImage', image[0]);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('error - ' + textStatus + " " + errorThrown);
        }
    });
}


// update progress bar

function progressHandlingFunction(e) {
    if (e.lengthComputable) {
        $('progress').attr({ value: e.loaded, max: e.total });
        // reset progress on complete
        if (e.loaded == e.total) {
            $('progress').attr('value', '0.0');
        }
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}





function truncate(elemClass, elemIdAfter, reqLen) {

    var target = $("." + elemClass)[0];

    var newTxt = jQuery.truncate(target, {
        length: reqLen
    });

    $("#" + elemIdAfter).empty();

    $("#" + elemIdAfter).append(newTxt);

}

// use to show validation message
function setError(name, message) {
    const span = $(`span[data-valmsg-for="${name}"]`);
    if (span && span.length > 0) {
        $(span).html(message);
        if (message) {
            $(`input[name="${name}"]`).addClass("input-validation-error");
            $(span).removeClass("field-validation-valid");
            $(span).addClass("field-validation-error");
        } else {
            $(`input[name="${name}"]`).removeClass("input-validation-error");
            $(span).removeClass("field-validation-error");
            $(span).addClass("field-validation-valid");
        }
    }
}