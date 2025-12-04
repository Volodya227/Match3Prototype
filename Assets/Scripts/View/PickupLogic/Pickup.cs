using UnityEngine;
using UnityEngine.EventSystems;
namespace View.Pickup
{
    public abstract class Pickup : MonoBehaviour, IPointerClickHandler
    {
        public virtual void OnPicked() { }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPicked();
        }
    }
}