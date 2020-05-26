//---- Variables ----//

// Create varaiable with month names
var monthNames = ["Jan", "Feb", "March", "April", "May", "June", "July", "August", "Sep", "Oct", "Nov", "Dec"];

//---- Public Methods ----//

// Load the hours of week to display on top of page
function LoadWeekHours(initialLoad, currentDate) {
    try {
        // Set this only once during first page load. Calls the SetWeekDate with current datetime
        if (initialLoad) {
            $('#weeklyDatePicker').datetimepicker({
                format: 'MMMM DD,YYYY',
                defaultDate: SetWeekDate(moment())
            });
            $('#weeklyDatePicker');
        }

        // Set Start and End dates
        var startDate = $("#startDate").val();
        var endDate = $("#endDate").val();

        // GetTimeEntriesDailyTotal() is called with Start and End dates to get the hours for one week
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/GetTimeEntriesDailyTotal",
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
                    var hoursData = responseData

                    $("#weeklyHours").html("");

                    _.each(hoursData.DailyHoursList, function (item, key, list) {
                        var template = _.template($("#weeklyHoursTemplate").text())(item);
                        $("#weeklyHours").append(template);
                    });

                    // If the current date is not selected
                    if (currentDate == "") {
                        // If Addin opened from appointment and its initial load
                        if (outlookItem != undefined && outlookItem.itemType == "appointment" && initialLoad) {
                            // Load the selected appointment date
                            var dayNumber = moment($("#isCalenderEntry").attr("data-date"), "MM/DD/YYYY").day() + 1;
                            $("#weeklyHours").find(".weekly_day:nth-child(" + dayNumber + ")").addClass("selected");

                            $("#weekDayName").find(".day_weekly_block.selected").removeClass("selected"); // Unselect previous selected day
                            var day = moment($("#isCalenderEntry").attr("data-date"), "MM/DD/YYYY").format("dddd");
                            $("#" + day).addClass("selected");
                        }
                        else {
                            if (initialLoad)
                            {
                                // Load Current Day
                                var todayNumber = moment().day() + 1 ;
                                $("#" + moment().format('dddd')).addClass("selected");
                                $("#weeklyHours").find(".weekly_day:nth-child(" + todayNumber + ")").addClass("selected");
                            }
                            else
                            {
                                // Load Monday
                                $("#weeklyHours").find(".weekly_day:nth-child(2)").addClass("selected");
                            }
                            
                            
                        }
                        // If initial load then call LoadSingleDayEntry() to load data for that day in Time Sheet page
                        if (!initialLoad) {
                            LoadSingleDayEntry(false);
                        }
                    }
                    else {
                        // Select the user selected day
                        $("#weeklyHours").find(".weekly_day[data-value='" + currentDate + "']").addClass("selected");
                    }
                }
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error loading data.");
            }
        });
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
    }
}

// Load TimeEntryDetails page where below is display
// either Assignments dropdown and CustomTemplates
// or selected Time entry data selected from Time Sheet page
function LoadTimeEntryViews() {
    try {
        $('#tabs').hide();
        $('#content').hide();
        $('#loadingImage').show();

        $.get("TimeEntry/TimeEntryDetails/TimeEntryDetails.html", function (data) {
            // Load TimeEntryDetails page in TimeEntryHome page
            $("#timeEntryDetailsContainer").html(data);
            // Load Client, Project, Task dropdown
            LoadClients();
            //Show hide meeting input Divs
            DisplayMeetingDivs()
            // Disable Billable checkbox
            SetBillableCheckBoxVisibility()
            // Load Custom Templates
            //LoadCustomTemplates();
        });

        $.get("TimeEntry/TimeEntries/TimeEntries.html", function (data) {
            // Load TimeEntries page (Time Sheet) in TimeEntryHome page
            $("#timeEntriesContainer").html(data);
        });

    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
    }
}

// Called to logout user and redirect to login screen
function Logoff() {
    $('#homePageContainer').hide();
    $('#loadingImageHome').show();
    try {
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/LogOff",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
                // On successful reload addin
                window.location.reload();
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error during logoff.");
                $('#homePageContainer').show();
                $('#loadingImageHome').hide();
            }
        });

    }
    catch (e) {
        // Show error if error comes
        ShowError("Error during logoff.");
        $('#homePageContainer').show();
        $('#loadingImageHome').hide();
    }
}

//---- Private Methods ----//

// Set the current date.
function SetWeekDate(date) {
    try {
        // If Addin opened from appointment then reset date
        if (outlookItem != undefined && outlookItem.itemType == "appointment") {
            date = moment($("#isCalenderEntry").attr("data-date"), "MM/DD/YYYY");
        }

        // Get date from moment-with-locales.js
        var firstDate = moment(date, "MMMM DD,YYYY").day(0).format("DD");
        var lastDate = moment(date, "MMMM DD,YYYY").day(6).format("DD");
        var month = monthNames[moment(date, "MMMM DD,YYYY").day(0).format("MM") - 1];
        var year = moment(date, "MMMM DD,YYYY").day(6).format("YYYY");

        // Four hidden fields - startDate, endDate, previousDate, nextDate
        $("#startDate").val(moment(date, "MM/DD/YYYY").day(0).format("MM/DD/YYYY")); // Current week start date
        $("#endDate").val(moment(date, "MM/DD/YYYY").day(6).format("MM/DD/YYYY")); // Current week end date

        $("#previousDate").val(moment(date, "MMMM DD,YYYY").day(-1)); // For Previous week end date
        $("#nextDate").val(moment(date, "MMMM DD,YYYY").day(7)); // For Next week start date

        // Setting Month, Date, Year for top date scroller
        $("#month").text(month);
        $("#dates").text(firstDate + "-" + lastDate);
        $("#year").text(year);
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
    }
}

// Set/update dates of next week for top scroller
function SetNextWeek() {
    try {
        // Get date from moment-with-locales.js
        var date = moment($("#nextDate").val());
        var firstDate = moment(date, "MMMM DD,YYYY").day(0).format("DD");
        var lastDate = moment(date, "MMMM DD,YYYY").day(6).format("DD");
        var month = monthNames[moment(date, "MMMM DD,YYYY").day(0).format("MM") - 1];
        var year = moment(date, "MMMM DD,YYYY").day(6).format("YYYY");

        // Four hidden fields - startDate, endDate, previousDate, nextDate
        $("#startDate").val(moment(date, "MM/DD/YYYY").day(0).format("MM/DD/YYYY")); // Current week start date
        $("#endDate").val(moment(date, "MM/DD/YYYY").day(6).format("MM/DD/YYYY")); // Current week end date

        $("#previousDate").val(moment(date, "MMMM DD,YYYY").day(-1)); // For previous week end date
        $("#nextDate").val(moment(date, "MMMM DD,YYYY").day(7)); // For next week start date

        // Setting Month, Date, Year for top date scroller
        $("#month").text(month);
        $("#dates").text(firstDate + "-" + lastDate);
        $("#year").text(year);

        $("#timeEntryId").val("");
        timeEntriesData = "";
        $("#btnSave").val("Enter Time");
        //Remove red outline from controls if css applied
        RemoveControlErrorStyle();

        // Call LoadWeekHours() to reload
        LoadWeekHours(false, "");

        //Show hide meeting input Divs
        DisplayMeetingDivs();

        // Loading Custom Templates data
        LoadCustomTemplatesWithDefaultData();
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
    }
}

// Set/update dates of previous week for top scroller
function SetPreviousWeek() {
    try {
        // Get date from moment-with-locales.js
        var date = moment($("#previousDate").val());
        var firstDate = moment(date, "MMMM DD,YYYY").day(0).format("DD");
        var lastDate = moment(date, "MMMM DD,YYYY").day(6).format("DD");
        var month = monthNames[moment(date, "MMMM DD,YYYY").day(0).format("MM") - 1];
        var year = moment(date, "MMMM DD,YYYY").day(6).format("YYYY");

        // Four hidden fields - startDate, endDate, previousDate, nextDate.
        $("#startDate").val(moment(date, "MM/DD/YYYY").day(0).format("MM/DD/YYYY")); // Current week start date
        $("#endDate").val(moment(date, "MM/DD/YYYY").day(6).format("MM/DD/YYYY")); // Current week end date

        $("#previousDate").val(moment(date, "MMMM DD,YYYY").day(-1)); // For previous week end date
        $("#nextDate").val(moment(date, "MMMM DD,YYYY").day(7)); // For next week start date

        // Setting Month, Date, Year for top date scroller
        $("#month").text(month);
        $("#dates").text(firstDate + "-" + lastDate);
        $("#year").text(year);

        $("#timeEntryId").val("");
        timeEntriesData = "";
        $("#btnSave").val("Enter Time");
        //Remove red outline from controls if css applied
        RemoveControlErrorStyle();

        // Call LoadWeekHours() to reload
        LoadWeekHours(false, "");

        //Show hide meeting input Divs
        DisplayMeetingDivs();

        // Loading Custom Templates data
        LoadCustomTemplatesWithDefaultData();
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
    }
}

// Called when day is clicked from top weekly hours
function SelectDate(weeklyHourDiv) {
    try {
        // Hide all errors first
        HideAllError();

        $("#weeklyHours").find(".weekly_day.selected").removeClass("selected"); // Unselect previous selected date 
        $("#weekDayName").find(".day_weekly_block.selected").removeClass("selected"); // Unselect previous selected day

        $(weeklyHourDiv).addClass("selected"); // Highlight selected Date

        var startDate = $("#weeklyHours").find(".weekly_day.selected").attr("data-value"); // Get selected date

        // Get date from moment-with-locales.js
        var day = moment(startDate).format("dddd");
        var month = moment(startDate).format("MMMM");
        var date = moment(startDate).format("DD");
        var year = moment(startDate).format("YYYY");

        // Set date in Time Sheet page
        $("#" + day).addClass("selected");
        $("#entriesDayDate").text(day + " " + month + " " + date + ", " + year); // Monday March 8, 2016

        $("#btnSave").val("Enter Time");

        //Remove red outline from controls if css applied
        RemoveControlErrorStyle();
        // Call LoadTimeEntries() to load data in Time Sheet page
        LoadTimeEntries(startDate, startDate)

        // Call Load Week Hours Details
        LoadWeekHoursDetails();
        // Hide TimeEntryDetails page and show Time Sheet page
        $("#tabTimeEntryDetails").removeClass("active");
        $("#tabTimeEntries").addClass("active");
        $("#timeEntryDetailsContainer").removeClass("active");
        $("#timeEntriesContainer").addClass("active");

        $("#timeEntryId").val("");
        timeEntriesData = "";
        //Show hide meeting input Divs
        DisplayMeetingDivs();

        // Loading Custom Templates data
        LoadCustomTemplatesWithDefaultData();
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
    }
}