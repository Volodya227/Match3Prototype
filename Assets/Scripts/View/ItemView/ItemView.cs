using UnityEngine;
namespace View.Items
{
    [System.Serializable]
    public class ItemView
    {
        private Data.ItemReadonly _item;
        private readonly GameObject _view;
        private bool _animationFalling;
        private bool _move;
        private bool _destroy;
        private readonly Grid.GridView _grid;
        public bool Active { get; private set; }
        public ItemView(Data.ItemReadonly item, GameObject view, Grid.GridView grid)
        {
            Active = true;
            _item = item;
            _view = view;
            if (_item != null)
            {
                _item.EventAnimatedFall += SetAnimationFalling;
                _item.EventDestroy += SetDestroy;
                _item.EventMove += SetMove;
            }
            _grid = grid;
        }
        //Hearing events from model
        private void SetAnimationFalling()
        {
            _animationFalling = true;
        }
        private void SetMove()
        {
            _move = true;
        }
        private void SetDestroy()
        {
            _destroy = true;
        }
        public void Update()
        {
            if (_view == null) Destroy();
            if (_destroy) Destroy();
            if (!Active) return;
            if (_animationFalling)
            {
                _animationFalling = false;
                SetParent(_grid.GetItem(_item.X, _item.Y), true);//TODO true -> false
            }
            if (_move) {
                _move = false;
                SetParent(_grid.GetItem(_item.X, _item.Y), true);
            }
        }
        public void Destroy(bool editMode = false)
        {
            Active = false;
            Dispose();
            if (_view != null)
            {
                if (editMode) Object.DestroyImmediate(_view);
                else Object.Destroy(_view);
            }
        }
        public void SetParent(Transform parent, bool ignore = false) {
            Vector3 position = _view.transform.position;
            _view.transform.SetParent(parent, false);
            if (ignore) return;
            _view.transform.position = position;//TODO start Animation
            //add coroutine?
        }
        private void Dispose()
        {
            if (_item == null) return;
            _item.EventAnimatedFall -= SetAnimationFalling;
            _item.EventDestroy -= SetDestroy;
            _item.EventMove -= SetMove;
            _item = null;
        }
    }
}