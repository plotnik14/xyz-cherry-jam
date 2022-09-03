using System.Collections;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Mobs.Patrolling;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class MobAI : MonoBehaviour
    {
        [Header("MobAI")]
        [SerializeField] protected ColliderCheck _vision;
        [SerializeField] protected ColliderCheck _canAttack;

        [SerializeField] protected float _alarmDelay;
        [SerializeField] protected float _attackCooldown;
        [SerializeField] protected float _missHeroCooldown;

        protected IEnumerator _current;
        protected GameObject _target;
        protected bool _isDead;

        protected SpawnListComponent _particles;
        protected Creature _creature;
        protected Animator _animator;
        protected Patrol _patrol;

        protected static readonly int IsDeadKey = Animator.StringToHash("is-dead");

        protected virtual void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;

            StartState(AgroToHero());
        }

        protected virtual IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();

                }

                yield return null;
            }

            StopCreature();

            _particles.Spawn("MissHero");
            yield return new WaitForSeconds(_missHeroCooldown);

            StartState(_patrol.DoPatrol());
        }

        protected IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }

        protected virtual void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }

        protected Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        private IEnumerator AgroToHero()
        {
            LookAtHero();
            _particles.Spawn("Exclamation");

            yield return new WaitForSeconds(_alarmDelay);

            StartState(GoToHero());
        }

        private void LookAtHero()
        {
            StopCreature();
            var direction = GetDirectionToTarget();
            _creature.UpdateSpriteDirection(direction);
        }

        protected void StopCreature()
        {
            _creature.SetDirection(Vector2.zero);
        }

        protected void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
            {
                StopCoroutine(_current);
            }

            _current = coroutine;
            StartCoroutine(coroutine);
        }

        public void DisableAI()
        {
            StopCreature();
            _vision.gameObject.SetActive(false);
            StopCoroutine(_current);
        }
        
        public void EnableAI()
        {
            _vision.gameObject.SetActive(true);
            StartState(_patrol.DoPatrol());
        }

        public void OnDie()
        {
            StopCreature();
            _isDead = true;

            _animator.enabled = true;
            _animator.SetBool(IsDeadKey, true);
            
            if (_current != null)
            {
                StopCoroutine(_current);
            }
        }

    }
}
