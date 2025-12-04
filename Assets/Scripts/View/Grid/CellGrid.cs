namespace View.Grid
{
    public class CellGrid : Pickup.Pickup
    {
        public event System.Action<int, int> EventOnCellClick;
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public void Init(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override void OnPicked()
        {
            EventOnCellClick?.Invoke(X, Y);
        }
    }
}