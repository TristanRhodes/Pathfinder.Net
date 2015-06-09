using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Pathfinder.UI.Enums;
using Pathfinder.UI.Messages;

namespace Pathfinder.UI.ViewModels
{
    public class ToolBarViewModel : PathfinderViewModelBase
    {
        protected override void SetupCommands()
        {
            this.NewMapCommand = new RelayCommand(ExecuteNewMap);
            this.SaveMapCommand = new RelayCommand(ExecuteSaveMap);
            this.LoadMapCommand = new RelayCommand(ExecuteLoadMap);
        }


        public ICommand NewMapCommand { get; private set; }

        public ICommand LoadMapCommand { get; private set; }

        public ICommand SaveMapCommand { get; private set; }


        public void ExecuteNewMap()
        {
            var msg = new ExecuteToolBarCommandMessage(ToolBarCommand.NewMap);
            MessengerInstance.Send(msg);
        }

        public void ExecuteSaveMap()
        {
            var msg = new ExecuteToolBarCommandMessage(ToolBarCommand.SaveMap);
            MessengerInstance.Send(msg);
        }

        public void ExecuteLoadMap()
        {
            var msg = new ExecuteToolBarCommandMessage(ToolBarCommand.LoadMap);
            MessengerInstance.Send(msg);
        }
    }
}
