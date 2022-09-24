﻿using CherryJam.Components.GoBased;
using UnityEngine;

namespace CherryJam.Creatures.Mobs.Boss.States
{
    public class BossNextStageState : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<CircularProjectileSpawner>();
            spawner.Stage++;

            var changeLight = animator.GetComponent<ChangeLightsComponent>();
            changeLight.SetColor();
        }
    }
}
