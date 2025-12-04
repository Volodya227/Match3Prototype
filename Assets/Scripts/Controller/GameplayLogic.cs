using UnityEngine;
namespace controller
{
    public class GameplayLogic : MonoBehaviour
    {
        [SerializeField] private View.ViewController _viewPrefab;
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
            _view.SetItemsState(_itemsState);
            _view.Init();
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