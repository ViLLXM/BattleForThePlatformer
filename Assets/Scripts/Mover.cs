using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class Mover : MonoBehaviour
{
    [SerializeField][Range(0, 15f)] private float _jumpForce;
    [SerializeField][Range(1, 10f)] private float _speed;

    private Rigidbody2D _rigidbody2D;
    private float _jumpPowerConverter;
    private float _checkGroundOffset;
    private float _checkGroundRadius;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _checkGroundOffset = 1f;
        _checkGroundRadius = 0.83f;
        _jumpPowerConverter = 0.01f;
    }

    public void Jump()
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - _checkGroundOffset), _checkGroundRadius);

        if (collider2D.Length > 1)
            _rigidbody2D.AddForce(Vector2.up * (_jumpForce * _jumpPowerConverter), ForceMode2D.Impulse);
    }

    public void Run(Vector3 direction)
    {
        transform.position += direction * (_speed * Time.deltaTime);
    }
}
