using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class CustomTemplateBO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public CustomTemplateType Type { get; set; }
        public string Required { get; set; }
        public string Description { get; set; }
        public string Hide { get; set; }
        public List<ValueItems> Values { get; set; }
    }

    public class ValueItems
    {
        public string ID { get; set; }
        public string Value { get; set; }
        public string Default { get; set; }
    }
    public enum CustomTemplateType
    {
        A, //Alphanumeric
        N, //Numeric
        D, //Date
        M, //Multiple Choice
        X //Exclusive Choice
    };
}

