using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class TimeEntryDailyHoursBO : BaseBO
    {
        public Dictionary<string,string> DailyHoursList { get; set; }
    }
}
