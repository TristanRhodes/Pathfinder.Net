using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pathfinder.UI.MenuTree
{
    /// <summary>
    /// Base class for menu items that are displayed in the menu host.
    /// </summary>
    public class MenuItem
    {
        public MenuItem()
        {
            Children = new List<MenuItem>();
        }


        /// <summary>
        /// The tooltip to display for the button
        /// </summary>
        public string ButtonToolTip { get; set; }
        
        /// <summary>
        /// The path for the image to display on this menu item
        /// </summary>
        public string ButtonImagePath { get; set; }


        /// <summary>
        /// Header for the menu item page
        /// </summary>
        public string MenuHeader { get; set; }

        /// <summary>
        /// The path for the image to display in the header when this menu item is selected
        /// </summary>
        public string MenuHeaderImagePath { get; set; }


        /// <summary>
        /// The items this menu can use
        /// </summary>
        public List<MenuItem> Children { get; set; }

        /// <summary>
        /// The target type of the parameter select item
        /// </summary>
        public MenuItemTarget TargetType { get; set; }


        /// <summary>
        /// The next item to move to in the parameter selection
        /// </summary>
        public MenuItem Next { get; set; }


        /// <summary>
        /// Command raised when a parameter is selected
        /// </summary>
        public ICommand ExecuteCommand { get; set; }

        /// <summary>
        /// Command raised when a parameter is unselected (via back)
        /// </summary>
        public ICommand UndoCommand { get; set; }
    }
}
