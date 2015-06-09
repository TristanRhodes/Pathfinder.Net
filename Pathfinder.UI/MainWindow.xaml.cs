using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Pathfinder.UI.Messages;
using Pathfinder.UI.Services;
using Pathfinder.UI.ViewModels;
using Pathfinder.UI.Views;
using Pathfinder.UI.Windows;

namespace Pathfinder.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RegisterMessages();

            // Setup new world
            ViewModel.CreateNewWorld(9, 9);
        }


        public PathfinderViewModel ViewModel
        {
            get { return (PathfinderViewModel)DataContext; }
        }


        private void RegisterMessages()
        {
            Messenger.Default.Register<ExceptionMessage>(this, HandleExceptionMessage);
            Messenger.Default.Register<ShowNewWorldDialogMessage>(this, HandleNewWorldDialog);
            Messenger.Default.Register<ShowSaveWorldDialogMessage>(this, HandleSaveWorldDialog);
            Messenger.Default.Register<ShowLoadWorldDialogMessage>(this, HandleLoadWorldDialog);
        }


        private void HandleExceptionMessage(ExceptionMessage message)
        {
            MessageBox.Show(message.Exception.ToString());
        }

        private void HandleNewWorldDialog(ShowNewWorldDialogMessage message)
        {
            var dialog = new NewWorldDialog();
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                message.Callback(new ShowNewWorldDialogMessage.Params(dialog.MapWidth, dialog.MapHeight));
            }
        }

        private void HandleSaveWorldDialog(ShowSaveWorldDialogMessage message)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = message.Filter;

            var result = dialog.ShowDialog(this);

            if (result == true)
            {
                message.Callback(dialog.FileName);
            }
        }

        private void HandleLoadWorldDialog(ShowLoadWorldDialogMessage message)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = message.Filter;

            var result = dialog.ShowDialog(this);

            if (result == true)
            {
                message.Callback(dialog.FileName);
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ResourceDictionary dictionary = LoadTheme();

            if (dictionary != null)
            {
                this.Resources.MergedDictionaries.Clear();
                this.Resources.MergedDictionaries.Add(dictionary);
            }
        }

        private ResourceDictionary LoadTheme()
        {
            string packUri = @"/Pathfinder.UI;component/Themes/ExpressionLight.xaml";
            return Application.LoadComponent(new Uri(packUri, UriKind.Relative)) as ResourceDictionary;
        }
    }
}
