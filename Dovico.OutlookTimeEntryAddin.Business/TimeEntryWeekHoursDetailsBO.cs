using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class TimeEntryWeekHoursDetailsBO : BaseBO
    {
        public string WeekHoursTotal { get; set; }

        public bool IsSubmitted { get; set; }
    }
}
