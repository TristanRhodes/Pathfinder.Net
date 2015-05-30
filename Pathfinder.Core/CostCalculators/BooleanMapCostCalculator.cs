using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core.CostCalculators
{
    public class BooleanMapCostCalculator : ICostCalculator
    {
        private World<bool> _world;


        public BooleanMapCostCalculator(World<bool> world)
        {
            _world = world;

            DiagonalMovementCost = 14;
            HorizontalAndVerticalMovementCost = 10;
        }


        public bool BlockPartialDiagonals { get; set; }


        public int DiagonalMovementCost { get; set; }

        public int HorizontalAndVerticalMovementCost { get; set; }


        public bool OpenCheck(int x, int y)
        {
            // Bounds Check
            if ((x < 0 || x > _world.Width - 1) ||
                (y < 0 || y > _world.Height - 1))
                return false;

            // Cell Open Check
            if (_world[x, y] == false)
                return false;

            return true;
        }

        public bool OpenCheck(int fromX, int fromY, int toX, int toY, int vectorX, int vectorY)
        {
            // Bounds Check
            if ((toX < 0 || toX > _world.Width - 1) ||
                (toY < 0 || toY > _world.Height - 1))
                return false;

            // Cell Open Check
            if (_world[toX, toY] == false)
                return false;

            bool isDiagonal = vectorX != 0 && vectorY != 0;

            // Block if we are moving diagonally around a blocking object
            if (BlockPartialDiagonals && isDiagonal)
            {
                // Calculate adjacent x cell
                if (_world[fromX + vectorX, fromY + 0] == false)
                    return false;

                // Calculate adjacent y cell
                if (_world[fromX + 0, fromY + vectorY] == false)
                    return false;
            }

            return true;
        }


        public int CalculateCost(int fromX, int fromY, int toX, int toY, int vectorX, int vectorY)
        {
            bool diagonal = fromX != toX && fromY != toY;
            return diagonal ? DiagonalMovementCost : HorizontalAndVerticalMovementCost;
        }
    }
}
