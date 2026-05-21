using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    // Tidak perlu assign di Inspector lagi
    private RectTransform dotTransform;
    private Image dotImage;
    private Image blackOverlay;
    private Canvas myCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SetupUI();

        // Listen setiap kali scene baru di-load
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Dipanggil otomatis setiap kali scene berganti
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset semua UI agar tidak keliatan di scene baru
        if (dotImage != null) dotImage.enabled = false;
        if (dotTransform != null) dotTransform.sizeDelta = Vector2.zero;
        if (blackOverlay != null)
        {
            blackOverlay.gameObject.SetActive(false);
            blackOverlay.color = new Color(0, 0, 0, 0);
        }
    }

    void SetupUI()
    {
        // Buat Canvas sendiri secara dinamis, tidak bergantung scene
        GameObject canvasGO = new GameObject("TransitionCanvas");
        canvasGO.transform.SetParent(transform);
        myCanvas = canvasGO.AddComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        myCanvas.sortingOrder = 999; // Selalu di atas semua UI

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Buat BlackOverlay
        GameObject overlayGO = new GameObject("BlackOverlay");
        overlayGO.transform.SetParent(canvasGO.transform, false);
        blackOverlay = overlayGO.AddComponent<Image>();
        blackOverlay.color = new Color(0, 0, 0, 0);
        RectTransform overlayRect = overlayGO.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.sizeDelta = Vector2.zero;
        blackOverlay.gameObject.SetActive(false);

        // Buat DotImage (lingkaran hitam)
        GameObject dotGO = new GameObject("DotImage");
        dotGO.transform.SetParent(canvasGO.transform, false);
        dotImage = dotGO.AddComponent<Image>();
        dotImage.color = Color.black;

        // Pakai sprite circle bawaan Unity
        dotImage.sprite = CreateCircleSprite(128);

        dotTransform = dotGO.GetComponent<RectTransform>();
        dotTransform.anchorMin = new Vector2(0.5f, 0.5f);
        dotTransform.anchorMax = new Vector2(0.5f, 0.5f);
        dotTransform.pivot = new Vector2(0.5f, 0.5f);
        dotTransform.sizeDelta = Vector2.zero;
        dotImage.enabled = false;
    }

    private float GetMaxDotSize()
    {
        RectTransform canvasRect = myCanvas.GetComponent<RectTransform>();
        return new Vector2(canvasRect.rect.width, canvasRect.rect.height).magnitude * 2.2f;
    }

    public void DotTransitionToScene(string sceneName)
    {
        StartCoroutine(DotExpand(sceneName));
    }

    IEnumerator DotExpand(string sceneName)
    {
        dotImage.enabled = true;
        dotTransform.sizeDelta = Vector2.zero;

        float duration = 0.8f;
        float maxSize = GetMaxDotSize();
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, t / duration);
            float size = Mathf.Lerp(0f, maxSize, progress);
            dotTransform.sizeDelta = new Vector2(size, size);
            yield return null;
        }

        dotTransform.sizeDelta = new Vector2(maxSize, maxSize);
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator FadeToBlack()
    {
        blackOverlay.gameObject.SetActive(true);
        blackOverlay.color = new Color(0, 0, 0, 0);
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 1.5f;
            blackOverlay.color = new Color(0, 0, 0, Mathf.Clamp01(t));
            yield return null;
        }
        blackOverlay.color = Color.black;
    }

    public IEnumerator FadeFromBlack()
    {
        blackOverlay.gameObject.SetActive(true);
        blackOverlay.color = Color.black;
        float t = 1f;
        while (t > 0)
        {
            t -= Time.deltaTime * 1.5f;
            blackOverlay.color = new Color(0, 0, 0, Mathf.Clamp01(t));
            yield return null;
        }
        blackOverlay.color = new Color(0, 0, 0, 0);
        blackOverlay.gameObject.SetActive(false);
    }

    public IEnumerator DotShrink()
    {
        float maxSize = GetMaxDotSize();
        dotTransform.sizeDelta = new Vector2(maxSize, maxSize);
        dotImage.enabled = true;

        float duration = 0.8f;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, t / duration);
            float size = Mathf.Lerp(maxSize, 0f, progress);
            dotTransform.sizeDelta = new Vector2(size, size);
            yield return null;
        }

        dotTransform.sizeDelta = Vector2.zero;
        dotImage.enabled = false;
    }
    
    Sprite CreateCircleSprite(int size)
    {
        Texture2D tex = new Texture2D(size, size);
        float center = size / 2f;
        float radius = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                tex.SetPixel(x, y, dist <= radius ? Color.white : Color.clear);
            }
        }

        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

}