using UnityEngine;

namespace CherryJam.Creatures
{
    public class RollingCreature : Creature
    {
        [SerializeField] private float _rollingSpeed;

        protected static readonly int RollingAttackKey = Animator.StringToHash("rolling-attack");

        private bool _isRolling;

        public bool IsRolling => _isRolling;

        public void StartRolling()
        {
            _isRolling = true;
            Animator.SetBool(RollingAttackKey, _isRolling);
        }

        public void StopRolling()
        {
            _isRolling = false;
            Animator.SetBool(RollingAttackKey, _isRolling);
        }

        protected override float CalculateXVelocity()
        {
            var speedMod = _isRolling ? _rollingSpeed : _speed;
            return Direction.x * speedMod;
        }
    }
}
