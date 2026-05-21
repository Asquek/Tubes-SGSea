using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance;

    [Header("Panel")]
    public GameObject panel;

    [Header("Stat Texts")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI enemiesText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI gradeText;

    [Header("Buttons")]
    public Button retryButton;
    public Button menuButton;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    void Start()
    {
        retryButton.onClick.AddListener(Retry);
        menuButton.onClick.AddListener(MainMenu);
    }

    public void Show()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);

        GameStats s = GameStats.Instance;

        // Format waktu
        int min = Mathf.FloorToInt(s.timeSurvived / 60f);
        int sec = Mathf.FloorToInt(s.timeSurvived % 60f);
        timeText.text = $"- Waktu Survive -\n{min:00}:{sec:00}";
        enemiesText.text = $" -Enemy Dibunuh -\n{s.enemiesKilled}";
        levelText.text = $"- Level Tercapai -\n{s.levelsReached}";
        gemsText.text = $"- Gems -\n{s.gemsCollected}";
        gradeText.text = GetGrade(s);
    }

    string GetGrade(GameStats s)
    {
        int score = 0;
        if (s.timeSurvived > 300f) score++;  // survive 5 menit
        if (s.timeSurvived > 600f) score++;  // survive 10 menit
        if (s.enemiesKilled > 100) score++;
        if (s.enemiesKilled > 300) score++;
        if (s.levelsReached > 10) score++;

        return score switch
        {
            5 => "Grade S",
            4 => "Grade A",
            3 => "Grade B",
            2 => "Grade C",
            1 => "Grade D",
            _ => "Grade F"
        };
    }

    void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}