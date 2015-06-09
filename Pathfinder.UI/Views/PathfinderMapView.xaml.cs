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
using Pathfinder.UI.ViewModels;

namespace Pathfinder.UI.Views
{
    /// <summary>
    /// Interaction logic for PathfinderMapView.xaml
    /// </summary>
    public partial class PathfinderMapView : UserControl
    {
        public PathfinderMapView()
        {
            InitializeComponent();
        }

        public MapHostViewModel ViewModel 
        {
            get { return (MapHostViewModel)DataContext; }
        }


        public void NodeRoot_MouseLeave(object sender, EventArgs e)
        {
            NodeDetails.Content = string.Empty;
        }

        public void NodeRoot_MouseMove(object sender, MouseEventArgs e)
        {
            var element = ((FrameworkElement)sender);
            if (element == null)
                return;

            var node = element.DataContext as NodeViewModel;
            if (node == null)
                return;

            NodeDetails.Content = string.Format("X:{0}, Y:{1}", node.XPosition, node.YPosition);
        }

        private void NodeRoot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsValidMouseAction())
                return;

            var element = ((FrameworkElement)sender);
            if (element == null)
                return;

            var node = element.DataContext as NodeViewModel;
            if (node == null)
                return;

            ViewModel.NodeClick(node);
        }

        private bool IsValidMouseAction()
        {
            var keys = new Key[] { Key.LeftCtrl, Key.RightCtrl, Key.LeftShift, Key.RightShift };
            
            foreach(var key in keys)
            {
                if (Keyboard.IsKeyDown(key))
                    return false;
            }

            return true;
        }


    }
}
