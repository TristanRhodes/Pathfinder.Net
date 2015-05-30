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
    public class AsAPathfinderWithATargetedHeuristicIWantTo
    {
        private World<bool> _world;

        private BooleanMapCostCalculator _costCalc;
        private TargetedHeuristic _heuristicCalc;

        private PathfinderEngine _explorer;


        private void SetupWorld(int width, int height)
        {
            _world = WorldBuilder.CreateEmptyWorld(width, height);

            _costCalc = new BooleanMapCostCalculator(_world);
            _heuristicCalc = new TargetedHeuristic(new Coordinate(0,0));

            _explorer = new PathfinderEngine(_world.Width, _world.Height, _costCalc, _heuristicCalc);
        }

        [TestMethod]
        public void ExploreFromTheCentreOfTheMapToTheBottomRightCorner()
        {
            SetupWorld(5, 5);

            _heuristicCalc.Target = new Coordinate(4, 4);

            _explorer.MovementMode = MovementMode.EightDirection;
            _explorer.ExploreToCompletion(new Coordinate(2, 2));

            Assert.AreEqual(ExplorerState.Completed, _explorer.State);

            // Check we didn't explore our corners
            Assert.AreEqual(false, _explorer.Solution[0, 0].Explored);
            Assert.AreEqual(false, _explorer.Solution[0, 4].Explored);
            Assert.AreEqual(false, _explorer.Solution[4, 0].Explored);
            
            // Check we explored our target
            Assert.AreEqual(true, _explorer.Solution[4, 4].Explored);
            Assert.AreEqual(28, _explorer.Solution[4, 4].Cost);
        }
    }
}
