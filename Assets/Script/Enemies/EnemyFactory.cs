using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    //[SerializeField] private GameObject playerRef;
    [SerializeField] private GameObject normalEnemyPrefab;
    [SerializeField] private GameObject tankEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;

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
    public GameObject Make(GameObject prefab, int hp, float speed)
    {
        GameObject enemy = Instantiate(prefab);
        EnemyBase enemyBrain = enemy.GetComponent<EnemyBase>();
        enemyBrain.SetHP(hp);
        enemyBrain.SetSpeed(speed);
        return enemy;
    }
}