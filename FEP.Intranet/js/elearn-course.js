
// hide/show text for video
$('.video-radio input[type="radio"]').change(function () {
    if ($(this).hasClass("exVideo")) {

        $('#Url').prop("disabled", false);
        $('#File').prop("disabled", true);
        setError("File", "");

        ($('input[name=VideoType]').val('ExternalVideo'));

    } else {

        $('#File').prop("disabled", false);
        setError("Url", "");

        $('#Url').prop("disabled", true);
        ($('input[name=VideoType]').val('UploadVideo'));
    }
});

$('.audio-radio input[type="radio"]').change(function () {
    if ($(this).hasClass("savedAudio")) {
        setError("File", "");
        $('#File').prop("disabled", true);
        $("#ContentFileId").prop("disabled", false);

        ($('input[name=AudioType]').val('SavedAudio'));

    } else {

        $('#File').prop("disabled", false);
        $("#ContentFileId").prop("disabled", true);
        setError("ContentFileId", "");

        ($('input[name=AudioType]').val('UploadAudio'));
    }
});

$('.document-radio input[type="radio"]').change(function () {

    console.log($(this));


    if ($(this).hasClass("savedDocument")) {
        setError("File", "");
        setError("Url", "");
        $('#File').prop("disabled", true);
        $("#ContentFileId").prop("disabled", false);
        $('#Url').prop("disabled", true);

        ($('input[name=DocumentType]').val('SavedDocument'));

    }

    if ($(this).hasClass("uploadDocument")) {

        $('#File').prop("disabled", false);
        $("#ContentFileId").prop("disabled", true);
        $('#Url').prop("disabled", true);


        setError("ContentFileId", "");
        setError("Url", "");

        ($('input[name=DocumentType]').val('UploadDocument'));
    }

    if ($(this).hasClass("useSlideshare")) {

        $('#File').prop("disabled", true);
        $("#ContentFileId").prop("disabled", true);
        $('#Url').prop("disabled", false);


        setError("ContentFileId", "");
        setError("File", "");

        ($('input[name=DocumentType]').val('UseSlideshare'));
    }
});

$('.completion-radio input[type="radio"]').change(function () {
    var completionType = $(this).val();
    console.log(completionType);

    if ($(this).hasClass("answerQuestion")) {
        $("#answerQuestion").prop("checked", true);
        $('#Timer').prop("disabled", true);
        $('#ContentQuestionId').prop("disabled", false);

        $('input[name=CompletionType]').val("AnswerQuestion");
    }

    if ($(this).hasClass("clickButton")) {
        $("#clickButton").prop("checked", true);
        $('#Timer').prop("disabled", true);
        $('#ContentQuestionId').prop("disabled", true);

        $('input[name=CompletionType]').val("ClickButton");
    }

    if ($(this).hasClass("timer1")) {
        $("#timer1").prop("checked", true);
        $('#Timer').prop("disabled", false);
        $('#ContentQuestionId').prop("disabled", true);

        $('input[name=CompletionType]').val("Timer");
    }

});


$('.custom-file-input').on('change', function () {
    let fileName = $(this).val().split('\\').pop();
    $(this).next('.custom-file-label').addClass("selected").html(fileName);


    var contentType = $('#ContentType').val();

    if (contentType == "Document") {

        var file = document.getElementsByName('File')[0].files[0];

        //console.log('File - ', file);
        //$('.loading').show();
        $("#Text").summernote('code', "Loading..");

        uploadFile(uploadDocUrl, file, function (value) {
            convertDocument(convertFileUrl, value.FileNameOnStorage, 1, 1, function (value) {

                //console.log('value ==- ', value);
                $("#Text").summernote('code', value);

                document.getElementById("otherDoc").style.display = "block";
                document.getElementById("slideShareDoc").style.display = "none";
            });
        });
        $('.loading').hide();
    }
});


$('#ContentFileId').on('change', function () {

    var contentId = $(this).val();

    console.log('contentid - ', contentId);

    var contentType = $('#ContentType').val();

    if (contentType == "Document") {

        //console.log('File - ', file);
        $('.loading').show();

        getDoc(contentId, function (value) {

            $("#Text").summernote('code', value);

            document.getElementById("otherDoc").style.display = "block";
            document.getElementById("slideShareDoc").style.display = "none";
        });


        $('.loading').hide();
    }
});


function isUrlValid(url) {
    return /^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(url);
}


$('#Url').on('change', function () {

    var newUrl = $(this).val();

    if (!isUrlValid(newUrl)) {
        setError("Url", "Please enter a valid url starting with http:// or https://");

        return;
    }

    var contentType = $('#ContentType').val();

    if (contentType == "Document") {

        //console.log('File - ', file);
        $('.loading').show();

        getSlideshare(newUrl, function (value) {

            console.log('slide - ', value);

            setError("Url", "");

            if (value.indexOf("Error") >= 0) {
                setError("Url", value);
            }
            else {
                $('#myslide').attr("src", value);
 
            }

            document.getElementById("otherDoc").style.display = "none";
            document.getElementById("slideShareDoc").style.display = "block";
        });


        $('.loading').hide();
    }
});

function validateForm() {


    var contentType = $('#ContentType').val();

    if (contentType == 'IFrame') {
        var newUrl = $('#Url').val();

        if (!isUrlValid(newUrl)) {
            setError("Url", "Please enter a valid url starting with http:// or https://");

            return false;
        }
    }

    if (contentType == 'Video') {

        console.log(contentType);
        if ($('#VideoType').val() == "ExternalVideo") {
            if ($.trim($('#Url').val()) == "") {
                setError("Url", "Please enter the youtube URL.");

                return false;
            }
            else {
                setError("Url", "");
            }

            var newUrl = $('#Url').val();

            if (!isUrlValid(newUrl)) {
                setError("Url", "Please enter a valid url starting with http:// or https://");

                return false;
            }
        }

        if ($('#VideoType').val() == "UploadVideo") {

            var UploadFileName = $('#UploadFileName').val();
            console.log('filename -', UploadFileName);

            if ($.trim($('#File').val()) == "" && $.trim(UploadFileName) == "" ) {
                setError("File", "Please browse for the file to upload.");

                return false;
            } else {
                setError("File", "");
            }

        }
    }

    if (contentType == 'Audio') {

        if ($('#AudioType').val() == "UploadAudio") {

            console.log('uploaded filename : ', $('#UploadFileName').val(), " filename-", $('#File').val());

            if ($.trim($('#UploadFileName').val()) == "" && $.trim($('#File').val()) == "") {
                setError("File", "Please browse for the file to upload.");

                return false;
            } else {
                setError("File", "");
            }
        }

        if ($('#AudioType').val() == "SavedVideo") {
            if ($.trim($('#ContentFileId').val()) == "-1" || $.trim($('#ContentFileId').val()) == "") {
                setError("ContentFileId", "Please select an audio");

                return false;
            }
            else {
                setError("ContentFileId", "");
            }
        }

    }


    if (contentType == 'Document') {

        if ($('#DocumentType').val() == "UploadDocument") {

            console.log('uploaded filename : ', $('#UploadFileName').val(), " filename-", $('#File').val());

            if ($.trim($('#UploadFileName').val()) == "" && $.trim($('#File').val()) == "") {
                setError("File", "Please browse for the file to upload.");

                return false;
            } else {
                setError("File", "");
            }
        }

        if ($('#DocumentType').val() == "SavedDocument") {
            if ($.trim($('#ContentFileId').val()) == "-1" || $.trim($('#ContentFileId').val()) == "") {
                setError("ContentFileId", "Please select a document");
                console.log("ERROR - ", $('#ContentFileId').val());

                return false;
            }
            else {
                setError("ContentFileId", "");
            }
        }


        if ($('#DocumentType').val() == "UseSlideshare") {
            if ($.trim($('#Url').val()) == "") {
                setError("Url", "Please key in the slideshare address.");

                return false;
            }
            else {
                setError("Url", "");
            }

            var newUrl = $('#Url').val();

            if (!isUrlValid(newUrl)) {
                setError("Url", "Please enter a valid url starting with http:// or https://");

                return false;
            }
        }
    }

    return true;
}


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