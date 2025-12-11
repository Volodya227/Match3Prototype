namespace Model
{
    public class GroupingSystem
    {
        public bool locked = false;
        private readonly System.Collections.Generic.Stack<Data.Item> _stack = new();
        private readonly Data.Item[] _itemsGroup;
        private readonly Direction[] _directions;
        private readonly ItemsStateModel _itemsStateModel;
        private int _groupingItemsCount;
        private int _newGroupItemsCount;
        private int _minCountGroup = 3;
        public int Count => _groupingItemsCount;
        public Data.Item GetItem(int i) => _itemsGroup[i];
        public GroupingSystem(ItemsStateModel itemsStateModel, DirectionMode mode, int minCountGroup = 3)
        {
            _directions = DirectionFactory.Directions(mode);
            _itemsStateModel = itemsStateModel;
            _itemsGroup = new Data.Item[_itemsStateModel.Width * _itemsStateModel.Height];
            _minCountGroup = minCountGroup;
        }
        public void Grouping()
        {
            for (int x = 0; x < _itemsStateModel.Width; x++)
            {
                for (int y = 1; y < _itemsStateModel.Height; y++)
                {
                    Data.Item item = _itemsStateModel.grid[x, y];
                    if (item == null) continue;
                    if(item.visited) continue;
                    ActivatedGroup(x, y);
                }
            }
            for (int x = 0; x < _itemsStateModel.Width; x++)
            {
                for (int y = 1; y < _itemsStateModel.Height; y++)
                {
                    Data.Item item = _itemsStateModel.grid[x, y];
                    if (item == null) continue;
                    item.visited = false;
                }
            }
        }
        public void Clear()
        {
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
        {
            _stack.Clear();
            Data.Item item = _itemsStateModel.grid[x, y];
            item.visited = true;
            FindNewItems(item);
            while (_stack.Count>0) {
                item = _stack.Pop();
                FindNewItems(item);
            }
            CancelGroup();
            _groupingItemsCount += _newGroupItemsCount;
            _newGroupItemsCount = 0;
            _stack.Clear();
        }
        private void FindNewItems(Data.Item item)
        {
            AddItem(item);


            Data.Item newItem;
            int x, y;
            for (int i = 0; i < _directions.Length; i++) {
                x = item.X + _directions[i].X;
                y = item.Y + _directions[i].Y;
                if (x < 0 || y < 1) continue;
                if(x >= _itemsStateModel.Width || y >= _itemsStateModel.Height) continue;
                //TODO if by color
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