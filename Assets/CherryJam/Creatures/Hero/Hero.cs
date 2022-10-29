using System;
using System.Collections;
using CherryJam.Components;
using CherryJam.Components.ColliderBased;
using CherryJam.Components.GoBased;
using CherryJam.Components.Health;
using CherryJam.Components.LevelManagement.SpawnPoints;
using CherryJam.Components.Light;
using CherryJam.Effects.CameraRelated;
using CherryJam.Model;
using CherryJam.Model.Definition.Repositories.Items;
using CherryJam.Utils;
using CherryJam.Utils.Disposables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CherryJam.Creatures.Hero
{
    public class Hero : Creature
    {
        [Header("Hero Params")]
        [SerializeField] protected float _slamDownVelocity;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private CheckCircleOverlap _platformCheck;
        [SerializeField] protected CheckCircleOverlap _superAttackRange;
        [SerializeField] protected InventoryController _inventory;


        [Header("Throw")]       
        [SerializeField] private float _multiThrowPressDuration = 1;
        [SerializeField] private int _multiThrowMaxCount = 3;
        [SerializeField] private float _delayBetweenThrows = 0.3f;
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private DirectionalSpawnComponent _rangeProjectileSpawner;

        [Header("Attack Params")]
        [SerializeField] private Cooldown _attackCooldown;
        [SerializeField] private Cooldown _superAttackCooldown;
        [SerializeField] private Cooldown _rangeAttackCooldown;
        
        [Space]
        [Header("Perks")]
        [SerializeField] private int _fireflyHeal;
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
        private HeroHealthComponent _healthComponent;
        private float _speedMod;
        private LightSourceComponent _lightComponent;
        private CameraShakeEffect _cameraShake;
        private Vector3 _rangeAttackTarget;

        private readonly Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed;
        private bool _isBoostedAttack;

        private readonly Cooldown _superThrowCooldown = new Cooldown();
        private readonly Cooldown _magicShieldCooldown = new Cooldown();
        private readonly Cooldown _freezeEnemiesCooldown = new Cooldown();

        private readonly Cooldown _activeMagicShieldCooldown = new Cooldown();

        private event Action<string> _onPerkUsed;
        private event Action _onDirectionChanged;
        
        private static readonly int IsLeftDirectionKey = Animator.StringToHash("is-left-direction"); // ToDo remove
        private static readonly int IsHeroKey = Animator.StringToHash("is-hero");
        private static readonly int IsBoostedKey = Animator.StringToHash("is-boosted");

        private const string SwordId = "Sword";
        private const string CoinId = "Coin";
        private const string ProjectileItemId = "Projectile";
        
        private int SwordsCount => _session.Data.Inventory.Count(SwordId);
        private int CoinsCount => _session.Data.Inventory.Count(CoinId);
        private int ProjectilesCount => _session.Data.Inventory.Count(ProjectileItemId);
        
        private bool CanThrow => SwordsCount > 1;
        // private bool CanThrow
        // {
        //     get
        //     {
        //         if (SelectedItemId == SwordId)
        //         {
        //             return SwordsCount > 1;
        //         }
        //         
        //         var def = DefsFacade.I.Items.Get(SelectedItemId);
        //         return def.HasTag(ItemTag.Throwable);
        //     }
        // }
        
        public bool IsLookingUp { get; set; }
        public bool IsLookingDown { get; set; }
        
        protected override void Awake()
        {
            base.Awake();

            _playerInput = GetComponent<PlayerInput>();
            _healthComponent = GetComponent<HeroHealthComponent>();
            _inventory = GetComponent<InventoryController>();
            _allowDoubleJump = true;
            _isMultiThrow = false;
            _speedMod = 1;
        }

        private void UpdatePosition()
        {
            var spawnType = GameSession.Instance.NextSpawnPointType;
            if (spawnType == SpawnPointType.Default) return;

            var config = FindObjectOfType<SpawnPointsConfigComponent>();
            var spawnPoint = config.GetPointByType(spawnType);
            transform.position = spawnPoint.Position;
        }

        // ToDo Move to a new controller
        public void ShowMainMenuInGame()
        {
            WindowUtils.CreateWindow("UI/InGameMenuWindow");
        }
        
        protected void Start()
        {
            _session = GameSession.Instance;
            
            UpdatePosition();
            
            Animator.SetBool(IsHeroKey, true);
            
            // _cameraShake = FindObjectOfType<CameraShakeEffect>();

            var health = GetComponent<HeroHealthComponent>();
            _session.Data.Inventory.OnChange += OnInventoryChanged;
            health.SetHealth(_session.Data.Hp.Value);
        }
        
        private void OnDestroy()
        {
            _session.Data.Inventory.OnChange -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)        
        {
            // if (id == SwordId)
            // {
            //     UpdateHeroWeapon();
            // }
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }
        
        private void SpeedUp(float value, float time)
        {
            _speedUpCooldown.Value = _speedUpCooldown.RemainingTime + time;
            _additionalSpeed = Mathf.Max(_additionalSpeed, value);
            _speedUpCooldown.Reset();
        }

        public bool AddToInventory(string id, int value)
        {
            return _inventory.Add(id, value);
        }
        
        private void SpawnThrownParticle(GameObject projectile)
        {
            // _rangeProjectileSpawner.SetPrefab(projectile);
            // _rangeProjectileSpawner.Spawn();
            // Sounds.Play("Range");
        }

        public void LockInput()
        {
            _playerInput.enabled = false;
        }

        public void UnlockInput()
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
            if (!IsGrounded && _allowDoubleJump) // ToDo - remove double jump
            {
                DoJumpVfx();
                _allowDoubleJump = false;
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public override void MeleeAttack()
        {
            var cooldown = _isBoostedAttack ? _superAttackCooldown : _attackCooldown;
            if (!cooldown.IsReady) return;

            base.MeleeAttack();
            cooldown.Reset();
        }
        
        // private void OnCollisionEnter2D(Collision2D other)
        // {
        //     if (other.gameObject.IsInLayer(_groundLayer))
        //     {
        //         var contact = other.contacts[0];
        //         if (contact.relativeVelocity.y >= _slamDownVelocity)
        //         {
        //             _particles.Spawn("SlamDown");
        //         }
        //     }
        // }

        public override void TakeDamage()
        {
            base.TakeDamage();

            // ToDo make invincible for some time 
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
        
        public void Heal(int healingValue)
        {
            _healthComponent.ApplyHealing(healingValue);
        }
        
        // My implementation
        public void BoostSpeed(float speedMod)
        {
            _speedMod = speedMod;
            // _particles.Spawn("Heal");
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

            // var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            var defaultSpeed = _speed;
            return defaultSpeed + _additionalSpeed;
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

        public IDisposable SubscribePerkUse(Action<string> call)
        {
            _onPerkUsed += call;
            return new ActionDisposable(() => _onPerkUsed -= call);
        }
        
        public IDisposable SubscribeDirectionChanged(Action call)
        {
            _onDirectionChanged += call;
            return new ActionDisposable(() => _onDirectionChanged -= call);
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

        public void RangeAttack(Vector3 target)
        {
            if (!_rangeAttackCooldown.IsReady) return;
            
            _rangeAttackTarget = target;
            base.RangeAttack();
            _rangeAttackCooldown.Reset();
        }
        
        protected override void OnRangeAttackAnimationTriggered()
        {
            if (ProjectilesCount <= 0) return;
            
            UpdateSpriteDirectionToCursor();
            
            var direction = _rangeAttackTarget - _rangeProjectileSpawner.gameObject.transform.position;
            direction.z = 0;
            _rangeProjectileSpawner.Spawn(direction.normalized);
            GameSession.Instance.Data.Inventory.Remove(ProjectileItemId, 1);
        }
        
        public virtual void OnSuperAttackAnimationTriggered()
        {
            _superAttackRange.Check();
        }

        private void UpdateSpriteDirectionToCursor()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            var target = Camera.main.ScreenToWorldPoint(mousePosition);
            var direction = target - transform.position;
            
            var localScale = transform.localScale;
            
            if (direction.x > 0)
            {
                transform.localScale = new Vector3( Mathf.Abs(localScale.x), localScale.y, localScale.z);
                Animator.SetBool(IsLeftDirectionKey, false);
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * Mathf.Abs(localScale.x), localScale.y, localScale.z);
                Animator.SetBool(IsLeftDirectionKey, true);
            }
        }

        // Hero is always turned to the cursor
        // public override void UpdateSpriteDirection(Vector2 _)
        // {
        //     var mousePosition = Mouse.current.position.ReadValue();
        //     var target = Camera.main.ScreenToWorldPoint(mousePosition);
        //     var direction = target - transform.position;
        //     
        //     // var multiplier = _invertScale ? -1 : 1;
        //     var localScale = transform.localScale;
        //     
        //     if (direction.x > 0)
        //     {
        //         transform.localScale = new Vector3( Mathf.Abs(localScale.x), localScale.y, localScale.z);
        //         Animator.SetBool(IsLeftDirectionKey, false);
        //     }
        //     else if (direction.x < 0)
        //     {
        //         transform.localScale = new Vector3(-1 * Mathf.Abs(localScale.x), localScale.y, localScale.z);
        //         Animator.SetBool(IsLeftDirectionKey, true);
        //     }
        // }

        public override void UpdateSpriteDirection(Vector2 direction)
        {
            var originalValue = transform.localScale.x;
            base.UpdateSpriteDirection(direction);
            var newValue = transform.localScale.x;
            
            if (Math.Abs(originalValue - newValue) < 0.01) return;

            _onDirectionChanged?.Invoke();
        }
        
        public void HealWithFirefly()
        {
            var firefliesCount = _session.Data.Inventory.Count(ItemId.FireflyToUse.ToString());
            if (firefliesCount <= 0) return;
            
            Animator.SetTrigger(HealKey);
            _healthComponent.ApplyHealing(_fireflyHeal);
            _session.Data.Inventory.Remove(ItemId.FireflyToUse.ToString(), 1);
        }

        public void SetBoostedAttack(bool isBoosted)
        {
            _isBoostedAttack = isBoosted;
            Animator.SetBool(IsBoostedKey, isBoosted);
        }
    }
}