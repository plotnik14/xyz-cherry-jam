using System.Collections;
using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using PixelCrew.Model;
using PixelCrew.Model.Definition;
using PixelCrew.Utils;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
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
        [SerializeField] private SpawnComponent _throwSpawner;

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
        private float _speedMod;

        private const string SwordId = "Sword";
        
        private int SwordsCount => _session.Data.Inventory.Count(SwordId);
        private int CoinsCount => _session.Data.Inventory.Count("Coin");

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;
        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                {
                    return SwordsCount > 1;
                }
                
                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }
        
        protected override void Awake()
        {
            base.Awake();

            _playerInput = GetComponent<PlayerInput>();
            _healthComponent = GetComponent<HealthComponent>();
            _allowDoubleJump = true;
            _isMultiThrow = false;
            _speedMod = 1;
        }


        // ToDo Move to a new controller
        public void ShowMainMenuInGame()
        {
            // ToDo check that menu is not created

            var window = Resources.Load<GameObject>("UI/MainMenuWindowInGame");
            var canvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
            Instantiate(window, canvas.transform);
        }
        
        protected void Start()
        {
            _session = FindObjectOfType<GameSession>();

            var health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChange += OnInventoryChanged;

            health.SetHealth(_session.Data.Hp.Value);

            UpdateHeroWeapon();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChange -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)        
        {
            if (id == SwordId)
            {
                UpdateHeroWeapon();
            }
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }
      
        public override void Attack()
        {
            if (SwordsCount <= 0) return;

            base.Attack();
        }

        public void Throw(double pressDuration)
        {
            if (!CanThrow) return;
            if (!_throwCooldown.IsReady) return;

            _isMultiThrow = pressDuration >= _multiThrowPressDuration;
            Animator.SetTrigger(ThrowKey);          
            _throwCooldown.Reset();
        }
        
        public bool AddToInventory(string id, int value)
        {
            return _session.Data.Inventory.Add(id, value);
        }

        public void OnThrowAnimationTriggered()
        {
            if (_isMultiThrow)
            {
                var itemsCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCountToThrow = SelectedItemId == SwordId ? itemsCount - 1 : itemsCount;
                var countToThrow = Mathf.Min(possibleCountToThrow, _multiThrowMaxCount);

                StartCoroutine(MultiThrow(countToThrow));
                
                _session.Data.Inventory.Remove(SelectedItemId, countToThrow);
                _isMultiThrow = false;
            }
            else
            {
                var throwableId = _session.QuickInventory.SelectedItem.Id;
                var throwableDef = DefsFacade.I.ThrowableItems.Get(throwableId);
                SpawnThrownParticle(throwableDef.Projectile);
                _session.Data.Inventory.Remove(throwableId, 1);
            }
        }

        
        private IEnumerator MultiThrow(int countToThrow)
        {
            LockInput();
            
            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.ThrowableItems.Get(throwableId);
            
            for (int i = 0; i < countToThrow; i++)
            {
                SpawnThrownParticle(throwableDef.Projectile);
                yield return new WaitForSeconds(_delayBetweenThrows);
            }

            UnlockInput();

            yield return null;
        }

        private void SpawnThrownParticle(GameObject projectile)
        {
            _throwSpawner.SetPrefab(projectile);
            _throwSpawner.Spawn();
            Sounds.Play("Range");
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
                DoJumpVfx();
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

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }
        
        public void UseItem()
        {
            var usableItemId = _session.QuickInventory.SelectedItem.Id;
            var usableItemDef = DefsFacade.I.UsableItems.Get(usableItemId);
            if (usableItemDef.IsVoid)
            {
                Debug.Log($"{usableItemId} is not usable item");
                return;
            }
            
            // ToDo rework to allow choose action in definition
            switch (usableItemDef.Id)
            {
                case "Health Potion":
                    UseHealthPotion((int)usableItemDef.Value);
                    break;
                case "Health Potion Strong":
                    UseHealthPotion((int)usableItemDef.Value);
                    break;
                case "Speed Potion":
                    UseSpeedPotion(usableItemDef.Value);
                    break;
                default:
                    Debug.Log($"Unsupported usable item: {usableItemDef.Id}");
                    break;
            }
            
            _session.Data.Inventory.Remove(usableItemId, 1);
        }

        private void UseHealthPotion(int healingValue)
        {
            _healthComponent.ApplyHealing(healingValue);
            _particles.Spawn("Heal");
        }
        
        private void UseSpeedPotion(float speedMod)
        {
            _speedMod = speedMod;
            _particles.Spawn("Heal");
            StartCoroutine(SpeedModifier());
        }

        private IEnumerator SpeedModifier()
        {
            yield return new WaitForSeconds(10);
            _speedMod = 1;
            yield return null;
        }
        
        protected override float CalculateXVelocity()
        {
            return base.CalculateXVelocity() * _speedMod;
        }
    }
}