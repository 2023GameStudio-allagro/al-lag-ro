
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject normalEnemyPrefab;
    public GameObject tankEnemyPrefab;
    public GameObject fastEnemyPrefab;
    //public GameObject enemyPrefabD;
    public float spawnInterval = 0.5f;
    [SerializeField] public Transform spawnParent;

    private float cameraHeight;
    private float cameraWidth;
    private float spriteWidth;
    private List<Vector2> spawnPositions;
    private float nextSpawnTime;

    private List<SpawnPatternData> levelPatternsCache;
    private SpawnPatternData currentSpawnPattern;
    private EnemyFactory factory;
    public int beatNo = 120;//temporal

    void Awake()
    {
        factory = GetComponent<EnemyFactory>();
        levelPatternsCache = new List<SpawnPatternData>();
        foreach (string addr in Constants.SPAWN_PATTERN_RESOURCES)
        {
            levelPatternsCache.Add(JsonLoader.LoadJsonData<SpawnPatternData>(addr));
        }
    }

    void Start()
    {
        LoadStage(1);

        CalculateCameraSize();
        CalculateSpriteWidth();
        CalculateSpawnPositions();
    }


    public void OnBeatReceived(int beatNo)
    {
        List<SpawnEnemyData> enemyData = currentSpawnPattern.GetEnemyDataFromBeatIndex(beatNo);
        SpawnEnemy(enemyData);
    }

    public void LoadStage(int stageNo)
    {
        if (stageNo < 1) return;
        currentSpawnPattern = levelPatternsCache[stageNo - 1];
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            List<SpawnEnemyData> enemyData = currentSpawnPattern.GetEnemyDataFromBeatIndex(beatNo);
            
            SpawnEnemy(enemyData);
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void CalculateCameraSize()
    {
        cameraHeight = Camera.main.orthographicSize * 2;
        cameraWidth = cameraHeight * Camera.main.aspect;
    }

    private void CalculateSpriteWidth()
    {
        SpriteRenderer spriteRenderer = normalEnemyPrefab.GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.bounds.size.x;
    }

    private void CalculateSpawnPositions()
    {
        float cameraX = Camera.main.transform.position.x + (Time.time % 0.5f) * 2f;
        float startY = cameraHeight / 18f;

        spawnPositions = new List<Vector2>
        {
            new Vector2(cameraX, startY * 2f),
            new Vector2(cameraX, startY * 4f),
            new Vector2(cameraX, startY * 6f),
            new Vector2(cameraX, startY * 8f)
        };
    }


    private void SpawnEnemy(List<SpawnEnemyData> enemyData)
    {
        if (enemyData == null || enemyData.Count == 0) return;

        foreach (SpawnEnemyData eachEnemyData in enemyData)
        {
            for (int i = 0; i < 1; i++)
            {
                GameObject enemyPrefab = GetRandomEnemyPrefab();
                if (enemyPrefab == null) continue;

                float randomY = GetRandomYPosition();
                float spawnX = Camera.main.transform.position.x + 8f;

                Vector2 spawnPosition = new Vector2(spawnX, randomY);

                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                //GameObject enemy= factory.Make(enemyPrefab, eachEnemyData.hp, eachEnemyData.speed);
                enemy.GetComponent<SpriteRenderer>().flipX = true;

                // enemymovement 접근해서 shouldfollowcamera 세팅
                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                
                enemyMovement.SetHP(eachEnemyData.hp);
                //enemyMovement.SetSpeed(eachEnemyData.speed);
                if (enemyMovement != null)
                {
                    enemyMovement.shouldFollowCamera = false;
                }
                

            }
        }
    }

    //private void SpawnEnemy(List<SpawnEnemyData> enemyData)
    //{
    //    if (enemyData == null || enemyData.Count == 0) return;

    //    foreach (SpawnEnemyData eachEnemyData in enemyData)
    //    {
    //        int spawnCount = Random.Range(0, 1); // Randomly determine the number of enemies to spawn (0 or 1)

    //        for (int i = 0; i <= spawnCount; i++)
    //        {
    //            GameObject enemyPrefab = GetRandomEnemyPrefab();
    //            if (enemyPrefab == null) continue;

    //            float randomY = GetRandomYPosition();
    //            float spawnX = Camera.main.transform.position.x + 8f;

    //            Vector2 spawnPosition = new Vector2(spawnX, randomY);

    //            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, spawnParent);
    //            enemy.transform.localScale = new Vector2(-1f, 1f); // Flip the enemy sprite to face left

    //            // Set the enemy's HP and speed using the data from SpawnEnemyData
    //            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
    //            EnemyBase enemyBrain = enemy.GetComponent<EnemyBase>();
    //            enemyBrain.SetHP(eachEnemyData.hp);
    //            enemyBrain.SetSpeed(eachEnemyData.speed);

    //            if (enemyMovement != null)
    //            {
    //                enemyMovement.shouldFollowCamera = false;
    //            }

    //        }
    //    }
    //}


    private GameObject GetRandomEnemyPrefab()
    {
        GameObject[] enemyPrefabs = { normalEnemyPrefab, tankEnemyPrefab, fastEnemyPrefab };
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
    }


    private Vector2 GetRandomSpawnPosition()
    {
        return spawnPositions[Random.Range(0, spawnPositions.Count)];
    }

    private float GetRandomYPosition()
    {
        float cameraY = Camera.main.transform.position.y; // Get the current y position of the camera

        StartCoroutine(UpdateCameraYPosition()); // Start a coroutine to continuously update the cameraY value

        //float startY = cameraY - (cameraHeight / 2f); // Calculate the starting y position based on the camera's current position

        int[] yPositions = { -4, -2, 0, 2 }; // Available y positions for enemy spawning
        int randomIndex = Random.Range(0, yPositions.Length);
        float randomY = cameraY + yPositions[randomIndex]; // Adjust the random y position based on the camera's current position

        return randomY;
    }

    private IEnumerator UpdateCameraYPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // Wait for half a second before updating the cameraY value
            float cameraY = Camera.main.transform.position.y; // Update the cameraY value
        }
    }
}
 