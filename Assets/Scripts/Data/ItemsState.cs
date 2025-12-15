using UnityEngine;
namespace Data
{
    public enum ItemType
    {
        Common1, Common2, Common3
    }
    //use quad form
    // Коментар "use quad form" незрозумілий. Що це означає?
    // Якщо це важлива інформація - розкрити детальніше.
    // Якщо застарілий коментар - видалити.
    
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
        // Назва "ItemReadonly" трохи дивна. 
        // Це швидше ItemBase або ItemData, бо клас не є readonly - він має protected setters.
        // "Readonly" в назві зазвичай означає immutable об'єкт.
        
        public event System.Action EventDestroy;
        public event System.Action EventMove;
        public event System.Action EventAnimatedFall;
        // Конвенція для events - використовувати дієслово в минулому часі 
        // або з префіксом On: OnDestroyed, OnMoved, OnFallAnimationStarted
        // Або: Destroyed, Moved, FallAnimationRequested
        
        public int X { get; protected set; }
        public int Y { get; protected set; }
        //set in Init
        public ItemType Type { get; protected set; }
        public ItemTypeColor Color { get; protected set; }
        
        public ItemReadonly(ItemType type, ItemTypeColor color)
        {
            Type = type;
            Type = ItemType.Common1; // only one type
            // Тут Type присвоюється двічі - спочатку з параметра, потім хардкодиться.
            // Це явно тимчасовий код. Або видалити другий рядок, або видалити параметр type з конструктора.
            
            Color = color;
        }
        protected void EventDestroyActivated() { EventDestroy?.Invoke(); }
        protected void EventAnimatedFallActivated() { EventAnimatedFall?.Invoke(); }
        protected void EventMoveActivated() { EventMove?.Invoke(); }
        // Методи називаються "...Activated" але вони "викликають" event.
        // Краще: RaiseDestroyEvent(), InvokeDestroy(), або OnDestroy() (protected virtual)
    }
    
    public class Item : ItemReadonly
    {
        public bool visited;// for find groups
        // Публічне поле visited порушує інкапсуляцію.
        // або зробити internal, або перенести логіку visited в GroupingSystem
        
        public bool Active { get; private set; }
        
        public Item(int x, int y, ItemType type, ItemTypeColor color) : base(type, color) { SetXY(x, y); Active = true; }
        // Занадто багато в одному рядку. Розбити на кілька рядків:
        // public Item(int x, int y, ItemType type, ItemTypeColor color) : base(type, color) 
        // { 
        //     SetXY(x, y); 
        //     Active = true; 
        // }
        
        public void SetXY(int x, int y) { X = x; Y = y; }
        
        public void Delete()
        {
            Active = false;//for model
            EventDestroyActivated();//for view
            // Гарні коментарі що пояснюють призначення кожної дії.
        }
        public void EventAnimatedFallActivate() { EventAnimatedFallActivated(); }
        public void EventMoveActivate() { EventMoveActivated(); }
        // Плутанина між EventAnimatedFallActivate і EventAnimatedFallActivated.
        // Один публічний, інший protected - але назви майже однакові.
        // Публічний метод краще назвати: TriggerFallAnimation(), RequestFallAnimation()
    }
    
    public class ItemsState
    {
        protected readonly Item[] _items;
        protected readonly Item[] _createdItem;
        // _createdItem - однина, але це масив. Краще _createdItems або _newlyCreatedItems
        
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int CreatedItemCount => _createdItem.Length;
   
        public ItemReadonly GetCreatedItemByID(int id) { return _createdItem[Mathf.Clamp(id, 0, _createdItem.Length - 1)]; }//can create fake item "copy item by old data in view"
        // Коментар "can create fake item" незрозумілий без контексту.
        
        public ItemsState(int width, int height)
        {
            Width = width;
            Height = height;
            _items = new Item[width * height];
            _createdItem = new Item[width];
        }
    }

}