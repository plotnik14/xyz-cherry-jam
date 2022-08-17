using PixelCrew.Components.Audio;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] protected bool _usedByGroup;
        [SerializeField] protected ColliderCheck _vision;
        [SerializeField] protected bool _shootAlways;

        [Header("Range")]
        [SerializeField] protected Cooldown _rangeCooldown;
        [SerializeField] protected SpawnComponent _rangeAttack;

        protected Animator _animator;
        protected PlaySoundsComponent Sounds;

        protected static readonly int Range = Animator.StringToHash("range");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        private void Update()
        {
            if (_usedByGroup) return;
            if (!_vision.IsTouchingLayer && !_shootAlways) return;
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
            Sounds.Play("Range");
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}
