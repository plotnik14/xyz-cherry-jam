using UnityEngine;

namespace CherryJam.Creatures.Mobs.Boss.Goblin.Stage
{
    public class BossSpawnStage : StateMachineBehaviour
    {
        private GoblinBoss _boss;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_boss == null)
                _boss = animator.GetComponent<GoblinBoss>();
            
            _boss.Spawn();
        }
    }
}