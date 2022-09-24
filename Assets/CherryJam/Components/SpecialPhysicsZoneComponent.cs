using UnityEngine;

namespace CherryJam.Components
{
    public class SpecialPhysicsZoneComponent : MonoBehaviour
    {
        [SerializeField] private float _liftSpeed;

        public virtual void Lift(GameObject go)
        {
            var rigidbody = go.GetComponent<Rigidbody2D>();

            if (rigidbody != null)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, _liftSpeed);
            }
        }
    }
}
