using System.Collections.Generic;

namespace CellularAutomata
{
    public class MatrixPath
    {
        public readonly List<Coordinate> Path = new List<Coordinate>();

        public void Add(Coordinate point)
        {
            Path.Add(point);
        }
    }
}