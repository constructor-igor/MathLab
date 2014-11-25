using System;
using System.Collections.Generic;
using System.Linq;
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

        public MatrixPath FindPath(Matrix<double> concentrations, Coordinate source, Coordinate target)
        {
            var matrixPath = new MatrixPath();
            Coordinate current = source;
            matrixPath.Add(source);

            int iteration = 0;
            while (!current.AreEqual(target))
            {
                int i = current.X - 1;
                int j = current.Y - 1;
                //double concentration = concentrations.Data[i][j];

                var pointers = new List<Pointer>();

                var upValue = new Pointer(new Coordinate(i - 1, j), concentrations.GetValue(i - 1, j));
                var dnValue = new Pointer(new Coordinate(i + 1, j), concentrations.GetValue(i + 1, j));
                var leftValue = new Pointer(new Coordinate(i, j - 1), concentrations.GetValue(i, j - 1));
                var rightValue = new Pointer(new Coordinate(i, j + 1), concentrations.GetValue(i, j + 1));
                pointers.Add(upValue);
                pointers.Add(dnValue);
                pointers.Add(leftValue);
                pointers.Add(rightValue);

                pointers.Sort((p1, p2)=> Math.Sign(p1.Concentration-p2.Concentration));
                Pointer maxPointer = pointers.Last();
                current = new Coordinate(maxPointer.Coordinate.X+1, maxPointer.Coordinate.Y+1);
                matrixPath.Add(current);

                matrixPath.Print(String.Format("iteration {0}: ", iteration++));
            }

            return matrixPath;
        }

        internal class Pointer
        {
            internal Coordinate Coordinate;
            internal double Concentration;

            internal Pointer(Coordinate coordinate, double concentration)
            {
                Coordinate = coordinate;
                Concentration = concentration;
            }
        }
    }
}
