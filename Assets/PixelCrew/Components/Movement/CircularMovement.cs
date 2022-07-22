using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace PixelCrew.Components
{
    public class CircularMovement : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed= 1f;
        [SerializeField] private bool _invert;

        private Rigidbody2D[] _rigidbodies;
        private float _directionMod;

        private void Awake()
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody2D>();
            _directionMod = _invert ? -1f : 1f;
        }

        private void FixedUpdate()
        {
           for (int i = 0; i < _rigidbodies.Length; i++)
            {
                if (_rigidbodies[i] == null) continue;

                var position = _rigidbodies[i].position;
                var angle = i * 2f * Mathf.PI / _rigidbodies.Length;
                position.x = transform.position.x + Mathf.Cos(angle + Time.time * _speed * _directionMod) * _radius;
                position.y = transform.position.y + Mathf.Sin(angle + Time.time * _speed * _directionMod) * _radius;
                _rigidbodies[i].MovePosition(position);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Transform[] transforms = GetComponentsInChildren<Transform>()
                .Where(t => t != transform)
                .ToArray();

            for (int i = 0; i < transforms.Length; i++)
            {
                var angle = i * 2f * Mathf.PI / transforms.Length;
                var newX = transform.position.x + Mathf.Cos(angle) * _radius;
                var newY = transform.position.y + Mathf.Sin(angle) * _radius;
                transforms[i].position = new Vector3(newX, newY, transform.position.z);
            }
        }

        private void OnDrawGizmos()
        {
            Handles.color = HandlesUtils.Green;
            Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
        }
    }
#endif
}
