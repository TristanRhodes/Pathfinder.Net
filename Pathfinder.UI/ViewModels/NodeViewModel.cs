using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.UI.ViewModels
{
    public class NodeViewModel : MainViewModel
    {
        private int _xPosition;
        private int _yPositon;

        private bool _open;
        private bool _isInWorkQueue;

        private String _text;


        public int XPosition
        {
            get { return _xPosition; }
            set { _xPosition = value; }
        }

        public int YPosition
        {
            get { return _yPositon; }
            set { _yPositon = value; }
        }


        public bool Open
        {
            get { return _open; }
            set 
            {
                if (_open == value)
                    return;

                _open = value;
                RaiseSmartPropertyChanged();
            }
        }


        public bool IsInWorkQueue
        {
            get { return _isInWorkQueue; }
            set
            {
                if (_isInWorkQueue == value)
                    return;

                _isInWorkQueue = value;
                RaiseAllPropertyChanged();
            }
        }


        public String Text
        {
            get { return _text; }
            set 
            {
                if (_text == value)
                    return;

                _text = value;
                RaiseSmartPropertyChanged();
            }
        }
    }
}
