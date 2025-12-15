using UnityEngine;
namespace View.Items
{
    public class ItemController
    {
        private readonly Data.ItemsState _state;
        private readonly ItemView[] _itemsView;
        private readonly Grid.GridView _grid;
        private readonly DataConfig.ItemDATAPrefabs _prefabs;
        private float _deltaItemsSize = .6f;
        public ItemController(Data.ItemsState state, int maxCount, Grid.GridView grid, DataConfig.ItemDATAPrefabs prefabs , float deltaItemsSize = .6f)
        {
            _state = state;
            _itemsView = new ItemView[Mathf.Max(1, maxCount)];
            _grid = grid;
            _prefabs = prefabs;
            _deltaItemsSize = deltaItemsSize;
        }
        
        public void Update()
        // Update() - може бути заплутано з Unity's Update().
        // Краще: Tick(), ProcessFrame(), UpdateViews()
        {
            for (int i = 0; i < _itemsView.Length; i++)
            {
                if (_itemsView[i] != null)
                {
                    _itemsView[i].Update();
                    if (!_itemsView[i].Active) _itemsView[i] = null;//update all in one iteration included with delete
                    //Добре, оптимізація - видалення в тому ж циклі що й оновлення.
                }
            }
            Create();
        }
        
        private void Create()
        // Create() - занадто загально. Краще: CreateNewItemViews() або SpawnPendingItems()
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
        // FactoryMethod - це патерн, не назва методу.
        // Краще: CreateItemView() або InstantiateItemView()
        {
            GameObject view = Object.Instantiate(GetRandomItemView(data));
            if (_grid.IsUI)
            {
                RectTransform rt = view.transform as RectTransform;
                float size = _deltaItemsSize * _grid.itemSizePixel;
                rt.sizeDelta = new Vector2(size, size);
                //view.transform.localScale *= _grid.itemSizePixel;// _grid.GetPosition(_grid.itemSizePixel, _grid.itemSizePixel, true);
                // Закоментований код - видалити якщо не потрібен.
            }
            return new ItemView(data, view, _grid);
        }
        
        private GameObject GetRandomItemView(Data.ItemReadonly data = null) {
        // GetRandomItemView - але метод не завжди повертає рандомний,
        // якщо data != null то по конкретному кольору/типу.
        // Краще: GetItemPrefab() або SelectItemPrefab()
            Data.ItemTypeColor color;
            int type;
            if (data != null)
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
            // Цей if-else ланцюжок можна замінити на switch або Dictionary.
            // Ще краще - змінити ItemDATAPrefabs щоб мати один метод GetPrefab(color, type).
            // Приклад:
            // return _prefabs.GetPrefab(color, type);
            // де GetPrefab всередині використовує Dictionary<ItemTypeColor, GameObject[]>
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
            // Добре - Warning коли не вдалось додати - правильно для дебагу.
            // Але краще детальніше: $"Created item lost - array full. Capacity: {_itemsView.Length}"
            
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