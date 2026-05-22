using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance;

    public GameObject menuPanel;
    public Button[] upgradeButtons; // 3 button
    public TextMeshProUGUI[] upgradeTexts; // teks tiap button

    // List upgrade yang tersedia
    private string[] allUpgrades = {
        "Damage +20%",
        "Speed +15%",
        "Fire Rate +25%",
        "HP +30",
        "Regen +1/s",
        "Projectile +1"
    };

    void Awake()
    {
        Instance = this;
        menuPanel.SetActive(false);
    }

    public void ShowMenu()
    {
        Time.timeScale = 0f; // pause game
        menuPanel.SetActive(true);

        // Pilih 3 upgrade random
        int[] picked = PickRandom3();
        for (int i = 0; i < 3; i++)
        {
            int idx = picked[i];
            upgradeTexts[i].text = allUpgrades[idx];

            int captured = idx; // capture buat lambda
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(captured));
        }
    }

    int[] PickRandom3()
    {
        int[] result = new int[3];
        System.Collections.Generic.List<int> pool =
            new System.Collections.Generic.List<int>();

        for (int i = 0; i < allUpgrades.Length; i++) pool.Add(i);

        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, pool.Count);
            result[i] = pool[rand];
            pool.RemoveAt(rand);
        }

        return result;
    }

    void SelectUpgrade(int idx)
    {
        ApplyUpgrade(allUpgrades[idx]);
        menuPanel.SetActive(false);
        Time.timeScale = 1f; // resume game
    }

    void ApplyUpgrade(string upgrade)
    {
        PlayerStats ps = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        WeaponOrb orb = GameObject.FindWithTag("Player").GetComponent<WeaponOrb>();

        switch (upgrade)
        {
            case "Damage +20%":
                // nanti connect ke weapon system
                Debug.Log("Damage naik 20%");
                break;
            case "Speed +15%":
                GameObject.FindWithTag("Player")
                    .GetComponent<PlayerMovement>().speed *= 1.15f;
                break;
            case "Fire Rate +25%":
                if (orb) orb.fireRate *= 1.25f;
                break;
            case "HP +30":
                if (ps) { ps.maxHP += 30; ps.currentHP += 30; }
                break;
            case "Regen +1/s":
                if (ps) ps.regenPerSecond += 1f;
                break;
            case "Projectile +1":
                WeaponOrb orb2 = GameObject.FindWithTag("Player").GetComponent<WeaponOrb>();
                if (orb2) orb2.projectileCount++;
                break;
        }
    }
}