using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.UI.ViewModels
{
    public class MapObjectViewModel : PathfinderViewModelBase
    {
        private int _x;
        private int _y;
        private bool _visible;


        public int XPosition
        {
            get { return _x; }
            set 
            {
                if (_x == value)
                    return;

                _x = value;
                RaiseSmartPropertyChanged();
            }
        }

        public int YPosition
        {
            get { return _y; }
            set
            {
                if (_y == value)
                    return;

                _y = value;
                RaiseSmartPropertyChanged();
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set 
            {
                if (_visible == value)
                    return;

                _visible = value;
                RaiseSmartPropertyChanged();
            }
        }
    }

    public class BluePinViewModel : MapObjectViewModel { }

    public class GreenPinViewModel : MapObjectViewModel { }
}
