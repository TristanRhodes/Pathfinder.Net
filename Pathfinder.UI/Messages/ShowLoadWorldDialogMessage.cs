using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.UI.Messages
{
    public class ShowLoadWorldDialogMessage : CallbackMessage<string>
    {
        public ShowLoadWorldDialogMessage(Action<string> callback) 
            : base(callback)
        { }

        public string Filter { get; set; }
    }
}
