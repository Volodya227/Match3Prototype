using UnityEngine;
namespace View
{
    public class ViewController : MonoBehaviour
    {
        public event System.Action<int, int> EventOnClickedCellGrid;
        // EventOnClickedCellGrid - надлишково.
        // Краще: CellClicked або OnCellClicked (без Event і Grid).
        
        [SerializeField] private Transform _cameraViewPosition;
        private CameraView.CameraController _cameraView;
        private bool _editMode = false;
        
        [Range(100, 4000)]
        public int width;
        [Range(100, 3000)]
        public int height;
        
        [SerializeField] private Grid.GridView _grid = new();
        private Items.ItemController _items;
        private Data.ItemsState _itemsState = null;
        // зайве = null - reference types вже null за замовчуванням.
        
        [SerializeField] private Items.DataConfig.ItemDATAPrefabs _dataItemsPrefab;
        [SerializeField, Range(0, 1)] private float _deltaItemsSize = .6f;
        // _deltaItemsSize - незрозуміло що це означає.
        // Краще: _itemScaleFactor або _itemSizeMultiplier
        
        public Grid.GridView Grid => _grid;
        
        private void Awake()
        {
            if (_editMode) return;
            _grid.GenerateGrid(width, height);
            for (int i = 0; i < _grid.width * _grid.height; i++) {
                _grid.GetEventItem(i).EventOnCellClick += GetTouchedCell;
            }
            // Підписка на events для всіх клітинок - правильний підхід.
        }
        
        public void Init(Data.ItemsState itemsState, CameraView.CameraController cameraView)
        {
            if (_editMode) return;
            _itemsState = itemsState;
            _items = new Items.ItemController(_itemsState, _grid.width * (_grid.height), _grid, _dataItemsPrefab, _deltaItemsSize);
            _cameraView = cameraView;
            if (_cameraViewPosition != null)
            {
                _cameraView.transform.SetParent(_cameraViewPosition, false);
            }
        }
        
        private void GetTouchedCell(int x, int y)
        // "Get" зазвичай означає повернення значення.
        // Тут краще: OnCellTouched(), HandleCellClick()
        {
            EventOnClickedCellGrid?.Invoke(x, y);
        }
        
        private void OnDestroy()
        {
            if (_editMode) return;
            for (int i = 0; i < _grid.width * _grid.height; i++)
            {
                _grid.GetEventItem(i).EventOnCellClick -= GetTouchedCell;
            }
            // Відписка від events - важливо для уникнення memory leaks.
            
            _grid.Clear(false);
        }
        
        public void UpdateTick()
        {
            _items.Update();
        }
        
        //ContextMenu
        [ContextMenu("GenerateGrid")]
        public void GenerateGrid()
        {
            _editMode = true;
            ClearItemsView();
            _grid.GenerateGrid(width, height, true);
        }
        
        [ContextMenu("ClearGrid")]
        public void ClearGrid()
        {
            _editMode = true;
            ClearItemsView();
            _grid.Clear(true);
        }
        
        [ContextMenu("ClearItems")]
        public void ClearItemsView()
        {
            _editMode = true;
            if (_items == null) return;
            _items.DestroyAll();
        }
        
        [ContextMenu("Random Items")]
        public void RandomItems()
        {
            _editMode = true;
            ClearItemsView();
            _items = new Items.ItemController(_itemsState, _grid.width * (_grid.height), _grid, _dataItemsPrefab, _deltaItemsSize);
            _items.CreateRandom();
        }
        
        // _editMode використовується для розділення логіки Play/Edit mode.
        // Це працює, але може бути заплутаним. Альтернатива - використовувати #if UNITY_EDITOR
        // для editor-only функціональності, або окремий EditorScript.
    }
}