//---- Public Methods ----//

// LoginRequest() is called from Login page on Login button
function LoginRequest() {
    try {
        // Hide all errors
        HideLoginError();

        $('#loadingImageLogin').show();

        // Fetch Company, Username, Password
        var company = $("#inputCompany").val().trim();
        var username = $("#inputUserName").val().trim();
        var password = $("#inputPassword").val().trim();

        // Calls AuthenticateUser() to authenticate user using Company, Username, Password
        if (company != "" && username != "" && password != "") {
            $.ajax({
                type: "POST",
                url: gl_ServicePath + "/AuthenticateUser",
                data: '{"company": "' + company + '", "username": "' + username + '", "password": "' + password + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    // If some data returned then Error
                    if (response.d)
                    {
                        var responseData = jQuery.parseJSON(response.d);

                        // If Status is 500 then Error
                        if (responseData.hasOwnProperty("StatusCode") && responseData.StatusCode == 500) {
                            ShowLoginError(responseData.ErrorMessage);
                            $('#loadingImageLogin').hide();
                        }
                    }
                    else
                    {
                        // After successful Login reload Home page
                        window.location.reload();
                    }
                },
                error: function (error) {
                    // Show error if error comes                   
                    ShowLoginError("Error on login.");
                    $('#loadingImageLogin').hide();
                }
            });
        }
        else
        {
            // Show error if error comes
            ShowLoginError("Company Name/Email/Password cannot be empty.");
            $('#loadingImageLogin').hide();
        }
    }
    catch (e) {
        // Show error if error comes
        ShowLoginError("Error on login.");

        $('#loadingImageLogin').hide();
    }
}

//---- Private Methods ----//

// Display login error
function ShowLoginError(errorMessage) {
    var errorTag = "<p>" + errorMessage + "</p>";
    $("#loginErrorList").html("");
    $("#loginErrorList").append(errorTag);
    $("#loginErrorContainer").show();
    $("#inputPassword").val("");
}

// Hide login error
function HideLoginError() {
    $("#loginErrorList").html("");
    $("#loginErrorContainer").hide();
}