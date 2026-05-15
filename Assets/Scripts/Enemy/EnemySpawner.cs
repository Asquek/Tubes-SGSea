using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnInterval = 2f;
    public int maxEnemies = 50;
    public float spawnRadius = 12f;

    private int currentEnemies = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        // Spawn di luar kamera, random arah
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = player.position + new Vector3(dir.x, dir.y, 0) * spawnRadius;

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemies++;

        // Kurangi counter kalau enemy mati
        enemy.GetComponent<EnemyStats>().enabled = true;
        StartCoroutine(TrackEnemy(enemy));
    }

    IEnumerator TrackEnemy(GameObject enemy)
    {
        while (enemy != null && enemy.activeInHierarchy)
        {
            yield return null;
        }
        currentEnemies--;
    }
}