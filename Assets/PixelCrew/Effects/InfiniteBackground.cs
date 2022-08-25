﻿using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Effects
{
    public class InfiniteBackground : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _container;

        private Bounds _containerBounds;
        private Bounds _allBounds;
        
        private Vector3 _containerDelta;
        private Vector3 _screenSize;

        private void Start()
        {
            var sprites = _container.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites)
            {
                _containerBounds.Encapsulate(sprite.bounds);
            }

            _allBounds = _containerBounds;
            _containerDelta = _container.position - _containerBounds.center;
        }

        private void LateUpdate()
        {
            var min = _camera.ViewportToWorldPoint(Vector3.zero); // bottom left
            var max = _camera.ViewportToWorldPoint(Vector3.one); // top right

            _screenSize = new Vector3(max.x - min.x, max.y - min.y);
            var camPosition = _camera.transform.position.x;
            
            var screenLeft = new Vector3(camPosition - _screenSize.x / 2, _containerBounds.center.y);
            var screenRight = new Vector3(camPosition + _screenSize.x / 2, _containerBounds.center.y);

            if (!_allBounds.Contains(screenLeft))
            {
                InstantiateContainer(_allBounds.min.x - _containerBounds.extents.x);
            }
            
            if (!_allBounds.Contains(screenRight))
            {
                InstantiateContainer(_allBounds.max.x + _containerBounds.extents.x);
            }
        }

        private void InstantiateContainer(float boundCenterX)
        {
            var newBounds = new Bounds(new Vector3(boundCenterX, _containerBounds.center.y), _containerBounds.size);
            _allBounds.Encapsulate(newBounds);

            var newContainerXPos = boundCenterX + _containerDelta.x;
            var newPosition = new Vector3(newContainerXPos, _container.transform.position.y);
            Instantiate(_container, newPosition, Quaternion.identity, transform);
        }

        // private void OnDrawGizmosSelected()
        private void OnDrawGizmos()
        {
            GizmosUtils.DrawBounds(_allBounds, Color.magenta);
            GizmosUtils.DrawBounds(_containerBounds, Color.yellow);
        }
    }
}