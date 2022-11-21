using UnityEngine;

namespace CherryJam.Creatures.Mobs.Boss.Master.Stage
{
    public class BossWaveAttackStage : StateMachineBehaviour
    {
        private BossMaster _bossMaster;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_bossMaster == null)
                _bossMaster = animator.GetComponent<BossMaster>();
            
            _bossMaster.WaveAttack();
        }
    }
}