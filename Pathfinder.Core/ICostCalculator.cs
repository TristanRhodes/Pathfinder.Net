using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core
{
    public interface ICostCalculator
    {
        bool OpenCheck(int x, int y);

        bool OpenCheck(int fromX, int fromY, int toX, int toY, int vectorX, int vectorY);

        int CalculateCost(int fromX, int fromY, int toX, int toY, int vectorX, int vectorY);
    }
}
