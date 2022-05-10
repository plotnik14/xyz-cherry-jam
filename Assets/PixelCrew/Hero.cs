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
        if (_direction == Vector2.zero) return;

        Vector2 delta = _direction * _speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + delta.x, transform.position.y + delta.y, transform.position.z);
    }
}
