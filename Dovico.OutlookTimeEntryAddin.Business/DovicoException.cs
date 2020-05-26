using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dovico.OutlookTimeEntryAddin.Business
{
    public class DovicoException : Exception
    {
        public DovicoException()
        {
        }

        public DovicoException(string message) : base(message)
        {
        }

        public DovicoException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
