//---- Public Variable ----//

var timeEntry_CustomData = ""; // For Custom Templates data

var validateData = { isvalid: true }; // For field Validations

// Initialize Validators
var requiredValidator = new Validator.RequiredValidator();
var numericValidator = new Validator.NumberValidator();
var alphanumericValidator = new Validator.AlphanumericValidator();
var timeValidator = new Validator.TimeValidator();
var dateValidator = new Validator.DateValidator();
var hourValidator = new Validator.HourValidator();

//---- Public Methods ----//

// Load Client dropdowns data
function LoadClients() {
    try {
        $("#actionMessage").text("Loading Data...");
        // GetClients() is called to get Client dropdowns data
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/GetClients",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var clientsData = jQuery.parseJSON(response.d);

                // If Status is 500 then Error else Success
                if (clientsData.hasOwnProperty("StatusCode") && clientsData.StatusCode == 500) {
                    ShowError(clientsData.ErrorMessage);
                    //Pass param Method name to show/hide corresponding changes
                    ShowDropdowns("LoadClients");
                }
                else {
                    // Load Client with Default Data
                    $("#ddlClient option").remove();
                    if (clientsData.length > 0) {

                        $('#ddlClient').removeAttr("disabled");

                        _.each(clientsData, function (item, key, list) {
                            var template = _.template($("#listTemplate").text())(item);
                            $("#ddlClient").append(template);
                        });
                        // Pass blank to select first/default value
                        LoadProjects("", "", false);
                    }
                    else {
                        //Pass param Method name to show/hide corresponding changes
                        ShowDropdowns("LoadClients");
                    }

                }
                $('#loadingImage').hide();
                $('#content').show();
                $('#tabs').show();
                $("#actionMessage").text("");
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error loading data.");

                $('#loadingImage').hide();
                $('#content').show();
                $('#tabs').show();
                $("#actionMessage").text("");
            }
        });       
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");

        $('#loadingImage').hide();
        $('#content').show();
        $('#tabs').show();
        $("#actionMessage").text("");
    }
}

// Load Projects and Task [Pass blank to load default/first values]
function LoadProjects(selectedProjectId, selectedTaskId, IsFromEdit) {
    try {
        $('#taskDiv').hide();
        $('#projectDiv').hide();
        $('#dropdownloadingImage').show();
        $("#dropdownActionMessage").text("Loading Projects and Tasks Data...");
        $('#ddlClient').attr("disabled", "disabled");

        // Fetch Client details and create object
        var clientId = $('#ddlClient :selected').val();
        var clientName = $('#ddlClient :selected').text();
        var clientAssignmentURI = $('#ddlClient :selected').attr('data-uri');

        var clientDetails = {
            ItemID: clientId,
            Name: clientName,
            GetAssignmentsURI: clientAssignmentURI
        };

        // GetProjects() is called to get Project dropdowns data
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/GetProjects",
            data: '{"client" : ' + JSON.stringify(clientDetails) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var projectsData = jQuery.parseJSON(response.d);

               // If Status is 500 then Error else Success
                if (projectsData.hasOwnProperty("StatusCode") && projectsData.StatusCode == 500) {
                    ShowError(projectsData.ErrorMessage);
                    //Pass param Method name to show/hide corresponding changes
                    ShowDropdowns("LoadProjects");
                }
                else {
                    // Load Projects with Default Data and Bind Projects
                    $("#ddlProject option").remove();
                    if (projectsData.length > 0) {

                        $('#ddlProject').removeAttr("disabled");

                        _.each(projectsData, function (item, key, list) {
                            var template = _.template($("#listTemplate").text())(item);
                            $("#ddlProject").append(template);
                        });


                        if (selectedProjectId != "") {
                            $("#ddlProject").val(selectedProjectId);
                            LoadTasks(selectedTaskId, IsFromEdit);
                        }
                        else {
                            // Pass blank to select first/default value
                            LoadTasks("", IsFromEdit);
                        }
                    }
                    else {
                        //Pass param Method name to show/hide corresponding changes
                        ShowDropdowns("LoadProjects"); 
                    }

                    $('#projectDiv').show();
                }
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error loading data.");

                $('#taskDiv').show();
                $('#projectDiv').show();
                $('#dropdownloadingImage').hide();
                $("#dropdownActionMessage").text("");
                $('#ddlClient').removeAttr("disabled");
            }
        });
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");

        $('#taskDiv').show();
        $('#projectDiv').show();
        $('#dropdownloadingImage').hide();
        $("#dropdownActionMessage").text("");
        $('#ddlClient').removeAttr("disabled");
    }
}

// Load Tasks [Pass blank to load default/first values]
function LoadTasks(taskId, IsFromEdit) {
    try {
        
        $('#taskDiv').hide();
        $('#dropdownloadingImage').show();
        $("#dropdownActionMessage").text("Loading Task Data...");
        $('#ddlProject').attr("disabled", "disabled");

        // Fetch Project details and create object
        var projectId = $('#ddlProject :selected').val();
        var projectName = $('#ddlProject :selected').text();
        var projectAssignmentURI = $('#ddlProject :selected').attr('data-uri');

        var projectDetails = {
            ItemID: projectId,
            Name: projectName,
            GetAssignmentsURI: projectAssignmentURI
        };

        // GetTask() is called to get Task dropdowns data
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/GetTask",
            data: '{"project": ' + JSON.stringify(projectDetails) + '}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var tasksData = jQuery.parseJSON(response.d);

                // If Status is 500 then Error else Success
                if (tasksData.hasOwnProperty("StatusCode") && tasksData.StatusCode == 500) {
                    ShowError(tasksData.ErrorMessage);
                }
                else {
                    // Load Tasks with Default Data and Bind Tasks
                    $("#ddlTask option").remove();

                    if (tasksData.length > 0) {

                        $('#ddlTask').removeAttr("disabled");

                        _.each(tasksData, function (item, key, list) {
                            var template = _.template($("#listTemplate").text())(item);
                            $("#ddlTask").append(template);
                        });

                        if (taskId != "") {
                            $("#ddlTask").val(taskId);
                        }

                        // Call to check/uncheck billable checkbox
                        CheckUncheckBillable(IsFromEdit);
                    }
                    else {
                        $('#ddlTask').attr("disabled", "disabled");
                    }
                }

                //Pass param Method name to show/hide corresponding changes
                ShowDropdowns("LoadTasks");

                $('#loadingImage').hide();
                $('#content').show();
                $('#tabs').show();


            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error loading data.");
                //Pass param Method name to show/hide corresponding changes
                ShowDropdowns("LoadTasks");

                $('#loadingImage').hide();
                $('#content').show();
                $('#tabs').show();
                $("#actionMessage").text("");
            }
        });
    }
    catch (e) {

        // Show error if error comes
        ShowError("Error loading data.");
        //Pass param Method name to show/hide corresponding changes
        ShowDropdowns("LoadTasks");

        $('#loadingImage').hide();
        $('#content').show();
        $('#tabs').show();
        $("#actionMessage").text("");
    }
}

// Check/uncheck billable checkbox 
function CheckUncheckBillable(IsFromEdit) {
    try {
        // Fetch Task details and create object
        var taskIsBillable = $('#ddlTask :selected').attr('data-IsBillable');
        var apiVersion = getCookie("APIVersion");
        // Check/Uncheck billable checkboxes for Meeting and Non meeting details
        if (!IsFromEdit) {
            if (apiVersion == 5) {
                if (taskIsBillable == "T") {
                    $("#meetingChkBillable").prop("checked", true);
                    $("#chkBillable").prop("checked", true);

                }
                else {
                    $("#meetingChkBillable").prop("checked", false);
                    $("#chkBillable").prop("checked", false);
                }
            }
            else {
                if (taskIsBillable == "T") {
                    console.log("Tasks is T");
                    $("#meetingChkBillable").prop("checked", true);
                    $("#chkBillable").prop("checked", true);
                    $("#chkBillable").prop("disabled", true);
                    $("#meetingChkBillable").prop("disabled", true);
                }
                else if (taskIsBillable == "F") {
                    console.log("Tasks is F");
                    $("#meetingChkBillable").prop("checked", false);
                    $("#chkBillable").prop("checked", false);
                    $("#chkBillable").prop("disabled", true);
                    $("#meetingChkBillable").prop("disabled", true);
                }
                else {
                    console.log("Tasks is U");
                    $("#meetingChkBillable").prop("checked", true);
                    $("#chkBillable").prop("checked", true);
                    $("#meetingChkBillable").removeProp("disabled");
                    $("#chkBillable").removeProp("disabled");
                }
            }
        }
        else {
            if (apiVersion > 5) {
                if (taskIsBillable == "T") {
                    $("#chkBillable").prop("disabled", true);
                    $("#meetingChkBillable").prop("disabled", true);
                }
                else if (taskIsBillable == "F") {
                    $("#chkBillable").prop("disabled", true);
                    $("#meetingChkBillable").prop("disabled", true);
                }
                else {
                    $("#meetingChkBillable").removeProp("disabled");
                    $("#chkBillable").removeProp("disabled");
                }
            }
        }
        //Bind custom templates specific to tasks
        LoadCustomTemplates(IsFromEdit);
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
    }
}

// Method to enable/disable billable box
function SetBillableCheckBoxVisibility() {
    try {
        // GetEmployeeOptions() is called to get if billable is enabled or disabled
        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/GetEmployeeOptions",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                employeeOptions = jQuery.parseJSON(response.d);
                // If Status is 500 then Error else Success
                if (employeeOptions.hasOwnProperty("StatusCode") && employeeOptions.StatusCode == 500) {
                    ShowError(responseData.ErrorMessage);
                }
                else {
                    var apiVersion = getCookie("APIVersion");
                    if (apiVersion == 5) {
                        // Disable billable checkboxes for Meeting and non meeting details
                        if (employeeOptions.ShowBillable == "T") {
                            $("#meetingChkBillable").removeProp("disabled");
                            $("#chkBillable").removeProp("disabled");
                        }
                        else {
                            $("#chkBillable").prop("disabled", true);
                            $("#meetingChkBillable").prop("disabled", true);
                        }
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

// Load Custom Templates data
function LoadCustomTemplates(IsFromEdit) {
    try {
        $('#customFields').hide();
        $('#customLoadingImage').show();
        // GetAllCustomTemplates() is called to get data for Custom Templates
        var selectedTaskId = $('#ddlTask :selected').val();

        $.ajax({
            type: "POST",
            url: gl_ServicePath + "/GetCustomTemplates",
            data: '{"taskId": "' + selectedTaskId + '"}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                timeEntry_CustomData = jQuery.parseJSON(response.d);

                // If Status is 500 then Error else Success
                if (timeEntry_CustomData.hasOwnProperty("StatusCode") && timeEntry_CustomData.StatusCode == 500) {
                    ShowError(responseData.ErrorMessage);
                }
                else {
                    // Load Custom Templates with default Data
                    LoadCustomTemplatesWithDefaultData();
                    var timeEntryId = $("#timeEntryId").val();
                    if (IsFromEdit || timeEntryId != '') {
                        BindCustomTemplateDetails(timeEntriesData);
                    }
                }
            },
            error: function (error) {
                // Show error if error comes
                ShowError("Error loading data.");
                $('#customLoadingImage').hide();
                $('#customFields').show();
            }
        });
    }
    catch (e) {
        // Show error if error comes
        ShowError("Error loading data.");
        $('#customLoadingImage').hide();
        $('#customFields').show();
    }
}

//---- Private Methods ----//

// Display Meeting Divs according to Item Type
function DisplayMeetingDivs() {
    // If opened from appointment then load Start/End time and show Start/End time divs
    if (outlookItem != undefined) {
        if (outlookItem.getItemType() == "appointment") {
            $("#meetingTimeDetails").show();
            $("#nonMeetingTimeDetails").hide();
            getMeetingStartEndTime();
        }
        else {
            $("#description").val("");
            $("#txtBillable").val("");
            $("#meetingTimeDetails").hide();
            $("#nonMeetingTimeDetails").show();
        }
    }
    else {
        $("#description").val("");
        $("#txtBillable").val("");
        $('#chkBillable').attr('checked', false);
        $("#meetingTimeDetails").hide();
        $("#nonMeetingTimeDetails").show();
    }
}

// Load Custom Templates with default values
function LoadCustomTemplatesWithDefaultData() {
    // If timeEntry_CustomData is not empty
    if (!_.isEmpty(timeEntry_CustomData)) {

        // Clear all Custom Templates from html
        $("#customFields").html("");

        // Loop thru the Custom Templates
        _.each(timeEntry_CustomData, function (item, key, list) {
            switch (item.Type) {
                case "A":
                    // For Alphanumeric Custom Templates
                    self.$list = $("#customFields");

                    // Get underscore template and append to main html
                    var template = _.template($("#alphanumericTemplate").text())(item);

                    if (item.Values != undefined && item.Values[0] != undefined) {
                        var innerelement = "";
                        if (item.Values[0].Default == "T")
                            innerelement = ($($.parseHTML(template)).find("textarea")).val(item.Values[0].Value);
                        else
                            innerelement = ($($.parseHTML(template)).find("textarea"));

                        var outerelement = $($.parseHTML(template)).children("textarea").remove().end();
                        var finalelement = innerelement.insertAfter(outerelement.find("label"));

                        if (finalelement != undefined && finalelement[0] != undefined)
                            self.$list.append(finalelement[0].parentElement);
                    }
                    else {
                        self.$list.append(template);
                    }

                    break;

                case "N":
                    // For Numeric Custom Templates
                    self.$list = $("#customFields");

                    // Get underscore template and append to main html
                    var template = _.template($("#numericTemplate").text())(item);

                    if (item.Values != undefined && item.Values[0] != undefined) {
                        var innerelement = "";
                        if (item.Values[0].Default == "T")
                            innerelement = ($($.parseHTML(template)).find("input")).attr("value", item.Values[0].Value);
                        else
                            innerelement = ($($.parseHTML(template)).find("input"));

                        var outerelement = $($.parseHTML(template)).children("input").remove().end();
                        var finalelement = innerelement.insertAfter(outerelement.find("label"));

                        if (finalelement != undefined && finalelement[0] != undefined)
                            self.$list.append(finalelement[0].parentElement);
                    }
                    else {
                        self.$list.append(template);
                    }

                    break;

                case "D":
                    // For Date Custom Templates
                    self.$list = $("#customFields");

                    // Get underscore template and append to main html
                    var template = _.template($("#datetimepickerTemplate").text())(item);
                    self.$list.append(template);

                    // Hook events
                    if (item.Values != undefined && item.Values[0] != undefined) {
                        $('#' + item.Id).datetimepicker({
                            format: 'MM/DD/YYYY',
                            ignoreReadonly: true

                        })
                        .on("dp.change", function (e) {
                            var selectedObjectValue = $('#' + item.Id).find("input.form-control").val();

                            if (selectedObjectValue != "") {
                                $("#" + item.Id + "_label").addClass("show");
                            }
                            else {
                                $("#" + item.Id + "_label").removeClass("show");
                            }

                        });

                        if (item.Values[0].Default == "T") {
                            $('#' + item.Id).find("input.form-control").val(moment(item.Values[0].Value).format("MM/DD/YYYY"));
                            $("#" + item.Id + "_label").addClass("show");
                        }

                    }
                    else {
                        // Hook events
                        $('#' + item.Id).datetimepicker({
                            format: 'MM/DD/YYYY',
                            ignoreReadonly: true
                        })
                        .on("dp.change", function (e) {
                            var selectedObjectValue = $('#' + item.Id).find("input.form-control").val();

                            if (selectedObjectValue != "") {
                                $("#" + item.Id + "_label").addClass("show");
                            }
                            else {
                                $("#" + item.Id + "_label").removeClass("show");
                            }
                        });
                    }

                    break;

                case "M":
                    // For Multi-select Templates. Uses bootstrap-multiselect.min.js
                    self.$list = $("#customFields");
                    var selectedCustomValues = [];
                    // Get underscore template and append to main html
                    var template = _.template($("#multiSelectTemplate").text())(item);
                    self.$list.append(template);

                    if (item.Values != undefined && item.Values[0] != undefined) {
                        _.each(item.Values, function (value, key, list) {
                            var optionTemplate = _.template($("#selectOptionTemplate").text())(value);
                            $('#' + item.Id).append(optionTemplate);
                            if(value.Default == "T")
                            {
                                selectedCustomValues.push(value.Value);
                            }
                        });
                    }
                    // Hook events
                    var placeHolderText = item.Required == 'T' ? item.Name + ' *' : item.Name;
                    $('#' + item.Id).multiselect({
                        numberDisplayed: 0,
                        nonSelectedText: placeHolderText,
                        onChange: function (option, checked, select) {
                            var selectedObjects = $('#' + item.Id + ' option:selected');
                            var selectedItems = [];
                            $(selectedObjects).each(function (index, selectedObject) {
                                selectedItems.push([$(this).val()]);
                            });

                            if (selectedItems.length > 0) {
                                $("#" + item.Id + "_label").addClass("show");
                            }
                            else {
                                $("#" + item.Id + "_label").removeClass("show");
                            }
                        }
                    });

                    if (selectedCustomValues.length > 0) {
                        $('#' + item.Id).multiselect('select', selectedCustomValues, true);
                    }

                    $(".caret").css('float', 'right').css('margin-top', '8px');
                    $("span.multiselect-native-select>.btn-group>.btn:first-child>span").css('float', 'left');

                    break;

                case "X":
                    // For Single-select Templates. Uses bootstrap-multiselect.min.js
                    self.$list = $("#customFields");

                    // Get underscore template and append to main html
                    var template = _.template($("#exclusiveSelectTemplate").text())(item);
                    self.$list.append(template);
                    var selectedCustomValues = [];
                    if (item.Values != undefined && item.Values[0] != undefined) {
                        _.each(item.Values, function (value, key, list) {
                            var optionTemplate = _.template($("#selectOptionTemplate").text())(value);
                            $('#' + item.Id).append(optionTemplate);

                            if (value.Default == "T") {
                                selectedCustomValues.push(value.Value);
                            }
                        });
                    }

                    var exclusiveplaceHolderText = item.Required == 'T' ? item.Name + ' *' : item.Name;

                    // Hook events
                    $('#' + item.Id).multiselect({
                        numberDisplayed: 1,
                        nonSelectedText: exclusiveplaceHolderText,
                        onChange: function (option, checked, select) {
                            var selectedObjects = $('#' + item.Id + ' option:selected');
                            var selectedItems = [];
                            $(selectedObjects).each(function (index, selectedObject) {
                                selectedItems.push([$(this).val()]);
                            });

                            if (selectedItems.length > 0) {
                                $("#" + item.Id + "_label").addClass("show");
                            }
                            else {
                                $("#" + item.Id + "_label").removeClass("show");
                            }
                        }
                    });
                    
                    //$('#' + item.Id).parent("span").find("button.multiselect").prop('title', exclusiveplaceHolderText);
                    //$('#' + item.Id).parent("span").find("span.multiselect-selected-text").text(exclusiveplaceHolderText);

                    if (selectedCustomValues.length > 0) {
                        //Deselect All value and refresh the placeholder text
                        var currentSelectedValue = $('#' + item.Id).parent("span").find("button.multiselect").prop('title');
                        $('#' + item.Id).multiselect('deselect', currentSelectedValue, true);
                        //Set value from response 
                        $('#' + item.Id).multiselect('select', selectedCustomValues[0], true);
                    }

                    $('#' + item.Id + "_label").addClass("show");
                    $(".caret").css('float', 'right').css('margin-top', '8px');
                    $("span.multiselect-native-select>.btn-group>.btn:first-child>span").css('float', 'left');

                    break;
            }
        });

        $('#customLoadingImage').hide();
        $("#customFields").show();
        // Set floating labels
        SetFloatingLabelsForInputAndSelect();
    }
    else {
        $("#customFields").html("");
        $("#customFields").hide();
        $('#customLoadingImage').hide();
    }
}

// To display floating labels above all controls
function SetFloatingLabelsForInputAndSelect() {
    // declare classes to be put on labels dynamically
    var onClass = "on";
    var showClass = "show";

    // For all input and textarea controls
    $("input, textarea")
      .bind("checkval", function () {   // Bind function
          // If there is value in the input control then show header label else hide
          var label = $(this).prev(".label_floating_label");

          if (this.value !== "")
              label.addClass(showClass);

          else
              label.removeClass(showClass);
      })
      .on("keyup", function () {    // Keyup function
          $(this).trigger("checkval");
      })
      .trigger("checkval");

    // Non-Meeting specific labels
    $("#txtBillable").bind("checkvalue", function () {
        // If there is value in the input control then show header label else hide
        if (this.value !== "")
            $("#floatingLabelHours").addClass(showClass);

        else
            $("#floatingLabelHours").removeClass(showClass);
    })
      .on("keyup", function () {
          $(this).trigger("checkvalue");
      })
      .trigger("checkvalue");

    // Meeting specific label
    $("#meetingStartTime, #meetingEndTime").bind("checkvalue", function () {
        // If there is value in the input control then show header label else hide
        if ($("#meetingEndTime").val() !== "" || $("#meetingStartTime, #meetingEndTime").val() !== "")
            $("#floatingLabelMeeting").addClass(showClass);

        else
            $("#floatingLabelMeeting").removeClass(showClass);
    })
      .on("keyup", function () {
          $(this).trigger("checkvalue");
      })
      .trigger("checkvalue");
}

// Gets Start and End time of appointment
function getMeetingStartEndTime() {
    try {
        var meetingStartTime;
        var meetingEndTime;
        var subject;

        var itemId = outlookItem.itemId;
        if (itemId === null || itemId == undefined) {
            //Is compose   
            outlookItem.start.getAsync(
            function (asyncResult) {
                // Get time if status is not failed
                if (asyncResult.status == Office.AsyncResultStatus.Failed) {
                    ShowTimeEntryDetailsError("Error loading meeting start time data.");
                }
                else {
                    // Show the header label
                    if (asyncResult.value != "")
                        $('#floatingLabelMeeting').addClass("show");

                    // Get appointment start time
                    $('#meetingStartTime').val(moment(asyncResult.value).format("hh:mm A"));
                    meetingStartTime = moment(asyncResult.value);

                    // Get appointment end time
                    outlookItem.end.getAsync(
                        function (asyncResultEndTime) {
                            // Get time if status is not failed
                            if (asyncResultEndTime.status == Office.AsyncResultStatus.Failed) {
                                ShowTimeEntryDetailsError("Error loading meeting end time data.");
                            }
                            else {
                                // Show the header label
                                if (asyncResultEndTime.value != "")
                                    $('#floatingLabelMeeting').addClass("show");

                                // Get appointment end time and find duration of the meeting in 2 decimal places
                                $('#meetingEndTime').val(moment(asyncResultEndTime.value).format("hh:mm A"));
                                var duration = Math.round(moment.duration(moment(asyncResultEndTime.value).diff(meetingStartTime)).asHours() * 100) / 100;

                                $("#meetingTotalHours").text(duration + "h");

                                //Get appointment subject to Description textbox
                                outlookItem.subject.getAsync(
                                        function (asyncResultSubject) {
                                            if (asyncResultSubject.status == Office.AsyncResultStatus.Failed) {
                                                ShowTimeEntryDetailsError("Error loading meeting subject data.");
                                            }
                                            else {
                                                if (asyncResultSubject.value != "")
                                                    $('#description').parent(".control-div").find("label.label_floating_label").addClass("show");

                                                $('#description').val(asyncResultSubject.value);
                                            }
                                        }
                                );
                            }
                        }
                   );
                }
            });
        }
        else {
            //Is read  
            // Get appointment start time
            $('#floatingLabelMeeting').addClass("show");
            $('#meetingStartTime').val(moment(outlookItem.start).format("hh:mm A"));
            meetingStartTime = moment(outlookItem.start);

            $('#floatingLabelMeeting').addClass("show");
            // Get appointment end time and find duration of the meeting in 2 decimal places
            $('#meetingEndTime').val(moment(outlookItem.end).format("hh:mm A"));
            var duration = Math.round(moment.duration(moment(outlookItem.end).diff(meetingStartTime)).asHours() * 100) / 100;
            $("#meetingTotalHours").text(duration + "h");

            $('#description').parent(".control-div").find("label.label_floating_label").addClass("show");
            $('#description').val(outlookItem.subject);

        }

    }
    catch (e) {
        // Show error if error comes
        ShowTimeEntryDetailsError("Error loading data.");
    }
}

// Saves Time Entry data
function SaveTimeEntry() {
    try {
        // To refresh validate object on each save click and clear all errors
        validateData.isvalid = true;
        HideAllError();

        // Get timeEntryid, clientid, projectid, taskid
        var timeEntryId = $('#timeEntryId').val();
        var clientId = $('#ddlClient :selected').val();
        var projectId = $('#ddlProject :selected').val();
        var taskId = $('#ddlTask :selected').val();
        
        // Get startDate and endDate from top
        var selectedDate = $("#weeklyHours").find(".weekly_day.selected").attr("data-value");        
        var startDate = moment(selectedDate).format("MM/DD/YYYY");
        var endDate = moment(selectedDate).format("MM/DD/YYYY");

        // Get description
        var totalHours = "";
        var description = $('#description').val();
        var isBillable = "";

        // Set total hours
        var fromCalendar = $("#isCalenderEntry").val();
        if (fromCalendar == "true") {   // From calendar entry
            // Validate the meeting entries
            ValidateMeetingEntries($("#meetingStartTime").val(), $("#meetingEndTime").val());

            // If valid then get startTime, stopTime and duration in hours
            if (validateData.isvalid) {
                var startTime = moment($("#meetingStartTime").val(), "LT").format("hh:mm A");
                var stopTime = moment($("#meetingEndTime").val(), "LT").format("hh:mm A");

                var totalHoursTime = (Math.round(moment(stopTime, 'hh:mm a').diff(moment(startTime, 'hh:mm a'), 'hours', true) * 100) / 100).toString();

                totalHours = totalHoursTime;
            }
            else {
                // If not valid then return
                $(window).scrollTop($("#homePageContainer").position().top);
                return false;
            }
            
            // Set billable/not-billable
            isBillable = $("#meetingChkBillable").prop("checked");
        }
        else {  // Non-calendar entry
            totalHours = $('#txtBillable').val();
            isBillable = $("#chkBillable").prop("checked");
        }

        // Set non-custom fields data
        var timeEntryDetails = {
            TimeEntryId: timeEntryId,
            ClientId: clientId,
            ProjectId: projectId,
            TaskId: taskId,
            StartDate: startDate,
            EndDate: endDate,
            Hours: totalHours,
            Description: description,
            IsBillableHours: isBillable,
            FromCalendar: fromCalendar,
            StartTime: startTime,
            StopTime: stopTime,
            CustomFields: []
        };
        
        // Set custom fields data by looping on each
        $("[data-type^='custom_']").each(function (i, item) {
            var customControlAttribute = item.attributes["data-type"].value;
            var customControlType = "";
            
            var customFieldDetails = {
                Id: "",
                TemplateId: "",
                Values: []
            };
            customControlType = customControlAttribute.split("_");
                    
            if (customControlType != "" && customControlType[1] != undefined) {

                // Set ID with whatever value - GUID or ""
                // Set TemplateID with whatever value - Numeric
                // Set Value only if data present
                switch (customControlType[1]) {

                    case "numeric":
                            customFieldDetails.Id = item.attributes["data-guid"].value;
                            customFieldDetails.TemplateId = item.id;
                            if(item.value.trim() != "")
                                customFieldDetails.Values.push(item.value);

                            timeEntryDetails.CustomFields.push(customFieldDetails);
                        break;

                    case "alphanumeric":
                            customFieldDetails.Id = item.attributes["data-guid"].value;
                            customFieldDetails.TemplateId = item.id;
                            if (item.value.trim() != "")
                                customFieldDetails.Values.push(item.value);

                            timeEntryDetails.CustomFields.push(customFieldDetails);
                        break;

                    case "datePicker":
                        customFieldDetails.Id = item.attributes["data-guid"].value;
                        customFieldDetails.TemplateId = item.id;                        

                        var customDate = $("#" + item.id).find("input").val();
                        if (customDate != "")
                            customFieldDetails.Values.push(moment(customDate).format("MM/DD/YYYY"));
                  
                            timeEntryDetails.CustomFields.push(customFieldDetails);
                        break;

                    case "multipleselect":
                        customFieldDetails.Id = item.attributes["data-guid"].value;
                        customFieldDetails.TemplateId = item.id;
                        var selectedValues = $("#" + item.id + " option:selected");
                        if (selectedValues.length > 0) {
                            $.each(selectedValues, function (i, item) {
                                customFieldDetails.Values.push(item.value.trim());
                            });
                        }
                        timeEntryDetails.CustomFields.push(customFieldDetails);

                        break;

                    case "exclusiveSelect":
                        customFieldDetails.Id = item.attributes["data-guid"].value;
                        customFieldDetails.TemplateId = item.id;

                        var selectedValues = $("#" + item.id).parent("span").find("button.multiselect").prop('title');
                        var controlName = $("#" + item.id).attr("placeholder");
                        if (selectedValues.trim() != controlName.trim())
                            customFieldDetails.Values.push(selectedValues);

                        timeEntryDetails.CustomFields.push(customFieldDetails);

                        break;
                }
            }
        });

        // Validate TimeEntry Object
        ValidateTimeEntryDetails(timeEntryDetails);

        // Call SaveTimeEntry() to save data only if time entries are valid
        if (validateData.isvalid) {
            $('#tabs').hide();
            $('#content').hide();
            $('#loadingImage').show();
            $("#actionMessage").text("Saving Time Entry...");
            // Convert into JSON
            var jsonTimeEntryDetails = JSON.stringify(timeEntryDetails);
            $.ajax({
                type: "POST",
                url: gl_ServicePath + "/SaveTimeEntry",
                data: '{"timeEntry": ' + jsonTimeEntryDetails + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    // If some data returned then Error
                    if (response.d)
                    {
                        var responseData = jQuery.parseJSON(response.d);

                        // If Status is 500 then Error else Success
                        if (responseData.hasOwnProperty("StatusCode") && responseData.StatusCode == 500) {
                            ShowTimeEntryDetailsError(responseData.ErrorMessage);
                        }

                        $('#tabs').show();
                        $('#content').show();
                        $('#loadingImage').hide();
                    }
                    else
                    {
                        // Refresh Hours.
                        LoadWeekHours(false, selectedDate);
                        // Refresh Time Entry Details.
                        LoadTimeEntryViews();
                    }
                    $("#actionMessage").text("");
                },
                error: function (error) {
                    // Show error if error comes
                    ShowTimeEntryDetailsError("Error saving data.");

                    $('#tabs').show();
                    $('#content').show();
                    $('#loadingImage').hide();
                    $("#actionMessage").text("");
                }
            });
        }
        else
        {
            ShowTimeEntryDetailsError("Please check errors below.");
            $('#tabs').show();
            $('#content').show();
            $('#loadingImage').hide();
            $("#actionMessage").text("");
            $(window).scrollTop($("#homePageContainer").position().top);
        }
    }
    catch (e) {
        // Show error if error comes
        ShowTimeEntryDetailsError("Error saving data.");

        $('#tabs').show();
        $('#content').show();
        $('#loadingImage').hide();
        $("#actionMessage").text("");
    }
}

// Validate time entry details object
function ValidateTimeEntryDetails(timeEntryData)
{
    //Remove red outline from controls if css applied
    RemoveControlErrorStyle();

    // Required check for ClientId
    if (!requiredValidator.isAcceptable(timeEntryData.ClientId))
    {
        validateData.isvalid = false;
        $("#errorClient").addClass("show");
        $("#ddlClient").addClass("red-outline");
    }
    else {
        $("#errorClient").removeClass("show");
        $("#ddlClient").removeClass("red-outline");
    }

    // Required check for ProjectId
    if (!requiredValidator.isAcceptable(timeEntryData.ProjectId))
    {
        validateData.isvalid = false;
        $("#errorProject").addClass("show");
        $("#ddlProject").addClass("red-outline");
    }
    else {
        $("#errorProject").removeClass("show");
        $("#ddlProject").removeClass("red-outline");
    }

    // Required check for TaskId
    if (!requiredValidator.isAcceptable(timeEntryData.TaskId))
    {
        validateData.isvalid = false;
        $("#errorTask").addClass("show");
        $("#ddlTask").addClass("red-outline");
    }
    else {
        $("#errorTask").removeClass("show");
        $("#ddlTask").removeClass("red-outline");
    }

    // Length check Description
    if (timeEntryData.Description.length > 4000) {
        validateData.isvalid = false;
        $("#errorDescription").addClass("show");
        $("#description").addClass("red-outline");
    }
    else {
        $("#errorDescription").removeClass("show");
        $("#description").removeClass("red-outline");
    }

    // Required Hours check
    // then correct format hours check
    // then greater then 0 check
    if (!requiredValidator.isAcceptable(timeEntryData.Hours))
    {
        validateData.isvalid = false;
        $("#errorHours").text("Hours field cannot be empty.");
        $("#errorHours").addClass("show");
        $("#txtBillable").addClass("red-outline");
    }
    else if (!hourValidator.isAcceptable(timeEntryData.Hours))
    {
        validateData.isvalid = false;
        $("#errorHours").text("Enter data in numeric format with 2 decimal places.");
        $("#errorHours").addClass("show");
        $("#txtBillable").addClass("red-outline");
    }
    else if (parseFloat(Number(timeEntryData.Hours)) == 0) {
        validateData.isvalid = false;
        $("#errorHours").text("Hours must be greater than zero.");
        $("#errorHours").addClass("show");
        $("#txtBillable").addClass("red-outline");
    }
    else {
        $("#errorHours").removeClass("show");
        $("#txtBillable").removeClass("red-outline");
    }
    
    // If Calendar entry then validate meeting entries
    if (timeEntryData.FromCalendar == "true") {
        ValidateMeetingEntries(timeEntryData.StartTime, timeEntryData.StopTime);
    }

    // Loop thru each Custom Fields and validate
    $.each(timeEntryData.CustomFields, function (index, item) {
        var customControlType = $("#" + item.TemplateId).attr("data-type").split("_"); // Get Custom Field data type
        var customControlRequired = $("#" + item.TemplateId).attr("data-required"); // Get Custom Field required flag

        switch (customControlType[1]) {
            
            case "numeric": // For Numeric Custom Fields
                // Check for required data and correct format
                if (customControlRequired == "T") {
                    if (item.Values.length > 0) {
                        if (!requiredValidator.isAcceptable(item.Values[0])) {
                            validateData.isvalid = false;
                            $("#" + item.TemplateId + "_errorCustomNumeric").text("Custom Numeric data is required.");
                            $("#" + item.TemplateId + "_errorCustomNumeric").addClass("show");
                            $("#" + item.TemplateId).addClass("red-outline");
                        }
                        else {
                            $("#" + item.TemplateId + "_errorCustomNumeric").removeClass("show");
                            $("#" + item.TemplateId).removeClass("red-outline");
                        }
                    }
                    else {
                        validateData.isvalid = false;
                        $("#" + item.TemplateId + "_errorCustomNumeric").text("Custom Numeric data is required.");
                        $("#" + item.TemplateId + "_errorCustomNumeric").addClass("show");
                        $("#" + item.TemplateId).addClass("red-outline");

                    }
                }
                if (item.Values != undefined && item.Values[0] != undefined) {
                    if (!numericValidator.isAcceptable(item.Values[0])) {
                        validateData.isvalid = false;
                        $("#" + item.TemplateId + "_errorCustomNumeric").text("Enter data in numeric format only.");
                        $("#" + item.TemplateId + "_errorCustomNumeric").addClass("show");
                        $("#" + item.TemplateId).addClass("red-outline");
                    }
                    else {
                        $("#" + item.TemplateId + "_errorCustomNumeric").removeClass("show");
                        $("#" + item.TemplateId).removeClass("red-outline");
                    }
                }

                break;
            
            case "alphanumeric":    // For Alphanumeric Custom Fields
                // Check for required data and correct format
                if (customControlRequired == "T") {
                    if (item.Values.length > 0) {
                        if (!requiredValidator.isAcceptable(item.Values[0])) {
                            validateData.isvalid = false;
                            $("#" + item.TemplateId + "_errorCustomAlphaNumeric").text("Custom alphanumeric data is required.");
                            $("#" + item.TemplateId + "_errorCustomAlphaNumeric").addClass("show");
                            $("#" + item.TemplateId).addClass("red-outline");
                        }
                        else {
                            $("#" + item.TemplateId + "_errorCustomAlphaNumeric").removeClass("show");
                            $("#" + item.TemplateId).removeClass("red-outline");
                        }
                    }
                    else {
                        validateData.isvalid = false;
                        $("#" + item.TemplateId + "_errorCustomAlphaNumeric").text("Custom alphanumeric data is required.");
                        $("#" + item.TemplateId + "_errorCustomAlphaNumeric").addClass("show");
                        $("#" + item.TemplateId).addClass("red-outline");
                    }
                }
                if (item.Values != undefined && item.Values[0] != undefined) {
                    if (!alphanumericValidator.isAcceptable(item.Values[0])) {
                        validateData.isvalid = false;
                        $("#" + item.TemplateId + "_errorCustomAlphaNumeric").text("Special characters are not allowed.");
                        $("#" + item.TemplateId + "_errorCustomAlphaNumeric").addClass("show");
                        $("#" + item.TemplateId).addClass("red-outline");
                    }
                    else {
                        $("#" + item.TemplateId + "_errorCustomAlphaNumeric").removeClass("show");
                        $("#" + item.TemplateId).removeClass("red-outline");
                    }
                }

                break;

            case "datePicker":  // For Date Custom Fields
                // Check for required data and correct format
                if (customControlRequired == "T") {
                    if (item.Values.length > 0) {
                        if (!requiredValidator.isAcceptable(item.Values[0])) {
                            validateData.isvalid = false;
                            $("#" + item.TemplateId + "_errorCustomdate").text("Custom date is required.");
                            $("#" + item.TemplateId + "_errorCustomdate").addClass("show");
                            $("#" + item.TemplateId).addClass("red-outline");
                        }
                        else {
                            $("#" + item.TemplateId + "_errorCustomdate").removeClass("show");
                            $("#" + item.TemplateId).removeClass("red-outline");
                        }
                    }
                    else {
                        validateData.isvalid = false;
                        $("#" + item.TemplateId + "_errorCustomdate").text("Custom date is required.");
                        $("#" + item.TemplateId + "_errorCustomdate").addClass("show");
                        $("#" + item.TemplateId).addClass("red-outline");
                    }
                }
                    if (item.Values != undefined && item.Values[0] != undefined) {
                        if (!dateValidator.isAcceptable(item.Values[0])) {
                            validateData.isvalid = false;
                            $("#" + item.TemplateId + "_errorCustomdate").text("Custom date provided is out of range.");
                            $("#" + item.TemplateId + "_errorCustomdate").addClass("show");
                            $("#" + item.TemplateId).addClass("red-outline");
                        }
                        else {
                            $("#" + item.TemplateId + "_errorCustomdate").removeClass("show");
                            $("#" + item.TemplateId).removeClass("red-outline");
                        }
                    }

                    break;

            case "multipleselect":  // For multi-select Custom Fields
                // Check for required data and correct format
                if (item.Values != undefined) {
                    if (customControlRequired == "T") {
                        if (!item.Values.length > 0) {
                            validateData.isvalid = false;

                            $("#" + item.TemplateId + "_errorMultiple").addClass("show");
                            $("#" + item.TemplateId).parent("span.multiselect-native-select").find("button.multiselect").addClass("red-outline");
                        }
                        else {
                            $("#" + item.TemplateId + "_errorMultiple").removeClass("show");
                            $("#" + item.TemplateId).parent("span.multiselect-native-select").find("button.multiselect").removeClass("red-outline");
                        }
                    }
                }

                break;

            case "exclusiveSelect": // For Exclusive select Custom Fields
                // Check for required data and correct format
                if (item.Values != undefined) {
                    if (customControlRequired == "T") {
                        if (!item.Values.length > 0) {
                            validateData.isvalid = false;

                            $("#" + item.TemplateId + "_errorExclusive").addClass("show");
                            $("#" + item.TemplateId).parent("span.multiselect-native-select").find("button.multiselect").addClass("red-outline");
                        }
                        else if (item.Values[0] == "[None]") {
                            validateData.isvalid = false;

                            $("#" + item.TemplateId + "_errorExclusive").addClass("show");
                            $("#" + item.TemplateId).parent("span.multiselect-native-select").find("button.multiselect").addClass("red-outline");
                        }
                        else {
                            $("#" + item.TemplateId + "_errorExclusive").removeClass("show");
                            $("#" + item.TemplateId).parent("span.multiselect-native-select").find("button.multiselect").removeClass("red-outline");
                        }
                    }
                }

            break;
        }
    });
}

// Validate the meeting entries
function ValidateMeetingEntries(StartTime, StopTime) {
    var meetingDetailsError = false;

    // Check for StartTime required
    if (!requiredValidator.isAcceptable(StartTime)) {
        validateData.isvalid = false;
        $("#errorMeetingDetails").text("Start/Stop time field cannot be empty.");
        $("#errorMeetingDetails").addClass("show");
        $("#meetingStartTime").addClass("red-outline");
        meetingDetailsError = true;
    }

    // Check for StopTime required
    if (!requiredValidator.isAcceptable(StopTime)) {
        validateData.isvalid = false;
        $("#errorMeetingDetails").text("Start/Stop time field cannot be empty.");
        $("#errorMeetingDetails").addClass("show");
        $("#meetingEndTime").addClass("red-outline");
        meetingDetailsError = true;
    }

    // Check for StartTime correct time format
    // then Check for StopTime correct time format
    // then Check if StopTime is after StartTime
    if (!timeValidator.isAcceptable(StartTime)) {
        validateData.isvalid = false;
        $("#errorMeetingDetails").text("Start/Stop time is not in correct format.");
        $("#errorMeetingDetails").addClass("show");
        $("#meetingStartTime").addClass("red-outline");
        meetingDetailsError = true;
    }
    else if (!timeValidator.isAcceptable(StopTime)) {
        validateData.isvalid = false;
        $("#errorMeetingDetails").text("Start/Stop time is not in correct format.");
        $("#errorMeetingDetails").addClass("show");
        $("#meetingEndTime").addClass("red-outline");
        meetingDetailsError = true;
    }
    else if (!moment(StopTime, "LT").isAfter(moment(StartTime, "LT"))) {
        validateData.isvalid = false;
        $("#errorMeetingDetails").text("Start time cannot be greater than Stop time.");
        $("#errorMeetingDetails").addClass("show");
        $("#meetingEndTime").addClass("red-outline");
        meetingDetailsError = true;
    }

    // If errors in meeting time then show error
    if (!meetingDetailsError) {
        $("#errorMeetingDetails").removeClass("show");
        $("#meetingEndTime").removeClass("red-outline");
    }
}

//Refresh controls on UI
function RemoveControlErrorStyle()
{
    //Remove Red outline
    $("#timeEntryDetailsContainer").find(".red-outline").removeClass("red-outline");

    //Hide Labels
    $("#timeEntryDetailsContainer").find(".error_floating_label").removeClass("show");
}

// Trigger Keyup events for input, textarea, Hours, Meeting controls
function TriggerEvents() {
    $("input, textarea").trigger("checkval");
    $("#txtBillable").trigger("checkvalue");
    $("#meetingStartTime, #meetingEndTime").trigger("checkvalue");
}

function ShowDropdowns(fromMethod)
{
    if (fromMethod == "LoadClients") {
        $('#ddlClient').attr("disabled", "disabled");
        $("#ddlProject option").remove();
        $('#ddlProject').attr("disabled", "disabled");
        $("#ddlTask option").remove();
        $('#ddlTask').attr("disabled", "disabled");
    }
    else if (fromMethod == "LoadProjects") {
        $('#ddlClient').removeAttr("disabled");
        $('#ddlProject').attr("disabled", "disabled");
        $("#ddlTask option").remove();
        $('#ddlTask').attr("disabled", "disabled");
    }
    else if (fromMethod == "LoadTasks") {
        $('#ddlClient').removeAttr("disabled");
        $('#ddlProject').removeAttr("disabled");

    }
    
    $('#taskDiv').show();
    $('#projectDiv').show();
    $('#dropdownloadingImage').hide();
    $("#dropdownActionMessage").text("");

}
function GetMeetingTimeDuration() {
    // To refresh validate object on each save click and clear all errors
    validateData.isvalid = true;
    $("#meetingTotalHours").text("");

    var startTime = moment($("#meetingStartTime").val(), "LT").format("hh:mm A");
    var stopTime = moment($("#meetingEndTime").val(), "LT").format("hh:mm A");
    if (startTime != "Invalid date")
        $('#meetingStartTime').val(startTime);

    if (stopTime != "Invalid date")
        $('#meetingEndTime').val(stopTime);

    ValidateMeetingEntries($("#meetingStartTime").val(), $("#meetingEndTime").val());

    if (validateData.isvalid) {
        var totalHoursTime = (Math.round(moment(stopTime, 'hh:mm a').diff(moment(startTime, 'hh:mm a'), 'hours', true) * 100) / 100).toString();
        $("#meetingTotalHours").text(totalHoursTime + "h");
    }
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    
    return "";
}
