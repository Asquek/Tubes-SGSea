using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs; // drag semua prefab obstacle
    public int obstaclesPerChunk = 5;
    public float chunkSize = 20f;
    public float minDistanceFromCenter = 2f; // biar gak spawn di tengah (posisi player awal)

    // Nyimpen obstacle per chunk position
    private Dictionary<Vector2Int, List<GameObject>> spawnedObstacles
        = new Dictionary<Vector2Int, List<GameObject>>();

    // Dipanggil oleh InfiniteBackground saat chunk recycle
    public void RefreshChunk(Vector2Int chunkCoord, Vector3 chunkWorldPos)
    {
        // Hapus obstacle lama di chunk ini
        if (spawnedObstacles.ContainsKey(chunkCoord))
        {
            foreach (var obj in spawnedObstacles[chunkCoord])
                if (obj != null) Destroy(obj);
            spawnedObstacles[chunkCoord].Clear();
        }
        else
        {
            spawnedObstacles[chunkCoord] = new List<GameObject>();
        }

        // Spawn obstacle baru dengan seed berdasarkan koordinat chunk
        // Seed konsisten = obstacle sama tiap balik ke chunk yang sama
        Random.State oldState = Random.state;
        Random.InitState(chunkCoord.x * 1000 + chunkCoord.y);

        for (int i = 0; i < obstaclesPerChunk; i++)
        {
            Vector3 randomPos = chunkWorldPos + new Vector3(
                Random.Range(-chunkSize / 2f + 1f, chunkSize / 2f - 1f),
                Random.Range(-chunkSize / 2f + 1f, chunkSize / 2f - 1f),
                0
            );

            // Skip kalau terlalu deket center (spawn point player)
            if (chunkWorldPos == Vector3.zero &&
                Vector3.Distance(randomPos, Vector3.zero) < minDistanceFromCenter)
                continue;

            // Pilih prefab random
            GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            GameObject obstacle = Instantiate(prefab, randomPos, Quaternion.identity);

            obstacle.transform.rotation = Quaternion.identity;

            spawnedObstacles[chunkCoord].Add(obstacle);
        }

        // Restore random state biar gak ngaruh ke sistem lain
        Random.state = oldState;
    }
}