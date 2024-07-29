using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 10f;
    public Terrain terrain;
    private Transform playerTransform;
    private Pool bulletPool;
    private GameObject[] enemyPrefabs;
    private bool enemySpawnActive;

    public Transform PlayerTransform
    {
        get { return playerTransform; }
        set { playerTransform = value; }
    }

    public Pool BulletPool
    {
        get { return bulletPool; }
        set { bulletPool = value; }
    }

    public void Initialize(GameObject[] enemies)
    {
        enemyPrefabs = enemies; 
    }

    private void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player transform not assigned.");
        }

        if (bulletPool == null)
        {
            Debug.LogError("BulletPool not assigned.");
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (GameManager.Instance.IsGameActive())
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 || terrain == null || playerTransform == null || bulletPool == null)
        {
            Debug.LogError("Missing references for spawning enemy.");
            return;
        }

        
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;
        float spawnX = Random.Range(0, terrainWidth);
        float spawnZ = Random.Range(0, terrainHeight);
        float spawnY = terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ)) + 1.0f;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<Enemy>().Initialize(playerTransform, bulletPool);
    }

    public void SetEnemySpawnActive(bool status)
    {
        enemySpawnActive = status;
    }
}
