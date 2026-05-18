using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("HP")]
    public Slider hpBar;

    [Header("XP")]
    public Slider xpBar;

    [Header("Text")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timerText;

    private float timer;
    private PlayerStats playerStats;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        UpdateLevel(1);
    }

    void Update()
    {
        // Timer naik terus
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Update HP bar tiap frame
        if (playerStats != null)
        {
            hpBar.value = playerStats.currentHP / playerStats.maxHP;
        }
    }

    public void UpdateXP(float ratio)
    {
        xpBar.value = ratio;
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "LV " + level;
    }
}