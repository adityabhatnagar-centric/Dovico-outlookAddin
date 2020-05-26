using Dovico.CommonLibrary;
using Dovico.CommonLibrary.Data;
using Dovico.CommonLibrary.StringBuilders;
using Dovico.OutlookTimeEntryAddin.Business;
using Dovico.OutlookTimeEntryAddin.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Script.Serialization;

namespace Dovico.OutlookTimeEntryAddin.Integration
{
    /// <summary>
    /// This class contains Time Entry integration methods with Dovico.CommonLibrary
    /// </summary>
    public class TimeEntryService
    {
        /// <summary>
        /// Gets the employee data
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <returns>EmployeeBO</returns>
        public EmployeeBO GetEmployeeData(string consumerSecretToken, string dataAccessToken, string version)
        {
            EmployeeBO employee = null;

            // Calls Dovico.CommonLibrary CEmployee.GetInfoMe method
            APIRequestResult apiRequestResult = new APIRequestResult(consumerSecretToken, dataAccessToken, version);
            CEmployee cEmployee = CEmployee.GetInfoMe(apiRequestResult);

            // Convert data to EmployeeBO
            if (cEmployee != null)
            {
                employee = new EmployeeBO()
                {
                    ID = Convert.ToString(cEmployee.ID.ID),
                    FirstName = cEmployee.FirstName,
                    LastName = cEmployee.LastName
                };
            }

            return employee;
        }

        /// <summary>
        /// Returns if the Employee is Billable
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <returns>string</returns>
        public string ShowEmployeeBillable(string consumerSecretToken, string dataAccessToken, string version)
        {
            // Calls Dovico.CommonLibrary CEmployee.GetInfoMeOptions method
            APIRequestResult apiRequestResult = new APIRequestResult(consumerSecretToken, dataAccessToken, version) { ContentType = CRestApiHelper.MIME_TYPE_APPLICATION_JSON };
            string result = CEmployee.GetInfoMeOptions(ref apiRequestResult);

            return result;
        }

        /// <summary>
        /// Gets data for Assignments (Client, Project, Task) based on employeeId
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <param name="employeeId"></param>
        /// <returns>IList<AssignmentBO></returns>
        public IList<AssignmentBO> GetAssignments(string consumerSecretToken, string dataAccessToken, string version, int employeeId)
        {
            IList<AssignmentBO> assignments = null;

            // Calls Dovico.CommonLibrary CAssignments.GetAssignments method
            APIRequestResult apiRequestResult = new APIRequestResult(consumerSecretToken, dataAccessToken, version)
                                                                    { ContentType = CRestApiHelper.MIME_TYPE_APPLICATION_JSON };
            string cAssignmentsResult = CAssignments.GetAssignments(employeeId, ref apiRequestResult);

            // Convert data to AssignmentBO
            if (Utility.IsJson(cAssignmentsResult))
            {
                assignments = DeserializeObject(cAssignmentsResult).Assignments;
            }
            else
            {
                throw new DovicoException(cAssignmentsResult);
            }

            return assignments;
        }

        /// <summary>
        /// Gets data for Assignments (Client, Project, Task) based on uri
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <param name="uri"></param>
        /// <returns>IList<AssignmentBO></returns>
        public IList<AssignmentBO> GetAssignments(string consumerSecretToken, string dataAccessToken, string version, string uri)
        {
            IList<AssignmentBO> assignments = null;

            // Calls Dovico.CommonLibrary CAssignments.GetAssignments method
            APIRequestResult apiRequestResult = new APIRequestResult(consumerSecretToken, dataAccessToken, version)
                                                                         { ContentType = CRestApiHelper.MIME_TYPE_APPLICATION_JSON };
            string cAssignmentsResult = CAssignments.GetAssignments(uri, ref apiRequestResult);

            // Convert data to AssignmentBO
            if (Utility.IsJson(cAssignmentsResult))
            {
                assignments = DeserializeObject(cAssignmentsResult).Assignments;
            }
            else
            {
                throw new DovicoException(cAssignmentsResult);
            }

            return assignments;
        }

        /// <summary>
        /// Get Time Entries data for employee
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <param name="employeeId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>IList<TimeEntryBO></returns>
        public IList<TimeEntryBO> GetTimeEntries(string consumerSecretToken, string dataAccessToken, string version, int employeeId, DateTime startDate, DateTime? endDate = null)
        {
            // Create date variables
            string fromDate = startDate.ToString("yyyy-MM-dd");
            string toDate = endDate == null ? fromDate : ((DateTime)endDate).ToString("yyyy-MM-dd");

            // Create query parameters and query string
            string datetimeParamter = string.Concat(fromDate, " ", toDate);
            string employeeParameter = string.Concat("Employee/", employeeId.ToString(CultureInfo.InvariantCulture));
            string queryString = string.IsNullOrEmpty(datetimeParamter) ? string.Empty : string.Concat("daterange=", datetimeParamter);

            // Create uri to hit and make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string uri = CRestApiHelper.BuildURI(string.Concat("TimeEntries/", employeeParameter, "/"), queryString, version);
            string result = CRestApiHelper.MakeAPIRequest(uri, "GET", CRestApiHelper.MIME_TYPE_APPLICATION_JSON,
                                                            String.Empty, consumerSecretToken, dataAccessToken);

            // Check for errors in return value
            UtilityService.CheckErrorsInDovicoAPI(result);

            // Convert data to TimeEntryBO
            IList<TimeEntryBO> timeEntires = DeserializeObject(result).TimeEntries;

            return timeEntires;
        }

        ///// <summary>
        ///// Get Custom Templates data for specific type
        ///// </summary>
        ///// <param name="consumerSecretToken"></param>
        ///// <param name="dataAccessToken"></param>
        ///// <param name="version"></param>
        ///// <param name="type"></param>
        ///// <returns>IList<CustomTemplateBO></returns>
        //public IList<CustomTemplateBO> GetCustomTemplates(string consumerSecretToken, string dataAccessToken, string version, string type)
        //{
        //    // Create uri to hit and make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
        //    string queryString = string.Empty;
        //    string uri = CRestApiHelper.BuildURI(string.Concat("CustomFieldTemplates/Type/", type, "/"), queryString, version);
        //    string result = CRestApiHelper.MakeAPIRequest(uri, "GET", CRestApiHelper.MIME_TYPE_APPLICATION_JSON,
        //                                                    String.Empty, consumerSecretToken, dataAccessToken);

        //    // Check for errors in return value
        //    UtilityService.CheckErrorsInDovicoAPI(result);

        //    // Convert data to CustomTemplateBO
        //    IList<CustomTemplateBO> customTemplates = DeserializeObject(result).CustomTemplates;

        //    return customTemplates;
        //}

        /// <summary>
        /// Get Custom Templates data for the task
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <param name="taskId"></param>
        /// <returns>IList<CustomTemplateBO></returns>
        public IList<CustomTemplateBO> GetCustomTemplates(string consumerSecretToken, string dataAccessToken, string version, string taskId)
        {
            // Create uri to hit and make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string queryString = string.Concat("linkid=", taskId);
            string uri = CRestApiHelper.BuildURI(string.Concat("CustomFieldTemplates/Type/M/"), queryString, version);
            string result = CRestApiHelper.MakeAPIRequest(uri, "GET", CRestApiHelper.MIME_TYPE_APPLICATION_JSON,
                                                            String.Empty, consumerSecretToken, dataAccessToken);

            // Check for errors in return value
            UtilityService.CheckErrorsInDovicoAPI(result);

            // Convert data to CustomTemplateBO
            IList<CustomTemplateBO> customTemplates = DeserializeObject(result).CustomTemplates;

            return customTemplates;
        }

        /// <summary>
        /// Saves the Time Entry
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <param name="timeEntrySubmissionDetails"></param>
        /// <returns>IList<TimeEntryBO></returns>
        public IList<TimeEntryBO> SaveTimeEntry(string consumerSecretToken, string dataAccessToken, string version, TimeEntrySubmissionBO timeEntrySubmissionDetails)
        {
            string fromDate = timeEntrySubmissionDetails.StartDate.ToString("yyyy-MM-dd");
            timeEntrySubmissionDetails.Description = timeEntrySubmissionDetails.Description.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "'");

            // Create Post Data
            CStringBuilder postData = new CStringBuilder();
            postData.Append("[");
            postData.Append("{");
            if (!string.IsNullOrEmpty(timeEntrySubmissionDetails.ClientId))
            {
                postData.Append(string.Concat("\"ClientID\":\"", timeEntrySubmissionDetails.ClientId, "\","));
            }
            postData.Append(string.Concat("\"ProjectID\":\"", timeEntrySubmissionDetails.ProjectId, "\","));
            postData.Append(string.Concat("\"TaskID\":\"", timeEntrySubmissionDetails.TaskId, "\","));
            postData.Append(string.Concat("\"EmployeeID\":\"", timeEntrySubmissionDetails.EmployeeId, "\","));
            postData.Append(string.Concat("\"Date\":\"", fromDate, "\","));
            // Convert start/stop time to 24 hours format
            if (!string.IsNullOrEmpty(timeEntrySubmissionDetails.StartTime))
            {
                postData.Append(string.Concat("\"StartTime\":\"", Convert.ToDateTime(timeEntrySubmissionDetails.StartTime).ToString("HHmm"), "\","));
            }
            if (!string.IsNullOrEmpty(timeEntrySubmissionDetails.StopTime))
            {
                postData.Append(string.Concat("\"StopTime\":\"", Convert.ToDateTime(timeEntrySubmissionDetails.StopTime).ToString("HHmm"), "\","));
            }
            postData.Append(string.Concat("\"Description\":\"", timeEntrySubmissionDetails.Description, "\","));
            postData.Append(string.Concat("\"Billable\":\"", timeEntrySubmissionDetails.Billable ? "T" : "F", "\","));
            postData.Append(string.Concat("\"TotalHours\":\"", timeEntrySubmissionDetails.Hours, "\","));
            postData.Append("\"CustomFields\":");
            postData.Append("[");
            StringBuilder innerPostData = new StringBuilder();
            // For Custom Fields
            if (timeEntrySubmissionDetails.CustomFields != null && timeEntrySubmissionDetails.CustomFields.Count > 0)
            {
                foreach(CustomFieldSubmissionBO customField in timeEntrySubmissionDetails.CustomFields)
                {
                    innerPostData.Append("{");
                    innerPostData.Append(string.Concat("\"ID\":\"", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "\","));
                    innerPostData.Append(string.Concat("\"TemplateID\":\"", customField.TemplateID, "\","));
                    innerPostData.Append("\"Values\":");
                    innerPostData.Append("[");
                    if(customField.Values != null) { 
                        foreach(ValueItemsSubmission item in customField.Values)
                        {
                            innerPostData.Append(string.Concat("\"", item.Value, "\","));
                        }
                        innerPostData.Remove(innerPostData.Length - 1, 1);
                    }                    
                    innerPostData.Append("]");
                    innerPostData.Append("},");
                }
                innerPostData.Remove(innerPostData.Length - 1, 1);
            }
            postData.Append(innerPostData.ToString());
            postData.Append("]");
            postData.Append("}");
            postData.Append("]");

            // Create uri to hit and make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string uri = CRestApiHelper.BuildURI("TimeEntries", null, version);
            string result = CRestApiHelper.MakeAPIRequest(uri, "POST", CRestApiHelper.MIME_TYPE_APPLICATION_JSON, postData, consumerSecretToken, dataAccessToken);

            UtilityService.CheckErrorsInDovicoAPI(result);

            IList<TimeEntryBO> timeEntries = DeserializeObject(result).TimeEntries;

            return timeEntries;
        }

        /// <summary>
        /// For Updating time entry
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <param name="timeEntrySubmissionDetails"></param>
        /// <returns>IList<TimeEntryBO></returns>
        public IList<TimeEntryBO> UpdateTimeEntry(string consumerSecretToken, string dataAccessToken, string version, TimeEntrySubmissionBO timeEntrySubmissionDetails)
        {
            string fromDate = timeEntrySubmissionDetails.StartDate.ToString("yyyy-MM-dd");
            timeEntrySubmissionDetails.Description = timeEntrySubmissionDetails.Description.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "'");

            // Create Post Data
            CStringBuilder postData = new CStringBuilder();
            postData.Append("{");
            if (!string.IsNullOrEmpty(timeEntrySubmissionDetails.ClientId))
            {
                postData.Append(string.Concat("\"ClientID\":\"", timeEntrySubmissionDetails.ClientId, "\","));
            }
            postData.Append(string.Concat("\"ProjectID\":\"", timeEntrySubmissionDetails.ProjectId, "\","));
            postData.Append(string.Concat("\"TaskID\":\"", timeEntrySubmissionDetails.TaskId, "\","));
            postData.Append(string.Concat("\"EmployeeID\":\"", timeEntrySubmissionDetails.EmployeeId, "\","));
            postData.Append(string.Concat("\"Date\":\"", fromDate, "\","));
            // Convert start/stop time to 24 hours format
            if (!string.IsNullOrEmpty(timeEntrySubmissionDetails.StartTime))
            {
                postData.Append(string.Concat("\"StartTime\":\"", Convert.ToDateTime(timeEntrySubmissionDetails.StartTime).ToString("HHmm"), "\","));
            }
            if (!string.IsNullOrEmpty(timeEntrySubmissionDetails.StopTime))
            {
                postData.Append(string.Concat("\"StopTime\":\"", Convert.ToDateTime(timeEntrySubmissionDetails.StopTime).ToString("HHmm"), "\","));
            }
            postData.Append(string.Concat("\"Description\":\"", timeEntrySubmissionDetails.Description, "\","));
            postData.Append(string.Concat("\"Billable\":\"", timeEntrySubmissionDetails.Billable ? "T" : "F", "\","));
            postData.Append(string.Concat("\"TotalHours\":\"", timeEntrySubmissionDetails.Hours, "\","));

            postData.Append("\"CustomFields\":");
            postData.Append("[");
            StringBuilder innerPostData = new StringBuilder();
            // For Custom Fields
            if (timeEntrySubmissionDetails.CustomFields != null && timeEntrySubmissionDetails.CustomFields.Count > 0)
            {
                foreach (CustomFieldSubmissionBO customField in timeEntrySubmissionDetails.CustomFields)
                {
                    innerPostData.Append("{");
                    if (String.IsNullOrEmpty(customField.ID))
                    {
                        innerPostData.Append(string.Concat("\"ID\":\"", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", "\","));
                    }
                    else
                    { 
                        innerPostData.Append(string.Concat("\"ID\":\"", customField.ID, "\","));
                    }
                    innerPostData.Append(string.Concat("\"TemplateID\":\"", customField.TemplateID, "\","));
                    innerPostData.Append("\"Values\":");
                    innerPostData.Append("[");
                    if (customField.Values != null)
                    {
                        foreach (ValueItemsSubmission item in customField.Values)
                        {
                            innerPostData.Append(string.Concat("\"", item.Value, "\","));
                        }
                        innerPostData.Remove(innerPostData.Length - 1, 1);
                    }
                    innerPostData.Append("]");
                    innerPostData.Append("},");
                }
                innerPostData.Remove(innerPostData.Length - 1, 1);
            }
            postData.Append(innerPostData.ToString());
            postData.Append("]");
            postData.Append("}");

            // Create uri to hit and make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string uri = CRestApiHelper.BuildURI(string.Concat("TimeEntries/", timeEntrySubmissionDetails.TimeEntryId), null, version);
            string result = CRestApiHelper.MakeAPIRequest(uri, "PUT", CRestApiHelper.MIME_TYPE_APPLICATION_JSON, postData, consumerSecretToken, dataAccessToken);

            UtilityService.CheckErrorsInDovicoAPI(result);

            IList<TimeEntryBO> timeEntries = DeserializeObject(result).TimeEntries;

            return timeEntries;
        }

        /// <summary>
        /// Submit the time entries
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <param name="employeeId"></param>
        /// <param name="startDateOfWeek"></param>
        /// <param name="endDateOfWeek"></param>
        /// <returns>IList<TimeEntryBO></returns>
        public IList<TimeEntryBO> SubmitWeekTimeEntryForApproval(string consumerSecretToken, string dataAccessToken, string version, int employeeId, 
                                                            DateTime startDateOfWeek, DateTime endDateOfWeek)
        {
            // Create parameters and uri
            string dateRangeParameter = string.Concat(startDateOfWeek.ToString("yyyy-MM-dd"), " ", endDateOfWeek.ToString("yyyy-MM-dd"));
            string employeeParameter = string.Format("TimeEntries/Employee/{0}/Submit", Convert.ToString(employeeId));
            string queryString = string.Concat("daterange=", dateRangeParameter);
            string uri = CRestApiHelper.BuildURI(employeeParameter, queryString, version);

            CStringBuilder postData = new CStringBuilder();
            postData.Append("{");
            postData.Append("}");

            // Make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string result = CRestApiHelper.MakeAPIRequest(uri, "POST", CRestApiHelper.MIME_TYPE_APPLICATION_JSON, postData, consumerSecretToken, dataAccessToken);

            UtilityService.CheckErrorsInDovicoAPI(result);

            IList<TimeEntryBO> timeEntries = DeserializeObject(result).TimeEntries;

            return timeEntries;
        }

        /// <summary>
        /// Gets CustomTerminology
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <returns>CustomTerminologyBO</returns>
        public CustomTerminologyBO GetCustomTerminology(string consumerSecretToken, string dataAccessToken, string version)
        {
            // Create uri to hit and make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string uri = CRestApiHelper.BuildURI("CustomTerminology", null, version);
            string result = CRestApiHelper.MakeAPIRequest(uri, "GET", CRestApiHelper.MIME_TYPE_APPLICATION_JSON, string.Empty, consumerSecretToken, dataAccessToken);

            // Check for errors in return value
            UtilityService.CheckErrorsInDovicoAPI(result);

            JavaScriptSerializer objJavaScriptSerializer = new JavaScriptSerializer();
            objJavaScriptSerializer.MaxJsonLength = int.MaxValue;

            // Convert data to CustomTerminologyBO
            CustomTerminologyBO objJsonResultRootObject = (CustomTerminologyBO)objJavaScriptSerializer.Deserialize(result, typeof(CustomTerminologyBO));

            return objJsonResultRootObject;
        }

        /// <summary>
        /// Delete a  specific Time Entry
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <param name="timeEntryId"></param>
        public void DeleteTimeEntry(string consumerSecretToken, string dataAccessToken, string version, string timeEntryId)
        {
            // Create uri to hit and make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string uri = CRestApiHelper.BuildURI(string.Concat("TimeEntries/", timeEntryId), null, version);
            string result = CRestApiHelper.MakeAPIRequest(uri, "DELETE", CRestApiHelper.MIME_TYPE_APPLICATION_JSON, string.Empty, consumerSecretToken, dataAccessToken);

            // Check for errors in return value
            UtilityService.CheckErrorsInDovicoAPI(result);
        }

        /// <summary>
        /// Get Time Entry details for specific time entry
        /// </summary>
        /// <param name="consumerSecretToken"></param>
        /// <param name="dataAccessToken"></param>
        /// <param name="version"></param>
        /// <param name="timeEntryId"></param>
        /// <returns>IList<TimeEntryBO></returns>
        public IList<TimeEntryBO> GetSingleTimeEntry(string consumerSecretToken, string dataAccessToken, string version, string timeEntryId)
        {
            // Create uri to hit and make request using Dovico.CommonLibrary CRestApiHelper.MakeAPIRequest method
            string uri = CRestApiHelper.BuildURI(string.Concat("TimeEntries/", timeEntryId), null, version);
            string result = CRestApiHelper.MakeAPIRequest(uri, "GET", CRestApiHelper.MIME_TYPE_APPLICATION_JSON, string.Empty, consumerSecretToken, dataAccessToken);

            // Check for errors in return value
            UtilityService.CheckErrorsInDovicoAPI(result);

            // Convert data to TimeEntryBO
            IList<TimeEntryBO> timeEntries = DeserializeObject(result).TimeEntries;

            return timeEntries;
        }

        /// <summary>
        /// Method to deserialize the object passed
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <returns>RootObject</returns>
        private RootObject DeserializeObject(string serializedObject)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = int.MaxValue;

            // Convert to RootObject
            RootObject rootObject = (RootObject)javaScriptSerializer.Deserialize(serializedObject, typeof(RootObject));

            return rootObject;
        }        
    }
}
