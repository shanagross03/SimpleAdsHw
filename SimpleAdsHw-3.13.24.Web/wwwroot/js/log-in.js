$("input").on("input", function () {
    $("button").prop('disabled', isValidForm())
})

function isValidForm() {
    let status = false;
    $("input").each(function () {
        if (!$(this).val() || $("h7").text().length != 0) {
            status = true;
        }
    })
    return status;
};