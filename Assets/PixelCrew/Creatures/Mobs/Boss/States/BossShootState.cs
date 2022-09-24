using System;
using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.States
{
    public class BossShootState : StateMachineBehaviour
    {
        [SerializeField] private bool _isPerlShootingStage;
        [SerializeField] private bool _isCannonBallsShootingStage;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = GetSpawner(animator);
            spawner.LaunchProjectiles();
        }

        private IProjectileSpawner GetSpawner(Animator animator)
        {
            if (_isCannonBallsShootingStage)
                return animator.GetComponentInChildren<GroupProjectileSpawner>();
            
            if (_isPerlShootingStage)
                 return animator.GetComponent<CircularProjectileSpawner>();

            throw new InvalidOperationException("BossShootState has invalid stages configuration");
        }
    }
}
