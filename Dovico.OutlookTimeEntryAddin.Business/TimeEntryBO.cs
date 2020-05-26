using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class TimeEntryBO :  BaseBO
    {
        public string ID { get; set; }
        public SheetBO Sheet { get; set; }
        public ClientBO Client { get; set; }
        public ProjectBO Project { get; set; }
        public TaskBO Task { get; set; }
        public EmployeeBO Employee { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string StopTime { get; set; }
        public string TotalHours { get; set; }
        public string Description { get; set; }
        public string Billable { get; set; }
        public string Charge { get; set; }
        public ChargeCurrencyBO ChargeCurrency { get; set; }
        public string OTCharge { get; set; }
        public string Wage { get; set; }
        public WageCurrencyBO WageCurrency { get; set; }
        public string OTWage { get; set; }
        public string Prorate { get; set; }
        public string Integrate { get; set; }

        public List<CustomFieldBO> CustomFields { get; set; }
    }
}
