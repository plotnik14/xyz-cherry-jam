using System.Collections;
using CherryJam.Components.ColliderBased;
using UnityEngine;

namespace CherryJam.Creatures.Mobs
{
    public class PinkStarAI : MobAI
    {
        [Space][Header("Pink Star")]
        [SerializeField] private ColliderCheck _obstacleCheck;

        private RollingCreature _rollingCreatiure;

        protected override void Awake()
        {
            base.Awake();

            _rollingCreatiure = _creature as RollingCreature;
        }

        protected override IEnumerator GoToHero()
        {
            var direction = GetDirectionToTarget();
            _rollingCreatiure.StartRolling();

            while (!_obstacleCheck.IsTouchingLayer)
            {
                _rollingCreatiure.SetDirection(direction);
                yield return null;
            }

            StopCreature();
            _rollingCreatiure.StopRolling();

            if (!_vision.IsTouchingLayer)
            {
                _particles.Spawn("MissHero");
            }
            
            yield return new WaitForSeconds(_missHeroCooldown);

            StartState(_patrol.DoPatrol());
        }
    }
}
