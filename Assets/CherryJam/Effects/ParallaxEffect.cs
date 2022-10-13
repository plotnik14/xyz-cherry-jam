using UnityEngine;

namespace CherryJam.Effects
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField][Range(0f, 1f)] private float _effectStrength = 0.1f;
        [SerializeField] private bool _disableVertical = true;
        [SerializeField] private bool _invertDirection;

        private Vector3 _previousTargetPosition;

        private void Start()
        {
            _previousTargetPosition = _followTarget.position;
        }

        private void LateUpdate()
        {
            var delta = _followTarget.position - _previousTargetPosition;
            var directionMod = _invertDirection ? -1 : 1;

            if (_disableVertical)
                delta.y = 0;

            _previousTargetPosition = _followTarget.position;
            transform.position += delta * _effectStrength * directionMod;
        }
    }
}