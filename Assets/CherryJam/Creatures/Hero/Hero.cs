using System;
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
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private CheckCircleOverlap _platformCheck;
        [SerializeField] protected InventoryController _inventory;
        [SerializeField] private int _fireflyHeal;

        [Header("Attack Params")]
        [SerializeField] protected CheckCircleOverlap _superAttackRange;
        [SerializeField] private DirectionalSpawnComponent _rangeProjectileSpawner;
        [SerializeField] private Cooldown _attackCooldown;
        [SerializeField] private Cooldown _superAttackCooldown;
        [SerializeField] private Cooldown _rangeAttackCooldown;

        private PlayerInput _playerInput;
        private bool _allowDoubleJump;
        private HeroHealthComponent _healthComponent;
        private LightSourceComponent _lightComponent;
        private CameraShakeEffect _cameraShake;
        private Vector3 _rangeAttackTarget;

        private readonly Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed;
        private bool _isBoostedAttack;
        
        private event Action _onDirectionChanged;
        
        private static readonly int IsHeroKey = Animator.StringToHash("is-hero");
        private static readonly int IsBoostedKey = Animator.StringToHash("is-boosted");

        private int ProjectilesCount => GameSession.Instance.Data.Inventory.Count(ItemId.Projectile.ToString());

        public bool IsLookingUp { get; set; }
        public bool IsLookingDown { get; set; }
        
        protected override void Awake()
        {
            base.Awake();

            _playerInput = GetComponent<PlayerInput>();
            _healthComponent = GetComponent<HeroHealthComponent>();
            _inventory = GetComponent<InventoryController>();
            
            _allowDoubleJump = false;
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
            UpdatePosition();
            Animator.SetBool(IsHeroKey, true);
            
            var health = GetComponent<HeroHealthComponent>();
            health.SetHealth(GameSession.Instance.Data.Hp.Value);
        }
        
        public void OnHealthChanged(int currentHealth)
        {
            GameSession.Instance.Data.Hp.Value = currentHealth;
        }
        
        public bool AddToInventory(string id, int value)
        {
            return _inventory.Add(id, value);
        }

        public void LockInput()
        {
            _playerInput.enabled = false;
        }

        public void UnlockInput()
        {
            _playerInput.enabled = true;
        }

        protected override float CalculateYVelocity()
        {
            if (IsGrounded)
            {
                _allowDoubleJump = false;
                IsJumping = false;
            }

            var isJumpingDown = Direction.y > 0 && IsLookingDown; // Jump pressed
            if (isJumpingDown)
            {
                TryJumpDown();
                return 0;
            }

            return base.CalculateYVelocity();
        }

        private void TryJumpDown()
        {
            _platformCheck.Check();
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

        public override void MeleeAttack()
        {
            var cooldown = _isBoostedAttack ? _superAttackCooldown : _attackCooldown;
            if (!cooldown.IsReady) return;

            base.MeleeAttack();
            cooldown.Reset();
        }
        
        public override void OnDie()
        {
            LockInput();
            base.OnDie();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }
        
        protected override float CalculateSpeed()
        {
            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;
            
            var defaultSpeed = _speed;
            return defaultSpeed + _additionalSpeed;
        }

        public IDisposable SubscribeDirectionChanged(Action call)
        {
            _onDirectionChanged += call;
            return new ActionDisposable(() => _onDirectionChanged -= call);
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
            GameSession.Instance.Data.Inventory.Remove(ItemId.Projectile.ToString(), 1);
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
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            }
        }
        
        public override void UpdateSpriteDirection(Vector2 direction)
        {
            var originalValue = transform.localScale.x;
            base.UpdateSpriteDirection(direction);
            var newValue = transform.localScale.x;
            
            if (Math.Abs(originalValue - newValue) < 0.01) return;

            _onDirectionChanged?.Invoke();
        }
        
        public void Heal()
        {
            var firefliesCount = GameSession.Instance.Data.Inventory.Count(ItemId.FireflyToUse.ToString());
            if (firefliesCount <= 0) return;
            
            Animator.SetTrigger(HealKey);
            _healthComponent.ApplyHealing(_fireflyHeal);
            GameSession.Instance.Data.Inventory.Remove(ItemId.FireflyToUse.ToString(), 1);
        }

        public void SetBoostedAttack(bool isBoosted)
        {
            _isBoostedAttack = isBoosted;
            Animator.SetBool(IsBoostedKey, isBoosted);
        }
    }
}