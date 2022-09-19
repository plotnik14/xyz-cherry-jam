using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using PixelCrew.Components.Light;
using PixelCrew.Creatures.UsableItems;
using PixelCrew.Effects.CameraRelated;
using PixelCrew.Model;
using PixelCrew.Model.Definition;
using PixelCrew.Model.Definition.Player;
using PixelCrew.Model.Definition.Repositories;
using PixelCrew.Model.Definition.Repositories.Items;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{
    public class Hero : Creature
    {
        [Header("Hero Params")]
        [SerializeField] protected float _slamDownVelocity;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private CheckCircleOverlap _platformCheck;

        [Header("Throw")]       
        [SerializeField] private float _multiThrowPressDuration = 1;
        [SerializeField] private int _multiThrowMaxCount = 3;
        [SerializeField] private float _delayBetweenThrows = 0.3f;
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private SpawnComponent _throwSpawner;

        [Space]
        [Header("Perks")]
        [SerializeField] private GameObject _magicShield;

        [Space]
        [Header("AnimatorController")]
        [SerializeField] private RuntimeAnimatorController _armed;
        [SerializeField] private RuntimeAnimatorController _unarmed;

        [Space]
        [SerializeField] private ProbabilityDropComponent _hitDrop;
        
        [Space]
        [SerializeField] private GameObject _lightSource;

        private PlayerInput _playerInput;
        private bool _allowDoubleJump;
        private GameSession _session;
        private bool _isMultiThrow;
        private HealthComponent _healthComponent;
        private float _speedMod;
        private LightSourceComponent _lightComponent;
        private CameraShakeEffect _cameraShake;
        
        private readonly Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed;
        
        private readonly Cooldown _superThrowCooldown = new Cooldown();
        private readonly Cooldown _magicShieldCooldown = new Cooldown();
        private readonly Cooldown _freezeEnemiesCooldown = new Cooldown();
        
        private readonly Cooldown _activeMagicShieldCooldown = new Cooldown();

        private event Action<string> _onPerkUsed;

        // ToDo move to proper place
        private Dictionary<UseActionDef, AbstractUseAction> _useActions;

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

            // ToDo move to proper place. Some config in scriptable object&
            _useActions = new Dictionary<UseActionDef, AbstractUseAction>
            {
                { UseActionDef.Heal, new HealingAction(this) },
                { UseActionDef.BoostSpeed, new BoostSpeedAction(this) }
            };
        }


        // ToDo Move to a new controller
        public void ShowMainMenuInGame()
        {
            // ToDo check that menu is not created
            WindowUtils.CreateWindow("UI/InGameMenuWindow");
        }
        
        protected void Start()
        {
            _session = GameSession.Instance;
            _cameraShake = FindObjectOfType<CameraShakeEffect>();

            var health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChange += OnInventoryChanged;

            _session.StatsModel.OnUpgraded += OnHeroUpgraded;
            health.SetHealth(_session.Data.Hp.Value);

            UpdateHeroWeapon();
            UpdateCooldown();
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = (int)_session.StatsModel.GetValue(statId);
                    _session.Data.Hp.Value = health;
                    _healthComponent.SetHealth(health);
                    break;
            }
        }

        private void UpdateCooldown()
        {
            var superThrowPerkDef = DefsFacade.I.Perks.Get("super-throw");
            _superThrowCooldown.Value = superThrowPerkDef.Cooldown;
            
            var magicShieldThrowPerkDef = DefsFacade.I.Perks.Get("magic-shield");
            _magicShieldCooldown.Value = magicShieldThrowPerkDef.Cooldown;
            
            var freezeEnemiesThrowPerkDef = DefsFacade.I.Perks.Get("freezing-enemies");
            _freezeEnemiesCooldown.Value = freezeEnemiesThrowPerkDef.Cooldown;
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

        public void UseInventory(double pressDuration)
        {
            if (IsSelectedItem(ItemTag.Throwable))
                PerformThrowing(pressDuration);
            else if (IsSelectedItem(ItemTag.Potion))
                UsePotion();
        }

        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch (potion.Effect)
            {
                case Effect.AddHp:
                    Heal((int)potion.Value);
                    break;
                case Effect.SpeedUp:
                    SpeedUp(potion.Value, potion.Time);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _session.Data.Inventory.Remove(potion.Id, 1);
        }
        
        private void SpeedUp(float value, float time)
        {
            _speedUpCooldown.Value = _speedUpCooldown.RemainingTime + time;
            _additionalSpeed = Mathf.Max(_additionalSpeed, value);
            _speedUpCooldown.Reset();
        }

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }

        private void PerformThrowing(double pressDuration)
        {
            if (!CanThrow) return;
            
            _isMultiThrow = pressDuration >= _multiThrowPressDuration && _session.PerksModel.IsSuperThrowSupported;

            if (_isMultiThrow)
                if (!_superThrowCooldown.IsReady) return;
            else
                if (!_throwCooldown.IsReady) return;
            
            Animator.SetTrigger(ThrowKey);

            if (_isMultiThrow)
            {
                _superThrowCooldown.Reset();
                _onPerkUsed?.Invoke("super-throw");
            }
            else
                _throwCooldown.Reset();
        }
        
        public bool AddToInventory(string id, int value)
        {
            return _session.Data.Inventory.Add(id, value);
        }

        public void OnThrowAnimationTriggered()
        {
            if (_isMultiThrow && _session.PerksModel.IsSuperThrowSupported)
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

        public void LockInput()
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

            var isJumpingDown = Direction.y < 0;
            if (isJumpingDown)
                TryJumpDown();

            return base.CalculateYVelocity();
        }

        private void TryJumpDown()
        {
            _platformCheck.Check();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported)
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

            _cameraShake.Shake();
            
            if (CoinsCount > 0)
            {
                SpawnCoinParticles();
            }
        }

        public override void OnDie()
        {
            LockInput();
            base.OnDie();
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
        
        // My implementation of usable items
        public void UseItem()
        {
            var usableItemId = _session.QuickInventory.SelectedItem.Id;
            var usableItemDef = DefsFacade.I.UsableItems.Get(usableItemId);
            if (usableItemDef.IsVoid)
            {
                Debug.Log($"{usableItemId} is not usable item");
                return;
            }

            var actionDef = usableItemDef.Action;
            var action = _useActions[actionDef];
            action.Use(usableItemDef.Value);
            
            _session.Data.Inventory.Remove(usableItemId, 1);
        }

        public void Heal(int healingValue)
        {
            _healthComponent.ApplyHealing(healingValue);
            _particles.Spawn("Heal");
        }
        
        // My implementation
        public void BoostSpeed(float speedMod)
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

        protected override float CalculateSpeed()
        {
            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;

            var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            return defaultSpeed + _additionalSpeed;
        }

        public void UseMagic()
        {
            if (_session.PerksModel.IsMagicShieldSupported)
                ActivateMagicShield();
            else if (_session.PerksModel.IsFreezeEnemiesSupported)
                FreezeEnemies();
        }
        
        public void ActivateMagicShield()
        {
            if (!_magicShieldCooldown.IsReady) return;
            
            _healthComponent.MakeInvincible();
            _magicShield.SetActive(true);
            
            _activeMagicShieldCooldown.Value = 10f; // ToDo move to perk config (when it is created)
            _activeMagicShieldCooldown.Reset();
            _magicShieldCooldown.Reset();
            _onPerkUsed?.Invoke("magic-shield");
        }

        private void FreezeEnemies()
        {
            if (!_freezeEnemiesCooldown.IsReady) return;
        
            _magicRange.Check();

            _freezeEnemiesCooldown.Reset();
            _onPerkUsed?.Invoke("freezing-enemies");
        }

        protected override void CheckActiveBuffs()
        {
            base.CheckActiveBuffs();
            
            CheckMagicShield();
        }

        private void CheckMagicShield()
        {
            if (_healthComponent.IsInvincible && _activeMagicShieldCooldown.IsReady )
            {
                _healthComponent.MakeVulnerable();
                _magicShield.SetActive(false);
            }
        }
        
        public IDisposable SubscribePerkUse(Action<string> call)
        {
            _onPerkUsed += call;
            return new ActionDisposable(() => _onPerkUsed -= call);
        }

        public void SwitchLight()
        {
            _lightSource.SetActive(!_lightSource.activeSelf);
        }

        public void RefillLightFuel()
        {
            if (_lightComponent == null)
                _lightComponent = _lightSource.GetComponent<LightSourceComponent>();
            
            _lightComponent.Refill();
        }
    }
}