using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhodes.WpfSandbox.MenuTree;

namespace Rhodes.WpfSandbox.MenuTree
{
    public class SetRootMenuMessage
    {
        public SetRootMenuMessage(MenuItem root)
        {
            if (root == null)
                throw new ArgumentNullException("root");

            Root = root;
        }

        public MenuItem Root { get; private set; }
    }
}
