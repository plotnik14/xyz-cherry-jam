﻿using PixelCrew.Components;
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
        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        private PlayerInput _playerInput;
        private HeroInventory _heroInventory;
        private bool _allowDoubleJump;
        private GameSession _session;
        private bool _isMultiThrow;


        protected override void Awake()
        {
            base.Awake();

            _playerInput = GetComponent<PlayerInput>();
            _heroInventory = GetComponent<HeroInventory>();
            _allowDoubleJump = true;
            _isMultiThrow = false;
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

        public void Throw(double pressDuration)
        {
            if (!_session.Data.IsArmed) return;
            if (!_throwCooldown.IsReady) return;

            var swordsCount = _heroInventory.GetSwordsCount();
            if (swordsCount <= 1)
            {
                Debug.Log("You cannot throw last sword");
                return;
            }

            _isMultiThrow = pressDuration >= _multiThrowPressDuration;

            Animator.SetTrigger(ThrowKey);          
            _throwCooldown.Reset();
        }

        public void OnThrowAnimationTriggered()
        {
            if (_isMultiThrow)
            {
                var swordsToThrow = _heroInventory.GetSwordsCount() - 1;
                swordsToThrow = Mathf.Min(swordsToThrow, _multiThrowMaxCount);

                StartCoroutine(MultiThrow(swordsToThrow));
                
                _heroInventory.DecreaseSwords(swordsToThrow);
                _isMultiThrow = false;
            }
            else
            {
                _particles.Spawn("Throw");
                _heroInventory.DecreaseSwords(1);
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

        public void ArmHero()
        {
            _session.Data.IsArmed = true;
            _heroInventory.AddSwords(1);
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