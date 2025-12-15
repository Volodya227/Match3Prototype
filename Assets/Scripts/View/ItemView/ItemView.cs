using UnityEngine;
using DG.Tweening;
// Використання DOTween - правильний вибір для анімацій в Unity.
// Це перевірена бібліотека що працює краще ніж самописні рішення.

namespace View.Items
{
    [System.Serializable]
    public class ItemView
    {
        private const float _timeDestroy = .35f;
        private const float _timeMoving = .15f;
        // Константи для magic numbers - правильний підхід.
        // Конвенція для констант - PascalCase без underscore:
        // private const float DestroyDuration = 0.35f;
        // private const float MoveDuration = 0.15f;
        
        private Data.ItemReadonly _item;
        private readonly GameObject _view;
        private bool _animationFalling;
        private bool _move;
        private bool _destroy;
        // Ці bool поля - прапорці стану. Краще з префіксом:
        // _shouldAnimateFall, _shouldMove, _shouldDestroy
        // або _pendingFallAnimation, _pendingMove, _pendingDestroy
        
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
            // Підписка на events від Model - правильна реалізація MVC.
            // View слухає зміни в Model і реагує відповідно.
        }
        
        //Hearing events from model
        // Добре - оментар пояснює призначення методів нижче.
        
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
            // Перевірка !Active після Destroy() може бути проблемою
            // якщо Destroy() не встановлює Active = false негайно (через анімацію).
            // Зараз працює бо Destroy() встановлює Active = false на початку.
            
            if (_animationFalling)
            {
                _animationFalling = false;
                SetParent(_grid.GetItem(_item.X, _item.Y), false);//TODO true -> false
                //TODO коментар - якщо зміна зроблена, видалити TODO.
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
                    // Анімація знищення з callback - правильний підхід з DOTween.
                }
            }
        }
        
        public void SetParent(Transform parent, bool ignore = false) {
        // ignore - неінформативно. Що ігнорується?
        // Краще: bool skipAnimation або bool instant
            Vector3 position = _view.transform.position;
            _view.transform.SetParent(parent, false);
            if (ignore) return;
            _view.transform.position = position;
            // Зберігаємо world position перед SetParent, потім відновлюємо.
            // Це дозволяє плавно анімувати до нової позиції.
            
            position = _view.transform.localPosition;
            float t = _timeMoving * position.magnitude;
            // Тривалість анімації залежить від відстані - природніше виглядає.
            
            _view.transform.DOKill();
            // DOKill() перед новою анімацією - запобігає конфліктам.
            
            _view.transform.DOLocalMove(Vector3.zero, t).SetEase(Ease.OutCubic);
        }
        
        private void Dispose()
        {
            if (_item == null) return;
            _item.EventAnimatedFall -= SetAnimationFalling;
            _item.EventDestroy -= SetDestroy;
            _item.EventMove -= SetMove;
            _item = null;
            // Правильний cleanup - відписка від events і очищення reference.
        }
        
        // Цей клас добре реалізований - чітке розділення відповідальностей,
        // правильна робота з events та анімаціями. Можна розглянути реалізацію IDisposable
        // для явного контракту очищення ресурсів.
    }
}