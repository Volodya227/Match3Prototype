using UnityEngine;
using DG.Tweening;
namespace View.Items
{
    [System.Serializable]
    public class ItemView
    {
        private const float _timeDestroy = .35f;
        private const float _timeMoving = .15f;
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
                SetParent(_grid.GetItem(_item.X, _item.Y), false);//TODO true -> false
            }
            if (_move) {
                _move = false;
                SetParent(_grid.GetItem(_item.X, _item.Y), false);
            }
        }
        public void Destroy(bool editMode = false)
        {
            Active = false;
            Dispose();
            if (_view != null)
            {
                if (editMode) Object.DestroyImmediate(_view);
                else
                {

                    _view.transform.DOScale(Vector3.zero, _timeDestroy)
                     .SetEase(Ease.InBack)
                     .OnComplete(() =>
                     {
                         Object.Destroy(_view);
                     });
                }
            }
        }
        public void SetParent(Transform parent, bool ignore = false) {
            Vector3 position = _view.transform.position;
            _view.transform.SetParent(parent, false);
            if (ignore) return;
            _view.transform.position = position;
            position = _view.transform.localPosition;
            float t = _timeMoving * position.magnitude;
            _view.transform.DOKill();
            _view.transform.DOLocalMove(Vector3.zero, t).SetEase(Ease.OutCubic);
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