$(document).ready(function () {

    $("#ShowRegistrationButton").click(function () {
        $('legend').text("Registration");
        $("#LoginForm").slideUp();
        $('#RegisterForm').slideDown();
    });

    $("#ShowLoginButton").click(function () {
        $('legend').text("Authorization");
        $("#RegisterForm").slideUp();
        $('#LoginForm').slideDown();
    });

    $("#LoginButton").click(function () {
        $.post("/Account/Login", { Login: $("#Login").val(), LoginPass: $("#LoginPass").val() }, function (data) {

            if (data == "" || data === undefined) {
                location.href = "/";
            }
            else {
                $("#LoginError").text(data);
            }
        })
    });

    $("#ShowRemindButton").click(function () {
        $('legend').text("Remind password");
        $("#LoginForm").slideUp();
        $('#ForgotPassForm').slideDown();
    });

    $("#CancleRemind").click(function() {
        $('#ForgotPassForm').slideUp();
        $("#LoginForm").slideDown();
    });

    $("#SendForgottenEmail").click(function () {
        var email_ = $("#ForgotEmail").val();
        $.post("/Account/ForgotPassword", { email: email_ })
            .success(function (result) {
                result = parseInt(result);
                switch (result) {
                    case 1:
                        $("#ForgotPassForm").html("<p class='content'>Link sent to your email</p>");
                    case 0:
                        $("#RemindError").text("Email not confirmed or doesn't exist");
                }
            })
            .fail(function (data) { console.log(data); });
    });
    //$("#RegistrationForm").validate({
    //    rules: {
    //        FirstName: { required: true },
    //        LastName: { required: true },
    //        Email: { required: true, email: true },
    //        Password: { required: true },
    //        ConfirmPassword: { equalTo: "#Password" }
    //    },
    //    tooltip_options: {
    //        placement: 'top'
    //    },

    //    showErrors: function (errorMap, errorList) {
    //        $("#RegistrationForm").find("input").each(function () {
    //            $(this).removeClass("input-validation-error");
    //        });
    //        $("#myErrorContainer").html("");
    //        if (errorList.length) {
    //            $("#myErrorContainer").html(errorList[0]['message']);
    //            $(errorList[0]['element']).addClass("input-validation-error");
    //        }
    //    }
    //});

});