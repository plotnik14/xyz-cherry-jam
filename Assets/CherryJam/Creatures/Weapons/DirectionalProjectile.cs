using UnityEngine;

namespace CherryJam.Creatures.Weapons
{
    public class DirectionalProjectile : BaseProjectile
    {
        public void Launch(Vector2 direction)
        {
            // Rotate(direction);
            
            Rigidbody = GetComponent<Rigidbody2D>();
            Rigidbody.AddForce(direction * _speed, ForceMode2D.Impulse);
        }

        private void Rotate(Vector2 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 100);
            // transform.rotation = Quaternion.LookRotation(new Vector3(0,0, 180), direction);
        }

        public void Stop()
        {
            Rigidbody.velocity = Vector2.zero;
        }
    }
}