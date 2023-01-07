using System;
using System.Linq;
using CherryJam.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.ColliderBased
{
    public class OverlapCheck : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] protected LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;
        [SerializeField] private UnityEvent _onCheckComplete;

        protected readonly Collider2D[] InteractionResult = new Collider2D[10];

        public void Check()
        {
            var size = CheckOverlap();

            for (int i = 0; i < size; i++)
            {
                var overlapResult = InteractionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));

                if (isInTags)
                {
                    FireOverlapEvent(overlapResult);
                }
            }
            
            _onCheckComplete?.Invoke();
        }

        protected virtual int CheckOverlap()
        {
            return Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                InteractionResult,
                _mask);
        }
        
        protected virtual void FireOverlapEvent(Collider2D overlap)
        {
            _onOverlap?.Invoke(overlap.gameObject);
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.TransparentRed01;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
#endif


        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject> { }
    }
}
