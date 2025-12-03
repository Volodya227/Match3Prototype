using UnityEngine;
namespace View.Items
{
    public class ItemController
    {
        private readonly Data.ItemsState _state;
        private readonly ItemView[] _itemsView;
        private readonly Grid.GridView _grid;
        private readonly DataConfig.ItemDATAPrefabs _prefabs;
        public ItemController(Data.ItemsState state, int maxCount, Grid.GridView grid, DataConfig.ItemDATAPrefabs prefabs)
        {
            _state = state;
            _itemsView = new ItemView[Mathf.Max(1, maxCount)];
            _grid = grid;
            _prefabs = prefabs;
        }
        public void Update()
        {
            Create();
            for (int i = 0; i < _itemsView.Length; i++)
            {
                if (_itemsView[i] != null)
                {
                    _itemsView[i].Update();
                    if (!_itemsView[i].Active) _itemsView[i] = null;//update all in one iteration included with delete
                }
            }
        }
        private void Create()
        {
            for (int i = 0; i < _state.CreatedItemCount; i++)
            {
                Data.ItemReadonly item = _state.GetCreatedItemByID(i);
                if (item != null)
                {
                    ItemView itemView = FactoryMethod(item);
                    AddNewItemToList(itemView);//not null
                    itemView.SetParent(_grid.GetItem(item.X, item.Y), true);
                }
            }
        }
        public void CreateRandom()
        {
            DestroyAll();
            for (int i = 0; i < _itemsView.Length; i++)
            {
                if (_itemsView[i] == null)
                {
                    ItemView itemView = FactoryMethod();
                    AddNewItemToList(itemView);
                    itemView.SetParent(_grid.GetItemByIndex(i), true);
                }
            }
        }
        private ItemView FactoryMethod(Data.ItemReadonly data = null)
        {
            GameObject view = Object.Instantiate(GetRandomItemView(data));
            return new ItemView(data, view, _grid);
        }
        private GameObject GetRandomItemView(Data.ItemReadonly data = null) {
            int color = 1;
            if (data == null)
            {
                color = Random.Range(1, 5);
            }
            GameObject res;
            if (color > 2) {
                res = _prefabs.GetPrefabCommonGreen(Random.Range(0, 2));
            }
            else
            {
                res = _prefabs.GetPrefabCommonBlue(Random.Range(0, 2));
            }
            return res;
        }
        private void AddNewItemToList(ItemView item)
        {
            if (item == null) return;
            for (int i = 0; i < _itemsView.Length; i++)
            {
                if (_itemsView[i] == null)
                {
                    _itemsView[i] = item;
                    break;
                }
            }
        }
        public void DestroyAll()
        {
            for (int i = 0; i < _itemsView.Length; i++)
            {
                if (_itemsView[i] != null) {
                    _itemsView[i].Destroy(true);
                    _itemsView[i] = null;
                }
            }
        }
    }
}