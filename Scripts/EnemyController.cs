using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int zombieCount = 0;
    public int maxZombies = 25;
    public float spawnInterval = 2f;
    public GameObject zombie;
    public Vector2 spawnRadius = new(10f, 10f);

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && zombieCount < maxZombies)
        {
            SpawnZombie();
            timer = 0f;
        }
    }

    void SpawnZombie()
    {
        Vector2 randomOffset = new Vector2(
            Random.Range(-spawnRadius.x, spawnRadius.x),
            Random.Range(-spawnRadius.y, spawnRadius.y)
        );

        Vector3 spawnPosition = transform.position + (Vector3)randomOffset;

        Instantiate(zombie, spawnPosition, Quaternion.identity);
        zombieCount++;
    }
}
