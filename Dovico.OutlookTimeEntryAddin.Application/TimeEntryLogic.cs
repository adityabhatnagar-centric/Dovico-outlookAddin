using Dovico.OutlookTimeEntryAddin.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using Dovico.OutlookTimeEntryAddin.Integration;
using NLog;
using Dovico.OutlookTimeEntryAddin.Common;

namespace Dovico.OutlookTimeEntryAddin.Application
{
    /// <summary>
    /// This class contains Time Entry specific logic
    /// </summary>
    public class TimeEntryLogic
    {
        // For error logging
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the employee data
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <returns>EmployeeBO</returns>
        public EmployeeBO GetEmployeeData(string dataAccessToken, string apiVersion)
        {
            TimeEntryService integrationService = new TimeEntryService();
            EmployeeBO employee = new EmployeeBO();

            try
            {
                // Calls GetEmployeeData of Integration layer to fetch employee details
                employee = integrationService.GetEmployeeData(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), apiVersion);
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                employee.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                employee.ErrorMessage = "Error Fetching Employee Data.";
            }

            return employee;
        }

        /// <summary>
        /// Gets the employee options data
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <returns>EmployeeOptionsBO</returns>
        public EmployeeOptionsBO GetEmployeeOptionsData(string dataAccessToken, string apiVersion)
        {
            TimeEntryService integrationService = new TimeEntryService();
            EmployeeOptionsBO employeeOptions = new EmployeeOptionsBO();

            try
            {
                // Calls ShowEmployeeBillable of Integration layer to fetch employee options
                employeeOptions.ShowBillable = integrationService.ShowEmployeeBillable(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), apiVersion);
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                employeeOptions.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                employeeOptions.ErrorMessage = "Error Fetching Employee Options Data.";
            }

            return employeeOptions;
        }

        /// <summary>
        /// Gets Time Entries week details for employee
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="employeeId"></param>
        /// <param name="startdate"></param>
        /// <param name="endDate"></param>
        /// <returns>TimeEntryWeekHoursDetailsBO</returns>
        public TimeEntryWeekHoursDetailsBO GetTimeEntriesWeekHoursDetails(string dataAccessToken, string apiVersion, int employeeId, DateTime startdate, DateTime? endDate = null)
        {
            TimeEntryWeekHoursDetailsBO weekHoursDetailsData = new TimeEntryWeekHoursDetailsBO();

            try
            {
                // Calls GetTimeEntries of Integration layer to fetch time entries data
                TimeEntryService integrationService = new TimeEntryService();
                TimeEntryListBO timeEntriesData = new TimeEntryListBO();
                timeEntriesData.timeEntries = integrationService.GetTimeEntries(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), apiVersion, employeeId,
                                                                    startdate, endDate);

                // Loop thru the days and fill week total
                weekHoursDetailsData.IsSubmitted = true;
                weekHoursDetailsData.WeekHoursTotal = timeEntriesData.timeEntries.Sum(x => System.Double.Parse(x.TotalHours)).ToString("0.##");
                while (startdate <= endDate)
                {
                    // Check the status of sheet. If "N" then set Submitted time to false
                    var sheetStatusForNotSubmitted = timeEntriesData.timeEntries.Where(x => System.DateTime.Parse(x.Date) == startdate).Any(x => x.Sheet.Status.Equals("N"));
                    if (sheetStatusForNotSubmitted)
                        weekHoursDetailsData.IsSubmitted = false;

                    // Check the status of sheet. If "R" then set Submitted time to false
                    var sheetStatusForRejected = timeEntriesData.timeEntries.Where(x => System.DateTime.Parse(x.Date) == startdate).Any(x => x.Sheet.Status.Equals("R"));
                    if (sheetStatusForRejected)
                        weekHoursDetailsData.IsSubmitted = false;

                    startdate = startdate.AddDays(1);
                }
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                weekHoursDetailsData.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                weekHoursDetailsData.ErrorMessage = "Error Fetching Time Entries Week Total Data.";
            }

            return weekHoursDetailsData;
        }

        /// <summary>
        /// Gets Time Entries daily total for employee
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="employeeId"></param>
        /// <param name="startdate"></param>
        /// <param name="endDate"></param>
        /// <returns>TimeEntryDailyHoursBO</returns>
        public TimeEntryDailyHoursBO GetTimeEntriesDailyTotal(string dataAccessToken, string apiVersion, int employeeId, DateTime startdate, DateTime? endDate = null)
        {
            TimeEntryDailyHoursBO dailyHoursData = new TimeEntryDailyHoursBO();

            try
            {
                // Calls GetTimeEntries of Integration layer to fetch time entries data
                TimeEntryService integrationService = new TimeEntryService();
                TimeEntryListBO timeEntriesData = new TimeEntryListBO();
                timeEntriesData.timeEntries = integrationService.GetTimeEntries(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), apiVersion, employeeId,
                                                                    startdate, endDate);

                // Loop thru the days and fill daily total
                dailyHoursData.DailyHoursList = new Dictionary<string, string>();
                while (startdate <= endDate)
                {
                    // Add hours
                    double hours = timeEntriesData.timeEntries.Where(x => System.DateTime.Parse(x.Date) == startdate).Sum(x => System.Double.Parse(x.TotalHours));
                    dailyHoursData.DailyHoursList.Add(Convert.ToString(startdate), Convert.ToString(hours));

                    startdate = startdate.AddDays(1);
                }
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                dailyHoursData.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                dailyHoursData.ErrorMessage = "Error Fetching Time Entries Daily Total Data.";
            }

            return dailyHoursData;
        }
        
        /// <summary>
        /// Gets data for Clients
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="employeeId"></param>
        /// <returns>AssignmentListBO</returns>
        public AssignmentListBO GetClients(string dataAccessToken, string apiVersion, int employeeId)
        {
            AssignmentListBO outputClients = new AssignmentListBO();
            TimeEntryService integrationService = new TimeEntryService();

            try
            {
                // Fetch Assignments available to Employee
                IList<AssignmentBO> assignments = integrationService.GetAssignments(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken),
                                                                                    apiVersion, employeeId);

                // Take out Clients from all Assignments
                var clientListTemp = assignments.Where(client => client.AssignmentID.StartsWith("C")).Select(p => p);
                var clientList = clientListTemp as IList<AssignmentBO> ?? clientListTemp.ToList();
                outputClients.Assignments = clientList.Any() ? clientList.ToList() : new List<AssignmentBO>();

                // Check for extra Assignments that are not Clients and add [None] client to tackle those
                var remainingAssignments = assignments.Except((IList<AssignmentBO>)outputClients.Assignments);
                if (remainingAssignments.Count() > 0)
                {
                    // Add [None]
                    AssignmentBO client = new AssignmentBO { AssignmentID = "0", ItemID = "-1", Name = "[None]" };
                    outputClients.Assignments.Add(client);
                }

                // Call GetClientsVisible method to fetch valid Clients to show
                //outputClients.Assignments = GetVisibleClients(dataAccessToken, outputClients.Assignments);
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                outputClients.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                outputClients.ErrorMessage = "Error Fetching Clients.";
            }

            return outputClients;
        }

        /// <summary>
        /// Get Projects under Client
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="client"></param>
        /// <param name="employeeAssignments"></param>
        /// <returns>AssignmentListBO</returns>
        public AssignmentListBO GetProjects(string dataAccessToken, string apiVersion, int employeeId, AssignmentBO client)
        {
            AssignmentListBO outputProjects = new AssignmentListBO();
            TimeEntryService integrationService = new TimeEntryService();

            try
            { 
                if (client != null)
                {
                    if (client.Name.Equals(Constants.NONE_TEXT))
                    {
                        // Fetch Assignments available to Employee
                        IList<AssignmentBO> assignments = integrationService.GetAssignments(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken),
                                                                                            apiVersion, employeeId);

                        // Take out Projects from all Assignments
                        var projectListTemp = assignments.Where(project => !project.AssignmentID.StartsWith("C")).Select(project => project);
                        var projectList = projectListTemp as IList<AssignmentBO> ?? projectListTemp.ToList();
                        outputProjects.Assignments = projectList.Any() ? projectList.ToList() : new List<AssignmentBO>();
                    }
                    else
                    {
                        // Calls GetAssignments of Integration layer to fetch assignments
                        outputProjects.Assignments = integrationService.GetAssignments(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), apiVersion,
                                                                            client.GetAssignmentsURI).ToList();
                    }

                    // Call GetProjectsVisible method to fetch valid Projects to show
                    //outputProjects.Assignments = GetVisibleProjects(dataAccessToken, outputProjects.Assignments);
                }
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                outputProjects.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                outputProjects.ErrorMessage = "Error Fetching Projects.";
            }

            return outputProjects;
        }

        /// <summary>
        /// Get Tasks under Project
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="project"></param>
        /// <returns>AssignmentListBO</returns>
        public AssignmentListBO GetTasks(string dataAccessToken, string apiVersion, AssignmentBO project)
        {
            AssignmentListBO outputTasks = new AssignmentListBO();
            TimeEntryService integrationService = new TimeEntryService();

            try
                { 
                if (project != null)
                {
                    // Fetch Tasks for Project
                    var projectRelatedAssignments = integrationService.GetAssignments(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken),
                                                                                        apiVersion, project.GetAssignmentsURI);
                    outputTasks.Assignments = new List<AssignmentBO>();
                    foreach (AssignmentBO assignment in projectRelatedAssignments)
                    {
                        // If Assignment is TaskGroup
                        if (assignment.AssignmentID.StartsWith("G"))
                        {
                            // Fetch Assignments for TaskGroup
                            var taskRelatedAssignment = integrationService.GetAssignments(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken),
                                                                                            apiVersion, assignment.GetAssignmentsURI);
                            taskRelatedAssignment.ToList().ForEach(outputTasks.Assignments.Add);
                        }
                        else
                        {
                            outputTasks.Assignments.Add(assignment);
                        }
                    }

                    // Call GetVisibleTasks method to fetch valid Tasks to show
                    outputTasks.Assignments = GetVisibleTasks(dataAccessToken,apiVersion ,outputTasks.Assignments);
                }
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                outputTasks.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                outputTasks.ErrorMessage = "Error Fetching Tasks.";
            }

            return outputTasks;
        }

        /// <summary>
        /// Get Time Entries for employee
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="employeeId"></param>
        /// <param name="startdate"></param>
        /// <param name="endDate"></param>
        /// <returns>TimeEntryListBO</returns>
        public TimeEntryListBO GetTimeEntries(string dataAccessToken, string apiVersion, int employeeId, DateTime startdate, DateTime? endDate = null)
        {
            TimeEntryListBO outputTimeEntries = new TimeEntryListBO();

            try
            {
                // Calls GetTimeEntries of Integration layer to fetch Time Entries
                TimeEntryService integrationService = new TimeEntryService();
                outputTimeEntries.timeEntries = integrationService.GetTimeEntries(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), apiVersion, employeeId,
                                                                    startdate, endDate);
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                outputTimeEntries.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                outputTimeEntries.ErrorMessage = "Error Fetching Time Entries Data.";
            }
            
            return outputTimeEntries;
        }

        ///// <summary>
        ///// Get Custom Templates for specific Type passed
        ///// </summary>
        ///// <param name="dataAccessToken"></param>
        ///// <param name="type"></param>
        ///// <returns>CustomTemplateListBO</returns>
        //public CustomTemplateListBO GetCustomTemplates(string dataAccessToken, string type)
        //{
        //    CustomTemplateListBO outputCustomTemplates = new CustomTemplateListBO(); ;

        //    try
        //    {
        //        // Calls GetCustomTemplates of Integration layer to fetch custom templates for specific type 
        //        TimeEntryService integrationService = new TimeEntryService();
        //        outputCustomTemplates.CustomTemplates = integrationService.GetCustomTemplates(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), Constants.VERSION_5, type);
        //    }
        //    catch (DovicoException e)
        //    {
        //        logger.Log(LogLevel.Error, e);
        //        outputCustomTemplates.ErrorMessage = e.Message;
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Log(LogLevel.Error, e);
        //        outputCustomTemplates.ErrorMessage = "Error Fetching Custom Templates";
        //    }

        //    return outputCustomTemplates;
        //}

        /// <summary>
        /// Get Custom Templates for the Task
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="taskId"></param>
        /// <returns>CustomTemplateListBO</returns>
        public CustomTemplateListBO GetCustomTemplates(string dataAccessToken, string apiVersion, string taskId)
        {
            CustomTemplateListBO outputCustomTemplates = new CustomTemplateListBO(); ;

            try
            {
                // Calls GetCustomTemplates of Integration layer to fetch custom templates for the task 
                TimeEntryService integrationService = new TimeEntryService();
                outputCustomTemplates.CustomTemplates = integrationService.GetCustomTemplates(Constants.CONSUMER_SECRET_TOKEN, 
                                                                        UtilityService.DecryptedText(dataAccessToken), 
                                                                        apiVersion, taskId);
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                outputCustomTemplates.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                outputCustomTemplates.ErrorMessage = "Error Fetching Custom Templates";
            }

            return outputCustomTemplates;
        }

        /// <summary>
        /// Delete a  specific Time Entry
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="timeEntryId"></param>
        /// <returns>BaseBO</returns>
        public BaseBO DeleteTimeEntry(string dataAccessToken, string apiVersion, string timeEntryId)
        {
            BaseBO output = new BaseBO();

            try
            {
                // Calls DeleteTimeEntry of Integration layer to delete time entry for specific timeEntryId
                TimeEntryService integrationService = new TimeEntryService();
                integrationService.DeleteTimeEntry(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), apiVersion, timeEntryId);
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                output.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                output.ErrorMessage = "Time Entry cannot be deleted.";
            }

            return output;
        }

        /// <summary>
        /// Get Time Entry details for specific time entry
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="timeEntryId"></param>
        /// <returns>TimeEntryListBO</returns>
        public TimeEntryListBO GetSingleTimeEntry(string dataAccessToken, string apiVersion, string timeEntryId)
        {
            TimeEntryListBO outputTimeEntries = new TimeEntryListBO();

            try
            {
                // Calls GetSingleTimeEntry of Integration layer to fetch time entry details for a specific timeEntryId
                TimeEntryService integrationService = new TimeEntryService();
                outputTimeEntries.timeEntries = integrationService.GetSingleTimeEntry(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), apiVersion, timeEntryId);
                
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                outputTimeEntries.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                outputTimeEntries.ErrorMessage = "Error Fetching Time Entries Data.";
            }

            return outputTimeEntries;
        }

        /// <summary>
        /// Saves the Time Entry
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="timeEntrySubmissionDetails"></param>
        /// <returns>TimeEntryListBO</returns>
        public TimeEntryListBO SaveTimeEntry(string dataAccessToken, string apiVersion, TimeEntrySubmissionBO timeEntrySubmissionDetails)
        {
            ErrorBO output = null;
            TimeEntryListBO outputTimeEntries = new TimeEntryListBO();

            try
            {
                // Calls the Validations before submitting data
                output = SubmitValidation(dataAccessToken, apiVersion ,timeEntrySubmissionDetails);

                // If validations pass
                if (output != null && string.IsNullOrEmpty(output.Message))
                {
                    // Call SaveTimeEntry / UpdateTimeEntry of Integration layer based on timeentryId
                    TimeEntryService integrationService = new TimeEntryService();
                    if (String.IsNullOrEmpty(timeEntrySubmissionDetails.TimeEntryId))
                    {
                        outputTimeEntries.timeEntries = integrationService.SaveTimeEntry(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken),
                                                                                            apiVersion, timeEntrySubmissionDetails);
                    }
                    else
                    {
                        outputTimeEntries.timeEntries = integrationService.UpdateTimeEntry(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken),
                                                                                            apiVersion, timeEntrySubmissionDetails);
                    }
                }
                else
                {
                    outputTimeEntries.ErrorMessage = output.Message;
                }                
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                outputTimeEntries.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                outputTimeEntries.ErrorMessage = "Error Saving Time Entry.";
            }

            return outputTimeEntries;
        }

        /// <summary>
        /// Submits the Time Entries
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="employeeId"></param>
        /// <param name="startDateOfWeek"></param>
        /// <param name="endDateOfWeek"></param>
        /// <returns>TimeEntryListBO</returns>
        public TimeEntryListBO SubmitWeekTimeEntryForApproval(string dataAccessToken, string apiVersion, int employeeId, DateTime startDateOfWeek, DateTime endDateOfWeek)
        {
            TimeEntryListBO outputTimeEntries = new TimeEntryListBO();

            try
            {
                // Calls SubmitWeekTimeEntryForApproval of Integration layer to submit the time
                TimeEntryService integrationService = new TimeEntryService();
                outputTimeEntries.timeEntries = integrationService.SubmitWeekTimeEntryForApproval(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken),
                                                                                                    apiVersion, employeeId, startDateOfWeek, endDateOfWeek);
            }
            catch (DovicoException e)
            {
                logger.Log(LogLevel.Error, e);
                outputTimeEntries.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                outputTimeEntries.ErrorMessage = "Error Submitting Time Entries.";
            }

            return outputTimeEntries;
        }

        #region Private Methods
        
        /// <summary>
        /// Gets valid and visible Tasks
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="tasks"></param>
        /// <returns>IList<AssignmentBO></returns>
        private IList<AssignmentBO> GetVisibleTasks(string dataAccessToken, string apiVersion, IList<AssignmentBO> tasks)
        {
            IList<AssignmentBO> outputTasks = new List<AssignmentBO>();
            TimeEntryService integrationService = new TimeEntryService();

            if (tasks.Any())
            {
                foreach (AssignmentBO task in tasks)
                {
                    // If Task is TaskGroup then fetch Tasks for that TaskGroup
                    if (task.AssignmentID.StartsWith("G"))
                    {
                        // Fetch Assignments for TaskGroup
                        var taskRelatedAssignments = integrationService.GetAssignments(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken),
                                                                                        apiVersion, task.GetAssignmentsURI).ToList();
                        IList<AssignmentBO> taskList = taskRelatedAssignments.Any() ? taskRelatedAssignments.ToList() : new List<AssignmentBO>();
                        // Call visible Tasks for that TaskGroup
                        IList<AssignmentBO> visibleTasks = GetVisibleTasks(dataAccessToken, apiVersion, taskList);
                        ((List<AssignmentBO>)outputTasks).AddRange(visibleTasks);
                    }
                    else
                    {
                        // Fetch Assignments for Task
                        var taskRelatedAssignments = integrationService.GetAssignments(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken),
                                                                                        apiVersion, task.GetAssignmentsURI).ToList();
                        AssignmentBO tasksdetails = taskRelatedAssignments.Any() ? taskRelatedAssignments.SingleOrDefault() : new AssignmentBO();
                        // Add Task if not hidden
                        if (apiVersion == Constants.VERSION_5)
                        {
                            if (tasksdetails.Hide == "F")
                            {
                                task.TimeBillableByDefault = tasksdetails.TimeBillableByDefault;
                                outputTasks.Add(task);
                            }
                        }
                        else
                        {
                            task.TimeBillableByDefault = tasksdetails.TimeBillableByDefault;
                            outputTasks.Add(task);
                        }
                    }
                }
            }

            return outputTasks;
        }

        /// <summary>
        /// For Validating the time entry before saving
        /// </summary>
        /// <param name="dataAccessToken"></param>
        /// <param name="timeEntrySubmissionDetails"></param>
        /// <returns>ErrorBO</returns>
        private ErrorBO SubmitValidation(string dataAccessToken, string apiVersion, TimeEntrySubmissionBO timeEntrySubmissionDetails)
        {
            ErrorBO error = new ErrorBO();

            try
            {
                CustomTerminologyBO customTerminology = GetCustomTerminology(Constants.CONSUMER_SECRET_TOKEN, dataAccessToken, apiVersion);

                // Validate Clientid, ProjectId, TaskId for required
                if (customTerminology != null)
                {
                    if (string.IsNullOrEmpty(timeEntrySubmissionDetails.ClientId))
                    {
                        error.Message = "Please select" + customTerminology.Client.Singular;
                        return error;
                    }
                    if (string.IsNullOrEmpty(timeEntrySubmissionDetails.ProjectId))
                    {
                        error.Message = "Please select" + customTerminology.Project.Singular;
                        return error;
                    }
                    if (string.IsNullOrEmpty(timeEntrySubmissionDetails.TaskId))
                    {
                        error.Message = "Please select" + customTerminology.Task.Singular;
                        return error;
                    }
                }

                // Check Description for 4000 lenght
                if (!string.IsNullOrEmpty(timeEntrySubmissionDetails.Description))
                {
                    if (timeEntrySubmissionDetails.Description.Length > 4000)
                    {
                        error.Message = "Description cannot be greater than 4000 Characters.";
                        return error;
                    }
                }

                // Check hours for greater than 0
                if (!timeEntrySubmissionDetails.CalendarEntry)
                {
                    if (timeEntrySubmissionDetails.Hours == 0.0)
                    {
                        error.Message = "Please Enter Total Hours";
                        return error;
                    }
                }

                // If Calendar entry check start/stop time for required and validity
                if (timeEntrySubmissionDetails.CalendarEntry)
                {
                    if (string.IsNullOrEmpty(timeEntrySubmissionDetails.StartTime))
                    {
                        error.Message = "Please Enter Start Time.";
                        return error;
                    }
                    if (string.IsNullOrEmpty(timeEntrySubmissionDetails.StopTime))
                    {
                        error.Message = "Please Enter Stop Time.";
                        return error;
                    }

                    if (!Utility.IsValidTimeFormat(timeEntrySubmissionDetails.StartTime))
                    {
                        error.Message = "Please enter valid start time.";
                        return error;
                    }

                    if (!Utility.IsValidTimeFormat(timeEntrySubmissionDetails.StopTime))
                    {
                        error.Message = "Please enter valid stop time.";
                        return error;
                    }

                    if (!Utility.IsValidTime(Convert.ToDateTime(timeEntrySubmissionDetails.StartDate.ToShortDateString() + " " + timeEntrySubmissionDetails.StartTime),
                                                Convert.ToDateTime(timeEntrySubmissionDetails.EndDate.ToShortDateString() + " " + timeEntrySubmissionDetails.StopTime)))
                    {
                        error.Message = "Stop Time should be greater than Start Time";
                        return error;
                    }
                }

                return error;
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Error, e);
                error.Message = e.Message;
                return error;
            }
        }

        /// <summary>
        /// Gets CustomTerminology
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <returns>CustomTerminologyBO</returns>
        private CustomTerminologyBO GetCustomTerminology(string consumerSecretToken, string dataAccessToken, string apiVersion)
        {
            TimeEntryService integrationService = new TimeEntryService();
            CustomTerminologyBO customTerminologyBO = new CustomTerminologyBO();

            // Calls GetCustomTerminology of Integration layer
            customTerminologyBO = integrationService.GetCustomTerminology(Constants.CONSUMER_SECRET_TOKEN, UtilityService.DecryptedText(dataAccessToken), apiVersion);

            return customTerminologyBO;
        }

        #endregion
    }
}