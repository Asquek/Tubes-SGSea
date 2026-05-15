using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Ambil input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize biar diagonal gak lebih cepet
        movement = movement.normalized;

        // Flip sprite kiri/kanan
        if (movement.x > 0) sr.flipX = false;
        if (movement.x < 0) sr.flipX = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }
}