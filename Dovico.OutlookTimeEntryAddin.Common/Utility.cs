using System;
using System.Globalization;

namespace Dovico.OutlookTimeEntryAddin.Common
{
    /// <summary>
    /// This class contains Utility methods
    /// </summary>
    public class Utility
    {
        // Method to check if input is json or not
        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        // Method to check if input hours is numeric or not
        public static bool NumericValidate(string hours)
        {
            hours = hours.Replace(",", ".");
            bool isNumeric = IsTextNumeric(hours);

            if (isNumeric)
            {
                if (hours.IndexOf(".") != -1)
                {
                    if (hours.Substring(hours.IndexOf(".")).Length > 3)
                        return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        // Method to check if input is numeric
        public static bool IsTextNumeric(string input)
        {
            double timeEntryHours;
            bool returnValue = Double.TryParse(input, out timeEntryHours);

            return returnValue;
        }

        // Method to check if datetime passed is a valid time
        public static bool IsValidTime(DateTime startTimeEntryDate, DateTime stopTimeEntryDate)
        {
            try
            {
                if (startTimeEntryDate > stopTimeEntryDate)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Method to check if time is a valid format
        public static bool IsValidTimeFormat(string time)
        {
            try
            {
                string sysUIFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

                DateTime result;
                if (!DateTime.TryParseExact(time, sysUIFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
