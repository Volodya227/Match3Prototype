using UnityEngine;
namespace Model
{
    public class ChoiceCellGridData
    // "ChoiceCellGridData" - заплутана назва.
    // Це по суті SelectedCell або CellSelection. "Choice" і "Data" тут зайві.
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool Active { get; private set; }
        
        public ChoiceCellGridData()
        {
            Active = false;
            X = 0;
            Y = 0;
        }
        public void Set(int x, int y)
        {
            Active = true;
            X = x;
            Y = y;
        }
        public void Use() { Active = false; }
        // Use() - неінформативно. Краще: Consume(), Clear(), Reset()
    }
    
    public class ItemsStateModel : Data.ItemsState
    {
        // Добре: Наслідування від ItemsState для розширення функціональності в Model шарі - 
        // правильний підхід для MVC.
        
        public readonly Data.Item[,] grid;//update changing position item for find groups
        //  Публічний readonly масив - елементи все ще можна змінювати.
        // Якщо це навмисно для продуктивності - ок, але додати коментар.
        // Безпечніший варіант: internal або метод GetItem(x,y).

        public ItemsStateModel(int width, int height) : base(width, height)
        {
            grid = new Data.Item[Width, Height];
        }
        
        public void UpdateGravity()
        {
            int x, y;
            for (int i = 0; i < _items.Length; i++) {
                if (_items[i] != null) {
                    x = _items[i].X;
                    y = _items[i].Y;
                    int targetY;
                    for (targetY = y + 1; targetY < Height; targetY++) {
                    // y + 1 означає що шукаємо НИЖЧЕ поточної позиції?
                    // Якщо y=0 зверху і y збільшується вниз - це падіння вниз. Додати коментар.
                        if (grid[x, targetY] != null) {
                            targetY--;
                            break;
                        }
                    }
                    targetY = Mathf.Min(targetY, Height - 1);
                    if (targetY != y)
                    {
                        grid[x, y] = null;
                        y = targetY;
                        grid[x, y] = _items[i];
                        _items[i].SetXY(x, y);
                        _items[i].EventAnimatedFallActivate();
                    }
                }
            }
        }
        
        public void AddNewItem(Data.Item newItem)
        {
            //Add to list items
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                {
                    _items[i] = newItem;
                    grid[_items[i].X, _items[i].Y] = newItem;
                    break;
                }
            }
            
            //add to list for view
            for (int i = 0; i < _createdItem.Length; i++)
            {
                if(_createdItem[i] == null)
                {
                    _createdItem[i] = newItem;
                    break;
                }
            }
        }
        
        public void ClearCreatedItems()
        {
            for (int i = 0; i < _createdItem.Length; i++)
            {
                _createdItem[i] = null;//clear link, not object
                // Правильний коментар - пояснює що ми не видаляємо об'єкт.
            }
        }
        
        public void ClearDeletedItem()
        // Однина "Item" але метод очищає всі видалені items. 
        // Краще: ClearDeletedItems() або RemoveInactiveItems()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] != null)
                {
                    if (!_items[i].Active)
                    {
                        grid[_items[i].X, _items[i].Y] = null;
                        _items[i] = null;
                    }
                }
            }
        }
        
        public bool FullFilledGrid()
        // "FullFilled" -> "FullyFilled" або краще "IsGridFull()"
        // Конвенція для методів що повертають bool: Is..., Has..., Can...
        {
            for(int x  = 0; x < Width; x++)
            {
                for(int y = 1; y < Height; y++)
                // Знову y = 1. Це патерн по всьому коду.
                // Винести в константу: private const int PLAYABLE_START_ROW = 1;
                {
                    if (grid[x, y] == null) return false;
                }
            }
            return true;
        }
    }
    
    public class ModelController
    {
        public readonly GroupingSystem _groupingSystem;
        // Публічне поле з _ prefix - нетипово.
        // _ зазвичай для приватних полів. Або зробити private, або прибрати _.
        
        private readonly ChoiceCellGridData _choiceCellGridData;
        private readonly ItemsStateModel _itemsState;
        private readonly SwitchItemsSystem _switchItemsSystem;
        
        public ItemsStateModel ItemsState => _itemsState;
        
        public ModelController(int width, int height, DirectionMode modeGrouping, DirectionMode modeMoving, int minCountGroup = 3) {
            _choiceCellGridData = new();
            _itemsState = new ItemsStateModel(width, height);
            _groupingSystem = new GroupingSystem(_itemsState, modeGrouping, minCountGroup);
            _switchItemsSystem = new SwitchItemsSystem(_itemsState, modeMoving);
        }
        
        public void UpdateTick() {
            if (_itemsState.FullFilledGrid()) {
                SwitchItems();
            }
            //clear created items list
            _itemsState.ClearCreatedItems();
            //gravity
            UpdateItemsGravity();
            //create
            CreateNewItems();
            // Є коментарі до секцій - це добре.
        }
        
        private void CreateNewItems()
        {
            for(int i = 0; i < _itemsState.Width; i++)
            {
                if (_itemsState.grid[i,0] == null)
                // MAGIC NUMBER: grid[i,0] - тут 0 це spawn row.
                // Використати константу: grid[i, SPAWN_ROW]
                {
                    _itemsState.AddNewItem(ModelFactory.CreateItem(i));
                }
            }
        }
        
        private void UpdateItemsGravity()
        {
            _itemsState.UpdateGravity();
        }
        // Метод-обгортка з одного рядка. 
        // Можна або видалити і викликати напряму, або додати додаткову логіку.
        
        public void SetClickedCellGrid(int x, int y)
        {
            _choiceCellGridData.Set(x, y);
        }
        
        private void SwitchItems()
        {
            if (_choiceCellGridData.Active)
            {
                if (_switchItemsSystem.SetItem(_choiceCellGridData.X, _choiceCellGridData.Y))
                {
                    
                    // Зайвий пустий рядок
                    GroupByItem(_switchItemsSystem.GetItem);
                    GroupByItem(_switchItemsSystem.GetNewItem);
                    _switchItemsSystem.Clear();
                    DeleteGroups();
                }
                _choiceCellGridData.Use();
            }
        }
        
        private void GroupByItem(Data.Item item)
        {
            _groupingSystem.ActivatedGroup(item.X, item.Y);
        }
        
        private void DeleteGroups()
        {
            for (int i = 0; i < _groupingSystem.Count; i++)
            {
                _groupingSystem.GetItem(i).Delete();
            }
            _itemsState.ClearDeletedItem();
            _groupingSystem.Clear();

        }
        
        private void Delete()
        // Цей метод не використовується ніде в коді.
        // Видалити або позначити як TODO для майбутнього використання.
        {
            if (_groupingSystem.locked) return;
            _groupingSystem.Grouping();
            for (int i = 0; i < _groupingSystem.Count; i++)
            {
                _groupingSystem.GetItem(i).Delete();
            }
            _itemsState.ClearDeletedItem();
            if (_groupingSystem.Count == 0) _groupingSystem.locked = true;
            _groupingSystem.Clear();
        }

    }
}