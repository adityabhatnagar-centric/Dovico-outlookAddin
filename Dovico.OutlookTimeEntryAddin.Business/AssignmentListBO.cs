﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class AssignmentListBO : BaseBO
    {
        public IList<AssignmentBO> Assignments { get; set; }
    }
}
