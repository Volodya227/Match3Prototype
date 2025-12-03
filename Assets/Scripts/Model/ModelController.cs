using UnityEngine;

namespace Model
{
    public class ItemsStateModel : Data.ItemsState
    {
        private readonly int _width;
        private readonly int _height;
        public readonly Data.ItemReadonly[,] grid;//update in change position item

        public ItemsStateModel(int width, int height) : base(width, height)
        {
            _width = width;
            _height = height;
            grid = new Data.ItemReadonly[_width, _height];
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
    }
    public class ModelController
    {
        private readonly ItemsStateModel _itemsState;
        public ItemsStateModel ItemsState => _itemsState;
        public ModelController(int width, int height) {
            _itemsState = new ItemsStateModel(width, height);
        }
        public void UpdateTick() {
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
    }
}