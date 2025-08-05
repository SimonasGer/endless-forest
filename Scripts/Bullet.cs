using UnityEngine;
using UnityEngine.InputSystem;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 25.0f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Shoot();
        Destroy(gameObject, lifetime);
    }

    void Shoot()
    {
        rb.linearVelocity = MouseDirection() * speed;
    }

    private Vector2 MouseDirection()
    {
        Vector2 mouseAt = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mouseAt);
        worldMousePos.z = 0;

        Vector2 direction = (worldMousePos - transform.position).normalized;
        return direction;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject); // Destroy on impact
    }
}
