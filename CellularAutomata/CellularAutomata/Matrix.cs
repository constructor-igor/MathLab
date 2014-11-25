namespace CellularAutomata
{
    public class Matrix<T>
    {
        public readonly T[][] Data;
        public int ColSize { get; private set; }
        public int RowSize { get; private set; }

        public Matrix(T[][] data)
        {
            Data = data;
            InitSize();
        }
        public Matrix(int colSize, int rowSize)
        {
            var data = new T[rowSize][];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new T[colSize];
            }
            Data = data;
            InitSize();
        }

        private void InitSize()
        {
            RowSize = Data.Length;
            ColSize = Data[0].Length;
        }

        public T GetValue(int row, int col)
        {
            try
            {
                return Data[row][col];
            }
            catch
            {
                return default(T);
            }
        }
    }
}