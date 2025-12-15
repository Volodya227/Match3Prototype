namespace Model
{
    public class GroupingSystem
    {
        // Добре - використання Stack для DFS (Depth-First Search) замість рекурсії - 
        // правильний підхід для уникнення stack overflow на великих групах.
        
        public bool locked = false;
        // Публічне поле. Краще зробити property з private set,
        // або метод Lock()/Unlock() якщо потрібна більша контрольованість.
        
        private readonly System.Collections.Generic.Stack<Data.Item> _stack = new();
        private readonly Data.Item[] _itemsGroup;
        private readonly Direction[] _directions;
        private readonly ItemsStateModel _itemsStateModel;
        private int _groupingItemsCount;
        private int _newGroupItemsCount;
        private int _minCountGroup = 3;
        // _minCountGroup - незрозуміло чи це мінімальний розмір групи для видалення,
        // чи щось інше. Краще: _minGroupSizeToMatch або _minMatchCount
        
        public int Count => _groupingItemsCount;
        public Data.Item GetItem(int i) => _itemsGroup[i];
        // Немає перевірки меж. Якщо i >= _groupingItemsCount або i < 0 - буде помилка.
        
        public GroupingSystem(ItemsStateModel itemsStateModel, DirectionMode mode, int minCountGroup = 3)
        {
            _directions = DirectionFactory.Directions(mode);
            _itemsStateModel = itemsStateModel;
            _itemsGroup = new Data.Item[_itemsStateModel.Width * _itemsStateModel.Height];
            _minCountGroup = minCountGroup;
        }
        
        public void Grouping()
        {
            // Метод робить дві речі: знаходить групи І скидає visited флаги.
            // Варто або розділити, або додати коментар що пояснює структуру методу.
            
            for (int x = 0; x < _itemsStateModel.Width; x++)
            {
                for (int y = 1; y < _itemsStateModel.Height; y++)
                // Чому y починається з 1, а не з 0?
                // Якщо це навмисно (ряд 0 - це "spawn zone") - додати коментар.
                // Цей патерн повторюється в багатьох місцях.
                {
                    Data.Item item = _itemsStateModel.grid[x, y];
                    if (item == null) continue;
                    if(item.visited) continue;
                    ActivatedGroup(x, y);
                }
            }
            // Скидання visited флагів
            for (int x = 0; x < _itemsStateModel.Width; x++)
            {
                for (int y = 1; y < _itemsStateModel.Height; y++)
                {
                    Data.Item item = _itemsStateModel.grid[x, y];
                    if (item == null) continue;
                    item.visited = false;
                }
            }
            // Два однакових вкладених цикли підряд.
            // Можна винести скидання visited в окремий метод ResetVisited()
            // для кращої читабельності та перевикористання.
        }
        
        public void Clear()
        {
            // Тут теж скидається visited - дублювання з Grouping().
            // Краще мати один метод ResetVisited() який викликається з обох місць.
            
            for (int x = 0; x < _itemsStateModel.Width; x++)
            {
                for (int y = 1; y < _itemsStateModel.Height; y++)
                {
                    Data.Item item = _itemsStateModel.grid[x, y];
                    if (item == null) continue;
                    item.visited = false;
                }
            }
            for (int i = 0;  i < _itemsGroup.Length; i++)
            {
                _itemsGroup[i] = null;
            }
            _groupingItemsCount = 0;
            _newGroupItemsCount = 0;
        }
        
        public void ActivatedGroup(int x = 0, int y = 0)
        // "ActivatedGroup" - незрозуміла назва. Що активується?
        // Краще: FindGroupAt(int x, int y) або ProcessGroupStartingFrom(int x, int y)
        {
            _stack.Clear();
            Data.Item item = _itemsStateModel.grid[x, y];
            item.visited = true;
            FindNewItems(item);
            while (_stack.Count>0) {
            // Пробіли навколо операторів: _stack.Count > 0
                item = _stack.Pop();
                FindNewItems(item);
            }
            CancelGroup();
            // "CancelGroup" насправді перевіряє чи група достатньо велика.
            // Краще: ValidateGroupSize() або FinalizeGroup()
            
            _groupingItemsCount += _newGroupItemsCount;
            _newGroupItemsCount = 0;
            _stack.Clear();
            // _stack.Clear() на початку методу вже очищає стек.
            // Другий виклик тут зайвий, бо while loop завершується коли стек порожній.
        }
        
        private void FindNewItems(Data.Item item)
        // Метод не тільки "знаходить" - він додає поточний item і шукає сусідів.
        // Краще: ProcessItemAndFindNeighbors() або ExpandFromItem()
        {
            AddItem(item);

            Data.Item newItem;
            int x, y;
            for (int i = 0; i < _directions.Length; i++) {
                x = item.X + _directions[i].X;
                y = item.Y + _directions[i].Y;
                if (x < 0 || y < 1) continue;
                // REVIEW [QUESTION]: y < 1 - знову магічне число. Додати константу SPAWN_ROW = 0 або коментар.
                
                if(x >= _itemsStateModel.Width || y >= _itemsStateModel.Height) continue;
                //TODO if by color
                // TODO коментар але перевірка по кольору вже реалізована нижче.
                // Видалити застарілий TODO.
                
                newItem = _itemsStateModel.grid[x, y];
                if (newItem != null) {
                    if (newItem.visited) continue;
                    if (newItem.Color == item.Color)
                    {
                        newItem.visited = true;
                        _stack.Push(newItem);
                    }
                }
            }
        }
        // Між методами краще додати порожній рядок для кращої читабельності.
        private void AddItem(Data.Item item)
        {
            _itemsGroup[_groupingItemsCount + _newGroupItemsCount] = item;
            _newGroupItemsCount++;
        }
        private void CancelGroup()
        {
            if (_newGroupItemsCount < _minCountGroup)
            {
                for (int i = 0; i < _newGroupItemsCount; i++)
                {
                    int index = _groupingItemsCount + i;
                    _itemsGroup[index] = null;
                }
                _newGroupItemsCount = 0;
            }
        }
    }
}