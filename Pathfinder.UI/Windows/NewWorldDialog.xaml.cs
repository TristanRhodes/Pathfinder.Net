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
using System.Windows.Shapes;

namespace Pathfinder.UI.Windows
{
    /// <summary>
    /// Interaction logic for NewWorldDialog.xaml
    /// </summary>
    public partial class NewWorldDialog : Window
    {
        public NewWorldDialog()
        {
            InitializeComponent();

            MapWidth = 5;
            MapHeight = 5;

            DataContext = this;
        }

        public int MapWidth
        {
            get;
            set;
        }

        public int MapHeight
        {
            get;
            set;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
