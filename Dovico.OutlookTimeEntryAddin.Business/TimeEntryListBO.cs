using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class TimeEntryListBO : BaseBO
    {
        public IList<TimeEntryBO> timeEntries { get; set; }
    }
}
