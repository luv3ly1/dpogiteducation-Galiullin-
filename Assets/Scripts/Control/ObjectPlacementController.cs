using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Control
{
    public class ObjectPlacementController : MonoBehaviour
    {
        private const float SnapValue = 1.0f;
        private const float BoxScaleFactor = .5f; 
        private const int WallLayerIndex = 7;
        private const int BackWallLayerIndex = 9;
        private const float TimeToRotate = .4f;
        private const float delay = .05f;

        private Transform _view;
        private PlacementObject _pivot;
        private Rigidbody _rigidbody;
        [HideInInspector][SerializeField]public BoxCollider[] _colliders;
        private readonly List<GameObject> _planeObjects = new();
        private Vector3 _prevPosition;
        private bool _isDragging;
        private bool _isPlacement;
        private bool _isCorrectPlacement;

        public float _dragTime;

        public int Volume => _colliders.Length;
        public bool IsPlacement => _isPlacement;
        public bool IsCorrectPlacement => _isCorrectPlacement;

        private void OnValidate()
        {
            //_colliders = GetComponents<BoxCollider>();
        }

        private void Awake()
        {
            _colliders = GetComponents<BoxCollider>();
            _rigidbody = GetComponent<Rigidbody>();
            _pivot = transform.parent.GetComponent<PlacementObject>();
            _view = transform.GetChild(0);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OnInputDown(Vector3 mousePosition)
        {
            _dragTime = 0;
            _isDragging = true;
            _view.localScale /= 1.2f;
            Invoke(nameof(Cast), delay);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void Drag(Vector2 dir)
        {
            if(!_isDragging || !_pivot.isCanDrag) return;
            
            var newPos = new Vector3(_pivot.transform.position.x - dir.y, 0, _pivot.transform.position.z + dir.x);

            if (!_isPlacement && dir.y == 1f)
            {
                newPos = Vector3.zero;
                Move(newPos);
                _pivot.transform.rotation = Quaternion.identity;
                _isPlacement = true;
                newPos = FirstCheckCollision(newPos);
                _pivot.OnInCar();
                SoundManager.Instance.Play(SoundManager.Instance.onPlaceClip);
            }
            
            
            
            if (newPos.x < Level.Instance.MinXZValues.x - .1f ||
                 newPos.z < Level.Instance.MinXZValues.y - .1f || newPos.z > Level.Instance.MaxXZValues.y + .1f)
                return;
            if(newPos.x > Level.Instance.MaxXZValues.x + .1f && !_isPlacement)
                return;
            
            Move(newPos);
            
            if (GetCollisionType().Contains(CollisionType.BackWall))
            {
                if (!_isPlacement) return;
                _pivot.OnDeplace();
                _isPlacement = false;
                _isDragging = false;
                ClearCast();
                return;
            }
            
            CheckCollision(newPos);
            Invoke(nameof(Cast), delay);
            _isPlacement = true;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        public void OnInputUp()
        {
            _isDragging = false;
            _rigidbody.isKinematic = true;

            if (_dragTime <= TimeToRotate)
                Rotate();
            else
            {
                _isCorrectPlacement = !(GetHighestPoint() > Level.CurrentLevelData.height);
                Level.Instance.OnCheckHeight();
                Level.Instance.OnObjectPlaceChange();
            }
            
            
            
            if (_isPlacement)
                _pivot.OnPlace();
            
            _view.localScale *= 1.2f;
            Invoke(nameof(ClearCast), delay);
        }
        
        private void Move(Vector3 to)
        {
            if(to == _pivot.transform.position) return;
            
            _prevPosition = new Vector3(_pivot.transform.position.x, 0, _pivot.transform.position.z);
            _pivot.transform.position = to;
            _pivot.transform.rotation = Quaternion.identity;
        }   
        
        private void CheckCollision(Vector3 newPos)
        {
            var colision = GetCollisionType();
            if (colision.Contains(CollisionType.Wall) || colision.Contains(CollisionType.BackWall))
            {
                newPos = _prevPosition;
                Move(newPos);
            }
            
            while (GetCollisionType().Contains(CollisionType.DragController))
            {
                newPos.y += SnapValue;
                Move(newPos);
            }
        }

        private Vector3 FirstCheckCollision(Vector3 newPos)
        {
            while (GetCollisionType().Contains(CollisionType.BackWall))
            {
                newPos.x -= SnapValue;
                Move(newPos);
            }
            return new Vector3(newPos.x, 0, newPos.z);
        }
        
        private void Rotate()
        {
            if(!_isPlacement && _pivot.isCanDrag)
            {
                _pivot.ChangeVariant();
                
                if(Level.Instance)
                    Level.Instance.OnObjectInLine();
            }
        }
        
        private List<CollisionType> GetCollisionType()
        {
            var collisionTypes = new List<CollisionType>();
    
            foreach (var boxCollider in _colliders)
            {
                var colliderSize = boxCollider.size;
                var localColliderPosition = boxCollider.center;
                var halfExtents = new Vector3(colliderSize.x * transform.localScale.x / 2, 
                    colliderSize.y * transform.localScale.y / 2,
                    colliderSize.z * transform.localScale.z / 2);

                var overlapBoxPosition = transform.TransformPoint(localColliderPosition);
                var colliders = Physics.OverlapBox(overlapBoxPosition, halfExtents * BoxScaleFactor, transform.rotation);
    
                foreach (var collider in colliders)
                {
                    if (collider.gameObject.layer == WallLayerIndex)
                    {
                        collisionTypes.Add(CollisionType.Wall);
                    }
                    if (collider.gameObject.layer == BackWallLayerIndex)
                    {
                        collisionTypes.Add(CollisionType.BackWall);
                    }
                    if (collider.gameObject != gameObject && collider.gameObject.TryGetComponent(out ObjectPlacementController _))
                    {
                        collisionTypes.Add(CollisionType.DragController);
                    }
                }
            }
            return collisionTypes;
        }
        
        private float GetHighestPoint()
        {
            Vector3 highestPoint = new Vector3(0, 0, 0);

            foreach (var collider in _colliders)
            {
                Bounds bounds = collider.bounds;

                if (bounds.max.y > highestPoint.y)
                {
                    highestPoint = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
                }
            }

            return highestPoint.y;
        }
        
        private void Cast()
        {
            ClearCast();
            
            foreach (var boxCollider in _colliders)
            {
                var center = boxCollider.bounds.center;
                
                var halfHeight = boxCollider.size.y / 2.0f;
                var bottomCenter = center - new Vector3(0, halfHeight, 0);
                
                var ray = new Ray(bottomCenter, Vector3.down);
                var offsetVector = new Vector3(0, 0.001f, 0);
                if (!Physics.Raycast(ray, out var hit) || hit.transform.gameObject == gameObject) continue;
                _planeObjects.Add(hit.distance > .5f || GetHighestPoint() > Level.CurrentLevelData.height
                    ? Instantiate(Level.Instance.redPlane, hit.point + offsetVector, Quaternion.identity)
                    : Instantiate(Level.Instance.greenPlane, hit.point + offsetVector, Quaternion.identity));
            }
        }

        private void ClearCast()
        {
            _planeObjects.ForEach(Destroy);
            _planeObjects.Clear();
        }
    }

    public enum CollisionType
    {
        None,
        Wall,
        BackWall,
        DragController
    }
}