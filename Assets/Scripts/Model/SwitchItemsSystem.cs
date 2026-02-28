namespace Model
{
    public class SwitchItemsSystem
    {
        private readonly Direction[] _directionMove;
        private readonly ItemsStateModel _state;
        private Data.Item _item;
        private Data.Item _newItem;
        public Data.Item GetItem => _item;
        public Data.Item GetNewItem => _newItem;
        public SwitchItemsSystem(ItemsStateModel stateModel, DirectionMode directionMode)
        {
            _state = stateModel;
            _directionMove = DirectionFactory.Directions(directionMode);
        }
        public bool SetItem(int x, int y)
        {
            if (y == 0) return false;
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
        {
            foreach (Direction direction in _directionMove) {
                if(direction.X == _item.X-_newItem.X){
                    if (direction.Y == _item.Y - _newItem.Y){
                        return true;
                    }
                }
            }
            return false;
        }
        private void Switch()
        {
            _state.SwitchItems(_item, _newItem);return;
        }
        public void Clear()
        {
            _newItem = null;
            _item = null;
        }
    }
}