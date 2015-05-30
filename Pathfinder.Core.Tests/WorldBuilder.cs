using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core.Tests
{
    public static class WorldBuilder
    {
        /// <summary>
        /// Create an empty test world.
        /// </summary>
        /// <returns></returns>
        public static World<bool> CreateStandardEmptyWorld()
        {
            return CreateEmptyWorld(5, 5);
        }

        /// <summary>
        /// Create an empty test world.
        /// </summary>
        /// <returns></returns>
        public static World<bool> CreateEmptyWorld(int width, int height)
        {
            return new World<bool>(width, height, true);
        }

        /// <summary>
        /// Create a test world that has an V shaped block in it.
        /// </summary>
        /// <returns></returns>
        public static World<bool> CreateBlockedWorld()
        {
            var world = new World<bool>(10, 10, true);
            
            world[8, 8] = false;
            
            world[7, 8] = false;
            world[6, 8] = false;
            world[5, 8] = false;
            world[4, 8] = false;
            
            world[8, 7] = false;
            world[8, 6] = false;
            world[8, 5] = false;
            world[8, 4] = false;

            return world;
        }
    }
}
