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
        if (_direction.x == 0 || _direction.y != 0) return;

        var delta = _direction.x * _speed * Time.deltaTime;
        var newXPosition = transform.position.x + delta;

        delta = _direction.y * _speed * Time.deltaTime;
        var newYPosition = transform.position.y + delta;

        transform.position = new Vector3(newXPosition, newYPosition, transform.position.z);
    }
}
