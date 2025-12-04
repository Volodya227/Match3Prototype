using UnityEngine;
using UnityEngine.EventSystems;
namespace View.CameraView
{
    public class CameraController : MonoBehaviour
    {
        private RaycastHit _hit;
        private Ray _ray;
        [SerializeField] private LayerMask _InteractCellMask;
        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
                FindCellGrid();
        }
        public void FindCellGrid()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray: _ray, out _hit, layerMask: _InteractCellMask, maxDistance:1000))
            {
                _hit.collider.GetComponent<Pickup.Pickup>()?.OnPicked();
            }
        }
    }
}