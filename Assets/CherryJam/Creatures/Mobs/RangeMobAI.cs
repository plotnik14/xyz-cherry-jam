using System.Collections;
using UnityEngine;

namespace CherryJam.Creatures.Mobs
{
    public class RangeMobAI : MobAI
    {
        protected override IEnumerator AgroToHero()
        {
            LookAtHero();
            
            yield return new WaitForSeconds(_alarmDelay);
            
            StartState(Attack());
        }

        protected override IEnumerator Attack()
        {
            while (_vision.IsTouchingLayer)
            {
                LookAtHero();
                _creature.RangeAttack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StopCreature();
            
            yield return new WaitForSeconds(_missHeroCooldown);

            StartState(_patrol.DoPatrol());
        }
    }
}