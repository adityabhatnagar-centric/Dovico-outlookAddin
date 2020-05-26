using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class CustomTemplateListBO : BaseBO
    {
        public IList<CustomTemplateBO> CustomTemplates { get; set; }
    }
}
