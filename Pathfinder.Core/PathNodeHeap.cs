using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core
{
    public class PathNodeHeap : BinaryHeap<PathfinderNode>
    {
        public bool HasBetterExistingSolutionInQueue(int newX, int newY, int cost)
        {
            // NOTE: Enumerator in tight loop.
            foreach(var node in this)
            {
                if (node.X == newX && node.Y == newY && node.Cost <= cost)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
