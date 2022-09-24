using UnityEngine;

namespace CherryJam.Creatures.Weapons
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected bool _invertX;

        protected Rigidbody2D Rigidbody;
        protected int Direction;

        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            SetDirection();
        }

        protected void SetDirection()
        {
            var mod = _invertX ? -1 : 1;
            Direction = transform.lossyScale.x * mod > 0 ? 1 : -1;     
        }
    }
}
