using System;
using System.Threading.Tasks;

namespace CellularAutomata
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataM = new[]
            {
                new[] { 1, 0, 1, 1, 1 },
                new[] { 1, 0, 0, 0, 1 }, 
                new[] { 1, 1, 1, 0, 1 }, 
                new[] { 1, 1, 1, 0, 1 }, 
                new[] { 1, 0, 0, 0, 0 },
                new[] { 1, 0, 1, 1, 1 },
                new[] { 1, 0, 1, 1, 1 }
            };
            var M = new Matrix<int>(dataM);
            var target = new Coordinate(x:1, y:2);

            CA2 algorithm = new CA2();

            Matrix<double> concentrations = algorithm.Run(M, target, targetValue: 2);
            concentrations.Print("result");
        }
    }
}
