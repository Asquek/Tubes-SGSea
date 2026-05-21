using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;
    // Kolom untuk menaruh file audio klik
    public AudioClip sfxKlikStart;

    [Header("Scene Settings")]
    public string namaSceneGameplay = "IntroScene"; // Default kita arahkan ke Intro sesuai alurmu

    public void StartGame()
    {
        // 1. Memutar SFX klik khusus jika dipasang di Inspector
        if (audioSource != null && sfxKlikStart != null)
        {
            audioSource.PlayOneShot(sfxKlikStart);
        }

        // 2. Memanggil transisi dari TransitionManager menuju IntroScene
        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.DotTransitionToScene("IntroScene");
        }
        else
        {
            // Fail-safe jika kamu testing tanpa TransitionManager di hierarchy
            SceneManager.LoadScene("IntroScene");
        }
    }

    // FUNGSI BARU: Untuk kamu pasang di OnClick() tombol Credits
    public void BukaCredits()
    {
        // 1. Memutar SFX klik khusus jika dipasang di Inspector
        if (audioSource != null && sfxKlikStart != null)
        {
            audioSource.PlayOneShot(sfxKlikStart);
        }

        // 2. Memanggil transisi dari TransitionManager menuju CreditsScene
        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.DotTransitionToScene("CreditsScene");
        }
        else
        {
            // Fail-safe jika kamu testing tanpa TransitionManager di hierarchy
            SceneManager.LoadScene("CreditsScene");
        }
    }

    // Tambahkan fungsi ini di dalam script MainMenu.cs kamu jika memilih Cara 2
    public void KembaliKeMainMenu()
    {
        if (audioSource != null && sfxKlikStart != null)
        {
            audioSource.PlayOneShot(sfxKlikStart);
        }

        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.DotTransitionToScene("Main Menu");
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}