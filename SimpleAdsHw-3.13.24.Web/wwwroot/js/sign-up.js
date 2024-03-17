$(() => {
    $("input").on("input", function () {
        $("button").prop('disabled', isValidForm())
    })

    $('[name="password"]').on("input", function () {
        validPassword()
        $("button").prop('disabled', isValidForm())
    })

    function isValidForm() {
        let status = false;
        $("input").each(function () {
            if (!$(this).val() || $("span").text().length != 0) {
                status = true;
            }
        })
        return status;
    };

    function validPassword() {
        if ($('[name="password"]').val() && ($('[name="password"]').val().length < 5 || !containsSymbol())) {
            $("span").text("Invalid password! Password must contain at least 1 symbol and have a length of 5.")
        } else {
            $("span").text("")
        }
    }

    function containsSymbol() {
        let symbols = ['!', '@', '#', '$', '%', '^', '&', '*', '(', ')']

        let hasSymbol = false;
        symbols.forEach(s => {
            if ($('[name="password"]').val().toLowerCase().indexOf(s) !== -1) {
                hasSymbol = true;
            }
        })
        return hasSymbol;
    }
})
