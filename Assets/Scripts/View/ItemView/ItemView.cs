using UnityEngine;
namespace View.Items
{
    [System.Serializable]
    public class ItemView
    {
        private Data.ItemReadonly _item;
        private readonly GameObject _view;
        private bool _animationFalling;
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
            }
            _grid = grid;
        }
        private void SetAnimationFalling()
        {
            _animationFalling = true;
        }
        private void SetDestroy()
        {
            _destroy = true;
        }
        public void Update()
        {
            if (_view == null || _destroy) {
                Destroy();
            }
            if (_item == null) return;
            if (_animationFalling)
            {
                _animationFalling = false;
                SetParent(_grid.GetItem(_item.X, _item.Y), true);//TODO true -> false
            }
        }
        public void Destroy(bool editMode = false)
        {
            Dispose();
            if (_view != null)
            {
                if (editMode) Object.DestroyImmediate(_view);
                else Object.Destroy(_view);
            }
            Active = false;
        }
        public void SetParent(Transform parent, bool ignore = false) {
            Vector3 position = _view.transform.position;
            _view.transform.SetParent(parent, false);
            if (ignore) return;
            position.x = _view.transform.localScale.x;
            _view.transform.position = position;//TODO start Animation
            //add coroutine?
        }
        private void Dispose()
        {
            if (_item == null) return;
            _item.EventAnimatedFall -= SetAnimationFalling;
            _item.EventDestroy -= SetDestroy;
            _item = null;
        }
    }
}