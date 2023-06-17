using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerV2 : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    private EnemyFactory factory;
    private float y = 0;

    void Awake()
    {
        factory = GetComponent<EnemyFactory>();
    }

    public void OnNoteReceived(char enemyType)
    {
        y = GetYPosition(enemyType);
        SpawnEnemy(enemyType, y);
    }

    private void SpawnEnemy(char enemyType, float yPos)
    {
        if (enemyType == '.') return;

        GameObject enemyPrefab = factory.GetPrefab(enemyType);
        if (enemyPrefab == null) return;

        Vector2 spawnPosition = new Vector2(11f, yPos);

        GameObject enemy = factory.Make(enemyPrefab, enemyType);
        enemy.transform.position = spawnPosition;
    }

    private float GetYPosition(char enemyType)
    {
        switch(enemyType)
        {
            case 'z': return GetYPositionZitter(y, 0.5f);
            case 'x': return GetYPositionZitter(y, 1f);
            case 'c': return GetYPositionZitter(y, 2f);
            case 'v': return GetYPositionZitter(y, 3f);
            case 's': return GetYPositionZitter(y, 5f);
            default: return y;
        }
    }

    private float GetYPositionZitter(float baseY, float zitter)
    {
        float minY = -4;
        float maxY = 3;

        bool exceedUp = baseY + zitter > maxY;
        bool exceedDown = baseY - zitter < minY;

        if(exceedUp && exceedDown) return Random.Range(0f, 1f) < 0.5f ? minY : maxY;
        else if(exceedUp) return baseY - zitter;
        else if(exceedDown) return baseY + zitter;
        return baseY + zitter * (Random.Range(0, 2) * 2 - 1);
    }

    private float GetRandomYPosition()
    {
        float cameraY = Camera.main.transform.position.y; // Get the current y position of the camera

        int[] yPositions = { -4, -2, 0, 2 }; // Available y positions for enemy spawning
        int randomIndex = Random.Range(0, yPositions.Length);
        float randomY = cameraY + yPositions[randomIndex]; // Adjust the random y position based on the camera's current position

        return randomY;
    }
}
 