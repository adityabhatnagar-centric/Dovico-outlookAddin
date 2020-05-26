using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class AssignmentBO
    {
        public string AssignmentID { get; set; }
        public string ItemID { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string FinishDate { get; set; }
        public string EstimatedHours { get; set; }
        public string TimeBillableByDefault { get; set; }
        public string ExpensesBillableByDefault { get; set; }
        public string Charge { get; set; }
        public ChargeCurrencyBO ChargeCurrency { get; set; }
        public string Wage { get; set; }
        public WageCurrencyBO WageCurrency { get; set; }
        public string Prorate { get; set; }
        public string ETC { get; set; }
        public string Hide { get; set; }
        public string GetAssignmentsURI { get; set; }
        public string GetItemURI { get; set; }

        public IList<AssignmentBO> Assignments { get; set; }
    }
}
