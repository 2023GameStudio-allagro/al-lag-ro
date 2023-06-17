using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private GameObject normalEnemyPrefab;
    [SerializeField] private GameObject tankEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;
    [SerializeField] private GameObject empEnemyPrefab;
    [SerializeField] private GameObject parabolicEnemyPrefab;

    public GameObject GetPrefab(char enemyType)
    {
        switch (enemyType)
        {
            case 'z': 
            case 'x': return normalEnemyPrefab;
            case 'c': return fastEnemyPrefab;
            case 'v': return tankEnemyPrefab;
            case 's': return Random.Range(0, 2) == 0 ? empEnemyPrefab : parabolicEnemyPrefab;
        }
        return null;
    }
    public GameObject GetPrefab(string enemyType)
    {
        switch (enemyType)
        {
            case Constants.NORMAL_ENEMY: return normalEnemyPrefab;
            case Constants.FAST_ENEMY: return fastEnemyPrefab;
            case Constants.TANK_ENEMY: return tankEnemyPrefab;
        }
        return null;
    }
    public GameObject Make(GameObject prefab, char enemyType)
    {
        GameObject enemy = Instantiate(prefab);
        EnemyBase enemyBrain = enemy.GetComponent<EnemyBase>();
        enemyBrain.SetMarker(new EnemyMarkerV2(enemyType));
        enemyBrain.SetSpeed(1f);
        return enemy;
    }
}