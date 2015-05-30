using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.UI.Messages
{
    public class TickMessage
    {
        public TickMessage(int ticks)
        {
            Ticks = ticks;
        }

        public int Ticks { get; private set; }
    }
}
