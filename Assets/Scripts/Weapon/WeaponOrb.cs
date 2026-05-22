using UnityEngine;

public class WeaponOrb : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    public int projectileCount = 1; // nambah ini lewat upgrade

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
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector2 baseDir = (mouseWorld - transform.position).normalized;

        if (projectileCount == 1)
        {
            SpawnProjectile(baseDir);
            return;
        }

        // Spread projectile merata 360 derajat
        float angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Vector2 dir = Rotate(baseDir, angle);
            SpawnProjectile(dir);
        }
    }

    void SpawnProjectile(Vector2 dir)
    {
        GameObject proj = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );
        proj.GetComponent<ProjectileBase>()?.Init(dir);
    }

    // Rotate vector 2D sesuai angle (degrees)
    Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad),
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad)
        );
    }
}