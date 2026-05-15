using UnityEngine;

public class WeaponOrb : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1f;

    private float timer;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / fireRate)
        {
            timer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        // Konversi posisi mouse ke world position
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector2 dir = (mouseWorld - transform.position).normalized;

        GameObject proj = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        proj.GetComponent<ProjectileBase>()?.Init(dir);
    }
}