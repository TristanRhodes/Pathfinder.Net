using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.UI.Messages
{
    public class CallbackMessage<T>
    {
        public CallbackMessage(Action<T> callback)
        {
            Debug.Assert(callback != null);

            this.Callback = callback;
        }

        public Action<T> Callback { get; set; }
    }
}
