using UnityEngine;
using UnityEngine.Events;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    [Header("XP Settings")]
    public float currentXP;
    public float xpToNextLevel = 10f;
    public float xpScaling = 1.3f; // XP butuh naik tiap level
    public int currentLevel = 1;

    [Header("Audio Settings")]
    [Tooltip("Masukkan komponen AudioSource di sini")]
    public AudioSource audioSource;
    [Tooltip("Masukkan clip audio suara Level Up di sini")]
    public AudioClip sfxLevelUp;

    [Header("Events")]
    public UnityEvent onLevelUp;
    public UnityEvent<float> onXPChanged; // buat update UI

    void Awake()
    {
        Instance = this;
    }

    public void AddXP(float amount)
    {
        currentXP += amount;
        float ratio = currentXP / xpToNextLevel;
        onXPChanged?.Invoke(ratio);
        HUDManager.Instance?.UpdateXP(ratio); // update XP bar

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        xpToNextLevel *= xpScaling;
        currentLevel++;

        // --- KODE BARU: Memainkan SFX Level Up ---
        if (audioSource != null && sfxLevelUp != null)
        {
            audioSource.PlayOneShot(sfxLevelUp);
        }
        // -----------------------------------------

        GameStats.Instance.levelsReached = currentLevel;
        HUDManager.Instance?.UpdateXP(0f);
        HUDManager.Instance?.UpdateLevel(currentLevel);
        onLevelUp?.Invoke();
    }
}