using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.States
{
    public class BossFloodState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var floodController = animator.GetComponent<FloodController>();
            floodController.StartFlooding();
        }
    }
}