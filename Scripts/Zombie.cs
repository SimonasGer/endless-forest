using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int health = 10;
    public float speed = 15.0f;
    private Transform player;

    void Update()
    {
        player = GameObject.FindWithTag("Player").transform;
        if (player == null) return;
        Walk();
        Despawn();
    }

    void Walk()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    void Despawn()
    {
        if (Vector2.Distance(player.position, transform.position) > 50.0f)
        {
            Destroy(gameObject);
            FindFirstObjectByType<EnemyController>().zombieCount--;
        }
    }
}
