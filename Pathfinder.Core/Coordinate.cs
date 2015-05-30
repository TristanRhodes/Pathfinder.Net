using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder.Core
{
    public struct Coordinate
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;

        public int Y;


        public override string ToString()
        {
            return string.Format("X:{0}, Y:{1}", X, Y);
        }
    }
}
