//---- Variables ----//
var timeEntriesData = "";

//---- Public Methods ----//

// Load the selected date and then call LoadTimeEntries() to load Time Sheet page
function LoadSingleDayEntry(isFromTimeSheetTab) {
    // Get date from moment-with-locales.js
    try {
        if (isFromTimeSheetTab)
        {
            var isAlreadyActive = $("#tabTimeEntries").hasClass("active");
            if(!isAlreadyActive)
            {
                // Hide all errors
                HideAllError();

                var startDate = $("#weeklyHours").find(".weekly_day.selected").attr("data-value"); // Get selected date
                var day = moment(startDate).format("dddd");

                // Set selected date in Time Sheet page
                $("#weekDayName").find(".day_weekly_block.selected").removeClass("selected"); // Unselect previous selected day
                $("#" + day).addClass("selected");

                // Remove all errors
                RemoveControlErrorStyle();

                // Call Load Week Hours Details
                LoadWeekHoursDetails();

                // Call LoadTimeEntries() to load Time Sheet page.
                LoadTimeEntries(startDate, startDate)
            }
        }
        else
        {
            // Hide all errors
            HideAllError();

            var startDate = $("#weeklyHours").find(".weekly_day.selected").attr("data-value"); // Get selected date
            var day = moment(startDate).format("dddd");

            // Set selected date in Time Sheet page
            $("#weekDayName").find(".day_weekly_block.selected").removeClass("selected"); // Unselect previous selected day
            $("#" + day).addClass("selected");

            // Remove all errors
            RemoveControlErrorStyle();

            // Call Load Week Hours Details
            LoadWeekHoursDetails();

            // Call LoadTimeEntries() to load Time Sheet page.
            LoadTimeEntries(startDate, startDate)
        }
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
    }
}

// Load Week Hours Details
function LoadWeekHoursDetails() {
    try {
        // Set Start and End dates
        var startDate = $("#startDate").val();
        var endDate = $("#endDate").val();

        $("#weekHoursDetails").hide();

        // GetTimeEntriesWeekHoursDetails() is called with Start and End dates to get the hours for one week
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/GetTimeEntriesWeekHoursDetails",
            data: '{"start": "' + startDate + '", "end": "' + endDate + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                // Get response data
                var responseData = jQuery.parseJSON(response.d);

                // If Status is 500 then Error else Success
                if (responseData.hasOwnProperty("StatusCode") && responseData.StatusCode == 500) {
                    ShowError(responseData.ErrorMessage);
                }
                else {
                    // Load the hours on top of page
                    var weekHoursDetails = responseData

                    if (weekHoursDetails != null){
                        if (weekHoursDetails.IsSubmitted)
                            $("#btnSubmitTime").prop("disabled", true);
                        else
                            $("#btnSubmitTime").removeProp("disabled");

                        // Display total hours on Time Sheet page.
                        $("#weekTotalHours").text(weekHoursDetails.WeekHoursTotal + "h");
                    }
                }

                $("#weekHoursDetails").show();
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error loading week hours data.");

                $("#weekHoursDetails").show();
            }
        });
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading week hours data.");

        $("#weekHoursDetails").show();
    }
}

// Load data for Time Sheet page
function LoadTimeEntries(startDate, endDate) {
    try {
        $('.timeEntriestab').css("display", "none");
        $('#timeEntriesLoadingImage').show();

        // GetTimeEntries() is called with current date to get Time Entries for selected date
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/GetTimeEntries",
            data: '{"start": "' + startDate + '", "end": "' + endDate + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                // If successful bind entries to UI
                var timeEntriesData = jQuery.parseJSON(response.d);

                // If Status is 500 then Error else Success
                if (timeEntriesData.hasOwnProperty("StatusCode") && timeEntriesData.StatusCode == 500) {
                    ShowError(timeEntriesData.ErrorMessage);
                    $('#timeEntriesLoadingImage').hide();
                    $('.timeEntriestab').css("display", "block");
                }
                else {
                    $("#timeEntries").html("");
                    self.$list = $("#timeEntries");

                    _.each(timeEntriesData, function (item, key, list) {
                        var template = _.template($("#timeEntriesTemplate").text())(item);
                        self.$list.append(template);
                    });

                    // Get selected date
                    var startDate = $("#weeklyHours").find(".weekly_day.selected").attr("data-value");

                    var day = moment(startDate).format("dddd");
                    var month = moment(startDate).format("MMMM");
                    var date = moment(startDate).format("DD");
                    var year = moment(startDate).format("YYYY");

                    $("#timeEntriesDate").text(day + " " + month + " " + date + ", " + year); // Monday March 8, 2016

                    $('#timeEntriesLoadingImage').hide();
                    $('.timeEntriestab').css("display", "block");
                }
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error loading data.");

                $('#timeEntriesLoadingImage').hide();
                $('.timeEntriestab').css("display", "block");
            }
        });
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
        $('#timeEntriesLoadingImage').hide();
        $('.timeEntriestab').css("display", "block");
    }
}

// Call delete TimeEntry
function DeleteEntry(deletebutton) {
    try {
        // Hide all errors
        HideAllError();

        // Get time entry ID
        var timeEntryID = $(deletebutton).attr("data-value");

        $('#tabs').hide();
        $('#content').hide();
        $('#loadingImage').show();
        $("#actionMessage").text("Deleting Time Entry...");
        // DeleteTimEntry() is called to delete the time entry
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/DeleteTimeEntry",
            data: '{"timeEntryId": "' + timeEntryID + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                // Get response data
                if (response.d)
                {
                    var responseData = jQuery.parseJSON(response.d);

                    // If Status is 500 then Error else Success
                    if (responseData.hasOwnProperty("StatusCode") && responseData.StatusCode == 500) {
                        ShowError(responseData.ErrorMessage);
                    }
                    $('#loadingImage').hide();
                    $('#tabs').show();
                    $('#content').show();
                }
                else {
                    // Get selected date
                    var currentDate = $("#weeklyHours").find(".weekly_day.selected").attr("data-value");

                    // Refresh Time Entry Tab
                    LoadTimeEntryViews(outlookItem);

                    // Load the hours of week to display on top of page
                    LoadWeekHours(false, currentDate);

                    // Load LoadSingleEntry() to load the selected date
                    LoadSingleDayEntry(false);
                }
                $("#actionMessage").text("");
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error deleting time entry.");

                $('#loadingImage').hide();
                $('#tabs').show();
                $('#content').show();
                $("#actionMessage").text("");
            }
        });
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error deleting time entry.");

        $('#loadingImage').hide();
        $('#tabs').show();
        $('#content').show();
        $("#actionMessage").text("");
    }
}

// Call to fetch Single TimeEntry
function EditEntry(editbutton) {
    try {
        $('#tabs').hide();
        $('#content').hide();
        $('#loadingImage').show();
        $("#actionMessage").text("Loading Data...");
        // Get time entry ID
        var timeEntryID = $(editbutton).attr("data-value");

        //Remove red outline from controls if css applied
        RemoveControlErrorStyle();

        // Hide all errors
        HideAllError();

        // Call GetSingleTimeEntry() to fetch time entry details for the time entry
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/GetSingleTimeEntry",
            data: '{"timeEntryId": "' + timeEntryID + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
               
                timeEntriesData = jQuery.parseJSON(response.d);

                // If Status is 500 then Error else Success
                if (timeEntriesData.hasOwnProperty("StatusCode") && timeEntriesData.StatusCode == 500) {
                    ShowError(timeEntriesData.ErrorMessage);
                    $('#loadingImage').hide();
                    $('#tabs').show();
                    $('#content').show();
                    
                }
                else {
                    // Bind Time Entries and trigger events
                    BindEditEntryDetails(timeEntriesData);
                    TriggerEvents();
                    $("#btnSave").val("Update Time");
                }
                $("#actionMessage").text("");
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error loading data.");

                $('#loadingImage').hide();
                $('#tabs').show();
                $('#content').show();
                $("#actionMessage").text("");
            }
        });
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");

        $('#loadingImage').hide();
        $('#tabs').show();
        $('#content').show();
        $("#actionMessage").text("");
    }
}

// Submits Time Entry
function SubmitTimeEntry() {
    try {
        $('#tabs').hide();
        $('#content').hide();
        $('#loadingImage').show();
        $("#actionMessage").text("Submitting Time Entries...");

        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/SubmitWeekTimeEntry",
            data: '{"startDateOfWeek": "' + $('#startDate').val() + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                // If some data returned then Error
                if (response.d) {
                    var response = jQuery.parseJSON(response.d);

                    // If Status is 500 then Error else Success
                    if (responseData.hasOwnProperty("StatusCode") && responseData.StatusCode == 500) {
                        ShowError(responseData.ErrorMessage);
                    }
                    $('#loadingImage').hide();
                    $('#tabs').show();
                    $('#content').show();
                }
                else {
                    var currentDate = $("#weeklyHours").find(".weekly_day.selected").attr("data-value");

                    // Refresh Time Entry Tab
                    LoadTimeEntryViews(outlookItem);

                    // Load the hours of week to display on top of page
                    LoadWeekHours(false, currentDate);

                    // Load LoadSingleEntry() to load the selected date
                    LoadSingleDayEntry(false);
                }
                $("#actionMessage").text("");
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error submitting data.");

                $('#loadingImage').hide();
                $('#tabs').show();
                $('#content').show();
                $("#actionMessage").text("");
            }
        });
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error submitting data.");
        
        $('#loadingImage').hide();
        $('#tabs').show();
        $('#content').show();
        $("#actionMessage").text("");
    }
}

//---- Private Methods ----//

// Called to bind  time entry details fetched from single time entry
function BindEditEntryDetails(timeEntryDetails) {
    try {
        if (timeEntryDetails != undefined && timeEntryDetails[0] != undefined) {
            // Switch to TimeEntryDetails page
            $("#tabTimeEntryDetails").addClass("active");
            $("#tabTimeEntries").removeClass("active");
            $("#timeEntryDetailsContainer").addClass("active");
            $("#timeEntriesContainer").removeClass("active");

            // Bind Client, Project, Task and other details
            $("#ddlClient").val(timeEntryDetails[0].ClientId);
            //Load Projects and Task
            LoadProjects(timeEntryDetails[0].ProjectId, timeEntryDetails[0].TaskId, true);

            // Bind timeEntryId, Description and Billable details
            $("#timeEntryId").val(timeEntryDetails[0].TimeEntryId);
            $("#description").val(timeEntryDetails[0].Description);
            $("#txtBillable").val(timeEntryDetails[0].BillableHours);

            try
            {
                // Set Calendar controls
                var fromCalendar = $("#isCalenderEntry").val();
                if (fromCalendar == "true") {
                    var startTime = moment(timeEntryDetails[0].StartTime, "LT").format("hh:mm A");
                    var endTime = moment(timeEntryDetails[0].StopTime, "LT").format("hh:mm A");

                    $("#meetingStartTime").val(startTime);
                    $("#meetingEndTime").val(endTime);

                    var duration = Math.round(moment(endTime, 'hh:mm a').diff(moment(startTime, 'hh:mm a'), 'hours', true) * 100) / 100;

                    $("#meetingTotalHours").text(duration + "h");
                }
            }
            catch(e)
            {
                // Show error if error comes
                ShowError("Error loading start and end time.");
            }
            // Set Billable checkbox
            if (timeEntryDetails[0].IsBillable == "Billable") {
                $("#meetingChkBillable").prop("checked", true);
                $("#chkBillable").prop("checked", true);
                
            }
            else {
                $("#meetingChkBillable").prop("checked", false);
                $("#chkBillable").prop("checked", false);
            }


            $('#loadingImage').hide();
            $('#content').show();
            $('#tabs').show();
        }
        else {
            $('#loadingImage').hide();
            $('#content').show();
            $('#tabs').show();
        }
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");

        $('#loadingImage').hide();
        $('#content').show();
        $('#tabs').show();
    }
}

function BindCustomTemplateDetails(timeEntryCustomDetails) {
    
    //Bind Custom Fields data
    if (timeEntryCustomDetails != undefined && timeEntryCustomDetails[0] != undefined) {

        $("[data-type^='custom_']").each(function (i, item) {
            //Custom control from list
            var customField = timeEntryCustomDetails[0].CustomFields.filter(function (customFieldItem) {
                return customFieldItem.TemplateId === item.id;
            });

            // Get data-type attribute from customfield control
            var customControlAttribute = item.attributes["data-type"].value;
            var customControlType = "";

            // Control from UI to set value
            var cutomControlUI = $("#" + item.id);

            if (customField.length > 0) {
                customControlType = customControlAttribute.split("_");

                if (customControlType != "" && customControlType[1] != undefined) {
                    switch (customControlType[1]) {

                        case "numeric": // For numeric fields
                            cutomControlUI.attr("data-guid", customField[0].Id);
                            if (customField[0].Values != undefined && customField[0].Values[0] != "")
                                cutomControlUI.val(customField[0].Values[0]);
                            else
                                cutomControlUI.val("");
                            break;

                        case "alphanumeric":    // For alphanumeric fields
                            cutomControlUI.attr("data-guid", customField[0].Id);
                            if (customField[0].Values != undefined && customField[0].Values[0] != "") {
                                cutomControlUI.val(customField[0].Values[0]);
                            }
                            else {
                                cutomControlUI.text("");
                            }
                            break;

                        case "datePicker":  // For Date fields
                            cutomControlUI.attr("data-guid", customField[0].Id);
                            if (customField[0].Values != undefined && customField[0].Values[0] != "")
                                cutomControlUI.find("input.form-control").val(moment(customField[0].Values[0]).format("MM/DD/YYYY"));
                            else
                                cutomControlUI.find("input.form-control").val("");

                            cutomControlUI.trigger("dp.change");
                            break;

                        case "multipleselect":  // For multi-select fields
                            cutomControlUI.attr("data-guid", customField[0].Id);
                            cutomControlUI.multiselect('deselectAll', false, true);
                            cutomControlUI.multiselect('updateButtonText');

                            if (customField[0].Values != undefined && customField[0].Values.length > 0) {
                                cutomControlUI.multiselect('select', customField[0].Values, true);
                            }

                            break;

                        case "exclusiveSelect": // For exclusive select fields
                            cutomControlUI.attr("data-guid", customField[0].Id);
                            $('option', cutomControlUI).each(function (element) {
                                cutomControlUI.multiselect('deselect', $(this).val(), true);
                            });

                            if (customField[0].Values != undefined && customField[0].Values[0] != "") {
                                cutomControlUI.multiselect('select', customField[0].Values[0], true);
                            }
                            else {
                                //Get placeholder text form Label (it will be always same)
                                var placeholderText = $("#" + item.id + "_label").text();
                                cutomControlUI.parent("span").find("button.multiselect").prop('title', placeholderText);
                                cutomControlUI.parent("span").find("span.multiselect-selected-text").text(placeholderText);
                            }

                            break;
                    }
                }
            }
            else {
                // Set blank values if no-value
                customControlType = customControlAttribute.split("_");
                if (customControlType != "" && customControlType[1] != undefined) {
                    switch (customControlType[1]) {

                        case "numeric":
                            cutomControlUI.val("");

                            break;

                        case "alphanumeric":
                            cutomControlUI.val("");
                            cutomControlUI.text("");

                            break;

                        case "datePicker":
                            cutomControlUI.find("input.form-control").val("");

                            break;

                        case "multipleselect":
                            cutomControlUI.multiselect('deselectAll', false);
                            cutomControlUI.multiselect('updateButtonText');

                            break;

                        case "exclusiveSelect":
                            $('option', cutomControlUI).each(function (element) {
                                cutomControlUI.multiselect('deselect', $(this).val());
                            });
                            var selectedText = $("#" + item.id + "_label").text();
                            cutomControlUI.parent("span").find("button.multiselect").prop('title', selectedText);
                            cutomControlUI.parent("span").find("span.multiselect-selected-text").text(selectedText);
                            $("#" + item.id + "_label").removeClass("show");

                            break;
                    }
                }
            }
        });
    }
}

// Called to toggle open/close time entry on Time Sheet page
function ToggleDetails(obj) {
    try {
        $(obj).toggleClass("rotate-180");

        if ($(obj).hasClass("rotate-180")) {


            $(obj).closest(".timeentry_upper").find("#clientName").show();
            $(obj).closest(".timeentry_upper").find("#shortProjectName").hide();
            $(obj).closest(".timeentry_upper").find("#fullProjectName").show();

            var divHeight = $(obj).closest(".task_details").find(".task-details-content").height() + Number($(obj).attr("data-height")) + 25;
            $(obj).closest(".timeentry_upper").animate({ height: divHeight }, { queue: false, duration: 400 });



        }
        else {
            $(obj).closest(".timeentry_upper").animate({ height: "60" }, { queue: false, duration: 400 });

            $(obj).closest(".timeentry_upper").find("#shortProjectName").show();
            $(obj).closest(".timeentry_upper").find("#fullProjectName").hide();
            $(obj).closest(".timeentry_upper").find("#clientName").hide(400)
        }
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
    }
}