using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core
{
    public enum ExplorerState
    {
        Stopped,
        Working,
        Completed,
        Failed
    }

    public enum MovementMode
    {
        FourDirection,
        EightDirection
    }
}
