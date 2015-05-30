using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pathfinder.Core.CostCalculators;

namespace Pathfinder.Core.Tests
{
    [TestClass]
    public class AsAPathfinderWithAnEmptyHeuristicIWantTo
    {
        private World<bool> _world;

        private BooleanMapCostCalculator _costCalc;
        private EmptyHeuristicCalculator _heuristicCalc;
        
        private PathfinderEngine _explorer;


        private void SetupWorld(int width, int height)
        {
            _world = WorldBuilder.CreateEmptyWorld(width, height);

            _costCalc = new BooleanMapCostCalculator(_world);
            _heuristicCalc = new EmptyHeuristicCalculator();

            _explorer = new PathfinderEngine(_world.Width, _world.Height, _costCalc, _heuristicCalc);
        }

        [TestMethod]
        public void ExploreFromTheCentreOfTheMapAndReachAllCorners()
        {
            SetupWorld(5, 5);

            _explorer.MovementMode = MovementMode.EightDirection;
            _explorer.ExploreToCompletion(new Coordinate(2, 2));

            Assert.AreEqual(ExplorerState.Completed, _explorer.State);

            Assert.AreEqual(28, _explorer.Solution[0, 0].Cost);
            Assert.AreEqual(28, _explorer.Solution[0, 4].Cost);
            Assert.AreEqual(28, _explorer.Solution[4, 4].Cost);
            Assert.AreEqual(28, _explorer.Solution[4, 0].Cost);
        }


        [TestMethod]
        public void ExploreInFourDirections()
        {
            SetupWorld(3, 3);

            _explorer.MovementMode = MovementMode.FourDirection;
            _explorer.ExploreToCompletion(new Coordinate(1, 1));

            Assert.AreEqual(ExplorerState.Completed, _explorer.State);

            Assert.AreEqual(20, _explorer.Solution[0, 0].Cost);
            Assert.AreEqual(10, _explorer.Solution[0, 1].Cost);
            Assert.AreEqual(20, _explorer.Solution[0, 2].Cost);
            Assert.AreEqual(10, _explorer.Solution[1, 2].Cost);

            Assert.AreEqual(20, _explorer.Solution[2, 2].Cost);
            Assert.AreEqual(10, _explorer.Solution[2, 1].Cost);
            Assert.AreEqual(20, _explorer.Solution[2, 0].Cost);
            Assert.AreEqual(10, _explorer.Solution[1, 0].Cost);
        }

        [TestMethod]
        public void ExploreTheWholeMapInEightDirections()
        {
            SetupWorld(3, 3);

            _explorer.MovementMode = MovementMode.EightDirection;
            _explorer.ExploreToCompletion(new Coordinate(1, 1));

            Assert.AreEqual(ExplorerState.Completed, _explorer.State);

            Assert.AreEqual(14, _explorer.Solution[0, 0].Cost);
            Assert.AreEqual(10, _explorer.Solution[0, 1].Cost);
            Assert.AreEqual(14, _explorer.Solution[0, 2].Cost);
            Assert.AreEqual(10, _explorer.Solution[1, 2].Cost);

            Assert.AreEqual(14, _explorer.Solution[2, 2].Cost);
            Assert.AreEqual(10, _explorer.Solution[2, 1].Cost);
            Assert.AreEqual(14, _explorer.Solution[2, 0].Cost);
            Assert.AreEqual(10, _explorer.Solution[1, 0].Cost);
        }


        [TestMethod]
        public void ExploreAroundAPartialBlockWithoutDiagonalMovement()
        {
            SetupWorld(2, 2);
            _costCalc.BlockPartialDiagonals = true;
            _world[0, 1] = false;

            _explorer.MovementMode = MovementMode.EightDirection;
            _explorer.ExploreToCompletion(new Coordinate(0, 0));

            Assert.AreEqual(ExplorerState.Completed, _explorer.State);

            Assert.AreEqual(1, _explorer.Solution[1, 1].ParentX);
            Assert.AreEqual(0, _explorer.Solution[1, 1].ParentY);

            Assert.AreEqual(0, _explorer.Solution[0, 1].ParentX);
            Assert.AreEqual(0, _explorer.Solution[0, 1].ParentY);
        }

        [TestMethod]
        public void ExploreAroundAPartialBlockWithDiagonalMovement()
        {
            SetupWorld(2, 2);
            _costCalc.BlockPartialDiagonals = false;
            _world[0, 1] = false;

            _explorer.MovementMode = MovementMode.EightDirection;
            _explorer.ExploreToCompletion(new Coordinate(0, 0));

            Assert.AreEqual(ExplorerState.Completed, _explorer.State);

            Assert.AreEqual(0, _explorer.Solution[1, 1].ParentX);
            Assert.AreEqual(0, _explorer.Solution[1, 1].ParentY);

            Assert.AreEqual(0, _explorer.Solution[0, 1].ParentX);
            Assert.AreEqual(0, _explorer.Solution[0, 1].ParentY);
        }


        [TestMethod]
        public void ExploreToAMaximumMovementCost()
        {
            SetupWorld(5, 1);

            _explorer.MovementMode = MovementMode.FourDirection;
            _explorer.MaxCost = 28;
            _explorer.ExploreToCompletion(new Coordinate(0, 0));

            Assert.AreEqual(ExplorerState.Completed, _explorer.State);

            // Explored Area
            Assert.AreEqual(0, _explorer.Solution[1, 0].ParentX);
            Assert.AreEqual(0, _explorer.Solution[1, 0].ParentY);

            Assert.AreEqual(1, _explorer.Solution[2, 0].ParentX);
            Assert.AreEqual(0, _explorer.Solution[2, 0].ParentY);

            // Unexplored Area
            Assert.AreEqual(false, _explorer.Solution[3, 0].Explored);
            Assert.AreEqual(false, _explorer.Solution[4, 0].Explored);
        }

        [TestMethod]
        public void ExploreFromAnIsolatedAreaAndFailToExploreBlockedNodes()
        {
            SetupWorld(5, 1);
            _world[3, 0] = false;

            _explorer.MovementMode = MovementMode.FourDirection;
            _explorer.MaxCost = 28;
            _explorer.ExploreToCompletion(new Coordinate(0, 0));

            Assert.AreEqual(ExplorerState.Completed, _explorer.State);

            // Explored Area
            Assert.AreEqual(0, _explorer.Solution[1, 0].ParentX);
            Assert.AreEqual(0, _explorer.Solution[1, 0].ParentY);

            // Unexplored Area
            Assert.AreEqual(false, _explorer.Solution[3, 0].Explored);
            Assert.AreEqual(false, _explorer.Solution[4, 0].Explored);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExploreFromOutsideTheLowerBoundsOfXAndGetAnOutOfRangeException()
        {
            var world = WorldBuilder.CreateStandardEmptyWorld();
            var explorer = new PathfinderEngine(world.Width, world.Height, new BooleanMapCostCalculator(world), new EmptyHeuristicCalculator());

            explorer.ExploreFrom(new Coordinate(-1, 0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExploreFromOutsideTheUpperBoundsOfXAndGetAnOutOfRangeException()
        {
            var world = WorldBuilder.CreateStandardEmptyWorld();
            var explorer = new PathfinderEngine(world.Width, world.Height, new BooleanMapCostCalculator(world), new EmptyHeuristicCalculator());

            explorer.ExploreFrom(new Coordinate(world.Width, 0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExploreFromOutsideTheLowerBoundsOfYAndGetAnOutOfRangeException()
        {
            var world = WorldBuilder.CreateStandardEmptyWorld();
            var explorer = new PathfinderEngine(world.Width, world.Height, new BooleanMapCostCalculator(world), new EmptyHeuristicCalculator());

            explorer.ExploreFrom(new Coordinate(0, -1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExploreFromOutsideTheUpperBoundsOfYAndGetAnOutOfRangeException()
        {
            var world = WorldBuilder.CreateStandardEmptyWorld();
            var explorer = new PathfinderEngine(world.Width, world.Height, new BooleanMapCostCalculator(world), new EmptyHeuristicCalculator());

            explorer.ExploreFrom(new Coordinate(0, world.Height));
        }
    }
}