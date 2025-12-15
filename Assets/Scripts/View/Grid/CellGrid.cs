namespace View.Grid
{
    public class CellGrid : Pickup.Pickup
    {
        // Простий і зрозумілий клас. Наслідування від Pickup - правильний підхід.
        
        public event System.Action<int, int> EventOnCellClick;
        // Консистентно з іншими частинами коду, але краще просто Clicked або OnClicked.
        
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        // зайве = 0 - int вже 0 за замовчуванням.
        
        public void Init(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public override void OnPicked()
        {
            EventOnCellClick?.Invoke(X, Y);
        }
        // Чистий, мінімальний клас з однією відповідальністю.
    }
}