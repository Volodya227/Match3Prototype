using UnityEngine;
namespace Model
{
    public class ChoiceCellGridData{
        public int X { get; private set; }
        public int Y{ get; private set; }
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
    }
    public class ItemsStateModel : Data.ItemsState
    {
        private readonly int _width;
        private readonly int _height;
        public readonly Data.Item[,] grid;//update changing position item for find groups

        public ItemsStateModel(int width, int height) : base(width, height)
        {
            _width = width;
            _height = height;
            grid = new Data.Item[_width, _height];
        }
        public void UpdateGravity()
        {
            int x, y;
            for (int i = 0; i < _items.Length; i++) {
                if (_items[i] != null) {
                    x = _items[i].X;
                    y = _items[i].Y;
                    int targetY;
                    for (targetY = y + 1; targetY < _height; targetY++) {
                        if (grid[x, targetY] != null) {
                            targetY--;
                            break;
                        }
                    }
                    targetY = Mathf.Min(targetY, _height - 1);
                    if (targetY != y)
                    {
                        grid[x, y] = null;
                        y = targetY;
                        grid[x, y] = _items[i];
                        _items[i].SetXY(x, y);
                        _items[i].EventAnimatedFallActivated();
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
            }
        }
        public void ClearDeletedItem()
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
    }
    public class ModelController
    {
        private readonly ChoiceCellGridData _choiceCellGridData;
        private readonly ItemsStateModel _itemsState;
        public ItemsStateModel ItemsState => _itemsState;
        public ModelController(int width, int height) {
            _choiceCellGridData = new();
            _itemsState = new ItemsStateModel(width, height);
        }
        public void UpdateTick() {
            //Delete
            if (_choiceCellGridData.Active) {
                FindGroups();
                _itemsState.ClearDeletedItem();
            }
            //clear created items list
            _itemsState.ClearCreatedItems();
            //gravity
            UpdateItemsGravity();
            //create
            CreateNewItems();
        }
        private void CreateNewItems()
        {
            for(int i = 0; i < _itemsState.Width; i++)
            {
                if (_itemsState.grid[i,0] == null)
                {
                    _itemsState.AddNewItem(ModelFactory.CreateItem(i));
                }
            }
        }
        private void UpdateItemsGravity()
        {
            _itemsState.UpdateGravity();
        }
        public void SetClickedCellGrid(int x, int y)
        {
            _choiceCellGridData.Set(x, y);
        }
        private void FindGroups()
        {
            //TODO
            Delete();
            _choiceCellGridData.Use();
        }
        private void Delete()
        {
            _itemsState.grid[_choiceCellGridData.X, _choiceCellGridData.Y].Delete();
        }
    }
}