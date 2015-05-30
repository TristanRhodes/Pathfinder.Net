using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pathfinder.Core
{
    public class World<T>
    {
        private T[,] _cells;


        /// <summary>
        /// Constructor for serialization purposes only
        /// </summary>
        public World() { }

        public World(int width, int height, T value)
        {
            if (width < 1)
                throw new ArgumentOutOfRangeException("width", "Must be greater than 0");

            if (height < 1)
                throw new ArgumentOutOfRangeException("height", "Must be greater than 0");

            _cells = new T[width, height];

            this.Width = width;
            this.Height = height;
            Flood(value);
        }


        public int Width { get; private set; }

        public int Height { get; private set; }


        public T this[int x, int y]
        {
            get
            {
                return _cells[x, y];
            }

            set
            {
                _cells[x, y] = value;
            }
        }


        public void Flood(T value)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    this[x, y] = value;
                }
            }
        }
    }
}
