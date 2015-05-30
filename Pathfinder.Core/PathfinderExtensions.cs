using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core
{
    public static class PathfinderExtensions
    {
        public static void TickToCompletion(this PathfinderEngine explorer)
        {
            while (explorer.State == ExplorerState.Working)
            {
                explorer.Tick(1);
            }
        }

        public static void ExploreToCompletion(this PathfinderEngine explorer, Coordinate from)
        {
            explorer.ExploreFrom(from);
            explorer.TickToCompletion();
        }
    }
}
