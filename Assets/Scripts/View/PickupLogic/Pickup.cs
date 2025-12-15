using UnityEngine;
using UnityEngine.EventSystems;
namespace View.Pickup
{
    public abstract class Pickup : MonoBehaviour, IPointerClickHandler
    // Абстрактний базовий клас для інтерактивних об'єктів.
    // IPointerClickHandler - правильний вибір для UI елементів.
    {
        public virtual void OnPicked() { }
        // virtual з пустою реалізацією vs abstract.
        // Якщо всі нащадки ПОВИННІ реалізувати - краще abstract.
        // Якщо OnPicked опціональний - virtual ок.
        // В даному проекті CellGrid завжди override, тому abstract був би чіткіший контракт.

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPicked();
        }
        // Простий bridge між Unity's event system і логікою.
    }
    
    // Клас мінімалістичний і чистий.
}