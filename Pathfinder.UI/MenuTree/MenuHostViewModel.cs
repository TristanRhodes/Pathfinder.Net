using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Pathfinder.UI.MenuTree
{
    public class MenuHostViewModel : ViewModelBase
    {
        private MenuItem _currentPage;

        private Stack<MenuItem> _pageStack = new Stack<MenuItem>();
        
        private bool _showCancel;
        private bool _showBack;

        public MenuHostViewModel()
        {
            // Setup Commands
            ActionCommand = new RelayCommand<MenuItem>(Action, CanAction);
            BackCommand = new RelayCommand(Back, CanBack);
            CancelCommand = new RelayCommand(Cancel, CanCancel);

            // Register to events
            MessengerInstance.Register<MenuItemParameterSelectMessage>(this, HandleParameterSelectMessage);
            MessengerInstance.Register<SetRootMenuMessage>(this, HandleSetRootMenuMessage);
        }


        /// <summary>
        /// The current page displayed by this menu host.
        /// </summary>
        public MenuItem CurrentMenuPage
        {
            get { return _currentPage; }
            private set
            {
                if (_currentPage == value)
                    return;

                _currentPage = value;
                RaiseNotifyPropertyChanged();
            }
        }


        /// <summary>
        /// Header for the menu item page
        /// </summary>
        public string Header { get; private set; }

        /// <summary>
        /// The path for the image to display in the header when this menu item is selected
        /// </summary>
        public string HeaderImagePath { get; private set; }

        /// <summary>
        /// The menu options available on this page.
        /// </summary>
        public IEnumerable<MenuItem> MenuOptions { get; private set; }


        public bool ShowBack
        {
            get { return _showBack; }
            set
            {
                if (_showBack == value)
                    return;

                _showBack = value;
                RaiseNotifyPropertyChanged();
            }
        }

        public bool ShowCancel
        {
            get { return _showCancel; }
            set
            {
                if (_showCancel == value)
                    return;

                _showCancel = value;
                RaiseNotifyPropertyChanged();
            }
        }


        public ICommand ActionCommand { get; private set; }


        public ICommand BackCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }


        public bool CanBack()
        {
            return _pageStack.Count > 0;
        }

        public void Back()
        {
            if (CurrentMenuPage.UndoCommand != null)
                CurrentMenuPage.UndoCommand.Execute(null);

            var menuItem = _pageStack.Pop();
            LoadMenuTreeItem(menuItem);
        }


        public bool CanCancel()
        {
            return _pageStack.Count > 0;
        }

        public void Cancel()
        {
            // Unroll whole back stack.
            while(CanBack())
            {
                Back();
            }
        }


        public bool CanAction(MenuItem menuItem)
        {
            return true;
        }

        public void Action(MenuItem menuItem)
        {
            ExecutePageCommand(menuItem);
        }


        private void HandleSetRootMenuMessage(SetRootMenuMessage msg)
        {
            SetHomeMenuItem(msg.Root);
        }

        private void SetHomeMenuItem(MenuItem menuItem)
        {
            _pageStack.Clear();
            LoadMenuTreeItem(menuItem);
        }


        private void HandleParameterSelectMessage(MenuItemParameterSelectMessage message)
        {
            ExecutePageCommand(CurrentMenuPage, message.Parameter);
        }

        private void LoadMenuTreeItem(MenuItem menuItem)
        {
            CurrentMenuPage = menuItem;

            Header = menuItem.MenuHeader;
            HeaderImagePath = menuItem.MenuHeaderImagePath;
            
            ShowCancel = _pageStack.Count > 0;
            ShowBack = _pageStack.Count > 1;
            MenuOptions = menuItem.Children;

            RaiseNotifyPropertyChanged(null);
        }

        private void ExecutePageCommand(MenuItem menuItem, object parameter = null)
        {
            // NOTE: Really don't like this...
            if (menuItem.TargetType != MenuItemTarget.None && parameter == null)
            {
                NavigateToPage(menuItem);
                return;
            }

            var command = menuItem.ExecuteCommand;
            if (command != null && command.CanExecute(parameter))
                command.Execute(parameter);

            if (menuItem.Next != null)
                NavigateToPage(menuItem.Next);
        }

        private void NavigateToPage(MenuItem menuItem)
        {
            if (CurrentMenuPage != null)
                _pageStack.Push(CurrentMenuPage);

            LoadMenuTreeItem(menuItem);
        }


        public void RaiseNotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            RaisePropertyChanged(propertyName);
        }
    }
}
