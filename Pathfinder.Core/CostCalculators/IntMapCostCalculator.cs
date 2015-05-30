using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core.CostCalculators
{
    public class IntMapCostCalculator : ICostCalculator
    {
        private World<uint?> _world;


        public IntMapCostCalculator(World<uint?> world)
        {
            _world = world;
        }


        public bool OpenCheck(int x, int y)
        {
            return _world[x, y] != null;
        }

        public bool OpenCheck(int fromX, int fromY, int toX, int toY, int vectorX, int vectorY)
        {
            return OpenCheck(toX, toY);
        }

        public int CalculateCost(int fromX, int fromY, int toX, int toY, int vectorX, int vectorY)
        {
            return (int)_world[toX, toY];
        }
    }
}
