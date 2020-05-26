using System;
using System.Collections.Generic;

namespace Dovico.OutlookTimeEntryAddin.WebAddInWeb.ViewModels
{
    /// <summary>
    /// Class to hold Time Entry data
    /// </summary>
    public class TimeEntryVM
    {
        public string TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public string ClientId { get; set; }
        public string ProjectId { get; set; }
        public string TaskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Hours { get; set; }
        public string BillableHours { get; set; }
        private string isBillable;
        public string IsBillable
        {
            get
            {
                return isBillable;
            }
            set
            {
                if(value == "T")
                {
                    isBillable = "Billable";
                }
                else
                {
                    isBillable = string.Empty;
                }
            }
        }
        public string IsBillableHours { get; set; }
        public string FromCalendar { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public string ClientName { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public bool DeleteAllowed { get; set; }
        public bool EditAllowed { get; set; }
        public string SheetStatus { get; set; }

        public List<CustomFieldVM> CustomFields { get; set; }
    }

    public class CustomFieldVM
    {
        public string Id { get; set; }
        public string TemplateId { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; }
        public string GetCustomTemplateURI { get; set; }
    }
}