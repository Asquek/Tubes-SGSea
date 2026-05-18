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
        onXPChanged?.Invoke(currentXP / xpToNextLevel);

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        xpToNextLevel *= xpScaling; // makin tinggi level, butuh XP lebih banyak
        currentLevel++;

        Debug.Log("Level Up! Sekarang level " + currentLevel);
        onLevelUp?.Invoke();
    }
}