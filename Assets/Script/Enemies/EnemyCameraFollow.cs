using UnityEngine;

public class EnemyCameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float cameraSpeed = 2f; // Speed at which the camera moves
    public float edgeThreshold = 0.1f; // Threshold for camera following at the edge

    public Transform spawnParent; // Parent object for enemy spawning
    public GameObject enemyPrefab; // Enemy prefab to spawn
    public float spawnInterval = 0.5f; // Time interval between enemy spawns

    private float targetY; // Y position to follow the player
    private float spawnTimer; // Timer for enemy spawning

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void LateUpdate()
    {
        // Calculate the target Y position based on the player's position
        targetY = player.position.y;

        // Move the camera horizontally towards the player
        Vector3 targetPosition = transform.position;
        targetPosition.x = Mathf.Lerp(targetPosition.x, player.position.x, Time.deltaTime * cameraSpeed);
        transform.position = targetPosition;

        // Check if the player is at the top or bottom edge of the camera view
        float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        float screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float playerTop = player.position.y + player.localScale.y / 2;
        float playerBottom = player.position.y - player.localScale.y / 2;

        // Move the camera vertically only when the player is at the top or bottom edge
        if (playerTop >= screenTop && targetY >= targetPosition.y + edgeThreshold)
        {
            targetPosition.y = targetY - edgeThreshold;
        }
        else if (playerBottom <= screenBottom && targetY <= targetPosition.y - edgeThreshold)
        {
            targetPosition.y = targetY + edgeThreshold;
        }

        // Move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);

        // Update the spawn parent's position to match the camera's position
        spawnParent.position = new Vector3(transform.position.x, spawnParent.position.y, spawnParent.position.z);

        // Spawn enemies at regular intervals within the camera view
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        // Randomly choose a spawn position within the camera view
        float randomY = Random.Range(-4.5f, 4.5f);
        Vector3 spawnPosition = new Vector3(transform.position.x + 16f, randomY, 0f);

        // Instantiate the enemy prefab under the spawn parent at the chosen position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, spawnParent);
    }
}