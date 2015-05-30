using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathfinder.Core;
using Pathfinder.Core.CostCalculators;
using Rhodes.WpfSandbox.MenuTree;

namespace Pathfinder.UI.ViewModels
{
    public class MapHostViewModel : MainViewModel
    {
        // Core Components
        private World<bool> _world;

        // Dimensions
        private double _mapFrameWidth;
        private double _mapFrameHeight;

        // UI Binding Elements
        private NodeViewModel[,] _nodes;
        private ObservableCollection<NodeViewModel> _nodeList = new ObservableCollection<NodeViewModel>();
        private ObservableCollection<LineViewModel> _lineList = new ObservableCollection<LineViewModel>();
        private ObservableCollection<MapObjectViewModel> _mapObjectList = new ObservableCollection<MapObjectViewModel>();

        // Map Entities
        private BluePinViewModel _bluePinMapObject;
        private GreenPinViewModel _greenPinMapObject;


        /// <summary>
        /// MapHostViewModel - 
        /// </summary>
        public MapHostViewModel()
        {
            // Create and setup pins
            _bluePinMapObject = new BluePinViewModel();
            _bluePinMapObject.Visible = false;

            _greenPinMapObject = new GreenPinViewModel();
            _greenPinMapObject.Visible = false;

            // Add to map objects
            _mapObjectList.Add(_bluePinMapObject);
            _mapObjectList.Add(_greenPinMapObject);
        }


        public World<bool> World
        {
            get { return _world; }
        }


        public IEnumerable<NodeViewModel> NodeList
        {
            get { return _nodeList; }
        }

        public IEnumerable<LineViewModel> LineList
        {
            get { return _lineList; }
        }

        public IEnumerable<MapObjectViewModel> MapObjectList
        {
            get { return _mapObjectList; }
        }


        public double MapFrameWidth
        {
            get { return _mapFrameWidth; }
            set
            {
                if (_mapFrameWidth == value)
                    return;

                _mapFrameWidth = value;
                RaiseSmartPropertyChanged();
            }
        }

        public double MapFrameHeight
        {
            get { return _mapFrameHeight; }
            set
            {
                if (_mapFrameHeight == value)
                    return;

                _mapFrameHeight = value;
                RaiseSmartPropertyChanged();
            }
        }


        public void LoadWorld(World<bool> world)
        {
            _world = world;

            MapFrameWidth = _world.Width * UIConstants.CellSize;
            MapFrameHeight = _world.Height * UIConstants.CellSize;

            RedrawMap();
        }


        public void ShowBluePin(Coordinate location)
        {
            _bluePinMapObject.XPosition = location.X;
            _bluePinMapObject.YPosition = location.Y;
            _bluePinMapObject.Visible = true;
        }

        public void HideBluePin()
        {
            _bluePinMapObject.Visible = false;
        }

        public bool IsBluePinAtPosition(Coordinate location)
        {
            if (!_bluePinMapObject.Visible)
                return false;

            return (_bluePinMapObject.XPosition == location.X &&
                    _bluePinMapObject.YPosition == location.Y);
        }


        public void ShowGreenPin(Coordinate location)
        {
            _greenPinMapObject.XPosition = location.X;
            _greenPinMapObject.YPosition = location.Y;
            _greenPinMapObject.Visible = true;
        }

        public void HideGreenPin()
        {
            _greenPinMapObject.Visible = false;
        }

        public bool IsGreenPinAtPosition(Coordinate location)
        {
            if (!_greenPinMapObject.Visible)
                return false;

            return (_greenPinMapObject.XPosition == location.X &&
                    _greenPinMapObject.YPosition == location.Y);
        }


        public void SetNodeOpenState(Coordinate location, bool state)
        {
            _world[location.X, location.Y] = state;
            _nodes[location.X, location.Y].Open = state;
        }


        public void ClearMap()
        {
            ClearNodes();
            ClearLines();
        }


        public void NodeClick(NodeViewModel node)
        {
            var msg = new MenuItemParameterSelectMessage(node);
            MessengerInstance.Send(msg);
        }


        private void RedrawMap()
        {
            // Create new data
            _nodes = new NodeViewModel[_world.Width, _world.Height];
            _nodeList.Clear();

            // Build up
            for (int x = 0; x < _world.Width; x++)
            {
                for (int y = 0; y < _world.Height; y++)
                {
                    DrawNode(x, y);
                }
            }
        }

        private void DrawNode(int x, int y)
        {
            var cellState = _world[x, y];

            //Create and setup
            var cell = new NodeViewModel();
            cell.XPosition = x;
            cell.YPosition = y;
            cell.Open = cellState;
            cell.Text = string.Empty;

            _nodes[x, y] = cell;
            _nodeList.Add(cell);
        }


        public void DrawLines(IEnumerable<LineViewModel> lines)
        {
            _lineList.Clear();
            
            foreach(var line in lines)
            {
                _lineList.Add(line);
            }
        }

        public void ClearLines()
        {
            _lineList.Clear();
        }


        public void UpdateNode(int x, int y, bool inQueue, string text)
        {
            var cell = _nodes[x, y];
            cell.IsInWorkQueue = inQueue;
            cell.Text = text;
        }

        public void ClearNodes()
        {
            for (int x = 0; x < _world.Width; x++)
            {
                for (int y = 0; y < _world.Height; y++)
                {
                    var cell = _nodes[x, y];
                    cell.Text = string.Empty;
                    cell.IsInWorkQueue = false;
                }
            }
        }
    }
}
