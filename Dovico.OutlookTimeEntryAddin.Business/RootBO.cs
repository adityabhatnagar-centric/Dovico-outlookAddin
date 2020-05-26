using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class RootObject
    {
        public List<AssignmentBO> Assignments { get; set; }

        public List<TimeEntryBO> TimeEntries { get; set; }

        public List<CustomTemplateBO> CustomTemplates { get; set; }
    }
}
