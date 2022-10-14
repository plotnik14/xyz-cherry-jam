using System;
using System.Linq;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.ColliderBased
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;
        [SerializeField] private UnityEvent _onCheckComplete;

        private readonly Collider2D[] _interactionResult = new Collider2D[10];

        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResult,
                _mask);

            for (int i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));

                if (isInTags)
                {
                    FireOverlapEvent(overlapResult);
                }
            }
            
            _onCheckComplete?.Invoke();
        }

        protected virtual void FireOverlapEvent(Collider2D overlap)
        {
            _onOverlap?.Invoke(overlap.gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.TransparentRed;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
#endif


        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject> { }
    }
}
