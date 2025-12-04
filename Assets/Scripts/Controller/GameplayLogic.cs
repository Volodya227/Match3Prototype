using UnityEngine;
namespace controller
{
    public class GameplayLogic : MonoBehaviour
    {
        [SerializeField] private View.ViewController _viewPrefab;
        [SerializeField] private View.CameraView.CameraController _cameraView;
        private View.ViewController _view;
        private Data.ItemsState _itemsState;
        private Model.ModelController _model;
        private void Awake()
        {
            _view = Instantiate(_viewPrefab);
            _view.transform.position = Vector3.zero;
        }
        private void Start()
        {
            View.Grid.GridView gridView = _view.Grid;
            _model = new Model.ModelController(gridView.width, gridView.height);
            _itemsState = _model.ItemsState;
            _view.Init(_itemsState, _cameraView);
            _view.EventOnClickedCellGrid += _model.SetClickedCellGrid;
        }
        private void FixedUpdate()
        {
            _model.UpdateTick();
            _view.UpdateTick();
        }
        private void OnDestroy()
        {
            _view.EventOnClickedCellGrid -= _model.SetClickedCellGrid;
        }
    }
}