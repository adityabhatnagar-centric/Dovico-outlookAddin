using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class EmployeeBO : BaseBO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string GetItemURI { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public TeamBO Team { get; set; }
        public string Wage { get; set; }
        public WageCurrencyBO WageCurrency { get; set; }
        public string WageChangedDate { get; set; }
        public string Charge { get; set; }
        public ChargeCurrencyBO ChargeCurrency { get; set; }
        public string ChargeChangedDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string WorkDays { get; set; }
        public string Hours { get; set; }
        public string AltApproval { get; set; }
        public string NotificationTime { get; set; }
        public string NotificationExpense { get; set; }
        public string NotificationRejected { get; set; }
        public string Archive { get; set; }
        public string Integrate { get; set; }
        public double ApprovedDays { get; set; }
        public string Singular { get; set; }
        public string Plural { get; set; }
        public List<CustomFieldBO> CustomFields { get; set; }
    }   
}
