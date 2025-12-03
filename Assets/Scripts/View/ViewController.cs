using UnityEngine;
namespace View
{
    public class ViewController : MonoBehaviour
    {
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
            _grid.GenerateGrid(width, height);
        }
        public void Init()
        {
            _items = new Items.ItemController(_itemsState, _grid.width * (_grid.height), _grid, _dataItemsPrefab);
        }
        private void OnDestroy()
        {
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
        private void GetTouchedItem() { }
        //ContextMenu
        [ContextMenu("GenerateGrid")]
        public void GenerateGrid()
        {
            ClearItemsView();
            _grid.GenerateGrid(width, height, true);
        }
        [ContextMenu("ClearGrid")]
        public void ClearGrid()
        {
            ClearItemsView();
            _grid.Clear(true);
        }
        [ContextMenu("ClearItems")]
        public void ClearItemsView()
        {
            if (_items == null) return;
            _items.DestroyAll();
        }
        [ContextMenu("Random Items")]
        public void RandomItems()
        {
            ClearItemsView();
            _items = new Items.ItemController(_itemsState, _grid.width * (_grid.height), _grid, _dataItemsPrefab);
            _items.CreateRandom();
        }
    }
}