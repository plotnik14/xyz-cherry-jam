using System;
using UnityEngine;
using UnityEngine.Events;

namespace CherryJam.Components.ColliderBased
{
    public class OverlapCircleCheckWithDirection : OverlapCheck
    {
        [SerializeField] private OnOverlapWithDirectionEvent _onOverlapDirectional;

        protected override void FireOverlapEvent(Collider2D overlap)
        {
            base.FireOverlapEvent(overlap);
            _onOverlapDirectional?.Invoke(overlap.gameObject, transform.lossyScale);
        }

        [Serializable]
        public class OnOverlapWithDirectionEvent : UnityEvent<GameObject, Vector2> { }
    }
}