namespace Model
{
    [System.Serializable]
    public class DirectionMode
    {
        public bool horizontal;
        public bool vertical;
        public bool diagonal;
    }
    public class Direction
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }
    }
    public static class DirectionFactory
    {
        public static Direction[] Directions(DirectionMode mode)
        {
            int lenght = 0;
            if (mode.horizontal) lenght += 2;
            if (mode.vertical) lenght += 2;
            if (mode.diagonal) lenght += 4;
            Direction[] result = new Direction[lenght];
            int j = 0;
            if (mode.horizontal)
            {
                result[j] = new Direction(1, 0);
                result[j+1] = new Direction(-1, 0);
                j += 2;
            }
            if (mode.vertical)
            {
                result[j] = new Direction(0, 1);
                result[j + 1] = new Direction(0, -1);
                j += 2;
            }
            if (mode.diagonal)
            {
                result[j] = new Direction(1, 1);
                result[j + 1] = new Direction(1, -1);
                result[j + 2] = new Direction(-1, 1);
                result[j + 3] = new Direction(-1, -1);
                j += 4;
            }
            return result;
        }
    }
}