using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class ClientBO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public RegionBO Region { get; set; }
        public string Archive { get; set; }
        public string Integrate { get; set; }
        public string Singular { get; set; }
        public string Plural { get; set; }
        public List<CustomFieldBO> CustomFields { get; set; }
    }
}
