using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public int enemiesKilled;
    public float timeSurvived;
    public int levelsReached;
    public int gemsCollected;

    void Awake()
    {
        Instance = this;
    }
}