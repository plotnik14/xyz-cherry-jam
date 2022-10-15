using CherryJam.Creatures;
using UnityEngine;

namespace CherryJam.Components.Animations
{
    public class PushCreatureComponent : MonoBehaviour
    {
        [SerializeField] private float _pushStrength;
        
        public void Push(GameObject target, Vector2 direction)
        {
            var creature = target.GetComponent<Creature>();
            if (creature == null) return;

            var directionX = direction.x > 0 ? 1f : -1f;
            creature.ApplyPush(directionX, _pushStrength);
        }

        public void Push(GameObject target)
        {
            Push(target, transform.lossyScale);
        }
    }
}