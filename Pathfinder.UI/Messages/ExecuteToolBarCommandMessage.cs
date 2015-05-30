using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathfinder.UI.Enums;

namespace Pathfinder.UI.Messages
{
    public class ExecuteToolBarCommandMessage
    {
        public ExecuteToolBarCommandMessage(ToolBarCommand command)
        {
            Command = command;
        }

        public ToolBarCommand Command { get; private set; }
    }
}
