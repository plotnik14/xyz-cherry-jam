using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.Components.ColliderBased
{
    public class OverlapBoxCheckWithDirection : OverlapCheck
    {
        [SerializeField] private Vector2 _boxSize;
        [SerializeField] private float _boxAngle;
        
        [SerializeField] private OverlapCircleCheckWithDirection.OnOverlapWithDirectionEvent _onOverlapDirectional;

        protected override int CheckOverlap()
        {
            return Physics2D.OverlapBoxNonAlloc(
                transform.position,
                _boxSize,
                _boxAngle,
                InteractionResult,
                _mask);
        }

        protected override void FireOverlapEvent(Collider2D overlap)
        {
            base.FireOverlapEvent(overlap);
            _onOverlapDirectional?.Invoke(overlap.gameObject, transform.lossyScale);
        }
        
#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = HandlesUtils.TransparentRed04;
            Gizmos.DrawCube(transform.position, _boxSize);
            Gizmos.color = Color.white;
        }
#endif
    }
}