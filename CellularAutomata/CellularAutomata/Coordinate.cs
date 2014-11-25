namespace CellularAutomata
{
    public struct Coordinate
    {
        public int X;
        public int Y;
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool AreEqual(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}