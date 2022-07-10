using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        [Header("Hero Params")]
        [SerializeField] protected float _slamDownVelocity;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;
        
        [Space]
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        private HeroInventory _heroInventory;
        private bool _allowDoubleJump;
        private GameSession _session;


        protected override void Awake()
        {
            base.Awake();

            _heroInventory = GetComponent<HeroInventory>();
            _allowDoubleJump = true;
        }

        protected void Start()
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

        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;

            base.Attack();
        }

        public override void Throw()
        {
            if (!_session.Data.IsArmed) return;

            base.Throw();
        }

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            UpdateHeroWeapon();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _unarmed;
        }

        protected override float CalculateYVelocity()
        {
            if (IsGrounded)
            {
                _allowDoubleJump = true;
                IsJumping = false;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (_heroInventory.GetCoinsCount() > 0)
            {
                SpawnCoinParticles();
            }
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
            _interactionCheck.Check();
        }
    }
}