using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject enemySpawnerPrefab;
    [SerializeField] private GameObject itemSpawnerPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject bulletPoolPrefab;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject startButton;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text mainTimerText;
    [SerializeField] private GameObject joystick;
    [SerializeField] private float countdownTime = 3f;
    [SerializeField] private float mainTimerDuration = 60f;
    [SerializeField] private Terrain terrain;
    [SerializeField] private GameObject portalPrefab;

    private bool gameActive = false;
    private float mainTimer;
    private EnemySpawner enemySpawnerComponent;
    private ItemSpawner itemSpawnerComponent;
    private GameObject player;
    private Pool bulletPool;
    private bool portalSpawned = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        startButton.gameObject.SetActive(true);
    }

    public void OnStartButtonPressed()
    {
        startButton.SetActive(false);
        StartCoroutine(GameRoutine());
    }

    private IEnumerator GameRoutine()
    {
        yield return StartCoroutine(CountdownRoutine());
        StartGame();
        yield return StartCoroutine(MainTimerRoutine());
        EndGame(true);
    }

    private IEnumerator CountdownRoutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < countdownTime)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = Mathf.Ceil(countdownTime - elapsedTime).ToString();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        countdownText.text = "0";
    }

    private void StartGame()
    {
        countdownText.gameObject.SetActive(false);
        joystick.SetActive(true);

        // Instantiate bullet pool
        GameObject bulletPoolObject = Instantiate(bulletPoolPrefab, transform.root);
        bulletPool = bulletPoolObject.GetComponent<Pool>();

        // Instantiate player
        player = Instantiate(playerPrefab, GetPlayerSpawnPosition(), Quaternion.identity);
        Player playerComponent = player.GetComponent<Player>();
        playerComponent.Initialize(bulletPool);

        // Instantiate enemy spawner
        GameObject enemySpawner = Instantiate(enemySpawnerPrefab, Vector3.zero, Quaternion.identity);
        enemySpawnerComponent = enemySpawner.GetComponent<EnemySpawner>();
        enemySpawnerComponent.terrain = terrain;
        enemySpawnerComponent.PlayerTransform = player.transform;
        enemySpawnerComponent.BulletPool = bulletPool;
        enemySpawnerComponent.Initialize(enemyPrefabs);

        // Instantiate item spawner
        GameObject itemSpawner = Instantiate(itemSpawnerPrefab, Vector3.zero, Quaternion.identity);
        itemSpawnerComponent = itemSpawner.GetComponent<ItemSpawner>();
        itemSpawnerComponent.terrain = terrain;

        gameActive = true;
        mainTimer = mainTimerDuration;
    }

    private Vector3 GetPlayerSpawnPosition()
    {
        float terrainWidth = terrain.terrainData.size.x;
        float terrainHeight = terrain.terrainData.size.z;
        float spawnX = terrainWidth / 2;
        float spawnZ = terrainHeight / 2;
        float spawnY = terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ)) + 1.0f;
        return new Vector3(spawnX, spawnY, spawnZ);
    }

    private void Update()
    {
        if (gameActive)
        {
            mainTimerText.gameObject.SetActive(true);
            mainTimer -= Time.deltaTime;
            mainTimerText.text = Mathf.Max(0, Mathf.Ceil(mainTimer)).ToString();
            if (mainTimer <= 0)
            {
                EndGame(true);
            }
        }
    }

    private IEnumerator MainTimerRoutine()
    {
        while (mainTimer > 0)
        {
            yield return null;
        }
        mainTimerText.text = "0";
    }

    public void EndGame(bool playerWon)
    {
        gameActive = false;
        mainTimerText.text = string.Empty;
        enemySpawnerComponent.SetEnemySpawnActive(false);
        itemSpawnerComponent.SetItemSpawnActive(false);

        if (playerWon)
        {
           
            SpawnPortal();
           
        }
        else
        {
            Debug.Log("You Lose!");
        }

      
    }

    private void SpawnPortal()
    {
        if (!portalSpawned)
        {
            Vector3 portalPosition = GetPortalSpawnPosition();
            Instantiate(portalPrefab, portalPosition, Quaternion.identity);
            Debug.Log("Portal spawned at: " + portalPosition);
            portalSpawned = true;
        }
    }

    private Vector3 GetPortalSpawnPosition()
    {
        float portalHeight = 5f; 
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;
        float spawnX = terrainWidth / 2;
        float spawnZ = terrainLength / 1.1f;
        float spawnY = terrain.SampleHeight(new Vector3(spawnX, 0, spawnZ));

        return new Vector3(spawnX, spawnY + portalHeight / 2, spawnZ);
    }



    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnPlayerDeath()
    {
        
            EndGame(false);
            StartCoroutine(RestartGameWithDelay(2f)); 
        
    }

    public void OnPortalEntered()
    {
       
           RestartGame(); 
        
    }

    private IEnumerator RestartGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestartGame();
    }

    public bool IsGameActive()
    {
        return gameActive;
    }
}
