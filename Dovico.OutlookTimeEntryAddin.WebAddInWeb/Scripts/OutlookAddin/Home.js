//---- Variables ----//

// outlookItem variable to hold Office.context.mailbox.item
var outlookItem;

(function () {
    "use strict";

    // The Office initialize function must be run each time a new page is loaded
    Office.initialize = function (reason) {
        $(document).ready(function () {
            try
            {
                // Initialize the outlook app and fetch Office.context.mailbox.item to know if Addin is opened from Appointment or not
                app.initialize();
                outlookItem = Office.context.mailbox.item;
                if (outlookItem.itemType == "appointment") {
                    $("#isCalenderEntry").val(true);
                }
                else {
                    $("#isCalenderEntry").val(false);
                }

                // Set application wide error handling for forbidden
                $(document).ajaxError(function (event, xhr, options, exc) {
                    if (xhr.status == 403) {
                        window.location.reload();
                    }
                });

                // Show the loading image for ajax call
                $('#homePageContainer').hide();
                $('#loadingImageHome').show();
                // Calls the IsAuthenticated() method to know if the user is already authenticated
                // If user is already authenticated then show TimeEntryHome page else show Login page for authentication
                $.ajax({
                    type: "POST",
                    url: gl_ServicePath + "/IsAuthenticated",
                    data: {},
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var responseData = jQuery.parseJSON(response.d);

                        // If Status is 500 then Error else Success
                        if (responseData.hasOwnProperty("StatusCode") && responseData.StatusCode == 500) {
                            // If login failed/not-valid user - then show login screen.
                            LoadLoginView();
                            ShowLoginError(responseData.ErrorMessage);
                            $('#homePageContainer').show();
                            $('#loadingImageHome').hide();
                        }
                        else {
                            // If login successful then show TimeEntryHome else show Login oage
                            if (responseData) {
                                LoadTimeEntryHomeView();
                            }
                            else {
                                LoadLoginView();
                            }
                            $('#homePageContainer').show();
                            $('#loadingImageHome').hide();
                        }
                    },
                    error: function (error) {
                        // If login failed/not-valid user - then show login screen
                        LoadLoginView();
                        ShowLoginError("Error during authentication.");
                        $('#homePageContainer').show();
                        $('#loadingImageHome').hide();
                    }
                });
            }
            catch (e) {
                // Show error if error comes
                ShowLoginError("Error during authentication.");
            }
        });
    };

    // Load TimeEntryHome page.
    function LoadTimeEntryHomeView() {
        try {
            $.get("TimeEntry/TimeEntryHome.html", function (data) {
                var itemId = outlookItem.itemId;
                // If from appointment then get date from Calendar else use current
                if (outlookItem != undefined && outlookItem.itemType == "appointment") {

                    if (itemId === null || itemId == undefined) {
                        //Is Compose
                        outlookItem.start.getAsync(
                            function (asyncResult) {
                                if (asyncResult.status != Office.AsyncResultStatus.Failed) {
                                    $("#isCalenderEntry").attr("data-date", moment(asyncResult.value).format("MM/DD/YYYY"));
                                }

                                // Load TimeEntryHome page data in Home page.
                                $("#homePageContainer").html(data);
                                // Load hours of current week on top.
                                LoadWeekHours(true, "");
                                // Load Time Entry page.
                                LoadTimeEntryViews();
                            });
                    }
                    else {

                        //Is Read mode
                        $("#isCalenderEntry").attr("data-date", moment(outlookItem.start).format("MM/DD/YYYY"));
                        // Load TimeEntryHome page data in Home page.
                        $("#homePageContainer").html(data);
                        // Load hours of current week on top.
                        LoadWeekHours(true, "");
                        // Load Time Entry page.
                        LoadTimeEntryViews();

                    }
                    
                }
                else
                {
                    // Load TimeEntryHome page data in Home page.
                    $("#homePageContainer").html(data);
                    // Load hours of current week on top.
                    LoadWeekHours(true, "");
                    // Load Time Entry page.
                    LoadTimeEntryViews();
                }
            });
        }
        catch (e) {
            // Show error if error comes
            ShowError("Error loading data.");
        }
    }

    // Load Login page.
    function LoadLoginView() {
        try {
            $.get("Login/Login.html", function (data) {

                // Load Login page data in Home page.
                $("#homePageContainer").html(data);
            });
        }
        catch (e) {
            // Show error if error comes
            ShowLoginError("Error loading data.");
        }
    }

})();

//---- Public Methods ----//

// Display Home Page error on top of page
function ShowError(errorMessage) {
    var errorTag = "<p>" + errorMessage + "</p>";
    $("#errorList").html("");
    $("#errorList").append(errorTag);
    $("#errorContainer").css("display", "inline-block")
    $(window).scrollTop($("#errorContainer").position().top);
}

// Display Time Entry details page errors
function ShowTimeEntryDetailsError(errorMessage) {
    var errorTag = "<p>" + errorMessage + "</p>";
    $("#timeEntryDetailsErrorContainer").css("display", "inline-block")
    $("#timeEntryDetailsErrorContainer").find("#errorList").html("");
    $("#timeEntryDetailsErrorContainer").find("#errorList").append(errorTag);
    $(window).scrollTop($("#homePageContainer").position().top);
}

// Clear Home page error
function HideError(element) {
    $(element).parent("div.alert").find("#errorList").html("");
    $(element).parent("div.alert").css("display", "none");
}

// Clear all errors
function HideAllError()
{
    $("#timeEntryDetailsErrorContainer").css("display", "none")
    $("#errorContainer").css("display", "none")
}