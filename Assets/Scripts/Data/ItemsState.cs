using UnityEngine;
namespace Data
{
    public enum ItemType
    {
        Common1, Common2, Common3
    }
    //use quad form
    public enum ItemTypeColor
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple
    }
    public abstract class ItemReadonly
    {
        public event System.Action EventDestroy;
        public event System.Action EventAnimatedFall;
        public int X { get; protected set; }
        public int Y { get; protected set; }
        //set in Init
        public ItemType Type { get; protected set; }
        public ItemTypeColor Color { get; protected set; }
        public ItemReadonly(ItemType type, ItemTypeColor color)
        {
            Type = type;
            Type = ItemType.Common1; // only one type
            Color = color;
        }
        public void EventDestroyActivated() { EventDestroy?.Invoke(); }
        public void EventAnimatedFallActivated() { EventAnimatedFall?.Invoke(); }
    }
    public class Item : ItemReadonly
    {
        public bool Active { get; private set; }
        public Item(int x, int y, ItemType type, ItemTypeColor color) : base(type, color) { SetXY(x, y); Active = true; }
        public void SetXY(int x, int y) { X = x; Y = y; }
        public void Delete()
        {
            Active = false;//for model
            EventDestroyActivated();//for view
        }
    }
    public class ItemsState
    {
        protected readonly Item[] _items;
        protected readonly Item[] _createdItem;
        public int Width { get; protected set; }
        public int CreatedItemCount => _createdItem.Length;
        public ItemReadonly GetCreatedItemByID(int id) { return _createdItem[Mathf.Clamp(id, 0, _createdItem.Length - 1)]; }//can create fake item "copy item by old data in view"
        public ItemsState(int width, int height)
        {
            Width = width;
            _items = new Item[width * height];
            _createdItem = new Item[width];
        }
    }

}