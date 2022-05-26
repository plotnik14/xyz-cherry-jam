using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew
{

    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private SpawnComponent _footPrintParticles;

        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Collider2D[] _interactionResult = new Collider2D[1];

        private bool _isGrounded;
        private bool _allowDoubleJump;
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int IsOnGroundKey = Animator.StringToHash("is-on-ground");
        private static readonly int HitKey = Animator.StringToHash("hit");


        public void SetDirection(Vector2 direction)
        {
            _direction = direction;

        }

        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
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

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            bool isJumpPressed = _direction.y > 0;

            if (_isGrounded) _allowDoubleJump = true;

            if (isJumpPressed)
            {
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFaling = _rigidbody.velocity.y <= 0.01;
            if (!isFaling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpSpeed;
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
            }

            return yVelocity;
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
            _animator.SetTrigger(HitKey);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
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

        private void OnDrawGizmos()
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + new Vector3(-0.5f, 0.5f, 0), 0.1f);
        }
    }
}