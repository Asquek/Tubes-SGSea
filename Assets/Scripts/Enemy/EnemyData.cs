using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public float hp = 30f;
    public float damage = 10f;
    public float speed = 2f;
    public float xpDrop = 5f;
    public bool isBoss = false;
}