using UnityEngine;
using UnityEngine.EventSystems;
namespace View.CameraView
{
    public class CameraController : MonoBehaviour
    {
        // CameraController в namespace CameraView - надлишково.
        // Або namespace View.Camera і клас Controller,
        // або namespace View і клас CameraController.
        // Зараз повний шлях: View.CameraView.CameraController - занадто багато "Camera/View".
        
        private RaycastHit _hit;
        private Ray _ray;
        // Ці поля використовуються тільки в FindCellGrid().
        // Можна зробити локальними змінними. Кешування тут не дає переваги,
        // бо RaycastHit і Ray - це structs (value types).
        
        [SerializeField] private LayerMask _InteractCellMask;
        // _InteractCellMask - мішаний стиль (PascalCase після _).
        // Конвенція: _interactCellMask або _interactableCellMask
        
        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
                FindCellGrid();
            // Використання старої Input System.
            // Для нових проектів рекомендується нова Input System, але для навчального проекту - ок.
        }
        
        public void FindCellGrid()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            // Правильна перевірка - ігноруємо кліки по UI.
            
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // amera.main викликає FindGameObjectWithTag кожен раз.
            // Краще кешувати: private Camera _mainCamera; в Start(): _mainCamera = Camera.main;
            
            if (Physics.Raycast(ray: _ray, out _hit, layerMask: _InteractCellMask, maxDistance:1000))
            // MAGIC NUMBER: maxDistance: 1000 - краще зробити константою або SerializeField
            // для налаштування в Inspector.
            {
                _hit.collider.GetComponent<Pickup.Pickup>()?.OnPicked();
                // Null-conditional operator - правильне використання.
            }
        }
    }
}