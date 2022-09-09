using PixelCrew.Components.GoBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.States
{
    public class BossShootState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.LaunchProjectile();
        }
    }
}
