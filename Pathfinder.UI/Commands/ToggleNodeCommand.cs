using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Pathfinder.Core;
using Pathfinder.UI.ViewModels;

namespace Pathfinder.UI.Commands
{
    public class ToggleNodeCommand : ICommand
    {
        private MapHostViewModel _mapHost;
        private Coordinate _coordinate;
        private bool _state;


        public ToggleNodeCommand(MapHostViewModel mapHost, Coordinate coordinate, bool state)
        {
            _mapHost = mapHost;

            _coordinate = coordinate;
            _state = state;
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _mapHost.SetNodeOpenState(_coordinate, _state);
        }


        public event EventHandler CanExecuteChanged;
    }
}
