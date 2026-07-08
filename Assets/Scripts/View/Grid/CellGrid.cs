using UnityEngine;

namespace View.Grid
{
    public class CellGrid : Pickup.Pickup
    {
        public event System.Action<int, int> EventOnCellClick;
        [SerializeField] private Transform _cellObject;
        [SerializeField] private BoxCollider _collider;
        public Transform CellObject => (_cellObject == null) ? transform : _cellObject;
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public void SetVisualScale(Vector3 scale)
        {
            _cellObject.localScale = scale;
            _collider.size = scale;
        }
        public void Init(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override void OnPicked()
        {
            EventOnCellClick?.Invoke(X, Y);
        }
    }
}