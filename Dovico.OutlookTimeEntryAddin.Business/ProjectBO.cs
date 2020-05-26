using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class ProjectBO
    {
        public string ID { get; set; }
        public ClientBO Client { get; set; }
        public string Name { get; set; }
        public LeaderBO Leader { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public ProjectGroupBO ProjectGroup { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BillingBy { get; set; }
        public List<object> FixedCosts { get; set; }
        public CurrencyBO Currency { get; set; }
        public string BudgetRateDate { get; set; }
        public string HideTasks { get; set; }
        public string PreventEntries { get; set; }
        public string TimeBillableByDefault { get; set; }
        public string ExpensesBillableByDefault { get; set; }
        public string Linked { get; set; }
        public string MSPConfig { get; set; }
        public string RSProject { get; set; }
        public string Archive { get; set; }
        public string Integrate { get; set; }
        public string Singular { get; set; }
        public string Plural { get; set; }
        public List<CustomFieldBO> CustomFields { get; set; }
    }
}
