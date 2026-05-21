using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float currentHP;
    public float regenPerSecond = 0.5f;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent<float> onHPChanged; // buat update UI nanti

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        // Regen HP
        if (currentHP < maxHP)
        {
            currentHP += regenPerSecond * Time.deltaTime;
            currentHP = Mathf.Min(currentHP, maxHP);
            onHPChanged?.Invoke(currentHP);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        onHPChanged?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;
            onDeath?.Invoke();
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
        GameOverScreen.Instance?.Show();
    }
}