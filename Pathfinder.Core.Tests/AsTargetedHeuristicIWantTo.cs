using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pathfinder.Core.Tests
{
    [TestClass]
    public class AsTargetedHeuristicIWantTo
    {
        [TestMethod]
        public void ConfirmMovingCloserReducesCalculationValue()
        {
            var heuristic = new TargetedHeuristic(new Coordinate(5,5));

            Assert.AreEqual(50, heuristic.Calculate(0, 0));
            Assert.AreEqual(32, heuristic.Calculate(1, 1));
            Assert.AreEqual(18, heuristic.Calculate(2, 2));
            Assert.AreEqual(8, heuristic.Calculate(3, 3));
            Assert.AreEqual(2, heuristic.Calculate(4, 4));
            Assert.AreEqual(0, heuristic.Calculate(5, 5));
        }

        [TestMethod]
        public void ConfirmScalingIncreasesCost()
        {
            var heuristic = new TargetedHeuristic(new Coordinate(5, 5));
            heuristic.HeuristicScale = 2;

            Assert.AreEqual(100, heuristic.Calculate(0, 0));
            Assert.AreEqual(4, heuristic.Calculate(4, 4));
        }


        [TestMethod]
        public void VerifyThatCompletionCheckWorks()
        {
            var heuristic = new TargetedHeuristic(new Coordinate(1, 0));

            Assert.IsFalse(heuristic.Complete(0, 0));
            Assert.IsTrue(heuristic.Complete(1, 0));
        }
    }
}
