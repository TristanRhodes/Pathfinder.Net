using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using Pathfinder.Core;
using Pathfinder.Core.CostCalculators;
using Pathfinder.UI.Commands;
using Pathfinder.UI.Enums;
using Pathfinder.UI.Messages;
using Pathfinder.UI.Services;
using Pathfinder.UI.MenuTree;

namespace Pathfinder.UI.ViewModels
{
    public class PathfinderViewModel : PathfinderViewModelBase
    {
        // Core Components
        private PathfinderEngine _pathfinder;
        private World<bool> _world;
   
        // View Models
        private MapHostViewModel _mapHostViewModel;

        // Part Descriptor Parts
        private string _pathLabel;
        private string _pathNodesText;

        // Modifiers
        private MovementMode _movementMode = MovementMode.FourDirection;
        private bool _blockPartialDiagonals = false;
        private int _diagonalMovementCost = 14;
        private int _edgeMovementCost = 10;
        private int _heuristicMultiplier = 1;
        
        // Current pin positions
        private Coordinate _source;
        private Coordinate _target;


        // Command Holder
        private List<ICommand> _commands = new List<ICommand>();


        // Dependencies
        public IFileService FileService { get; private set; }


        public PathfinderViewModel(IFileService fileService)
        {
            this.FileService = fileService;

            _mapHostViewModel = CreateMap();
        }


        private MapHostViewModel CreateMap()
        {
            return new MapHostViewModel();
        }


        protected override void SubscribeToEvents()
        {
            this.MessengerInstance.Register<TickMessage>(this, HandleTick);
            this.MessengerInstance.Register<ExecuteToolBarCommandMessage>(this, HandleToolBarCommand);
        }


        public MapHostViewModel MapHost
        {
            get { return _mapHostViewModel; }
        }


        public string PathLabel
        {
            get { return _pathLabel; }

            set
            {
                if (_pathLabel == value)
                    return;

                _pathLabel = value;
                RaiseSmartPropertyChanged();
            }
        }

        public string PathNodesText
        {
            get { return _pathNodesText; }
            set
            {
                if (_pathNodesText == value)
                    return;

                _pathNodesText = value;
                RaiseSmartPropertyChanged();
            }
        }


        public MovementMode ExploreMode
        {
            get { return _movementMode; }
            set 
            {
                if (_movementMode == value)
                    return;

                _movementMode = value;
                RaiseSmartPropertyChanged();
            }
        }

        public bool BlockPartialDiagonals
        {
            get { return _blockPartialDiagonals; }
            set
            {
                if (_blockPartialDiagonals == value)
                    return;

                _blockPartialDiagonals = value;
                RaiseSmartPropertyChanged();
            }
        }

        public int HeuristicMultiplier
        {
            get { return _heuristicMultiplier; }
            set 
            {
                if (_heuristicMultiplier == value)
                    return;

                _heuristicMultiplier = value;
                RaiseSmartPropertyChanged();
            }
        }

        public int EdgeMovementCost
        {
            get { return _edgeMovementCost; }
            set 
            {
                if (_edgeMovementCost == value)
                    return;

                _edgeMovementCost = value;
                RaiseSmartPropertyChanged();
            }
        }

        public int DiagonalMovementCost
        {
            get { return _diagonalMovementCost; }
            set
            {
                if (_diagonalMovementCost == value)
                    return;

                _diagonalMovementCost = value;
                RaiseSmartPropertyChanged();
            }
        }


        public void CreateNewWorld(int width, int height)
        {
            var world = new World<bool>(width, height, true);
            LoadWorld(world);
            ConfigureMenu();
        }
        
        public void SaveWorld(string path)
        {
            this.SafeExecute(() => 
                    {
                        this.FileService.SaveWorld(MapHost.World, path);
                    });
        }
        
        public void LoadWorld(string path)
        {
            this.SafeExecute(() =>
                    {
                        var world = this.FileService.LoadWorld(path);
                        LoadWorld(world);
                    });
        }

        public void LoadWorld(World<bool> world)
        {
            _world = world;
            MapHost.LoadWorld(world);

            RefreshWorkQueue();
        }


        public bool CanToggleNode(NodeViewModel node)
        {
            return true;
        }

        public void ToggleNode(NodeViewModel node)
        {
            var coordinate = new Coordinate(node.XPosition, node.YPosition);
            var state = !(MapHost.World[node.XPosition, node.YPosition]);

            var command = new ToggleNodeCommand(MapHost, coordinate, state);
            _commands.Add(command);

            command.Execute(null);
        }


        public bool CanExploreFrom(NodeViewModel node)
        {
            // Check Cell Open
            if (!node.Open)
                return false;

            // Pass
            return true;
        }

        public void ExploreFrom(NodeViewModel node)
        {
            var coordinate = new Coordinate(node.XPosition, node.YPosition);
            ExploreFrom(coordinate);
        }

        public void ExploreFromBack()
        {
            MapHost.HideBluePin();
            MapHost.ClearMap();

            ClearPathfinder();
        }


        public bool CanSetExploreBetweenSource(NodeViewModel node)
        {
            // Check Cell Open
            if (!node.Open)
                return false;

            // Pass
            return true;
        }

        public void SetExploreBetweenSource(NodeViewModel node)
        {
            _source = new Coordinate(node.XPosition, node.YPosition);
            MapHost.ShowBluePin(_source);
        }

        public void SetExploreBetweenSourceBack()
        {
            MapHost.HideBluePin();
            MapHost.ClearMap();
            ClearPathfinder();
        }


        public bool CanSetExploreBetweenTarget(NodeViewModel node)
        {
            // Check Cell Open
            if (!node.Open)
                return false;

            // Check Blue Pin
            var coordinate = new Coordinate(node.XPosition, node.YPosition);
            if (MapHost.IsBluePinAtPosition(coordinate))
                return false;

            // Pass
            return true;
        }

        public void SetExploreBetweenTarget(NodeViewModel node)
        {
            _target = new Coordinate(node.XPosition, node.YPosition);

            // Green Pin
            MapHost.ShowGreenPin(_target);

            // Explore
            ExploreBetween(_source, _target);
        }

        public void SetExploreBetweenTargetBack()
        {
            MapHost.HideGreenPin();
            SetExploreBetweenSourceBack();
        }


        private void ExploreFrom(Coordinate from)
        {
            // Set Pins
            MapHost.ShowBluePin(from);

            // Setup
            var costCalculator = new BooleanMapCostCalculator(_world)
            {
                BlockPartialDiagonals = BlockPartialDiagonals,
                HorizontalAndVerticalMovementCost = EdgeMovementCost,
                DiagonalMovementCost = DiagonalMovementCost
            };
            var heuristic = new EmptyHeuristicCalculator();
            var pathfinder = new PathfinderEngine(_world.Width, _world.Height, costCalculator, heuristic, _movementMode);

            PerformExplore(pathfinder, from);
        }

        private void ExploreBetween(Coordinate from, Coordinate to)
        {
            // Set Pins
            MapHost.ShowBluePin(from);
            MapHost.ShowGreenPin(to);

            // Setup
            var costCalculator = new BooleanMapCostCalculator(_world) 
            { 
                BlockPartialDiagonals = BlockPartialDiagonals,
                HorizontalAndVerticalMovementCost = EdgeMovementCost,
                DiagonalMovementCost = DiagonalMovementCost
            };
            var heuristic = new TargetedHeuristic(to);
            var pathfinder = new PathfinderEngine(_world.Width, _world.Height, costCalculator, heuristic, _movementMode);

            PerformExplore(pathfinder, from);
        }

        private void PerformExplore(PathfinderEngine pathfinder, Coordinate from)
        {
            MapHost.ClearMap();

            _pathfinder = pathfinder;
            _pathfinder.ExploreFrom(from);

            RefreshWorkQueue();
        }


        private bool IsInQueue(int x, int y)
        {
            return _pathfinder.WorkQueue.Any(n => n.X == x && n.Y == y);
        }


        private void HandleToolBarCommand(ExecuteToolBarCommandMessage msg)
        {
            base.SafeExecute(() =>
            {
                switch (msg.Command)
                {
                    case ToolBarCommand.NewMap:
                        HandleCreateNewWorld();
                        break;

                    case ToolBarCommand.SaveMap:
                        HandleSaveWorld();
                        break;

                    case ToolBarCommand.LoadMap:
                        HandleLoadWorld();
                        break;

                    default:
                        break;
                }
            });
        }

        private void HandleTick(TickMessage msg)
        {
            Tick(msg.Ticks);
        }


        private void HandleCreateNewWorld()
        {
            var msg = new ShowNewWorldDialogMessage((param) => { CreateNewWorld(param.Width, param.Height); });
            this.MessengerInstance.Send(msg);
        }

        private void HandleSaveWorld()
        {
            var msg = new ShowSaveWorldDialogMessage((path) => { SaveWorld(path); });
            msg.Filter = "Pathmap Files (*.pmap)|*.pmap";

            this.MessengerInstance.Send(msg);
        }

        private void HandleLoadWorld()
        {
            var msg = new ShowLoadWorldDialogMessage((path) => { LoadWorld(path); });
            msg.Filter = "Pathmap Files (*.pmap)|*.pmap";

            this.MessengerInstance.Send(msg);
        }


        private void Tick(int ticks)
        {
             // Ignore if not working.
            if (_pathfinder == null ||_pathfinder.State != ExplorerState.Working)
                return;

            for (int i = 0; i < ticks; i++)
            {
                _pathfinder.Tick(1);
            }

            RefreshWorkQueue();
            RefreshNodeText();

            DrawHeuristicLines();
        }


        private void RefreshWorkQueue()
        {
            if (_pathfinder == null)
            {
                PathLabel = "-";
                PathNodesText = string.Empty;

                return;
            }

            PathLabel = string.Format("{0}", _pathfinder.WorkQueue.Count);

            // Update Queue
            var sb = new StringBuilder();
            foreach (var item in _pathfinder.WorkQueue)
            {
                var text = item.ToString();
                sb.AppendLine(text);
            }

            PathNodesText = sb.ToString();
        }

        private void RefreshNodeText()
        {
            for (var x = 0; x < _world.Width; x++)
            {
                for (var y = 0; y < _world.Height; y++)
                {
                    var node = _pathfinder.Solution[x, y];

                    var inQueue = IsInQueue(x, y);
                    var text = node.Explored ?
                        string.Format("({1}, {2}) {0}C: {3}{0}H: {4}{0}V: {5}",
                        Environment.NewLine,
                        node.X,
                        node.Y,
                        node.Cost,
                        node.Heuristic,
                        node.Value)
                        : string.Empty;

                    MapHost.UpdateNode(x, y, inQueue, text);
                }
            }
        }


        private void DrawHeuristicLines()
        {
            if (_pathfinder.State != ExplorerState.Completed)
                return;

            var lineList = new LinkedList<LineViewModel>();

            if (_pathfinder.HeuristicCalculator is EmptyHeuristicCalculator)
                DrawEmptyHeuristicLines(lineList);
            else if (_pathfinder.HeuristicCalculator is TargetedHeuristic)
                DrawTargetedHeuristicLines(lineList);

            // Load Lines
            MapHost.ClearNodes();
            MapHost.DrawLines(lineList);
        }

        private void DrawEmptyHeuristicLines(LinkedList<LineViewModel> lineList)
        {
            for (int x = 0; x < _world.Width; x++)
            {
                for (int y = 0; y < _world.Height; y++)
                {
                    var solution = _pathfinder.Solution[x, y];

                    if (solution.X == solution.ParentX && solution.Y == solution.ParentY)
                        continue;

                    // Add Line
                    var fromX = solution.X;
                    var fromY = solution.Y;

                    var toX = solution.ParentX;
                    var toY = solution.ParentY;

                    lineList.AddLast(new LineViewModel(fromX, fromY, toX, toY));
                }
            }
        }

        private void DrawTargetedHeuristicLines(LinkedList<LineViewModel> lineList)
        {
            PathfinderNode current = _pathfinder.Solution[_target.X, _target.Y];

            if (!current.Explored)
                return;

            while (current.X != _source.X || current.Y != _source.Y)
            {
                // Add Line
                var fromX = current.X;
                var fromY = current.Y;

                var toX = current.ParentX;
                var toY = current.ParentY;

                lineList.AddLast(new LineViewModel(fromX, fromY, toX, toY));

                current = _pathfinder.Solution[toX, toY];
            }
        }


        private void ClearPathfinder()
        {
            if (_pathfinder != null)
                _pathfinder.Stop();
        }

        private void ConfigureMenu()
        {
            // Create menu and notify control of root messages
            var menu = CreateMenu();
            var msg = new SetRootMenuMessage(menu);
            this.MessengerInstance.Send(msg);
        }

        /// <summary>
        /// Create command menu for use in the MenuTree.
        /// </summary>
        /// <returns></returns>
        private MenuItem CreateMenu()
        {
            var rootItem = new MenuItem();
            rootItem.MenuHeader = "Home";
            rootItem.MenuHeaderImagePath = "/Pathfinder.UI;component/Resources/Command-Home.png";

            rootItem.Children.Add(new MenuItem()
            {
                ExecuteCommand = new RelayCommand<NodeViewModel>(ToggleNode, CanToggleNode),
                TargetType = MenuItemTarget.SingleTarget,
                MenuHeader = "Toggle Node",
                MenuHeaderImagePath = "/Pathfinder.UI;component/Resources/Command-Toggle.png",
                ButtonToolTip = "Toggle Node",
                ButtonImagePath = "/Pathfinder.UI;component/Resources/Command-Toggle.png"
            });
            rootItem.Children.Add(new MenuItem()
            {
                ExecuteCommand = new RelayCommand<NodeViewModel>(ExploreFrom, CanExploreFrom),
                UndoCommand = new RelayCommand(ExploreFromBack),
                TargetType = MenuItemTarget.SingleTarget,
                MenuHeader = "Explore from",
                MenuHeaderImagePath = "/Pathfinder.UI;component/Resources/Pin-Blue.png",
                ButtonToolTip = "Solve whole map",
                ButtonImagePath = "/Pathfinder.UI;component/Resources/Map-One-Pin.png"
            });
            rootItem.Children.Add(new MenuItem()
            {
                Next = new MenuItem()
                {
                    ExecuteCommand = new RelayCommand<NodeViewModel>(SetExploreBetweenTarget, CanSetExploreBetweenTarget),
                    UndoCommand = new RelayCommand(SetExploreBetweenTargetBack),
                    TargetType = MenuItemTarget.SingleTarget,
                    MenuHeader = "Place Target Pin",
                    MenuHeaderImagePath = "/Pathfinder.UI;component/Resources/Pin-Green.png",
                },
                ExecuteCommand = new RelayCommand<NodeViewModel>(SetExploreBetweenSource, CanSetExploreBetweenSource),
                UndoCommand = new RelayCommand(SetExploreBetweenSourceBack),
                TargetType = MenuItemTarget.SingleTarget,
                MenuHeader = "Place Source Pin",
                MenuHeaderImagePath = "/Pathfinder.UI;component/Resources/Pin-Blue.png",
                ButtonToolTip = "Explore between two points",
                ButtonImagePath = "/Pathfinder.UI;component/Resources/Map-Two-Pin.png"
            });

            return rootItem;
        }
    }
}
