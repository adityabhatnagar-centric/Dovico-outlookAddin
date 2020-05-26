using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class CustomTerminologyBO
    {
        public EmployeeBO Employee { get; set; }
        public TeamBO Team { get; set; }
        public TaskBO Task { get; set; }
        public ProjectBO Project { get; set; }
        public ClientBO Client { get; set; }
        //public Billing Billing { get; set; }
        public RegionBO Region { get; set; }
        public string Quantity { get; set; }
    }
}
