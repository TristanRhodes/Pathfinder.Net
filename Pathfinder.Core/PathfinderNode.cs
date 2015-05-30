using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core
{
    public struct PathfinderNode : IComparable<PathfinderNode>
    {
        /// <summary>
        /// As this is a struct, this indicates whether the cell has been explored yet
        /// </summary>
        public bool Explored;


        /// <summary>
        /// The X Coordinate of the node
        /// </summary>
        public int X;

        /// <summary>
        /// The Y Coordinate of the node
        /// </summary>
        public int Y;


        /// <summary>
        /// The X Coordinate of the parent node
        /// </summary>
        public int ParentX;

        /// <summary>
        /// The Y Coordinate of the parent node
        /// </summary>
        public int ParentY;


        /// <summary>
        /// The cost to get to this node
        /// </summary>
        public int Cost;

        /// <summary>
        /// Determines how close to the solution this node is
        /// </summary>
        public int Heuristic;

        /// <summary>
        /// The total value of the node, used for sorting
        /// </summary>
        public int Value;

        public override string ToString()
        {
            return string.Format("X:{0}, Y:{1}, C:{2}, H:{3}, V:{4}", X, Y, Cost, Heuristic, Value);
        }

        public int CompareTo(PathfinderNode other)
        {
            return this.Value.CompareTo(other.Value);
        }
    }
}
