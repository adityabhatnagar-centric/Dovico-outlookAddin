using System;
using System.Collections.Generic;

namespace Dovico.OutlookTimeEntryAddin.WebAddInWeb.ViewModels
{
    /// <summary>
    /// Class to hold weekly Time Entry details data
    /// </summary>
    public class TimeEntryDailyHoursVM
    {
        public List<TimeEntryDailyHoursDetailsVM> DailyHoursList { get; set; }
    }

    /// <summary>
    /// Class to hold daily Time Entry details data
    /// </summary>
    public class TimeEntryDailyHoursDetailsVM
    {
        public string Date { get; set; }

        private string hours;
        public string Hours
        {
            get
            {
                return hours;
            }
            set
            {
                double doubleValue;
                Double.TryParse(value, out doubleValue);
                hours = doubleValue.ToString("0.##");
            }
        }
    }
}