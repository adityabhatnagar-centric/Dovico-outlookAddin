using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using Dovico.OutlookTimeEntryAddin.Application;
using Dovico.OutlookTimeEntryAddin.Business;
using Dovico.OutlookTimeEntryAddin.WebAddInWeb.ViewModels;
using NLog;

namespace Dovico.OutlookTimeEntryAddin.WebAddInWeb
{
    /// <summary>
    /// This service is an interface to Outlook Addin Application layer
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]    
    [System.Web.Script.Services.ScriptService]  // To allow this Web Service to be called from script, using ASP.NET AJAX
    public class ServiceInterface : System.Web.Services.WebService
    {
        // For error logging
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Authenticate user and saves dataaccesstoken in cookie
        /// </summary>
        /// <param name="company"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string AuthenticateUser(string company, string username, string password)
        {
            string output = string.Empty;

            try
            {
                // Call AuthenticateUser() to authenticate the user and return User details
                AuthenticateLogic authenticateLogic = new AuthenticateLogic();
                UserBO userBO = authenticateLogic.AuthenticateUser(company, username, password);
                if (userBO != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(userBO);
                    // Save Consumer Access Token in Cookie
                    SaveConsumerAccessToken(userBO.DataAccessToken);
                    //Get API Version
                    GetAPIInfo();
                }
                else
                {
                    // Create error object
                    output = ErrorOutputObject(Resources.Website.UnAuthorized);
                }
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.UnAuthorized);
            }

            return output;
        }

        /// <summary>
        /// Checks if the user is already authenticated
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        public string IsAuthenticated()
        {
            string output = string.Empty;
            string apiVersion = string.Empty;
            try
            {
                bool isValidUser = true;

                // Fetch dataaccesstoken from cookie
                string dataAccessToken = GetConsumerAccessToken();
                apiVersion = GetAPIVersion();
                if (!String.IsNullOrEmpty(dataAccessToken.Trim()))
                {
                    // Validate dataaccesstoken and return employee details
                    EmployeeBO employee = ValidateUser(dataAccessToken,apiVersion);
                    if (employee != null)
                    {
                        // Check for errors in API layer
                        CheckErrorsInAPI(employee);
                        //Get API Version
                        GetAPIInfo();
                    }
                    else
                    {
                        isValidUser = false;
                    }
                }
                else
                {
                    isValidUser = false;
                }

                var jsonSerialiser = new JavaScriptSerializer();
                output = jsonSerialiser.Serialize(isValidUser);
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.UnAuthorized);
            }

            return output;
        }

        /// <summary>
        /// Get user details
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        public string GetUserDetails()
        {
            string output = string.Empty;
            string apiVersion = string.Empty;
            try
            {

                apiVersion = GetAPIVersion();
                EmployeeBO employee = new EmployeeBO();
            
                // Fetch dataaccesstoken from cookie
                string dataAccessToken = GetConsumerAccessToken();

                if (!String.IsNullOrEmpty(dataAccessToken.Trim()))
                {
                    // Validate dataaccesstoken and return employee details
                    employee = ValidateUser(dataAccessToken,apiVersion);
                    if (employee != null)
                    {
                        // Check for errors in API layer
                        CheckErrorsInAPI(employee);
                    }
                }
                else
                {
                    return null;
                }

                var jsonSerialiser = new JavaScriptSerializer();
                output = jsonSerialiser.Serialize(employee);
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingUserDetails);
            }

            return output;
        }

        /// <summary>
        /// Get employee options data
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        public string GetEmployeeOptions()
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            try
            {
                apiVersion = GetAPIVersion();
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                EmployeeOptionsVM employeeOptions = new EmployeeOptionsVM();

                // Calls GetEmployeeOptionsData method to fetch employee options
                EmployeeOptionsBO employeeOptionsBO = timeEntryLogic.GetEmployeeOptionsData(dataAccessToken,apiVersion);

                if (employeeOptionsBO != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(employeeOptionsBO);

                    // Create EmployeeOptions object
                    employeeOptions.ShowBillable = employeeOptionsBO.ShowBillable;

                    var jsonSerialiser = new JavaScriptSerializer();
                    output = jsonSerialiser.Serialize(employeeOptions);
                }
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingEmployeeOptionsData);
            }

            return output;
        }

        /// <summary>
        /// Get details for week based on start and end dates
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>string</returns>
        [WebMethod]
        public string GetTimeEntriesWeekHoursDetails(string start, string end)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);

            try
            {
                apiVersion = GetAPIVersion();
                // Call GetTimeEntriesWeekHoursDetails() to fetch details for the week
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                TimeEntryWeekHoursDetailsBO weekHoursDetailsBO = timeEntryLogic.GetTimeEntriesWeekHoursDetails(dataAccessToken, apiVersion, Convert.ToInt32(emp.ID), startDate, endDate);
                TimeEntryWeekHoursDetailsVM weekHoursDetailsVM = new TimeEntryWeekHoursDetailsVM();

                if (weekHoursDetailsBO != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(weekHoursDetailsBO);

                    // Fill TimeEntryWeekHoursDetailsVM object
                    weekHoursDetailsVM.WeekHoursTotal = weekHoursDetailsBO.WeekHoursTotal;
                    weekHoursDetailsVM.IsSubmitted = weekHoursDetailsBO.IsSubmitted;

                    var jsonSerialiser = new JavaScriptSerializer();
                    output = jsonSerialiser.Serialize(weekHoursDetailsVM);
                }
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingTimeEntriesWeekHoursDetailsData);
            }

            return output;
        }

        /// <summary>
        /// Get hours data based on start and end dates
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>string</returns>
        [WebMethod]
        public string GetTimeEntriesDailyTotal(string start, string end)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);

            try
            {
                apiVersion = GetAPIVersion();
                // Call GetTimeEntriesDailyTotal() to fetch hours for the week
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                TimeEntryDailyHoursBO dailyHoursDataBO = timeEntryLogic.GetTimeEntriesDailyTotal(dataAccessToken, apiVersion, Convert.ToInt32(emp.ID), startDate, endDate);
                TimeEntryDailyHoursVM dailyHoursVM = new TimeEntryDailyHoursVM();
                dailyHoursVM.DailyHoursList = new List<TimeEntryDailyHoursDetailsVM>();
                
                if (dailyHoursDataBO != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(dailyHoursDataBO);

                    // Fill TimeEntryDailyHoursVM object
                    foreach (var dailyHours in dailyHoursDataBO.DailyHoursList)
                    {
                        TimeEntryDailyHoursDetailsVM dailyhourDetails = new TimeEntryDailyHoursDetailsVM { Date = dailyHours.Key, Hours = dailyHours.Value };
                        dailyHoursVM.DailyHoursList.Add(dailyhourDetails);
                    }

                    var jsonSerialiser = new JavaScriptSerializer();
                    output = jsonSerialiser.Serialize(dailyHoursVM);
                }
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingTimeEntriesDailyTotalData);
            }

            return output;
        }

        /// <summary>
        /// Get data for Client dropdowns
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        public string GetClients()
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            try
            {
                apiVersion = GetAPIVersion();
                // Call GetClients() to fetch data for Clients
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                AssignmentListBO clientBO = timeEntryLogic.GetClients(dataAccessToken, apiVersion,Convert.ToInt16(emp.ID));

                List<AssignmentVM> assignments = new List<AssignmentVM>();

                if (clientBO != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(clientBO);

                    foreach (AssignmentBO client in clientBO.Assignments)
                    {
                        AssignmentVM newClient = new AssignmentVM { ItemID = client.ItemID, Name = client.Name, GetAssignmentsURI = client.GetAssignmentsURI };
                        assignments.Add(newClient);
                    }

                    var jsonSerialiser = new JavaScriptSerializer();
                    output = jsonSerialiser.Serialize(assignments);
                }
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingClientData);
            }

            return output;
        }

        /// <summary>
        /// Get data for Projects dropdowns
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetProjects(AssignmentVM client)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            try
            {
                // Map AssignmentBO from AssignmentVM
                if(client != null)
                {

                    apiVersion = GetAPIVersion();
                    AssignmentBO assignmentBO = new AssignmentBO() { ItemID = client.ItemID, Name = client.Name, GetAssignmentsURI = client.GetAssignmentsURI };

                    // Call GetProjects() to fetch data for Projects
                    TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                    AssignmentListBO projectsBO = timeEntryLogic.GetProjects(dataAccessToken, apiVersion ,Convert.ToInt16(emp.ID), assignmentBO);

                    List<AssignmentVM> assignments = new List<AssignmentVM>();

                    if (projectsBO != null)
                    {
                        // Check for errors in API layer
                        CheckErrorsInAPI(projectsBO);

                        foreach (AssignmentBO project in projectsBO.Assignments)
                        {
                            AssignmentVM newProject = new AssignmentVM { ItemID = project.ItemID, Name = project.Name, GetAssignmentsURI = project.GetAssignmentsURI };
                            assignments.Add(newProject);
                        }

                        var jsonSerialiser = new JavaScriptSerializer();
                        output = jsonSerialiser.Serialize(assignments);
                    }
                }
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingProjectData);
            }

            return output;
        }

        /// <summary>
        /// Get data for Tasks dropdowns
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTask(AssignmentVM project)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            try
            {
                // Map AssignmentBO from AssignmentVM
                if (project != null)
                {

                    apiVersion = GetAPIVersion();
                    AssignmentBO assignmentBO = new AssignmentBO() { ItemID = project.ItemID, Name = project.Name, GetAssignmentsURI = project.GetAssignmentsURI };

                    // Call GetTasks() to fetch data for Tasks
                    TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                    AssignmentListBO tasksBO = timeEntryLogic.GetTasks(dataAccessToken, apiVersion, assignmentBO);

                    List<AssignmentVM> assignments = new List<AssignmentVM>();

                    if (tasksBO != null)
                    {
                        // Check for errors in API layer
                        CheckErrorsInAPI(tasksBO);

                        foreach (AssignmentBO task in tasksBO.Assignments)
                        {
                            AssignmentVM newTask = new AssignmentVM { ItemID = task.ItemID, Name = task.Name, GetAssignmentsURI = task.GetAssignmentsURI, TimeBillableByDefault = task.TimeBillableByDefault };
                            assignments.Add(newTask);
                        }

                        var jsonSerialiser = new JavaScriptSerializer();
                        output = jsonSerialiser.Serialize(assignments);
                    }
                }
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingTaskData);
            }

            return output;
        }

        /// <summary>
        /// Get Custom Templates data for the Task
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        public string GetCustomTemplates(string taskId)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;
            List<CustomTemplateVM> customTemplates = new List<CustomTemplateVM>();

            try
            {

                apiVersion = GetAPIVersion();
                // Call GetCustomTemplates() to fetch Custom Templates data
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                CustomTemplateListBO customTemplateBO = timeEntryLogic.GetCustomTemplates(dataAccessToken, apiVersion, taskId);
                if (customTemplateBO != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(customTemplateBO);

                    // Create CustomTemplate object
                    foreach (CustomTemplateBO data in customTemplateBO.CustomTemplates)
                    {
                        // If the Custom Field is not-hidden
                        if (data.Hide == "F")
                        {
                            CustomTemplateVM customTemplate = new CustomTemplateVM();

                            customTemplate.Id = data.ID;
                            customTemplate.Name = data.Name.Trim();
                            customTemplate.Type = data.Type.ToString();
                            customTemplate.Required = data.Required;
                            customTemplate.Description = data.Description;
                            customTemplate.Hide = data.Hide;

                            customTemplate.Values = new List<ViewModels.ValueItems>();
                            if(data.Type == CustomTemplateType.X)
                            {
                                customTemplate.Values.Add(new ViewModels.ValueItems { Id = "-1", Value ="[None]" , Default = "F" });
                            }
                            foreach (Business.ValueItems valueItemsData in data.Values)
                            {
                                ViewModels.ValueItems valueItem = new ViewModels.ValueItems();
                                valueItem.Id = valueItemsData.ID;
                                valueItem.Value = valueItemsData.Value;
                                valueItem.Default = valueItemsData.Default;
                                customTemplate.Values.Add(valueItem);
                            }
                            customTemplates.Add(customTemplate);
                        }
                    }
                }

                var jsonSerialiser = new JavaScriptSerializer();
                output = jsonSerialiser.Serialize(customTemplates);
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingCustomTemplates);
            }

            return output;
        }

        /// <summary>
        /// Returns data for TimeSheet screen where we show all the TimeEntries for specific start and end dates
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>string</returns>
        [WebMethod]
        public string GetTimeEntries(string start, string end)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null) { 
                return null;
            }

            string output = string.Empty;

            DateTime startDate = Convert.ToDateTime(start);
            DateTime endDate = Convert.ToDateTime(end);

            try
            {
                List<TimeEntryVM> timeEntries = new List<TimeEntryVM>();

                apiVersion = GetAPIVersion();
                // Call GetTimeEntries() to fetch time entries for specific start and end dates
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                TimeEntryListBO timeEntriesBO = timeEntryLogic.GetTimeEntries(dataAccessToken, apiVersion, Convert.ToInt32(emp.ID), startDate, endDate);

                if (timeEntriesBO != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(timeEntriesBO);

                    // Create TimeEntry object
                    foreach (TimeEntryBO timeEntryBO in timeEntriesBO.timeEntries)
                    {
                        TimeEntryVM timeEntry = new TimeEntryVM();

                        timeEntry.TimeEntryId = timeEntryBO.ID;
                        timeEntry.BillableHours = timeEntryBO.TotalHours;
                        timeEntry.IsBillable = timeEntryBO.Billable;
                        timeEntry.ClientName = timeEntryBO.Client.Name;
                        timeEntry.ProjectName = timeEntryBO.Project.Name;
                        timeEntry.TaskName = timeEntryBO.Task.Name;

                        timeEntry.Description = timeEntryBO.Description;

                        //A (approved), cant delete and edit
                        //N (not submitted),  = can delete and edit
                        //U (under review), cant delete and edit
                        //R (rejected) : can delete and edit
                        switch (timeEntryBO.Sheet.Status)
                        {
                            case "A":
                                timeEntry.DeleteAllowed = false;
                                timeEntry.EditAllowed = false;
                                timeEntry.SheetStatus = "Approved";
                            break;

                            case "N":
                                timeEntry.DeleteAllowed = true;
                                timeEntry.EditAllowed = true;
                                timeEntry.SheetStatus = "Not-Submitted";
                                break;

                            case "U":
                                timeEntry.DeleteAllowed = false;
                                timeEntry.EditAllowed = false;
                                timeEntry.SheetStatus = "Under-Review";
                                break;

                            case "R":
                                timeEntry.DeleteAllowed = true;
                                timeEntry.EditAllowed = true;
                                timeEntry.SheetStatus = "Rejected";
                                break;
                        }

                        timeEntry.CustomFields = new List<CustomFieldVM>();

                        // Create Custom fields
                        foreach (CustomFieldBO customField in timeEntryBO.CustomFields)
                        {
                            CustomFieldVM customTemplate = new CustomFieldVM();

                            customTemplate.Id = customField.ID;
                            customTemplate.TemplateId = customField.TemplateID;
                            customTemplate.Name = customField.Name;

                            customTemplate.Values = new List<string>();

                            foreach (string valueItemsData in customField.Values)
                            {
                                customTemplate.Values.Add(valueItemsData);
                            }
                            if(customTemplate.Values.Count> 0)
                               timeEntry.CustomFields.Add(customTemplate);
                        }

                        timeEntries.Add(timeEntry);
                    }

                    var jsonSerialiser = new JavaScriptSerializer();
                    output = jsonSerialiser.Serialize(timeEntries);
                }
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingTimeEntries);
            }

            return output;
        }

        /// <summary>
        /// Returns data for Single Time Entry for Edit functionality
        /// </summary>
        /// <returns>string</returns>
        [WebMethod]
        public string GetSingleTimeEntry(string timeEntryId)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            try
            {
                List<TimeEntryVM> timeEntries = new List<TimeEntryVM>();
                apiVersion = GetAPIVersion();
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                TimeEntryListBO timeEntriesBO = timeEntryLogic.GetSingleTimeEntry(dataAccessToken, apiVersion ,timeEntryId);

                if (timeEntriesBO != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(timeEntriesBO);

                    // Create TimeEntry object
                    foreach (TimeEntryBO timeEntryBO in timeEntriesBO.timeEntries)
                    {
                        TimeEntryVM timeEntry = new TimeEntryVM();

                        timeEntry.TimeEntryId = timeEntryBO.ID;
                        timeEntry.ClientId = timeEntryBO.Client.ID == "0" ? "-1" : timeEntryBO.Client.ID ;
                        timeEntry.ProjectId = timeEntryBO.Project.ID;
                        timeEntry.TaskId = timeEntryBO.Task.ID;
                        timeEntry.Description = timeEntryBO.Description;
                        timeEntry.BillableHours = timeEntryBO.TotalHours;
                        timeEntry.IsBillable = timeEntryBO.Billable;
                        timeEntry.StartTime = timeEntryBO.StartTime;
                        timeEntry.StopTime = timeEntryBO.StopTime;

                        timeEntry.CustomFields = new List<CustomFieldVM>();

                        // Create Custom fields
                        foreach (CustomFieldBO customField in timeEntryBO.CustomFields)
                        {
                            CustomFieldVM customTemplate = new CustomFieldVM();

                            customTemplate.Id = customField.ID;
                            customTemplate.TemplateId = customField.TemplateID;
                            customTemplate.Name = customField.Name;

                            customTemplate.Values = new List<string>();

                            foreach (string valueItemsData in customField.Values)
                            {
                                customTemplate.Values.Add(valueItemsData);
                            }
                            timeEntry.CustomFields.Add(customTemplate);
                        }

                        timeEntries.Add(timeEntry);
                    }

                    // Create json object
                    var jsonSerialiser = new JavaScriptSerializer();
                    output = jsonSerialiser.Serialize(timeEntries);
                }                
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorFetchingTimeEntries);
            }

            return output;
        }

        /// <summary>
        /// Delete Time Entry based on timeentryID
        /// </summary>
        /// <param name="timeEntryId"></param>
        /// <returns>string</returns>
        [WebMethod]
        public string DeleteTimeEntry(string timeEntryId)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            try
            {

                apiVersion = GetAPIVersion();
                // Call DeleteTimeEntry() to delete the time entry based on timeentryID
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                BaseBO resultDeleteEntry = timeEntryLogic.DeleteTimeEntry(dataAccessToken, apiVersion ,timeEntryId);

                if (resultDeleteEntry != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(resultDeleteEntry);
                }
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorDeletingTimeEntry);
            }

            return output;
        }

        /// <summary>
        /// Save Time Entry
        /// </summary>
        /// <param name="timeEntry"></param>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveTimeEntry(TimeEntryVM timeEntry)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            try
            {
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                TimeEntrySubmissionBO timeEntryDetails = null;
                string returnValue = string.Empty;
                apiVersion = GetAPIVersion();
                try
                {
                    // Create TimeEntrySubmissionBO object to hold submitted data and throw any format exception that occurs
                    timeEntryDetails = new TimeEntrySubmissionBO()
                    {
                        TimeEntryId = timeEntry.TimeEntryId,
                        EmployeeId = Convert.ToInt32(emp.ID),
                        ClientId = timeEntry.ClientId,
                        ProjectId = timeEntry.ProjectId,
                        TaskId = timeEntry.TaskId,
                        StartDate = Convert.ToDateTime(timeEntry.StartDate),
                        EndDate = Convert.ToDateTime(timeEntry.EndDate),
                        Hours = Math.Round((Double)timeEntry.Hours, 2),
                        Description = timeEntry.Description,
                        Billable = Convert.ToBoolean(timeEntry.IsBillableHours),
                        CalendarEntry = Convert.ToBoolean(timeEntry.FromCalendar),
                        StartTime = timeEntry.StartTime,
                        StopTime = timeEntry.StopTime
                    };
                }
                catch
                {
                    throw new FormatException();
                }

                // Add for Custom Fields
                if (timeEntry.CustomFields != null && timeEntryDetails != null)
                {
                    // Create CustonFields object to hold custom field values
                    timeEntryDetails.CustomFields = new List<CustomFieldSubmissionBO>();

                    // Loop thru the Custom Fields provided by user
                    foreach (CustomFieldVM customField in timeEntry.CustomFields)
                    {
                        // Create CustomFieldSubmissionBO object to hold custom field
                        CustomFieldSubmissionBO customFieldsBO = new CustomFieldSubmissionBO();
                        if(!string.IsNullOrEmpty(customField.Id))
                            customFieldsBO.ID = customField.Id;

                        customFieldsBO.TemplateID = customField.TemplateId;

                        // If values are provided by user
                        if (customField.Values != null && customField.Values.Count > 0)
                        {
                            // Create Values object and loop thru the Values provided by user
                            customFieldsBO.Values = new List<ValueItemsSubmission>();
                            foreach (string value in customField.Values)
                            {
                                // Create ValueItemsSubmission to hold Value
                                ValueItemsSubmission customFieldBOValue = new ValueItemsSubmission()
                                {
                                    Value = value == "[None]"? "": value 
                                };

                                // Add the ValueItemsSubmission object in customFieldsBO.Values
                                customFieldsBO.Values.Add(customFieldBOValue);
                            }
                        }

                        // Add the CustomFieldSubmissionBO object in timeEntryDetails.CustomFields
                        timeEntryDetails.CustomFields.Add(customFieldsBO);
                    }
                }

                // Call SaveTimeEntry method for saving data
                TimeEntryListBO timeEntries = timeEntryLogic.SaveTimeEntry(dataAccessToken, apiVersion ,timeEntryDetails);
                if (timeEntries != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(timeEntries);
                }
            }
            catch (FormatException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.DataInIncorrectFormat);
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorSavingTimeEntry);
            }

            return output;
        }

        /// <summary>
        /// Submit Time Entry
        /// </summary>
        /// <param name="startDateOfWeek"></param>
        /// <returns>string</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SubmitWeekTimeEntry(string startDateOfWeek)
        {
            // Authentication Check and return Employee details. If no Employee details then return null with 403 error
            string dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            EmployeeBO emp = IsValidUser(out dataAccessToken);
            if (emp == null)
            {
                return null;
            }

            string output = string.Empty;

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            try
            {

                apiVersion = GetAPIVersion();
                try
                {
                    // Get start date and end date
                    startDate = Convert.ToDateTime(startDateOfWeek);
                    endDate = startDate.AddDays(6);
                }
                catch
                {
                    throw new FormatException();
                }

                // Calls SubmitWeekTimeEntryForApproval method for submitting data
                TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
                TimeEntryListBO timeEntries = timeEntryLogic.SubmitWeekTimeEntryForApproval(dataAccessToken, apiVersion ,Convert.ToInt32(emp.ID), startDate, endDate);
                if (timeEntries != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(timeEntries);
                }
            }
            catch (FormatException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.DataInIncorrectFormat);
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.ErrorSubmittingTimeEntry);
            }

            return output;
        }

        /// <summary>
        /// Remove cookies during LogOff
        /// </summary>
        [WebMethod]
        public void LogOff()
        {
            // Fetch and remove cookies during LogOff
            HttpCookie dovicoCookieId = HttpContext.Current.Request.Cookies["DovicoId"];
            HttpContext.Current.Response.Cookies.Remove("DovicoId");
            dovicoCookieId.Expires = DateTime.Now.AddDays(-10);
            dovicoCookieId.Value = null;
            HttpContext.Current.Response.SetCookie(dovicoCookieId);

            HttpCookie dovicoCookieApi = HttpContext.Current.Request.Cookies["APIVersion"];
            HttpContext.Current.Response.Cookies.Remove("APIVersion");
            dovicoCookieApi.Expires = DateTime.Now.AddDays(-10);
            dovicoCookieApi.Value = null;
            HttpContext.Current.Response.SetCookie(dovicoCookieApi);
        }

        #region Private Methods

        /// <summary>
        /// Check if the user is valid or not
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <returns>EmployeeBO</returns>
        private EmployeeBO IsValidUser(out string dataAccessToken)
        {
            dataAccessToken = string.Empty;
            string apiVersion = string.Empty;
            try
            {

                apiVersion = GetAPIVersion();
                // Fetch dataaccesstoken from cookie. Validate user using dataaccesstoken and return employee details
                dataAccessToken = GetConsumerAccessToken();
                EmployeeBO employee = ValidateUser(dataAccessToken,apiVersion);

                if (employee != null)
                {
                    // Check for errors in API layer
                    CheckErrorsInAPI(employee);
                }
                else
                {
                    // Return 403 with null if not valid employee
                    Context.Response.StatusCode = 403;
                    Context.Response.StatusDescription = "Forbidden";
                    Context.ApplicationInstance.CompleteRequest();

                    return null;
                }

                return employee;
            }
            catch (ApplicationException e)
            {
                // Return 403 with null if application exception comes
                logger.Log(LogLevel.Error, e);
                Context.Response.StatusCode = 403;
                Context.Response.StatusDescription = "Forbidden";
                Context.ApplicationInstance.CompleteRequest();

                return null;
            }
            catch (Exception e)
            {
                // Return 403 with null if some exception comes
                logger.Log(LogLevel.Error, e);
                Context.Response.StatusCode = 403;
                Context.Response.StatusDescription = "Forbidden";
                Context.ApplicationInstance.CompleteRequest();

                return null;
            }
        }

        /// <summary>
        /// Save dataaccesstoken in cookie
        /// </summary>
        /// <param name="dataAccessToken"></param>
        private void SaveConsumerAccessToken(string dataAccessToken)
        {
            // Store DataAccessToken in DovicoId cookie with 30 days expiration
            HttpCookie dovicoCookie = new HttpCookie("DovicoId");

            dovicoCookie.Value = dataAccessToken;
            dovicoCookie.Expires = DateTime.Now.AddDays(30d);

            HttpContext.Current.Response.Cookies.Add(dovicoCookie);
        }

        /// <summary>
        /// Fetch dataaccesstoken from cookie
        /// </summary>
        /// <returns>string</returns>
        private string GetConsumerAccessToken()
        {
            HttpCookie dovicoCookie = HttpContext.Current.Request.Cookies["DovicoId"];
            if (dovicoCookie != null)
            {
                return dovicoCookie.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Validate user by passing dataaccesstoken and returning Employee details
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <returns>EmployeeBO</returns>
        private EmployeeBO ValidateUser(string dataAccessToken,string apiVersion)
        {
            // Call GetEmployeeData() to return Employee details
            TimeEntryLogic timeEntryLogic = new TimeEntryLogic();
            EmployeeBO employee = timeEntryLogic.GetEmployeeData(dataAccessToken,apiVersion);

            return employee;
        }

        /// <summary>
        /// Checks if there is error during API call
        /// </summary>
        /// <param name="input"></param>
        private void CheckErrorsInAPI(BaseBO input)
        {
            // If error during API call then throw AppException exception
            if (!String.IsNullOrEmpty(input.ErrorMessage))
            {
                //Context.Response.TrySkipIisCustomErrors = true;
                throw new AppException(input.ErrorMessage);
            }
        }

        /// <summary>
        /// Create error object
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns>string</returns>
        private string ErrorOutputObject(string errorMessage)
        {
            // Create BaseVM object with 500 error
            BaseVM baseVM = new BaseVM();
            baseVM.ErrorMessage = errorMessage;
            baseVM.StatusCode = 500;

            // Create json object
            var jsonSerialiser = new JavaScriptSerializer();
            return jsonSerialiser.Serialize(baseVM);
        }

        /// <summary>
        /// Fetch API version from cookie
        /// </summary>
        /// <returns>string</returns>
        private string GetAPIVersion()
        {
            HttpCookie dovicoCookie = HttpContext.Current.Request.Cookies["APIVersion"];
            if (dovicoCookie != null)
            {
                return dovicoCookie.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Save API Version in cookie
        /// </summary>
        /// <param name="dataAccessToken"></param>
        private void SaveAPIVersion(string version)
        {
            // Store DataAccessToken in DovicoId cookie with 30 days expiration
            HttpCookie dovicoCookie = new HttpCookie("APIVersion");

            dovicoCookie.Value = version;
            dovicoCookie.Expires = DateTime.Now.AddDays(30d);

            HttpContext.Current.Response.Cookies.Add(dovicoCookie);
        }

        /// <summary>
        /// Get API version from dataAccessTOken
        /// </summary>
        /// <param name="targetAPIVersion"></param>
        /// <returns>EmployeeBO</returns>
        private void GetAPIInfo()
        {
            string output = string.Empty;
            //string dataAccessToken = "b641fc6346334cccaa938a860712d8b9.42129";
            string dataAccessToken = GetConsumerAccessToken();
            try
            {
                // Call AuthenticateUser() to authenticate the user and return User details
                AuthenticateLogic authenticateLogic = new AuthenticateLogic();
                var version = authenticateLogic.GetAPIVersion(dataAccessToken);

                SaveAPIVersion(version);
            }
            catch (AppException e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(e.Message);
            }
            catch (Exception e)
            {
                // Create error object
                logger.Log(LogLevel.Error, e);
                output = ErrorOutputObject(Resources.Website.UnAuthorized);
            }
        }
        #endregion

    }
}