using CherryJam.Components.Audio;
using CherryJam.Components.ColliderBased;
using CherryJam.Components.GoBased;
using CherryJam.Creatures.Mobs;
using CherryJam.Utils;
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
        [SerializeField] protected int _damage;

        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] protected ColliderCheck _groundCheck;
        [SerializeField] protected CheckCircleOverlap _attackRange;
        [SerializeField] protected CheckCircleOverlap _magicRange;
        [SerializeField] protected SpawnListComponent _particles;
        
        [SerializeField] public DirectionalSpawnComponent _rangeProjectileSpawnerCreature;

        protected Rigidbody2D Rigidbody;
        protected Vector2 Direction;
        protected Animator Animator;
        protected PlaySoundsComponent Sounds;
        protected bool IsGrounded;
        protected bool IsJumping;
        protected bool IsFrozen;

        private Hero.Hero _hero;

        protected readonly Cooldown FreezeCooldown = new Cooldown();

        private Color _colorBeforeFreezing;
        
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

            IsGrounded = _groundCheck.IsTouchingLayer;
        }

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
            CheckActiveBuffs();
        }

        protected virtual void CheckActiveBuffs()
        {
            if (IsFrozen && FreezeCooldown.IsReady)
                Unfreeze();
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
            return Direction.x * CalculateSpeed();
        }

        protected virtual float CalculateSpeed()
        {
            return _speed;
        }

        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            bool isJumpPressed = Direction.y > 0;

            if (IsGrounded)
            {
                IsJumping = false;
            }

            if (isJumpPressed)
            {
                IsJumping = true;
                var isFalling = Rigidbody.velocity.y <= 0.01;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (IsJumping && Rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity += _jumpSpeed;
                DoJumpVfx();
            }

            return yVelocity;
        }

        protected void DoJumpVfx()
        {
            // _particles.Spawn("Jump");
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
        
        public void RangeAttack()
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

        public void Freeze()
        {   
            Animator.enabled = false;
            
            var sprite = GetComponent<SpriteRenderer>();
            _colorBeforeFreezing = sprite.color;
            var frozenColor = new Color(0f, 255f, 255f);
            sprite.color = frozenColor;

            var mobAI = GetComponent<MobAI>();
            if (mobAI != null)
                mobAI.DisableAI();

            FreezeCooldown.Value = 10; // ToDo Move for defs
            FreezeCooldown.Reset();
            IsFrozen = true;
        }
        
        private void Unfreeze()
        {
            IsFrozen = false;
            
            var mobAI = GetComponent<MobAI>();
            if (mobAI != null)
                mobAI.EnableAI();
            
            var sprite = GetComponent<SpriteRenderer>();
            sprite.color = _colorBeforeFreezing;

            Animator.enabled = true;
        }
    }
}