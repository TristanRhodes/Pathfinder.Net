using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pathfinder.Core.CostCalculators;

namespace Pathfinder.Core.Tests
{
    [TestClass]
    public class AsABooleanMapCostCalculatorIWantTo
    {
        [TestMethod]
        public void EnsureDiagonalMovementCostFunctionsCorrectly()
        {
            var world = new World<bool>(2, 2, true);

            var costCalculator = new BooleanMapCostCalculator(world);
            costCalculator.DiagonalMovementCost = 14;

            Assert.AreEqual(14, costCalculator.CalculateCost(0, 0, 1, 1, 1, 1));
            Assert.AreEqual(14, costCalculator.CalculateCost(0, 0, 1, -1, 1, -1));
            Assert.AreEqual(14, costCalculator.CalculateCost(0, 0, -1, 1, -1, 1));
            Assert.AreEqual(14, costCalculator.CalculateCost(0, 0, -1, -1, -1, -1));
        }

        [TestMethod]
        public void EnsureHorizontalAndVerticalMovementCostFunctionsCorrectly()
        {
            var world = new World<bool>(2, 1, true);

            var costCalculator = new BooleanMapCostCalculator(world);
            costCalculator.HorizontalAndVerticalMovementCost = 10;

            Assert.AreEqual(10, costCalculator.CalculateCost(0, 0, 1, 0, 1, 0));
            Assert.AreEqual(10, costCalculator.CalculateCost(0, 0, -1, 0, -1, 0));
            Assert.AreEqual(10, costCalculator.CalculateCost(0, 0, 0, 1, 0, 1));
            Assert.AreEqual(10, costCalculator.CalculateCost(0, 0, 0, -1, 0, -1));
        }

        [TestMethod]
        public void CheckICanIdentifyOpenAndClosedCells()
        {
            var world = new World<bool>(1, 1, true);
            var costCalculator = new BooleanMapCostCalculator(world);

            // Open State
            world[0, 0] = true;
            var open = costCalculator.OpenCheck(0, 0);
            Assert.AreEqual(open, true);

            // Closed State
            world[0, 0] = false;
            open = costCalculator.OpenCheck(0, 0);
            Assert.AreEqual(open, false);
        }
    }
}
