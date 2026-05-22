using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [Header("Settings")]
    public Transform player;
    public float chunkSize = 20f;

    private Transform[,] chunks = new Transform[3, 3];
    private Vector2Int[,] chunkCoords = new Vector2Int[3, 3];
    private ObstacleSpawner obstacleSpawner;

    // Flag biar clone gak jalanin Start()
    private bool isClone = false;

    void Start()
    {
        if (isClone) return; // clone skip semua logic

        obstacleSpawner = GetComponent<ObstacleSpawner>();

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GameObject chunk;

                if (x == 1 && y == 1)
                {
                    chunk = gameObject; // center = original
                }
                else
                {
                    chunk = Instantiate(gameObject);
                    // Tandai clone biar gak trigger Start() lagi
                    chunk.GetComponent<InfiniteBackground>().isClone = true;
                    chunk.GetComponent<InfiniteBackground>().enabled = false;
                }

                float posX = (x - 1) * chunkSize;
                float posY = (y - 1) * chunkSize;
                chunk.transform.position = new Vector3(posX, posY, 0);
                chunks[x, y] = chunk.transform;
                chunkCoords[x, y] = new Vector2Int(x - 1, y - 1);

                obstacleSpawner?.RefreshChunk(
                    chunkCoords[x, y],
                    chunk.transform.position
                );
            }
        }
    }

    void Update()
    {
        if (isClone) return;
        RecycleChunks();
    }

    void RecycleChunks()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Transform chunk = chunks[x, y];
                Vector3 diff = chunk.position - player.position;
                bool moved = false;
                Vector3 offset = Vector3.zero;

                if (diff.x < -chunkSize * 1.5f)
                { offset.x = chunkSize * 3; moved = true; }
                else if (diff.x > chunkSize * 1.5f)
                { offset.x = -chunkSize * 3; moved = true; }

                if (diff.y < -chunkSize * 1.5f)
                { offset.y = chunkSize * 3; moved = true; }
                else if (diff.y > chunkSize * 1.5f)
                { offset.y = -chunkSize * 3; moved = true; }

                if (moved)
                {
                    chunk.position += offset;

                    chunkCoords[x, y] += new Vector2Int(
                        Mathf.RoundToInt(offset.x / chunkSize),
                        Mathf.RoundToInt(offset.y / chunkSize)
                    );

                    obstacleSpawner?.RefreshChunk(
                        chunkCoords[x, y],
                        chunk.position
                    );
                }
            }
        }
    }
}