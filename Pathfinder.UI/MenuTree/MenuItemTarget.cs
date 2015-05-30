using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhodes.WpfSandbox.MenuTree
{
    public enum MenuItemTarget
    {
        /// <summary>
        /// Execute instantly
        /// </summary>
        None,

        /// <summary>
        /// Move to a targeting state and await selection input from external source.
        /// </summary>
        SingleTarget,
    }
}
