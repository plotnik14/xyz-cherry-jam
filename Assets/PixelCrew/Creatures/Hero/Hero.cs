using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures
{
    public class Hero : Creature
    {
        [Header("Hero Params")]
        [SerializeField] protected float _slamDownVelocity;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [Header("Throw")]       
        [SerializeField] private float _multiThrowPressDuration = 1;
        [SerializeField] private int _multiThrowMaxCount = 3;
        [SerializeField] private float _delayBetweenThrows = 0.3f;
        [SerializeField] private Cooldown _throwCooldown;

        [Space]
        [Header("AnimatorController")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [Space]
        [SerializeField] private ProbabilityDropComponent _hitDrop;

        private PlayerInput _playerInput;
        private bool _allowDoubleJump;
        private GameSession _session;
        private bool _isMultiThrow;
        private HealthComponent _healthComponent;

        private int SwordsCount => _session.Data.Inventory.Count("Sword");
        private int CoinsCount => _session.Data.Inventory.Count("Coin");
        private int HealthPotionsCount => _session.Data.Inventory.Count("Health Potion");

        protected override void Awake()
        {
            base.Awake();

            _playerInput = GetComponent<PlayerInput>();
            _healthComponent = GetComponent<HealthComponent>();
            _allowDoubleJump = true;
            _isMultiThrow = false;
        }

        protected void Start()
        {
            _session = FindObjectOfType<GameSession>();

            var health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChange += OnInventoryChanged;

            health.SetHealth(_session.Data.Hp);

            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChange -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)        
        {
            if (id == "Sword")
            {
                UpdateHeroWeapon();
            }
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }
      
        public override void Attack()
        {
            if (SwordsCount <= 0) return;

            base.Attack();
        }

        public void Throw(double pressDuration)
        {
            if (SwordsCount <= 1) return;
            if (!_throwCooldown.IsReady) return;

            _isMultiThrow = pressDuration >= _multiThrowPressDuration;
            Animator.SetTrigger(ThrowKey);          
            _throwCooldown.Reset();
        }

        public void Heal()
        {
            if (HealthPotionsCount <= 0) return;

            UseHealthPotion();
        }

        private void UseHealthPotion()
        {
            var healingValue = 5; // ToDo Move to Defs later ??
            _healthComponent.ApplyHealing(healingValue);
            _particles.Spawn("Heal");
            _session.Data.Inventory.Remove("Health Potion", 1);
        }

        public bool AddToInventory(string id, int value)
        {
            return _session.Data.Inventory.Add(id, value);
        }

        public void OnThrowAnimationTriggered()
        {
            if (_isMultiThrow)
            {
                var swordsToThrow = SwordsCount - 1;
                swordsToThrow = Mathf.Min(swordsToThrow, _multiThrowMaxCount);

                StartCoroutine(MultiThrow(swordsToThrow));
                
                _session.Data.Inventory.Remove("Sword", swordsToThrow);
                _isMultiThrow = false;
            }
            else
            {
                _particles.Spawn("Throw");
                _session.Data.Inventory.Remove("Sword", 1);
            }
        }

        private IEnumerator MultiThrow(int swordsToThrow)
        {
            LockInput();

            for (int i = 0; i < swordsToThrow; i++)
            {
                _particles.Spawn("Throw");
                yield return new WaitForSeconds(_delayBetweenThrows);
            }

            UnlockInput();

            yield return null;
        }

        private void LockInput()
        {
            _playerInput.enabled = false;
        }

        private void UnlockInput()
        {
            _playerInput.enabled = true;
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordsCount > 0 ? _armed : _unarmed;
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

            if (CoinsCount > 0)
            {
                SpawnCoinParticles();
            }
        }

        private void SpawnCoinParticles()
        {
            var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

            _hitDrop.SetCount(numCoinsToDispose);
            _hitDrop.CalculateDrop();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }
    }
}