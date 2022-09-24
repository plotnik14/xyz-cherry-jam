using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss.Goblin.Stage
{
    public class GoblinEatStage : StateMachineBehaviour
    {
        private GoblinBoss _goblin;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_goblin == null)
                _goblin = animator.GetComponent<GoblinBoss>();
            
            _goblin.Eat();
        }
    }
}