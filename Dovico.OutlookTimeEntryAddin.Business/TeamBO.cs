using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class TeamBO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Singular { get; set; }
        public string Plural { get; set; }
        public string GetItemURI { get; set; }
    }
}
