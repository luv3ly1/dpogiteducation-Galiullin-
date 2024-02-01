using System;
using UnityEngine;

namespace Control
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private SwipeController swipeController;
        private Camera _camera;
        private ObjectPlacementController _objectPlacementController;
        private float _distanceToScreen;

        private Vector3 lastMousePosition;
        private float threshold = 50.0f;
        
        private Action _onDragControlAction;
        private Action _onUpControlAction;
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit, float.MaxValue, 1 << 6))
                {
                    if (hit.collider.TryGetComponent(out _objectPlacementController))
                    {
                        _distanceToScreen = _camera.WorldToScreenPoint(gameObject.transform.position).z;
                        
                        _objectPlacementController.OnInputDown(GetMouseWorldPos());
                        lastMousePosition = Input.mousePosition;
                        _onDragControlAction = OnDraging;
                        _onUpControlAction = _objectPlacementController.OnInputUp;
                    }
                }
                else if(Input.mousePosition.y < Screen.height / 2)
                {
                    swipeController.OnStartDrag();
                    _onDragControlAction = swipeController.OnDrag;
                    _onUpControlAction = swipeController.OnEndDrag;
                }
            }

            if (Input.GetMouseButton(0))
            {
                _onDragControlAction?.Invoke();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _onUpControlAction?.Invoke();
                
                _onDragControlAction = null;
                _onUpControlAction = null;
            }
        }

        private void OnDraging()
        {
            _objectPlacementController._dragTime += Time.deltaTime;
            var dir = Vector2.zero;
            
            var currentMousePosition = Input.mousePosition;
            var mouseDelta = currentMousePosition - lastMousePosition;
            if (Math.Abs(mouseDelta.x) > threshold)
                dir.x += mouseDelta.x > 0 ? 1 : -1;
            if (Math.Abs(mouseDelta.y) > threshold)
                dir.y += mouseDelta.y > 0 ? 1 : -1;
            
            if(dir == Vector2.zero) return;
            
            lastMousePosition = Input.mousePosition;
            _objectPlacementController.Drag(dir);
        }
        
        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distanceToScreen);
            return _camera.ScreenToWorldPoint(mousePoint);
        }
    }
}