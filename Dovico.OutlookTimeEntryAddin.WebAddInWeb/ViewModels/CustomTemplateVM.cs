using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dovico.OutlookTimeEntryAddin.WebAddInWeb.ViewModels
{
    /// <summary>
    /// Class to hold Custom Template data
    /// </summary>
    public class CustomTemplateVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Required { get; set; }
        public string Description { get; set; }
        public string Hide { get; set; }
        public List<ValueItems> Values { get; set; }
    }

    public class ValueItems
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Default { get; set; }
    }
}