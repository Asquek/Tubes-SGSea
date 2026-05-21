using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI textIndonesia;
    public TextMeshProUGUI textEnglish;
    public CanvasGroup narrativeCanvasGroup; // taruh di NarrativePanel

    [Header("Narasi")]
    private string[] indonesiaSentences = new string[]
    {
        "Kematian bukanlah akhir... bagi mereka yang belum menebus dosanya.",
        "Ikarus, seorang jiwa yang terjerat dalam kegelapan abadi.",
        "Namun sebuah suara tanpa nama memanggilnya dari balik kesunyian...",
        "\u201cBangkitlah. Hadapi apa yang telah kau tinggalkan.\u201d",
        "Jelmaan setiap dosa yang pernah ia perbuat kini menanti di depan.",
        "Hanya dengan mengalahkan mereka semua... jalan menuju surga akan terbuka.",
        "Perjalananmu dimulai sekarang, Ikarus."
    };

    private string[] englishSentences = new string[]
    {
        "Death is not the end... for those who have yet to atone for their sins.",
        "Ikarus, a soul ensnared in eternal darkness.",
        "Yet a nameless voice called to him from within the silence...",
        "\u201cRise. Face what you have left behind.\u201d",
        "The embodiment of every sin he once committed now awaits ahead.",
        "Only by defeating them all... will the path to heaven open.",
        "Your journey begins now, Ikarus."
    };

    [Header("Timing")]
    public float fadeInDuration = 1.5f;
    public float holdDuration = 3f;
    public float fadeOutDuration = 1f;
    public float delayBetweenSentences = 0.3f;
    public float delayAfterLast = 2f;

    [Header("Scene")]
    public string gameplaySceneName = "GameScene";

    void Start()
    {
        narrativeCanvasGroup.alpha = 0;
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        // Jeda awal setelah layar hitam masuk
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < indonesiaSentences.Length; i++)
        {
            // Set kedua teks
            textIndonesia.text = indonesiaSentences[i];
            textEnglish.text = englishSentences[i];
            narrativeCanvasGroup.alpha = 0;

            // Fade in keduanya bersamaan
            yield return StartCoroutine(FadeCanvas(0f, 1f, fadeInDuration));

            // Tahan
            yield return new WaitForSeconds(holdDuration);

            // Fade out keduanya bersamaan
            yield return StartCoroutine(FadeCanvas(1f, 0f, fadeOutDuration));

            yield return new WaitForSeconds(delayBetweenSentences);
        }

        // Semua kalimat selesai
        yield return new WaitForSeconds(delayAfterLast);

        // Transisi ke gameplay
        StartCoroutine(TransitionToGameplay());
    }

    IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            narrativeCanvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        narrativeCanvasGroup.alpha = to;
    }

    IEnumerator TransitionToGameplay()
    {
        if (TransitionManager.Instance != null)
        {
            yield return StartCoroutine(TransitionManager.Instance.FadeToBlack());
        }

        SceneManager.LoadScene(gameplaySceneName);

        if (TransitionManager.Instance != null)
        {
            yield return StartCoroutine(TransitionManager.Instance.FadeFromBlack());
        }
    }
}