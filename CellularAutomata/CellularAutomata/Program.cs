using System;

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
            Matrix<double> concentrations = CA2(M, target, targetValue: 2);
            concentrations.Print("result");
        }

        private static Matrix<double> CA2(Matrix<int> matrix, Coordinate target, double targetValue)
        {
            var C = new Matrix<double>(matrix.ColSize, matrix.RowSize);
            C.Data[target.X-1][target.Y-1] = targetValue;

            bool notCompleted = true;
            int iteration = 0;
            while (notCompleted)
            {
                var nextC = new Matrix<double>(matrix.ColSize, matrix.RowSize);
                nextC.Data[target.X - 1][target.Y - 1] = C.Data[target.X - 1][target.Y - 1];
                for (int i = 0; i < matrix.RowSize; i++)
                {
                    for (int j = 0; j < matrix.ColSize; j++)
                    {
                        #region ignore target
                        if (i == target.X - 1 && j == target.Y - 1)
                        {
                            continue;
                        }
                        #endregion
                        // if border continue
                        if (matrix.Data[i][j]==1)
                            continue;
                        double upValue = C.GetValue(i - 1, j);
                        double dnValue = C.GetValue(i + 1, j);
                        double leftValue = C.GetValue(i, j - 1);
                        double rightValue = C.GetValue(i, j + 1);
                        double newValue = (upValue + dnValue + leftValue + rightValue)/4;
                        nextC.Data[i][j] = newValue;
                    }                    
                }
                bool changed = C.AreEqualTo(nextC);
                C = nextC;
                C.Print(String.Format("iteration {0}", iteration++));
                notCompleted = !changed;
            }
            
            return C;
        }
    }
}
