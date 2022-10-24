using UnityEngine;

namespace CherryJam.Creatures.Weapons
{
    public class DirectionalProjectile : BaseProjectile
    {
        [SerializeField] private bool _applyRotation;
        
        public void Launch(Vector2 direction)
        {
            if (_applyRotation)
                Rotate(direction);
            
            Rigidbody = GetComponent<Rigidbody2D>();
            Rigidbody.AddForce(direction * _speed, ForceMode2D.Impulse);
        }

        private void Rotate(Vector2 direction)
        {
            direction = UpdateScaledDirection(direction);
           var angleDeg = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; ;
           transform.rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);
        }

        private Vector2 UpdateScaledDirection(Vector2 direction)
        {
            return direction.x < 0
                ? -direction
                : direction;
        }

        public void Stop()
        {
            Rigidbody.velocity = Vector2.zero;
        }
    }
}