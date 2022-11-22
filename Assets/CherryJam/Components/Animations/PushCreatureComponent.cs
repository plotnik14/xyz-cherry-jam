using CherryJam.Creatures;
using UnityEngine;

namespace CherryJam.Components.Animations
{
    public class PushCreatureComponent : MonoBehaviour
    {
        [SerializeField] private float _pushStrength;
        [SerializeField] private bool _useVelocityForDirection;
        
        public void Push(GameObject target, Vector2 direction)
        {
            var creature = target.GetComponent<Creature>();
            if (creature == null) return;

            var directionX = direction.x > 0 ? 1f : -1f;
            creature.ApplyPush(directionX, _pushStrength);
        }

        public void Push(GameObject target)
        {
            var direction = GetDirection();
            Push(target, direction);
        }

        private Vector2 GetDirection()
        {
            if (!_useVelocityForDirection) return transform.lossyScale;
            
            var rigidbodyComponent = GetComponent<Rigidbody2D>();
            return rigidbodyComponent.velocity;

        }
    }
}