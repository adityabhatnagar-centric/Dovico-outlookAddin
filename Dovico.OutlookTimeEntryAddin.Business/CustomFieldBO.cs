using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class CustomFieldBO
    {
        public string ID { get; set; }
        public string TemplateID { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; }
        public string GetCustomTemplateURI { get; set; }
    }

}
