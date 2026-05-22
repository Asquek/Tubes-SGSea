using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public EnemyData enemyData;
        [Tooltip("Muncul di menit ke berapa")]
        public float startAtMinute;
        [Tooltip("Interval spawn dalam detik (boss diabaikan)")]
        public float spawnInterval = 2f;
        [Tooltip("Max enemy dari wave ini sekaligus (boss diabaikan)")]
        public int maxCount = 20;
        public bool isBossWave = false;
    }

    [Header("Wave Config")]
    public List<Wave> waves;

    [Header("Spawn Settings")]
    public Transform player;
    public float spawnRadius = 12f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sfxBossAppear;
    public AudioClip sfxBossDefeated;
    public AudioClip sfxGameClear;

    private bool gameCleared = false;
    private int bossesDefeated = 0;
    private int totalBosses = 0;

    void Awake() => Instance = this;

    void Start()
    {
        // Hitung total boss
        foreach (var w in waves)
            if (w.isBossWave) totalBosses++;

        // Aktifkan tiap wave sesuai waktunya
        foreach (var w in waves)
            StartCoroutine(ActivateWave(w));
    }

    IEnumerator ActivateWave(Wave wave)
    {
        yield return new WaitForSeconds(wave.startAtMinute * 60f);

        if (gameCleared) yield break;

        if (wave.isBossWave)
        {
            SpawnBoss(wave);
        }
        else
        {
            StartCoroutine(SpawnLoop(wave));
        }
    }

    IEnumerator SpawnLoop(Wave wave)
    {
        while (!gameCleared)
        {
            int currentCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (currentCount < wave.maxCount)
                SpawnEnemy(wave.enemyData);

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    void SpawnEnemy(EnemyData data)
    {
        if (data == null || data.prefab == null) return;

        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 pos = player.position + new Vector3(dir.x, dir.y, 0) * spawnRadius;

        GameObject obj = Instantiate(data.prefab, pos, Quaternion.identity);
        ApplyStats(obj, data);
    }

    void SpawnBoss(Wave wave)
    {
        if (wave.enemyData == null || wave.enemyData.prefab == null) return;

        // Spawn boss sedikit di kanan player
        Vector3 pos = player.position + new Vector3(spawnRadius, 0, 0);
        GameObject obj = Instantiate(wave.enemyData.prefab, pos, Quaternion.identity);
        ApplyStats(obj, wave.enemyData);

        // Init BossHealth buat track HP bar dan kematian
        BossHealth bh = obj.GetComponent<BossHealth>();
        if (bh != null) bh.Init(wave.enemyData.enemyName);

        // SFX + notif
        if (audioSource != null && sfxBossAppear != null)
            audioSource.PlayOneShot(sfxBossAppear);

        HUDManager.Instance?.ShowNotification(
            "BOSS MUNCUL: " + wave.enemyData.enemyName, Color.red
        );
    }

    void ApplyStats(GameObject obj, EnemyData data)
    {
        EnemyStats es = obj.GetComponent<EnemyStats>();
        if (es != null)
        {
            es.maxHP = data.hp;
            es.currentHP = data.hp;
            es.damage = data.damage;
            es.xpDrop = data.xpDrop;
        }

        EnemyAI ai = obj.GetComponent<EnemyAI>();
        if (ai != null)
            ai.speed = data.speed;
    }

    public void OnBossDefeated(string bossName)
    {
        bossesDefeated++;

        if (audioSource != null && sfxBossDefeated != null)
            audioSource.PlayOneShot(sfxBossDefeated);

        HUDManager.Instance?.ShowNotification(
            bossName + " dikalahkan!", Color.green
        );

        // Semua boss mati = game clear
        if (bossesDefeated >= totalBosses)
        {
            gameCleared = true;

            if (audioSource != null && sfxGameClear != null)
                audioSource.PlayOneShot(sfxGameClear);

            GameClearScreen.Instance?.Show();
        }
    }
}