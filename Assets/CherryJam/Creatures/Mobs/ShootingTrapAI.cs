using CherryJam.Components.Audio;
using CherryJam.Components.ColliderBased;
using CherryJam.Components.GoBased;
using CherryJam.Utils;
using UnityEngine;

namespace CherryJam.Creatures.Mobs
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private ColliderCheck _vision;
        
        [Header("Attack")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private DirectionalSpawnComponent _rangeAttack;
        [SerializeField] private bool _shootAlways;

        private Animator _animator;
        private PlaySoundsComponent _sounds;

        private static readonly int AttackKey = Animator.StringToHash("attack");
        
        private Hero.Hero _hero;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _sounds = GetComponent<PlaySoundsComponent>();
        }

        private void Update()
        {
            if (!_vision.IsTouchingLayer && !_shootAlways) return;

            if (_rangeCooldown.IsReady)
                RangeAttack();
            
        }

        public void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(AttackKey);
            _sounds.Play("Attack");
        }

        public void OnAttack()
        {
            if (_hero == null)
                _hero = FindObjectOfType<Hero.Hero>();
            
            var directionToHero = GetDirectionToTarget(_hero.gameObject);
            _rangeAttack.Spawn(directionToHero);
        }
        
        private Vector2 GetDirectionToTarget(GameObject target)
        {
            var direction = target.transform.position - transform.position;
            return direction.normalized;
        }
    }
}
