using UnityEngine;
namespace View.Grid
{
    [System.Serializable]
    public class GridView
    {
        [SerializeField] private bool _isUI = true;
        [SerializeField] private CellGrid _prefabGrid;
        [SerializeField] private Transform _backGroundGrid;
        // _backGroundGrid -> _backgroundGrid або _gridBackground
        
        private CellGrid[] _items;
        // _items для клітинок сітки - заплутує з ігровими items.
        // Краще: _cells або _gridCells
        
        private bool _init = false;
        public int itemSizePixel = 100;
        // Публічне поле. Краще [SerializeField] private з property.
        
        public int height;
        public int width;
        // Публічні поля що змінюються в GenerateGrid.
        // Небезпечно - зовнішній код може змінити їх і зламати логіку.
        // Краще: public int Height { get; private set; }
        
        public bool IsUI => _isUI;
        
        public Transform GetItem(int x, int y) => _items[x + width * y].transform;
        public Transform GetItemByIndex(int i) => _items[i].transform;
        public CellGrid GetEventItem(int i) => _items[i];
        // Немає перевірки меж масиву.
        // GetEventItem - "Event" в назві зайве. Краще: GetCell(int i)
        
        public void Clear(bool value)
        // bool value - неінформативно. Що означає true/false?
        // Краще: bool immediate або bool destroyImmediate
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
        // value знову - неінформативно.
        // widthPixel/heightPixel потім діляться на itemSizePixel.
        // Це заплутує - параметри в пікселях, але результат в клітинках.
        // Краще приймати кількість клітинок напряму, або чітко документувати.
        {
            Clear(value);
            _init = true;
            Vector3 StartPosition;
            // StartPosition - PascalCase для локальної змінної. Краще startPosition.
            
            StartPosition = GetPosition(-widthPixel / 2 + itemSizePixel / 2, +heightPixel / 2 + itemSizePixel / 2);
            widthPixel /= itemSizePixel;
            heightPixel /= itemSizePixel;
            // Перезапис параметрів - може бути заплутаним.
            // Краще: int cellsWide = widthPixel / itemSizePixel;
            
            width = widthPixel;
            height = heightPixel+1;
            // height = heightPixel + 1 - чому +1? Це для spawn row?
            // Обов'язково додати коментар!
            
            _items = new CellGrid[width * height];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    _items[w + widthPixel * h] = Object.Instantiate(_prefabGrid, parent: _backGroundGrid);
                    // Використовується widthPixel замість width.
                    // Після ділення вони однакові, але це може заплутати.
                    
                    if (_isUI)
                    {
                        RectTransform rt = _items[w + widthPixel * h].transform as RectTransform;

                        rt.sizeDelta = new Vector2(itemSizePixel, itemSizePixel);

                    }
                    else _items[w + widthPixel * h].transform.localScale = GetPosition(itemSizePixel, itemSizePixel, true);
                    _items[w + widthPixel * h].transform.localPosition = StartPosition + GetPosition(itemSizePixel * w, -itemSizePixel * h);
                    _items[w + widthPixel * h].Init(w, h);
                }
            }
        }
        
        public Vector3 GetPosition(int x, int y, bool scale = false) {
            int s = scale ? 1 : 0;
            return (_isUI) ? new Vector3(x, y, s) : new Vector3(x, s, y);
            // Логіка незрозуміла без контексту.
            // Додати коментар: для UI - XY площина, для 3D - XZ площина (горизонтальна).
        }
        
        private void SafeDestroy(GameObject obj, bool value = false)
        // value -> immediate
        {
            if (value) Object.DestroyImmediate(obj);
            else Object.Destroy(obj);
        }
        //get Bug between edit, play Modess
        // Незрозумілий коментар. Якщо це відомий баг - описати детальніше.
        // Якщо виправлено - видалити.
    }
}