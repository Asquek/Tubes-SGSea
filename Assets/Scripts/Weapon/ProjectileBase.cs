using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 20f;
    public float lifetime = 3f;

    private Vector2 direction;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyStats>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}