using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.UI.Messages
{
    public class ExceptionMessage
    {
        public ExceptionMessage(Exception ex)
        {
            this.Exception = ex;
        }

        public Exception Exception { get; private set; }
    }
}
