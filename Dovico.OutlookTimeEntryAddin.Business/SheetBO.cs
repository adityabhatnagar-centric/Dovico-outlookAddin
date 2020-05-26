using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class SheetBO
    {
        public string ID { get; set; }
        public string RejectedReason { get; set; }
        public EmployeeBO Employee { get; set; }
        public ManagerBO Manager { get; set; }
        public WorkflowBO Workflow { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
