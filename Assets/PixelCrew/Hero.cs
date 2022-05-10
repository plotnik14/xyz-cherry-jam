using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector2 _direction;

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void Update()
    {
        if (_direction.magnitude > 0)
        {
            Vector2 delta = _direction * _speed * Time.deltaTime;
            transform.position += new Vector3(delta.x, delta.y, transform.position.z);
        }
    }
}
