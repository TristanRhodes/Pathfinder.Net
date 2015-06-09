using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rhodes.WpfSandbox.MenuTree
{
    /// <summary>
    /// Interaction logic for MenuHost.xaml
    /// </summary>
    public partial class MenuHostView : UserControl
    {
        public MenuHostView()
        {
            InitializeComponent();
            ViewModel = new MenuHostViewModel();
        }

        
        public MenuHostViewModel ViewModel
        {
            get { return (MenuHostViewModel)DataContext; }
            set { DataContext = value; }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            if (element == null)
                return;

            var item = (MenuItem)element.DataContext;
            if (item == null)
                return;

            if (ViewModel.ActionCommand.CanExecute(item))
                ViewModel.ActionCommand.Execute(item);
        }
    }
}
