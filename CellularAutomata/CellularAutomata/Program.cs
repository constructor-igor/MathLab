using System;

namespace CellularAutomata
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sample1();
            //Sample2();
            //Sample3_no_way();
            Sample4_all_0(); 
        }

        protected static void Sample1()
        {
            var dataM = new[]
            {
                new[] {1, 0, 1, 1, 1},
                new[] {1, 0, 0, 0, 1},
                new[] {1, 1, 1, 0, 1},
                new[] {1, 1, 1, 0, 1},
                new[] {1, 0, 0, 0, 0},
                new[] {1, 0, 1, 1, 1},
                new[] {1, 0, 1, 1, 1}
            };
            var M = new Matrix<int>(dataM);
            var target = new Coordinate(x: 1, y: 2);

            var algorithm = new CA2();

            Matrix<double> concentrations = algorithm.Run(M, target, targetValue: 2);
            concentrations.Print("concentrations result");

            var source = new Coordinate(5, 5);
            //var source = new Coordinate(7, 2);
            MatrixPath mazePath = algorithm.FindPath(concentrations, source, target);
            mazePath.Print("found maze path: ");
        }
        protected static void Sample2()
        {
            var dataM = new[]
            {
                new[] {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                new[] {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                new[] {1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
                new[] {1, 1, 0, 1, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1 },
                new[] {1, 1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1 },
                new[] {1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1 },
                new[] {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 },
                new[] {1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1 },
                new[] {1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 1, 1 },
                new[] {1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1 },
                new[] {1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 1 },
                new[] {1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1 },
                new[] {1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1 },
                new[] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 1 },
                new[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1 },
                new[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1}
            };
            var M = new Matrix<int>(dataM);
            var target = new Coordinate(x: 1, y: 3);

            var algorithm = new CA2();

            Matrix<double> concentrations = algorithm.Run(M, target, targetValue: 2);
            concentrations.Print("concentrations result");

            var source = new Coordinate(16, 12);
            MatrixPath mazePath = algorithm.FindPath(concentrations, source, target);
            mazePath.Print("found maze path: ");
        }
        private static void Sample3_no_way()
        {
            var dataM = new[]
            {
                new[] {1, 0, 1, 1, 1},
                new[] {1, 0, 0, 0, 1},
                new[] {1, 1, 1, 1, 1},
                new[] {1, 1, 1, 0, 1},
                new[] {1, 0, 0, 0, 0},
                new[] {1, 0, 1, 1, 1},
                new[] {1, 0, 1, 1, 1}
            };
            var M = new Matrix<int>(dataM);
            var target = new Coordinate(x: 1, y: 2);

            var algorithm = new CA2();

            Matrix<double> concentrations = algorithm.Run(M, target, targetValue: 2);
            concentrations.Print("concentrations result");

            var source = new Coordinate(5, 5);
            //var source = new Coordinate(7, 2);
            MatrixPath mazePath = algorithm.FindPath(concentrations, source, target);
            if (mazePath!=null)
                mazePath.Print("found maze path: ");
            else
                Console.WriteLine("not found maze path");
        }
        private static void Sample4_all_0()
        {
            var dataM = new[]
            {
                new[] {0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 0},
                new[] {0, 0, 0, 0, 0}
            };
            var M = new Matrix<int>(dataM);
            var target = new Coordinate(x: 1, y: 2);

            var algorithm = new CA2();

            Matrix<double> concentrations = algorithm.Run(M, target, targetValue: 2);
            concentrations.Print("concentrations result");

            var source = new Coordinate(5, 5);
            //var source = new Coordinate(7, 2);
            MatrixPath mazePath = algorithm.FindPath(concentrations, source, target);
            if (mazePath != null)
                mazePath.Print("found maze path: ");
            else
                Console.WriteLine("not found maze path");
        }
    }
}
