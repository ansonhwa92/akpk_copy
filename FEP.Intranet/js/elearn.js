

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