using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("UI")]
    public Slider bossHPBar;

    private string bossName;
    private EnemyStats stats;
    private bool isDead = false;
    private GameObject xpGemPrefab;

    public void Init(string name)
    {
        bossName = name;
        stats = GetComponent<EnemyStats>();
        xpGemPrefab = stats != null ? stats.xpGemPrefab : null;

        if (bossHPBar != null)
        {
            bossHPBar.minValue = 0f;
            bossHPBar.maxValue = 1f;
            bossHPBar.value = 1f;
            bossHPBar.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (stats == null || isDead) return;

        // Update HP bar
        if (bossHPBar != null)
            bossHPBar.value = stats.currentHP / stats.maxHP;
    }

    public void NotifyDead()
    {
        if (isDead) return;
        isDead = true;

        if (bossHPBar != null)
            bossHPBar.gameObject.SetActive(false);

        // Drop XP gem
        if (xpGemPrefab != null)
        {
            GameObject gem = Instantiate(xpGemPrefab, transform.position, Quaternion.identity);
            XPGem xpGem = gem.GetComponent<XPGem>();
            if (xpGem != null && stats != null)
                xpGem.xpValue = stats.xpDrop;
        }

        WaveManager.Instance?.OnBossDefeated(bossName);
        Destroy(gameObject);
    }
}