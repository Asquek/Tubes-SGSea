using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameClearScreen : MonoBehaviour
{
    public static GameClearScreen Instance;

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

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip sfxGameClear;
    public AudioClip sfxKlikRetry;
    public AudioClip sfxKlikMenu;

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

        if (audioSource != null && sfxGameClear != null)
            audioSource.PlayOneShot(sfxGameClear);

        GameStats s = GameStats.Instance;

        int min = Mathf.FloorToInt(s.timeSurvived / 60f);
        int sec = Mathf.FloorToInt(s.timeSurvived % 60f);

        timeText.text = $"- Waktu Selesai -\n{min:00}:{sec:00}";
        enemiesText.text = $"- Enemy Dibunuh -\n{s.enemiesKilled}";
        levelText.text = $"- Level Tercapai -\n{s.levelsReached}";
        gemsText.text = $"- Gems -\n{s.gemsCollected}";
        gradeText.text = GetGrade(s);
    }

    string GetGrade(GameStats s)
    {
        int score = 0;
        if (s.timeSurvived < 600f) score++; // selesai dalam 10 menit
        if (s.timeSurvived < 300f) score++; // selesai dalam 5 menit
        if (s.enemiesKilled > 200) score++;
        if (s.levelsReached > 15) score++;
        if (s.gemsCollected > 300) score++;

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
        if (audioSource != null && sfxKlikRetry != null)
            audioSource.PlayOneShot(sfxKlikRetry);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MainMenu()
    {
        if (audioSource != null && sfxKlikMenu != null)
            audioSource.PlayOneShot(sfxKlikMenu);

        Time.timeScale = 1f;

        if (TransitionManager.Instance != null)
            TransitionManager.Instance.DotTransitionToScene("Main Menu");
        else
            SceneManager.LoadScene("Main Menu");
    }
}