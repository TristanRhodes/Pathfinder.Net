using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core
{
    internal class MoveVectorConstants
    {
        /// <summary>
        /// Array for movement N, NE, E, SE, S, SW, W, NW directions.
        /// </summary>
        internal static readonly Coordinate[] EightWayMoveVector = new[]
                                                       {
                                                           new Coordinate(-1, 1),
                                                           new Coordinate(0, 1),
                                                           new Coordinate(1, 1),
                                                           new Coordinate(1, 0),
                                                           new Coordinate(1, -1),
                                                           new Coordinate(0, -1),
                                                           new Coordinate(-1, -1),
                                                           new Coordinate(-1, 0),
                                                       };

        /// <summary>
        /// Array for movement N, E, S, W directions.
        /// </summary>
        internal static readonly Coordinate[] FourWayMoveVector = new[]
                                                       {
                                                           new Coordinate(0, 1),
                                                           new Coordinate(1, 0),
                                                           new Coordinate(0, -1),
                                                           new Coordinate(-1, 0),
                                                       };
    }
}
