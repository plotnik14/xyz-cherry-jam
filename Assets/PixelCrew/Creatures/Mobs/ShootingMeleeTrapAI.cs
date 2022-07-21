using PixelCrew.Utils;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Creatures
{
    public class ShootingMeleeTrapAI : ShootingTrapAI
    {
        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private LayerCheck _meleeCanAttack;

        protected static readonly int Melee = Animator.StringToHash("melee");
             
        public override bool PerformExtraActionAndStop()
        {
            if (_meleeCanAttack.IsTouchingLayer)
            {
                if (_meleeCooldown.IsReady)
                {
                    MeleeAttack();
                }
                return true;
            }

            return false;
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(Melee);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }
    }
}