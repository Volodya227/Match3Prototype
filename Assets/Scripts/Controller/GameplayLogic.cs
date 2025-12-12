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
        [SerializeField] private Model.DirectionMode _directionModeForMoving = new();
        [SerializeField] private Model.DirectionMode _directionModeForGrouping = new();
        [SerializeField, Range(2, 10)] private int _minCountGroup = 3;
        private float _timerForUpdateState;
        private float _timeUpdateStep = .3f;

        private void Awake()
        {
            _view = Instantiate(_viewPrefab);
            _view.transform.position = Vector3.zero;
        }
        private void Start()
        {
            View.Grid.GridView gridView = _view.Grid;
            _model = new Model.ModelController(gridView.width, gridView.height, _directionModeForGrouping, _directionModeForMoving, _minCountGroup);
            _itemsState = _model.ItemsState;
            _view.Init(_itemsState, _cameraView);
            _view.EventOnClickedCellGrid += _model.SetClickedCellGrid;
        }
        private void FixedUpdate()
        {
            _timerForUpdateState += Time.deltaTime;
            if (_timerForUpdateState >= _timeUpdateStep)
            {
                _timerForUpdateState = 0;
                _model.UpdateTick();
                _view.UpdateTick();
            }
        }
        private void OnDestroy()
        {
            _view.EventOnClickedCellGrid -= _model.SetClickedCellGrid;
        }
    }
}