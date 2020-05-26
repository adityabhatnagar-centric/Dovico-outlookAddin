using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class TimeEntrySubmissionBO
    {
        public string TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public string ClientId { get; set; }
        public string ProjectId { get; set; }
        public string TaskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Hours { get; set; }
        public string Description { get; set; }
        public bool Billable { get; set; }
        public bool CalendarEntry { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }

        public List<CustomFieldSubmissionBO> CustomFields { get; set; }
    }

    public class CustomFieldSubmissionBO
    {
        public string ID { get; set; }
        public string TemplateID { get; set; }
        public string Name { get; set; }
        public List<ValueItemsSubmission> Values { get; set; }
        public string GetCustomTemplateURI { get; set; }
    }

    public class ValueItemsSubmission
    {
        public string Value { get; set; }
    }
}
