using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float maxHP = 30f;
    public float currentHP;
    public float damage = 10f;
    public float xpDrop = 5f;

    public GameObject xpGemPrefab; // drag prefab gem di inspector

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0) Die();
    }

    void Die()
    {
        if (GameStats.Instance != null)
            GameStats.Instance.enemiesKilled++;

        if (xpGemPrefab != null)
        {
            GameObject gem = Instantiate(xpGemPrefab, transform.position, Quaternion.identity);
            gem.GetComponent<XPGem>().xpValue = xpDrop;
        }

        gameObject.SetActive(false);
    }
}