using UnityEngine;

public class XPGem : MonoBehaviour
{
    public float xpValue = 5f;
    public float magnetRadius = 3f;
    public float moveSpeed = 8f;

    private Transform player;
    private bool attracted = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Kalau player deket, gem terbang ke player
        if (dist < magnetRadius) attracted = true;

        if (attracted)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            XPManager.Instance?.AddXP(xpValue);

            if (GameStats.Instance != null)
                GameStats.Instance.gemsCollected++;

            Destroy(gameObject);
        }
    }
}