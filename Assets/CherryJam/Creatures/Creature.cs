using System.Collections;
using CherryJam.Components.Audio;
using CherryJam.Components.ColliderBased;
using CherryJam.Components.GoBased;
using UnityEngine;

namespace CherryJam.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] protected bool _invertScale;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] protected float _damageJumpSpeed;
        [SerializeField] protected float _damageAnimationDuration;

        [Header("Checkers")]
        [SerializeField] protected ColliderCheck _groundCheck;
        [SerializeField] protected OverlapCheck _attackRange;
        [SerializeField] public DirectionalSpawnComponent _rangeProjectileSpawnerCreature;

        protected Rigidbody2D Rigidbody;
        protected Vector2 Direction;
        protected Animator Animator;
        protected PlaySoundsComponent Sounds;
        protected SpriteRenderer Sprite;
        protected bool IsGrounded;
        protected bool IsJumping;
        protected bool IsJumpActivated;
        protected float DamageXModifier;

        private Hero.Hero _hero;

        private Coroutine _hitEffect;
        private Coroutine _pushEffect;
        
        protected static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        protected static readonly int IsRunningKey = Animator.StringToHash("is-running");
        protected static readonly int IsOnGroundKey = Animator.StringToHash("is-on-ground");
        protected static readonly int HitKey = Animator.StringToHash("hit");
        protected static readonly int AttackKey = Animator.StringToHash("attack");
        protected static readonly int RangeAttackKey = Animator.StringToHash("range-attack");
        protected static readonly int IsDeadKey = Animator.StringToHash("is-dead");
        protected static readonly int HealKey = Animator.StringToHash("heal");


        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
            Sprite = GetComponent<SpriteRenderer>();

            IsGrounded = _groundCheck.IsTouchingLayer;
        }

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }

        protected virtual void FixedUpdate()
        {
            var xVelocity = CalculateXVelocity();
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetFloat(VerticalVelocityKey, Rigidbody.velocity.y);
            Animator.SetBool(IsRunningKey, Direction.x != 0);
            Animator.SetBool(IsOnGroundKey, IsGrounded);

            UpdateSpriteDirection(Direction);
        }

        protected virtual float CalculateXVelocity()
        {
            return Direction.x * CalculateSpeed() + DamageXModifier;
        }

        protected virtual float CalculateSpeed()
        {
            return _speed;
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isUpDirection = Direction.y > 0;

            if (IsGrounded)
                IsJumping = false;
            

            if (isUpDirection)
            {
                IsJumping = true;
                
                if (IsJumpActivated) return yVelocity;
                
                IsJumpActivated = true;
                return CalculateJumpVelocity(yVelocity);
            }

            IsJumpActivated = false;
            
            if (IsJumping && Rigidbody.velocity.y > 0)
            {
                return yVelocity * 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity = _jumpSpeed;
                DoJumpVfx();
            }

            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            Sounds.Play("Jump");
        }

        public virtual void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            var localScale = transform.localScale;
            
            if (direction.x > 0)
            {
                transform.localScale = new Vector3( Mathf.Abs(localScale.x) * multiplier, localScale.y, localScale.z);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * Mathf.Abs(localScale.x) * multiplier, localScale.y, localScale.z);
            }
        }
        
        public virtual void TakeDamage()
        {
            IsJumping = false;
            Animator.SetTrigger(HitKey);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpSpeed);

            if (_hitEffect == null)
                _hitEffect = StartCoroutine(HitEffect());
        }

        private IEnumerator HitEffect()
        {
            var originalColor = Sprite.color;
            Sprite.color = Color.red;

            yield return new WaitForSeconds(_damageAnimationDuration);
            
            Sprite.color = originalColor;
            _hitEffect = null;
        }

        public void ApplyPush(float directionX, float pushStrength)
        {
            if (_pushEffect == null)
                StartCoroutine(PushEffect(directionX, pushStrength));
        }

        private IEnumerator PushEffect(float directionX, float pushStrength)
        {
            DamageXModifier = directionX * pushStrength;
            
            yield return new WaitForSeconds(_damageAnimationDuration);
            
            DamageXModifier = 0;
            _pushEffect = null;
        }

        public virtual void OnDie()
        {
            Animator.SetTrigger(IsDeadKey);
        }

        public virtual void MeleeAttack()
        {
            Animator.SetTrigger(AttackKey);
        }

        public void AttackSound()
        {
            Sounds.Play("Melee");
        }
        
        public virtual void RangeAttack()
        {
            Animator.SetTrigger(RangeAttackKey);
        } 
        
        public void RangeAttackSound()
        {
            Sounds.Play("Range");
        }

        public virtual void OnAttackAnimationTriggered()
        {
            _attackRange.Check();
        }
        
        protected virtual void OnRangeAttackAnimationTriggered()
        {
            if (_hero == null)
                _hero = FindObjectOfType<Hero.Hero>();

            var rangeAttackTarget = _hero.transform.position;
            
            var directionToHero = GetDirectionToTarget(_hero.gameObject);
            UpdateSpriteDirection(directionToHero);
            
            var direction = rangeAttackTarget - _rangeProjectileSpawnerCreature.gameObject.transform.position;
            direction.z = 0;
            _rangeProjectileSpawnerCreature.Spawn(direction.normalized);
        }
        
        protected Vector2 GetDirectionToTarget(GameObject target)
        {
            var direction = target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        public virtual void OnDieAnimationEnded()
        {
            Destroy(gameObject);
        }
    }
}