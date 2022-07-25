using PixelCrew.Utils;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] protected bool _usedByGroup;
        [SerializeField] protected ColliderCheck _vision;

        [Header("Range")]
        [SerializeField] protected Cooldown _rangeCooldown;
        [SerializeField] protected SpawnComponent _rangeAttack;

        protected Animator _animator;

        protected static readonly int Range = Animator.StringToHash("range");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_usedByGroup) return;
            if (!_vision.IsTouchingLayer) return;
            if (PerformExtraActionAndStop()) return;

            if (_rangeCooldown.IsReady)
            {
                RangeAttack();
            }
        }

        public virtual bool PerformExtraActionAndStop()
        {
            return false;
        }

        public void RangeAttack()
        {
            if (!_usedByGroup)
            {
                _rangeCooldown.Reset();
            }
            
            _animator.SetTrigger(Range);
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}
