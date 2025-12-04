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
            for (int i = 0; i < _itemsView.Length; i++)
            {
                if (_itemsView[i] != null)
                {
                    _itemsView[i].Update();
                    if (!_itemsView[i].Active) _itemsView[i] = null;//update all in one iteration included with delete
                }
            }
            Create();
        }
        private void Create()
        {
            for (int i = 0; i < _state.CreatedItemCount; i++)
            {
                Data.ItemReadonly item = _state.GetCreatedItemByID(i);
                if (item != null)
                {
                    ItemView itemView = FactoryMethod(item);
                    itemView.SetParent(_grid.GetItem(item.X, item.Y), true);
                    AddNewItemToList(itemView);//not null
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
                    itemView.SetParent(_grid.GetItemByIndex(i), true);
                    AddNewItemToList(itemView);
                }
            }
        }
        private ItemView FactoryMethod(Data.ItemReadonly data = null)
        {
            GameObject view = Object.Instantiate(GetRandomItemView(data));
            return new ItemView(data, view, _grid);
        }
        private GameObject GetRandomItemView(Data.ItemReadonly data = null) {
            Data.ItemTypeColor color;
            int type;
            if (data == null)
            {
                color = data.Color;
                type = (int)data.Type;
            }
            else
            {
                color = (Data.ItemTypeColor)Random.Range(0, System.Enum.GetValues(typeof(Data.ItemTypeColor)).Length);
                type = Random.Range(0, System.Enum.GetValues(typeof(Data.ItemType)).Length);
            }
            GameObject res;
            if (color == Data.ItemTypeColor.Green) {
                res = _prefabs.GetPrefabCommonGreen(type);
            }
            else if (color == Data.ItemTypeColor.Yellow)
            {
                res = _prefabs.GetPrefabCommonYellow(type);
            }
            else if (color == Data.ItemTypeColor.Purple)
            {
                res = _prefabs.GetPrefabCommonPurple(type);
            }
            else if (color == Data.ItemTypeColor.Red)
            {
                res = _prefabs.GetPrefabCommonRed(type);
            }
            else
            {
                res = _prefabs.GetPrefabCommonBlue(type);
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
                    return;
                }
            }
            Debug.LogWarning("created item losing");
            item.Destroy();
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