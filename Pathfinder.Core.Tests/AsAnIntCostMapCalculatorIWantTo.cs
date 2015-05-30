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
    public class AsAnIntCostMapCalculatorIWantTo
    {
        [TestMethod]
        public void EnsureMovementCostFunctionsCorrectly()
        {
            var world = new World<uint?>(2, 2, 1);
            world[0, 1] = 5;
            world[1, 0] = 10;
            world[1, 1] = 15;

            var costCalculator = new IntMapCostCalculator(world);

            Assert.AreEqual(5, costCalculator.CalculateCost(0, 0, 0, 1, 0, 1));
            Assert.AreEqual(10, costCalculator.CalculateCost(0, 0, 1, 0, 1, 0));
            Assert.AreEqual(15, costCalculator.CalculateCost(0, 0, 1, 1, 1, 1));
        }

        [TestMethod]
        public void CheckICanIdentifyOpenAndClosedCells()
        {
            var world = new World<uint?>(1, 1, null);
            var costCalculator = new IntMapCostCalculator(world);

            // Open State
            world[0, 0] = 1;
            var open = costCalculator.OpenCheck(0, 0);
            Assert.AreEqual(open, true);

            // Closed State
            world[0, 0] = null;
            open = costCalculator.OpenCheck(0, 0);
            Assert.AreEqual(open, false);
        }
    }
}
