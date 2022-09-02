using PixelCrew.Creatures;
using PixelCrew.Creatures.Mobs;
using UnityEngine;

namespace PixelCrew.Components.Magic
{
    public class MagicEffectComponent : MonoBehaviour
    {
        public void Freeze(GameObject target)
        {
            var creature = target.GetComponent<Creature>();
            if (creature != null)
                creature.Freeze();

            var trap = target.GetComponent<ShootingTrapAI>();
            if (trap != null)
                trap.Freeze();
        }
    }
}