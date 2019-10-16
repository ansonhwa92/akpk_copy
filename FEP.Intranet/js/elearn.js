
// for summernote image upload
//function uploadFile(url, file, editor, welEditable) {
function uploadFile(url, file, callback) {

    var data = new FormData();
    data.append("file", file);

    //url = url + "/UploadFile";
    //console.log("try upload file - ", data, " url -", url);

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

            //console.log("success upload file - ", result);
            result = result.replace(/\\/g, '');

            var message = "";

            if (result.slice(0, 1) == '"') {
                message = JSON.parse(result.slice(1, -1));
            } else {
                message = JSON.parse(result);
            }

            //console.log('result - ', result, ' result2 - ', message);
            //console.log('message - ', message, ' = ', message.FileNameOnStorage);

            callback(message);

           // return message;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('error - ' + textStatus + " " + errorThrown);
        }
    });
}

// for document
function convertDocument(url, fileName, fileType, courseId, callback) {

    url = "/eLearning/File/DocToHTML";

    console.log(fileType, courseId, fileName);

    $.ajax({
        url: url,
        data: {
            fileType: fileType,
            courseId: courseId,
            fileName: fileName
        },

        type: 'POST',
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) myXhr.upload.addEventListener('progress', progressHandlingFunction, false);
            return myXhr;
        },
        success: function (result) {
            callback(result);

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('error - ' + textStatus + " " + errorThrown);
        }
    });
}


// for document type - select from existing doc in courses
function getDoc(contentId, callback) {

    url = "/eLearning/File/GetDoc";

    //console.log(fileType, courseId, fileName);

    $.ajax({
        url: url,
        data: {
            contentId: contentId,
        },
        type: 'GET',
        xhr: function () {
            var myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) myXhr.upload.addEventListener('progress', progressHandlingFunction, false);
            return myXhr;
        },
        success: function (result) {
            callback(result);

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('error - ' + textStatus + " " + errorThrown);
        }
    });
}

// for document type - select from existing doc in courses
function getSlideshare(newUrl, callback) {

    url = "/eLearning/CourseContents/GetSlideshare";


    $.ajax({
        url: url,
        data: {
            newUrl: newUrl,
        },
        type: 'GET',
        contentType: "text/plain",
        success: function (result) {

            callback(result);

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