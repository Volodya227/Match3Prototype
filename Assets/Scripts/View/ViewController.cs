using UnityEngine;
namespace View
{
    public class ViewController : MonoBehaviour
    {
        public event System.Action<int, int> EventOnClickedCellGrid;
        private bool _editMode = false;
        [Range(100, 4000)]
        public int width;
        [Range(100, 3000)]
        public int height;
        [SerializeField] private Grid.GridView _grid = new();
        private Items.ItemController _items;
        private Data.ItemsState _itemsState = null;
        [SerializeField] private Items.DataConfig.ItemDATAPrefabs _dataItemsPrefab;
        public Grid.GridView Grid => _grid;
        private void Awake()
        {
            if (_editMode) return;
            _grid.GenerateGrid(width, height);
            for (int i = 0; i < _grid.width * _grid.height; i++) {
                _grid.GetEventItem(i).EventOnCellClick += GetTouchedCell;
            }
        }
        public void Init()
        {
            if (_editMode) return;
            _items = new Items.ItemController(_itemsState, _grid.width * (_grid.height), _grid, _dataItemsPrefab);
        }
        private void GetTouchedCell(int x, int y)
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
            _grid.Clear(false);
        }
        public void SetItemsState(Data.ItemsState itemsState)
        {
            _itemsState = itemsState;
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
            _items = new Items.ItemController(_itemsState, _grid.width * (_grid.height), _grid, _dataItemsPrefab);
            _items.CreateRandom();
        }
    }
}