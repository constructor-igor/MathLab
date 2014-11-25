using System;
using System.Threading.Tasks;

namespace CellularAutomata
{
    public class CA2
    {
        public Matrix<double> Run(Matrix<int> matrix, Coordinate target, double targetValue)
        {
            var C = new Matrix<double>(matrix.ColSize, matrix.RowSize);
            C.Data[target.X - 1][target.Y - 1] = targetValue;

            bool notCompleted = true;
            int iteration = 0;
            while (notCompleted)
            {
                var nextC = new Matrix<double>(matrix.ColSize, matrix.RowSize);
                nextC.Data[target.X - 1][target.Y - 1] = C.Data[target.X - 1][target.Y - 1];
                //for (int i = 0; i < matrix.RowSize; i++)
                Parallel.For(0, matrix.RowSize, i =>
                {
                    //for (int j = 0; j < matrix.ColSize; j++)
                    Parallel.For(0, matrix.ColSize, j =>
                    {
                        #region ignore target

                        if (i == target.X - 1 && j == target.Y - 1)
                        {
                            return; //continue;
                        }

                        #endregion

                        // if border continue
                        if (matrix.Data[i][j] == 1)
                            return; //continue;
                        double upValue = C.GetValue(i - 1, j);
                        double dnValue = C.GetValue(i + 1, j);
                        double leftValue = C.GetValue(i, j - 1);
                        double rightValue = C.GetValue(i, j + 1);
                        double newValue = (upValue + dnValue + leftValue + rightValue) / 4;
                        nextC.Data[i][j] = newValue;
                    });
                });
                bool changed = C.AreEqualTo(nextC);
                C = nextC;
                C.Print(String.Format("iteration {0}", iteration++));
                notCompleted = !changed;
            }

            return C;
        }
    }
}
