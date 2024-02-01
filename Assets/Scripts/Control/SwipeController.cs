using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Control
{
    public class SwipeController : MonoBehaviour
    {
        [SerializeField] private float spacing = 1f;
        [SerializeField] private float speed = 10.0f;
        [SerializeField] private float maxZ = -30f;
        [SerializeField] private float animTime = 1f;
        [SerializeField] private bool isBuildOnStart;

        private Vector3 _lastMousePosition;

        private void Start()
        {
            if(isBuildOnStart) BuildLine();
        }

        public void OnStartDrag()
        {
            _lastMousePosition = Input.mousePosition;
        }

        public void OnDrag()
        {
            var delta = Input.mousePosition - _lastMousePosition;
            transform.Translate(0, 0, delta.x * speed * Time.deltaTime);
            _lastMousePosition = Input.mousePosition;
        }

        public void OnEndDrag()
        {
            if (transform.localPosition.z > 0f)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            else if (transform.localPosition.z < maxZ)
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, maxZ);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void BuildLine(Transform newObject = null)
        {
            var currentZ = 0f;
            var maxZItem = float.MinValue;

            var objectsAndTargets = new List<Tuple<Transform, Vector3>>();
            
            StopAllCoroutines();
            
            if(newObject) newObject.SetSiblingIndex(1);
            
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var objectColliders = child.GetComponentsInChildren<Collider>();
                var size = CalculateTotalSize(objectColliders);
                
                objectsAndTargets.Add(new Tuple<Transform, Vector3>(child, new Vector3(-size.x / 2, 0, currentZ)));
                child.rotation = transform.rotation;
                
                maxZItem = Math.Max(maxZItem, child.localPosition.z + size.z / 2 + spacing);
                currentZ += size.z + spacing;
            }
            
            MoveToObjs(objectsAndTargets);
            maxZ = -(currentZ - 2f);
        }

        private void MoveToObjs(List<Tuple<Transform, Vector3>> objectsAndTargets)
        {
            foreach (var objectAndTarget in objectsAndTargets)
            {
                StartCoroutine(MoveTo(objectAndTarget.Item1, objectAndTarget.Item2)); 
            }
        }
        
        private IEnumerator MoveTo(Transform obj,Vector3 target)
        {
            float elapsedTime = 0;
            obj.localPosition = new Vector3(target.x, obj.localPosition.y, obj.localPosition.z);
            Vector3 startingPos = obj.localPosition;
            
            while (elapsedTime < animTime)
            {
                if (obj.parent != transform) yield break;
                obj.localPosition = Vector3.Lerp(startingPos, target, elapsedTime / animTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            obj.localPosition = target;
        }

        private Vector3 CalculateTotalSize(IEnumerable<Collider> colliders)
        {
            var min = Vector3.one * float.MaxValue;
            var max = Vector3.one * float.MinValue;

            foreach (var collider in colliders)
            {
                var localMin = collider.transform.parent.InverseTransformPoint(collider.bounds.min);
                var localMax = collider.transform.parent.InverseTransformPoint(collider.bounds.max);

                min = Vector3.Min(min, localMin);
                max = Vector3.Max(max, localMax);
            }

            return max - min;
        }
    }
}