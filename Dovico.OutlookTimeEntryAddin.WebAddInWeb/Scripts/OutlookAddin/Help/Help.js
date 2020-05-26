(function () {
    "use strict";

    // The Office initialize function must be run each time a new page is loaded
    Office.initialize = function (reason) {
        $(document).ready(function () {
            try {
                // Initialize the outlook app
                app.initialize();

                // Hide div used for displaying name
                $('#nameBox').hide();

                // Calls the GetUserDetail() method to get user details like FirstName and LastName
                $.ajax({
                    type: "POST",
                    url: gl_ServicePath + "/GetUserDetails",
                    data: {},
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        // If data is null he's not authorized user, hide div
                        if (response.d)
                        {
                            var responseData = jQuery.parseJSON(response.d);

                            // If data is null he's not authorized user, hide div
                            if (responseData != null) {
                                // If Status is 500 then Error else Success
                                if (responseData.hasOwnProperty("StatusCode") && responseData.StatusCode == 500) {
                                    // If user info retrieval failed/not-valid user - then show error.
                                    ShowHelpError(responseData.ErrorMessage);
                                    $('#nameBox').hide();
                                }
                                else {
                                    // If successful then show user details
                                    if (responseData) {
                                        $('#nameBox').show();

                                        $('#userName').text(responseData.FirstName + " " + responseData.LastName);
                                    }
                                }
                            }
                            else {
                                // If reponse is null, then hide user details section
                                $('#nameBox').hide();
                            }
                        }
                        else {
                            // If reponse is null, then hide user details section
                            $('#nameBox').hide();
                        }
                    },
                    error: function (error) {
                        // If login failed/not-valid user - then hide user details section
                        $('#nameBox').hide();
                    }
                });
            }
            catch (e) {
                // Show error if error comes
                ShowHelpError("Error fetching user details.");
                $('#nameBox').hide();
            }
        });
    };
})();

//---- Public Methods ----//

// Display Home Page error on top of page
function ShowHelpError(errorMessage) {
    var errorTag = "<p>" + errorMessage + "</p>";
    $("#helpErrorList").html("");
    $("#helpErrorList").append(errorTag);
    $("#helpErrorContainer").css("display", "inline-block")
    $(window).scrollTop($("#helpErrorContainer").position().top);
}

// Clear Home page error
function HideHelpError(element) {
    $(element).parent("div.alert").find("#helpErrorList").html("");
    $(element).parent("div.alert").css("display", "none");
}