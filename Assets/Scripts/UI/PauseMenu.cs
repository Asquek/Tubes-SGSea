using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [Header("Panel")]
    public GameObject panel;

    [Header("Buttons")]
    public Button resumeButton;
    public Button retryButton;
    public Button menuButton;

    private bool isPaused = false;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        retryButton.onClick.AddListener(Retry);
        menuButton.onClick.AddListener(MainMenu);
    }

    void Update()
    {
        Debug.Log("PauseMenu Update jalan");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC kepencet");
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
{
    if (Time.timeScale == 0f) return;
    isPaused = true;
    Time.timeScale = 0f;
    panel.SetActive(true);
}

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        panel.SetActive(false);
    }

    void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MainMenu()
    {
        Time.timeScale = 1f;

        if (TransitionManager.Instance != null)
            TransitionManager.Instance.DotTransitionToScene("Main Menu");
        else
            SceneManager.LoadScene("Main Menu");
    }
}   