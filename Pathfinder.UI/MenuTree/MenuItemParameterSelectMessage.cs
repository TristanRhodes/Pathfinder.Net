using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhodes.WpfSandbox.MenuTree
{
    public class MenuItemParameterSelectMessage
    {
        public MenuItemParameterSelectMessage(object parameter)
        {
            Parameter = parameter;
        }

        public object Parameter { get; private set; }
    }
}
