using CherryJam.Components.Audio;
using CherryJam.Components.ColliderBased;
using CherryJam.Components.GoBased;
using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.Creatures.Mobs
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
        protected bool IsFrozen;
        protected readonly Cooldown FreezeCooldown = new Cooldown();
        
        private Color _colorBeforeFreezing;

        protected static readonly int Range = Animator.StringToHash("range");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        private void Update()
        {
            CheckActiveBuffs();
            
            if (IsFrozen) return;
            
            if (_usedByGroup) return;
            if (!_vision.IsTouchingLayer && !_shootAlways) return;
            if (PerformExtraActionAndStop()) return;

            if (_rangeCooldown.IsReady)
            {
                RangeAttack();
            }
        }

        private void CheckActiveBuffs()
        {
            if (IsFrozen && FreezeCooldown.IsReady)
                Unfreeze();
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

        public void Freeze()
        {
            _animator.enabled = false;
            
            var sprite = GetComponent<SpriteRenderer>();
            _colorBeforeFreezing = sprite.color;
            var frozenColor = new Color(0f, 255f, 255f);
            sprite.color = frozenColor;

            FreezeCooldown.Value = 10; // ToDo Move for defs
            FreezeCooldown.Reset();
            IsFrozen = true;
        }
        
        private void Unfreeze()
        {
            IsFrozen = false;
            
            var sprite = GetComponent<SpriteRenderer>();
            sprite.color = _colorBeforeFreezing;
            
            _animator.enabled = true;
        }
    }
}
