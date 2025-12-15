using UnityEngine;

namespace Model
{
    public class SwitchItemsSystem
    {
        // "SwitchItemsSystem" - можна спростити до "SwapSystem" або "ItemSwapper"
        
        private readonly Direction[] _directionMove;
        private readonly ItemsStateModel _state;
        private Data.Item _item;
        private Data.Item _newItem;
        // _item і _newItem - незрозуміло що є що.
        // Краще: _firstSelectedItem, _secondSelectedItem
        // або _sourceItem, _targetItem
        
        public Data.Item GetItem => _item;
        public Data.Item GetNewItem => _newItem;
        // Properties з префіксом Get - нетипово для C#.
        // Краще: FirstItem, SecondItem або SelectedItem, TargetItem
        
        public SwitchItemsSystem(ItemsStateModel stateModel, DirectionMode directionMode)
        {
            _state = stateModel;
            _directionMove = DirectionFactory.Directions(directionMode);
        }
        
        public bool SetItem(int x, int y)
        // Метод SetItem насправді робить багато:
        // - Якщо перший клік: запам'ятовує item
        // - Якщо другий клік: перевіряє валідність і робить swap
        // Краще розділити на SelectItem() і TrySwap() або додати детальний коментар.
        {
            if (y == 0) return false;
            //MAGIC NUMBER y == 0 - spawn row, не можна вибрати. Додати константу.
            
            _newItem = _state.grid[x, y];
            if (_item != null)
            {
                if (CheckCorectlyAction())
                {
                    Switch();
                    return true;
                }
            }
            _item = _newItem;
            _newItem = null;
            return false;
        }
        
        private bool CheckCorectlyAction()
        // Назва не відображає що перевіряється.
        // Краще: IsValidSwap(), AreItemsAdjacent(), CanSwapItems()
        {
            foreach (Direction direction in _directionMove) {
                if(direction.X == _item.X-_newItem.X){
                // Пробіли навколо операторів: _item.X - _newItem.X
                    if (direction.Y == _item.Y - _newItem.Y){
                        return true;
                    }
                }
            }
            return false;
        }
        
        private void Switch()
        // В C# конвенція - Swap() для обміну значень.
        {
            _state.grid[_item.X, _item.Y] = _newItem;
            _state.grid[_newItem.X, _newItem.Y] = _item;
            int x = _newItem.X;
            int y = _newItem.Y;
            _newItem.SetXY(_item.X, _item.Y);
            _item.SetXY(x, y);
            _item.EventMoveActivate();
            _newItem.EventMoveActivate();
            // Правильна логіка swap - спочатку оновлюємо grid, потім координати items.
        }
        
        public void Clear()
        {
            _newItem = null;
            _item = null;
        }
    }
}