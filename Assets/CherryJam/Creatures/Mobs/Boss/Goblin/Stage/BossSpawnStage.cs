using UnityEngine;

namespace CherryJam.Creatures.Mobs.Boss.Goblin.Stage
{
    public class BossSpawnStage : StateMachineBehaviour
    {
        private BossMaster _bossMaster;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_bossMaster == null)
                _bossMaster = animator.GetComponent<BossMaster>();
            
            _bossMaster.Spawn();
        }
    }
}