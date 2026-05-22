using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer sr;

    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); // tambah ini
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        if (movement.x > 0) sr.flipX = false;
        if (movement.x < 0) sr.flipX = true;

        // Update animator
        anim.SetFloat("Speed", movement.magnitude);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * speed;
    }

}