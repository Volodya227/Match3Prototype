using UnityEngine;
namespace View.Grid
{
    [System.Serializable]
    public class GridView
    {
        [SerializeField] private bool _isUI = true;
        [SerializeField] private Transform _prefabGrid;
        [SerializeField] private Transform _backGroundGrid;
        private Transform[] _items;
        private bool _init = false;
        public int itemSizePixel = 100;
        public int height;
        public int width;
        public Transform GetItem(int x, int y) => _items[x + width * y];
        public Transform GetItemByIndex(int i) => _items[i];
        public void Clear(bool value)
        {
            if (!_init) return;
            _init = false;
            for(int i = 0; i < _items.Length; i++)
            {
                if (_items[i] != null)
                {
                    SafeDestroy(_items[i].gameObject, value);
                    _items[i] = null;
                }
            }
            _items = null;
        }
        public void GenerateGrid(int widthPixel, int heightPixel, bool value = false)
        {
            Clear(value);
            _init = true;
            Vector3 StartPosition;
            StartPosition = GetPosition(-widthPixel / 2 + itemSizePixel / 2, +heightPixel / 2 + itemSizePixel / 2);
            widthPixel /= itemSizePixel;
            heightPixel /= itemSizePixel;
            width = widthPixel;
            height = heightPixel+1;
            _items = new Transform[widthPixel * (heightPixel + 1)];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    _items[w + widthPixel * h] = Object.Instantiate(_prefabGrid, parent: _backGroundGrid);
                    _items[w + widthPixel * h].transform.localPosition = StartPosition + GetPosition(itemSizePixel * w, -itemSizePixel * h);
                }
            }
        }
        private Vector3 GetPosition(int x, int y) => (_isUI) ? new Vector3(x, y, 0) : new Vector3(x, 0, y);
        private void SafeDestroy(GameObject obj, bool value = false)
        {
            if (value) Object.DestroyImmediate(obj);
            else Object.Destroy(obj);
        }
        //get Bug between edit, play Modess
    }
}