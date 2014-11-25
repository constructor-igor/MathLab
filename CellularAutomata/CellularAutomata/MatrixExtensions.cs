using System;

namespace CellularAutomata
{
    public static class MatrixExtensions
    {
        public static void Print<T>(this Matrix<T> matrix, string caption)
        {
            Console.WriteLine("{0}", caption);
            for (int i = 0; i < matrix.RowSize; i++)
            {
                for (int j = 0; j < matrix.ColSize; j++)
                {
                    Console.Write("{0:0.0000} ", matrix.Data[i][j]);
                }
                Console.WriteLine();
            }
        }

        public static bool AreEqualTo(this Matrix<double> matrixX, Matrix<double> matrixY)
        {
            for (int i = 0; i < matrixX.RowSize; i++)
            {
                double[] row = matrixX.Data[i];
                for (int j = 0; j < row.Length; j++)
                {
                    if (matrixX.Data[i][j] != matrixY.Data[i][j])
                        return false;
                }
            }
            return true;
        }
    }
}