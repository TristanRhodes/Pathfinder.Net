using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Pathfinder.Core
{
    public class PathfinderEngine
    {
        private int _width;
        private int _height;

        private PathNodeHeap _workingQueue = new PathNodeHeap();

        private PathfinderNode[,] _solution;
        
        private Coordinate[] _moveVectors;

        private MovementMode _explorationMode;


        public PathfinderEngine(int width, int height, 
            ICostCalculator costCalculator, 
            IHeuristicCalculator heuristicCalculator, 
            MovementMode explorationMode = MovementMode.FourDirection)
        {
            Debug.Assert(width > 0, "width <= 0");
            Debug.Assert(height > 0, "height <= 0");

            _width = width;
            _height = height;

            this.MovementMode = explorationMode;

            _solution = new PathfinderNode[width, height];

            CostCalculator = costCalculator;
            HeuristicCalculator = heuristicCalculator;
        }


        public BinaryHeap<PathfinderNode> WorkQueue
        {
            get { return _workingQueue; }
        }

        public PathfinderNode[,] Solution
        {
            get
            {
                return _solution;
            }
        }


        public Coordinate Source { get; private set; }

        public int? MaxCost { get; set; }

        public ExplorerState State { get; private set; }


        public ICostCalculator CostCalculator { get; set; }

        public IHeuristicCalculator HeuristicCalculator { get; set; }


        public MovementMode MovementMode 
        {
            get { return _explorationMode; } 

            set
            {
                _explorationMode = value;

                switch (_explorationMode)
                {
                    case MovementMode.FourDirection:
                        this._moveVectors = MoveVectorConstants.FourWayMoveVector;
                        break;

                    case MovementMode.EightDirection:
                        this._moveVectors = MoveVectorConstants.EightWayMoveVector;
                        break;

                    default:
                        throw new InvalidOperationException(string.Format("ExplorationMode '{0}' not handled. ", _explorationMode));
                }
            }
        }


        public void ExploreFrom(Coordinate from)
        {
            if (from.X < 0 || from.X > _width - 1 ||
                from.Y < 0 || from.Y > _height - 1)
                throw new ArgumentOutOfRangeException("from", "Outside bounds of map");

            Source = from;
            State = ExplorerState.Working;

            ClearSolution();

            var root = new PathfinderNode();
            root.Heuristic = 0;
            root.ParentX = from.X;
            root.ParentY = from.Y;
            root.X = from.X;
            root.Y = from.Y;
            root.Cost = 0;
            root.Heuristic = 0;
            root.Value = root.Cost + root.Heuristic;

            _workingQueue.Add(root);
        }
        
        public void Tick(int ticks)
        {
            // Break if we are not working
            if (State != ExplorerState.Working)
                return;

            for (int i = 0; i < ticks; i++)
            {               
                // Completed - if the queue is empty
                if (_workingQueue.Count == 0)
                {
                    State = ExplorerState.Completed;
                    return;
                }

                // Dequeue
                var current = _workingQueue.Remove();
               
                // Discard if we already have a better solution
                var better = HasBetterExistingSolutionOnMap(current.X, current.Y, current.Cost);
                if (better)
                {
                    i -= 1;
                    continue;
                }

                // Store current solution
                _solution[current.X, current.Y] = current;

                // If the heuristic is complete
                if (HeuristicCalculator.Complete(current.X, current.Y))
                {
                    State = ExplorerState.Completed;
                    return;
                }

                // Explore Neighbours
                for (int j = 0; j < _moveVectors.Length; j++)
                {
                    var moveVector = _moveVectors[j];

                    // Calculate new coordinates
                    var newX = current.X + moveVector.X;
                    var newY = current.Y + moveVector.Y;

                    // Discard if the cell is not open / available
                    if (!CostCalculator.OpenCheck(current.X, current.Y, newX, newY, moveVector.X, moveVector.Y))
                        continue;

                    // Evaluate Node
                    int cost = current.Cost + CostCalculator.CalculateCost(current.X, current.Y, newX, newY, moveVector.X, moveVector.Y);
                    int heuristic = HeuristicCalculator.Calculate(newX, newY);
                    var value = heuristic + cost;

                    // Discard if we have exceeded our maximum cost
                    if (MaxCost.HasValue && cost > MaxCost.Value)
                        continue;

                    // Discard if we already have a better solution on map or in queue
                    better = HasBetterExistingSolutionOnMap(newX, newY, cost);
                    if (better)
                        continue;

                    better = HasBetterExistingSolutionInQueue(newX, newY, cost);
                    if (better)
                        continue;

                    // Enqueue New Tile
                    var newNode = new PathfinderNode();
                    newNode.X = newX;
                    newNode.Y = newY;

                    newNode.ParentX = current.X;
                    newNode.ParentY = current.Y;

                    newNode.Cost = cost;
                    newNode.Heuristic = heuristic;
                    newNode.Value = value;

                    newNode.Explored = true;

                    // Store solution in map
                    _workingQueue.Add(newNode);
                }
            }
        }


        private bool HasBetterExistingSolutionInQueue(int newX, int newY, int cost)
        {
            return _workingQueue.HasBetterExistingSolutionInQueue(newX, newY, cost);
        }

        private bool HasBetterExistingSolutionOnMap(int newX, int newY, int cost)
        {
            var current = _solution[newX, newY];

            return current.Explored ? current.Cost <= cost : false;
        }


        private void ClearSolution()
        {
            for (int x = 0; x < _solution.GetLength(0); x++)
            {
                for (int y = 0; y < _solution.GetLength(1); y++)
                {
                    _solution[x, y] = new PathfinderNode()
                    {
                        Explored = false
                    };
                }
            }

            _workingQueue.Clear();
        }

        public void Stop()
        {
            State = ExplorerState.Stopped;
            _workingQueue.Clear();
        }
    }
}