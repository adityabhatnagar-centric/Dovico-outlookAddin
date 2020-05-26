using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class TaskBO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public TaskGroupBO TaskGroup { get; set; }
        public string Description { get; set; }
        public string ForceDescription { get; set; }
        public string Global { get; set; }
        public string Prorate { get; set; }
        public string RSTask { get; set; }
        public string WBS { get; set; }
        public string Archive { get; set; }
        public string Integrate { get; set; }
        public double TakenDays { get; set; }
        public double TotalDays { get; set; }
        public double TaskPercentage { get; set; }
        public string GetItemURI { get; set; }
        public string Singular { get; set; }
        public string Plural { get; set; }
    }   
}
