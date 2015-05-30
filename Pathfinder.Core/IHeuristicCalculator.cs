using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core
{
    public interface IHeuristicCalculator
    {
        int Calculate(int x, int y);

        bool Complete(int newX, int newY);
    }

    /// <summary>
    /// Default empty heuristic evaluator. Used to explore equally in all directions, based entirely on cost.
    /// </summary>
    public class EmptyHeuristicCalculator : IHeuristicCalculator
    {
        public int Calculate(int x, int y)
        {
            return 0;
        }


        public bool Complete(int newX, int newY)
        {
            return false;
        }
    }

    /// <summary>
    /// Heuristic gets lower the closer you get to the target
    /// </summary>
    public class TargetedHeuristic : IHeuristicCalculator
    {
        public TargetedHeuristic() 
            : this(new Coordinate(0, 0))
        {
        }

        public TargetedHeuristic(Coordinate coordinate)
        {
            Target = coordinate;
            HeuristicScale = 1;
        }


        public Coordinate Target { get; set; }

        public int HeuristicScale { get; set; }


        public int Calculate(int x, int y)
        {
            var xDif = Target.X - x;
            var yDif = Target.Y - y;

            return ((xDif * xDif) + (yDif * yDif)) * HeuristicScale;
        }


        public bool Complete(int newX, int newY)
        {
            return (newX == Target.X && newY == Target.Y);
        }
    }
}
