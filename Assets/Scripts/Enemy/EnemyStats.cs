using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float maxHP = 30f;
    public float currentHP;
    public float damage = 10f;
    public float xpDrop = 5f;

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
        // nanti spawn XP gem di sini
        Debug.Log("Enemy mati, drop " + xpDrop + " XP");
        gameObject.SetActive(false); // return to pool nanti
    }
}