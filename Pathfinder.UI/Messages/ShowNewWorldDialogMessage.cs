using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace Pathfinder.UI.Messages
{
    public class ShowNewWorldDialogMessage : CallbackMessage<ShowNewWorldDialogMessage.Params>
    {
        public class Params
        {
            public Params(int width, int height)
            {
                Debug.Assert(width > 2);
                Debug.Assert(height > 2);

                Width = width;
                Height = height;
            }

            public int Width { get; set; }

            public int Height { get; set; }
        }

        public ShowNewWorldDialogMessage(Action<Params> callback) 
            :base(callback)
        { 
        }
    }
}
