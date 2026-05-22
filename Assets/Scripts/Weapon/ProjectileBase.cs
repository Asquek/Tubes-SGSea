using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 20f;
    public float lifetime = 3f;

    [Header("Audio Settings")]
    [Tooltip("Masukkan clip audio suara tembakan di sini")]
    public AudioClip sfxTembak;

    private Vector2 direction;

    // Fungsi Start dipanggil tepat di frame pertama peluru ini lahir
    void Start()
    {
        // --- KODE BARU: Memutar SFX melalui AudioSource Player ---
        // Mencari objek dengan tag "Player" di dalam scene game kamu
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // Mengambil komponen AudioSource yang menempel pada Player
            AudioSource playerAudio = player.GetComponent<AudioSource>();

            if (playerAudio != null && sfxTembak != null)
            {
                // PlayOneShot membuat suara tidak akan terpotong walau peluru hancur,
                // karena yang memutar suara adalah Player, bukan pelurunya!
                playerAudio.PlayOneShot(sfxTembak);
            }
        }
        // --------------------------------------------------------
    }

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
            Destroy(gameObject); // Sekarang ini 100% aman, suara tidak akan terputus!
        }
    }
}