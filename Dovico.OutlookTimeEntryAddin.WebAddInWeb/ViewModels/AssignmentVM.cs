using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dovico.OutlookTimeEntryAddin.WebAddInWeb.ViewModels
{
    /// <summary>
    /// Class to hold Assignment data
    /// </summary>
    public class AssignmentVM
    {
        public string ItemID { get; set; }
        public string Name { get; set; }
        public string GetAssignmentsURI { get; set; }
        public string TimeBillableByDefault { get; set; }
    }
}