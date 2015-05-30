using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Pathfinder.Core;
using Pathfinder.Core.CostCalculators;
using Pathfinder.UI.ViewModels;

namespace Pathfinder.UI.Commands
{
    public class ExploreFromCommand : ICommand
    {
        private World<bool> _world;
        private MapHostViewModel _mapHost;
        private Coordinate _from;
        private Coordinate _to;
        private bool _blockPartialDiagonals;
        private MovementMode _moveMode;


        public ExploreFromCommand(
            World<bool> world,
            MapHostViewModel mapHost, 
            Coordinate from, 
            Coordinate to,
            bool blockPartialDiagonals,
            MovementMode moveMode)
        {
            _world = world;
            _mapHost = mapHost;
            _from = from;
            _to = to;
            _blockPartialDiagonals = blockPartialDiagonals;
            _moveMode = moveMode;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // Set Pins
            _mapHost.ShowBluePin(_from);
            _mapHost.ShowGreenPin(_to);

            // Setup
            var costCalculator = new BooleanMapCostCalculator(_world) { BlockPartialDiagonals = _blockPartialDiagonals };
            var heuristic = new TargetedHeuristic(_to);
            var pathfinder = new PathfinderEngine(_world.Width, _world.Height, costCalculator, heuristic, _moveMode);

            _mapHost.ClearMap();

            pathfinder.ExploreFrom(_from);

            // TODO: Load Pathfinder into host
            // TODO: Reset WorkQueue
        }

        public event EventHandler CanExecuteChanged;
    }
}
