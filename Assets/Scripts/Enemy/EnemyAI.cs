using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float separationRadius = 1f;
    public float separationForce = 2f;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Arah ke player
        Vector2 toPlayer = (player.position - transform.position).normalized;

        // Separation � hindarin enemy lain yang terlalu deket
        Vector2 separation = GetSeparation();

        // Gabungin dua arah
        Vector2 finalDir = (toPlayer + separation).normalized;
        rb.linearVelocity = finalDir * speed;
    }

    Vector2 GetSeparation()
    {
        Vector2 sep = Vector2.zero;

        // Deteksi enemy lain di sekitar
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(
            transform.position, separationRadius
        );

        foreach (Collider2D col in neighbors)
        {
            // Skip diri sendiri dan player
            if (col.gameObject == gameObject) continue;
            if (col.CompareTag("Player")) continue;
            if (!col.GetComponent<EnemyAI>()) continue;

            Vector2 pushDir = transform.position - col.transform.position;

            // Makin deket = makin kuat dorongannya
            float strength = 1f - (pushDir.magnitude / separationRadius);
            sep += pushDir.normalized * strength;
        }

        return sep * separationForce;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>()?.TakeDamage(
                GetComponent<EnemyStats>().damage * Time.deltaTime
            );
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualisasi radius separation di Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}