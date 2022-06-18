using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using System;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private float _fallingVelocity;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerCheck _groundCheck;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [SerializeField] private int _damage;
        [SerializeField] private CheckCircleOverlap _attackRange;

        [Space][Header("Particles")]
        [SerializeField] private SpawnComponent _footPrintParticles;
        [SerializeField] private SpawnComponent _jumpParticles;
        [SerializeField] private SpawnComponent _fallParticles;
        [SerializeField] private SpawnComponent _attackParticles;
        [SerializeField] private ParticleSystem _hitParticles;
        

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Collider2D[] _interactionResult = new Collider2D[1];
        private HeroInventory _heroInventory;

        private bool _isGrounded;
        private bool _allowDoubleJump;
        private float _maxJumpPositionY;
        private bool _isJumping;

        private GameSession _session;

        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int IsOnGroundKey = Animator.StringToHash("is-on-ground");
        private static readonly int HitKey = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");


        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _heroInventory = GetComponent<HeroInventory>();
            _maxJumpPositionY = transform.position.y;
            _allowDoubleJump = true;
        }

        public void Start()
        {
            _session = FindObjectOfType<GameSession>();
            
            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.Hp);

            UpdateHeroWeapon();
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }

        public void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);
            _animator.SetBool(IsRunningKey, _direction.x != 0);
            _animator.SetBool(IsOnGroundKey, _isGrounded);

            UpdateSpriteDirection();
        }

        public void Attack()
        {
            if (!_session.Data.IsArmed) return;

            _animator.SetTrigger(AttackKey);
        }

        public void OnAttackAnimationTriggered()
        {
            var gos = _attackRange.GetObjectsInRange();
            foreach (var go in gos)
            {
                var hp = go.GetComponent<HealthComponent>();
                if (hp != null && go.CompareTag("Enemy"))
                {
                    hp.ApplyDamage(_damage);
                }
            }
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _unarmed;
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            bool isJumpPressed = _direction.y > 0;

            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }

            if (isJumpPressed)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_isJumping && _rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.01;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
                SpawnJumpParticles();
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                SpawnJumpParticles();
                _allowDoubleJump = false;
            }

            return yVelocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _fallingVelocity)
                {
                    SpawnLandingParticles();
                }
            }
        }

        public void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void UpdateSpriteDirection()
        { 
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            } else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
        }

        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(HitKey);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

            if (_heroInventory.GetCoinsCount() > 0)
            {
                SpawnCoinParticles();
            }
        }

        public void Lift(float liftSpeed)
        {
            _isJumping = false;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, liftSpeed);
        }
        
        private void SpawnCoinParticles()
        {
            var coins = _heroInventory.GetCoinsCount();
            var numCoinsToDispose = Mathf.Min(coins, 5);
            _heroInventory.LoseCoins(numCoinsToDispose);

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void OnDisable()
        {
            _hitParticles.gameObject.SetActive(false);
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position, 
                _interactionRadius, 
                _interactionResult, 
                _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        public void SpawnFootPrints()
        {
            _footPrintParticles.Spawn();
        }

        public void SpawnJumpParticles()
        {
            _jumpParticles.Spawn();
        }

        public void SpawnLandingParticles()
        {
            _fallParticles.Spawn();
        }

        public void SpawnAttackParticles()
        {
            _attackParticles.Spawn();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position + new Vector3(-0.5f, 0.5f, 0), Vector3.forward, 0.1f);
        }
#endif

    }
}